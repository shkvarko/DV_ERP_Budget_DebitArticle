USE [ERP_Budget]
GO
/****** Object:  StoredProcedure [dbo].[sp_EditDebitArticle]    Script Date: 12/16/2012 18:34:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- Изменение свойств статьи расходов ( dbo.T_DEBITARTICLE )
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор
--	@PARENT_GUID_ID - идентификатор родительской статьи
--	@DEBITARTICLE_NAME - наименование статьи
--	@DEBITARTICLE_DESCRIPTION - описание
--	@DEBITARTICLE_NUM - номер статьи расходов
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    1 - статья расходов с заданным именем и номером существует
--    2 - статья расходов с заданным идентификатором не найдена
--    3 - ошибка добавления информации в базу данных

ALTER PROCEDURE [dbo].[sp_EditDebitArticle] 
  @GUID_ID D_GUID,	
	@PARENT_GUID_ID D_GUID = NULL,
	@DEBITARTICLE_NAME D_FULLNAME,
	@DEBITARTICLE_DESCRIPTION D_DESCRIPTION = NULL,
	@DEBITARTICLE_TRANSPORTREST D_BOOLEAN,
  @DEBITARTICLE_DONTCHANGE D_BOOLEAN,
	@DEBITARTICLE_NUM D_NAME,
  @DEBITARTICLE_ID D_ID,
  @ERROR_NUMBER int output,
  @ERROR_MESSAGE nvarchar(4000) output

AS

BEGIN
  
  DECLARE @BEGIN_DATE D_DATE;
  SELECT @BEGIN_DATE = BEGIN_DATE FROM dbo.T_DEBITARTICLE_PERIOD
  WHERE DEBITARTICLE_GUID_ID = @GUID_ID;
  
  -- Проверяем наличие записи с заданным именем и номером
  IF EXISTS ( SELECT GUID_ID FROM dbo.T_DEBITARTICLE
    WHERE 
          ( DEBITARTICLE_NAME = @DEBITARTICLE_NAME ) 
      AND ( DEBITARTICLE_NUM = @DEBITARTICLE_NUM ) 
      AND ( GUID_ID <> @GUID_ID )
      AND ( GUID_ID IN ( SELECT DEBITARTICLE_GUID_ID FROM dbo.T_DEBITARTICLE_PERIOD WHERE BEGIN_DATE = @BEGIN_DATE ) ) )
    RETURN 1;

  -- Проверяем наличие записи с заданным уникальным идентификатором
  IF NOT EXISTS ( SELECT GUID_ID FROM dbo.T_DEBITARTICLE WHERE GUID_ID = @GUID_ID )
    RETURN 2;

	BEGIN TRY
    -- изменяем запись в таблице
    UPDATE dbo.T_DEBITARTICLE SET PARENT_GUID_ID = @PARENT_GUID_ID, 
      DEBITARTICLE_NAME = @DEBITARTICLE_NAME, DEBITARTICLE_NUM = @DEBITARTICLE_NUM, 
	  DEBITARTICLE_TRANSPORTREST = @DEBITARTICLE_TRANSPORTREST, DEBITARTICLE_ID = @DEBITARTICLE_ID, 
    DEBITARTICLE_DONTCHANGE = @DEBITARTICLE_DONTCHANGE	
    WHERE GUID_ID = @GUID_ID;
    -- описание
    IF( @DEBITARTICLE_DESCRIPTION IS NOT NULL )
      BEGIN
        UPDATE dbo.T_DEBITARTICLE SET DEBITARTICLE_DESCRIPTION = @DEBITARTICLE_DESCRIPTION
        WHERE GUID_ID = @GUID_ID;
      END
	  IF( @PARENT_GUID_ID IS NOT NULL )
	   BEGIN
        UPDATE dbo.T_DEBITARTICLE SET 
          DEBITARTICLE_DONTCHANGE = @DEBITARTICLE_DONTCHANGE, 
          DEBITARTICLE_TRANSPORTREST = 
		      ( SELECT DEBITARTICLE_TRANSPORTREST FROM dbo.T_DEBITARTICLE WHERE GUID_ID = @PARENT_GUID_ID ) 
  	    WHERE GUID_ID = @GUID_ID;	
	 END	

	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();
		RETURN 3;
	END CATCH;

	RETURN 0;
END












