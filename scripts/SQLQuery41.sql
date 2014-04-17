USE [ERP_Budget]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Импорт данных в справочник статей расходов
--
-- Входящие параметры:
-- 
--  @BEGIN_DATE		начало периода
--  @END_DATE			конец периода
--
-- Выходные параметры:
--  @ERROR_NUM - номер ошибки
--  @ERROR_MES - текст ошибки
--
-- Результат:
--    0 - Успешное завершение
--    <>0 - ошибка

ALTER PROCEDURE [dbo].[usp_ImportDebitArticleList] 
	@ClearDebitArticleperiod	D_BIT = 0,
	@BEGIN_DATE								D_DATE,
	@END_DATE									D_DATE,

  @ERROR_NUM								int output,
  @ERROR_MES								nvarchar(4000) output

AS

BEGIN

	BEGIN TRY

    SET @ERROR_NUM = 0;
    SET @ERROR_MES = '';

		BEGIN TRANSACTION UpdateData;

		IF( @ClearDebitArticleperiod = 1 )
			EXEC usp_DeleteDebitArticlePeriod @BEGIN_DATE = @BEGIN_DATE, @END_DATE = @END_DATE, 
				@ERROR_NUM = @ERROR_NUM output, @ERROR_MES = @ERROR_MES output; 
		
		IF( @ERROR_NUM <> 0 )
			BEGIN
				ROLLBACK TRANSACTION UpdateData;
				RETURN @ERROR_NUM;
			END

		-- проверка на наличие ссылок
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = 0;
		
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = -1, 
			[IMPORTRESULT_DESCRIPTION] = 'Не найдена запись в справочнике "План счетов"' 
		WHERE [ACCOUNTPLAN_GUID] IS NULL;
		
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = -1, 
			[IMPORTRESULT_DESCRIPTION] = 'Не найдена запись в справочнике "Проекты"' 
		WHERE [BUDGETPROJECT_GUID] IS NULL;
		
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = -1, 
			[IMPORTRESULT_DESCRIPTION] = 'Не найдена запись в справочнике "Бюджетные подразделения"' 
		WHERE [BUDGETDEP_GUID] IS NULL;
		
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = -1,
			[IMPORTRESULT_DESCRIPTION] = 'Не найдена запись в справочнике "Типы бюджетных расходов"' 
		WHERE [BUDGETEXPENSETYPE_GUID] IS NULL;

		-- регистрация статей расходов в справочнике
		CREATE TABLE #DEBITARTICLE( DEBITARTICLE_NUM nvarchar(56), DEBITARTICLE_NAME nvarchar(128),  
			DEBITARTICLE_ID int, PARENT_DEBITARTICLE_ID int, 
			DEBITARTICLE_GUID uniqueidentifier, PARENT_DEBITARTICLE_GUID uniqueidentifier );
		
		DECLARE @BUDGETITEM_NUM	D_NAME;
		DECLARE @BUDGETITEM_NAME	D_FULLNAME;
		DECLARE @PREV_BUDGETITEM_NUM	D_NAME;
		DECLARE @PARENT_BUDGETITEM_NUM	D_NAME;
		DECLARE @DEBITARTICLE_ID D_ID;
		DECLARE @PARENT_DEBITARTICLE_ID D_ID;
		DECLARE @PREV_DEBITARTICLE_ID D_ID;

		SET @BUDGETITEM_NUM	= NULL;
		SET @BUDGETITEM_NAME = NULL;
		SET @PREV_BUDGETITEM_NUM	= NULL;
		SET @PARENT_BUDGETITEM_NUM	= NULL;
		SET @PARENT_DEBITARTICLE_ID = NULL;
		SET @PREV_DEBITARTICLE_ID = NULL;

		DECLARE crSynch CURSOR FOR SELECT [BUDGETITEM_NUM], [BUDGETITEM_NAME], [DEBITARTICLE_ID]
		FROM [dbo].[T_IMPORTDATA_DEBITARTICLE] WHERE [IMPORTISOK] = 0 ORDER BY [DEBITARTICLE_ID]
		OPEN crSynch;
		FETCH next FROM crSynch INTO @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID;
		WHILE @@fetch_status = 0
			BEGIN
				IF( ( @PREV_BUDGETITEM_NUM IS NOT NULL ) AND ( @PREV_BUDGETITEM_NUM <> @BUDGETITEM_NUM )  )
					BEGIN
						IF( Charindex( '.', @BUDGETITEM_NUM ) = 0 )
							BEGIN
								-- статья верхнего уровня
								SET @PARENT_BUDGETITEM_NUM = NULL;
								SET @PARENT_DEBITARTICLE_ID = NULL;

								INSERT INTO #DEBITARTICLE( DEBITARTICLE_NUM, DEBITARTICLE_NAME, DEBITARTICLE_ID, PARENT_DEBITARTICLE_ID )
								VALUES( @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID, NULL );
							END
						ELSE
							BEGIN
								-- подстатья
								IF( Charindex( @PREV_BUDGETITEM_NUM, @BUDGETITEM_NUM ) > 0 )
									BEGIN
										-- новая подстатья
										SET @PARENT_BUDGETITEM_NUM = @PREV_BUDGETITEM_NUM;
										SET @PARENT_DEBITARTICLE_ID = @PREV_DEBITARTICLE_ID;
									END
								INSERT INTO #DEBITARTICLE( DEBITARTICLE_NUM, DEBITARTICLE_NAME, DEBITARTICLE_ID, PARENT_DEBITARTICLE_ID )
								VALUES( @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID, @PARENT_DEBITARTICLE_ID );
							END
						
					END
				
				SET @PREV_BUDGETITEM_NUM = @BUDGETITEM_NUM;
				SET @PREV_DEBITARTICLE_ID = @DEBITARTICLE_ID;

				FETCH next FROM crSynch INTO @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID;
			END
		CLOSE crSynch;
		DEALLOCATE crSynch;

		-- определены подчинения у статей
		-- производится поиск и регистрация статей в справочнике

		DECLARE @DEBITARTICLE_NUM nvarchar(56); 
		DECLARE @DEBITARTICLE_NAME nvarchar(128);
		DECLARE @DEBITARTICLE_GUID uniqueidentifier;
		DECLARE @PARENT_DEBITARTICLE_GUID uniqueidentifier;
		DECLARE @ACCOUNTPLAN_GUID uniqueidentifier;
		DECLARE @BUDGETDEP_GUID uniqueidentifier;
		DECLARE @BUDGETEXPENSETYPE_GUID uniqueidentifier;
		DECLARE @BUDGETPROJECT_GUID uniqueidentifier;

		CREATE TABLE #DEBITARTICLE_2( DEBITARTICLE_NUM nvarchar(56), DEBITARTICLE_NAME nvarchar(128),  
			DEBITARTICLE_ID int, PARENT_DEBITARTICLE_ID int, 
			DEBITARTICLE_GUID uniqueidentifier, PARENT_DEBITARTICLE_GUID uniqueidentifier );

		DECLARE crSynch CURSOR FOR SELECT [BUDGETITEM_NUM], [BUDGETITEM_NAME], [DEBITARTICLE_ID], PARENT_DEBITARTICLE_ID
		FROM #DEBITARTICLE ORDER BY [DEBITARTICLE_ID]
		OPEN crSynch;
		FETCH next FROM crSynch INTO @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID, @PARENT_DEBITARTICLE_ID;
		WHILE @@fetch_status = 0
			BEGIN
				SET @DEBITARTICLE_GUID = NULL;
				SET @PARENT_DEBITARTICLE_GUID = NULL;

				SELECT @ACCOUNTPLAN_GUID = [ACCOUNTPLAN_GUID], @BUDGETDEP_GUID = [BUDGETDEP_GUID], 
					@BUDGETEXPENSETYPE_GUID = [BUDGETEXPENSETYPE_GUID], @BUDGETPROJECT_GUID = [BUDGETPROJECT_GUID]
				FROM [dbo].[T_IMPORTDATA_DEBITARTICLE]
				WHERE [DEBITARTICLE_ID] = @DEBITARTICLE_ID;

				SELECT @DEBITARTICLE_GUID = DEBITARTICLE_GUID FROM #DEBITARTICLE_2 WHERE DEBITARTICLE_ID = @DEBITARTICLE_ID AND DEBITARTICLE_GUID IS NOT NULL;

				IF( @DEBITARTICLE_GUID IS NULL )
					-- поиск статьи по номеру, наименованию и финансовому периоду
					SELECT @DEBITARTICLE_GUID = [GUID_ID], @PARENT_DEBITARTICLE_GUID = [PARENT_GUID_ID] FROM [dbo].[T_DEBITARTICLE]
					WHERE [DEBITARTICLE_NUM] = @BUDGETITEM_NUM
						AND [DEBITARTICLE_NAME] = @BUDGETITEM_NAME
						AND [GUID_ID] IN ( SELECT [DEBITARTICLE_GUID_ID] FROM [dbo].[T_DEBITARTICLE_PERIOD] 
															 WHERE [BEGIN_DATE] = @BEGIN_DATE AND [END_DATE] = @END_DATE );
				
				IF( @DEBITARTICLE_GUID IS NULL )
					BEGIN
						-- такая статья для указанного периода не зарегистрирована
						IF( @PARENT_DEBITARTICLE_ID IS NOT NULL )
							BEGIN
								-- перед добавлением подстатьи необходимо найти её родителя
								SELECT @PARENT_DEBITARTICLE_GUID = DEBITARTICLE_GUID FROM #DEBITARTICLE_2
								WHERE DEBITARTICLE_ID = @PARENT_DEBITARTICLE_ID;

								IF( @PARENT_DEBITARTICLE_GUID IS NULL ) 
									BEGIN
										-- должна быть ссылка на родительскую статью/подстатью
										UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = -1, 
											[IMPORTRESULT_DESCRIPTION] = 'Не найдена ссылка на родительскую статью' 
										WHERE [DEBITARTICLE_ID] = @DEBITARTICLE_ID;
									END
								ELSE
									BEGIN
										SET @DEBITARTICLE_GUID = NEWID();
										-- регистрация подстатьи
										INSERT INTO dbo.T_DEBITARTICLE( GUID_ID, PARENT_GUID_ID, DEBITARTICLE_NAME, DEBITARTICLE_ID,
											DEBITARTICLE_NUM, DEBITARTICLE_TRANSPORTREST, DEBITARTICLE_DONTCHANGE, ACCOUNTPLAN_GUID )
										VALUES( @DEBITARTICLE_GUID, @PARENT_DEBITARTICLE_GUID, @BUDGETITEM_NAME, @DEBITARTICLE_ID, 
											@BUDGETITEM_NUM, 1, 1, @ACCOUNTPLAN_GUID );
									END
							END
						ELSE
							BEGIN
								-- статья верхнего уровня
								SET @DEBITARTICLE_GUID = NEWID();
								-- регистрация подстатьи
								INSERT INTO dbo.T_DEBITARTICLE( GUID_ID, DEBITARTICLE_NAME, DEBITARTICLE_ID,
									DEBITARTICLE_NUM, DEBITARTICLE_TRANSPORTREST, DEBITARTICLE_DONTCHANGE, ACCOUNTPLAN_GUID )
								VALUES( @DEBITARTICLE_GUID, @BUDGETITEM_NAME, @DEBITARTICLE_ID, 
									@BUDGETITEM_NUM, 1, 1, @ACCOUNTPLAN_GUID );
							END

						INSERT INTO #DEBITARTICLE_2( DEBITARTICLE_NUM, DEBITARTICLE_NAME,  
							DEBITARTICLE_ID, PARENT_DEBITARTICLE_ID, DEBITARTICLE_GUID, PARENT_DEBITARTICLE_GUID )
						VALUES( @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID, @PARENT_DEBITARTICLE_ID, 
							@DEBITARTICLE_GUID, @PARENT_DEBITARTICLE_GUID );
					END

				IF( @DEBITARTICLE_GUID IS NOT NULL )
					BEGIN
						-- финансовый период
						IF NOT EXISTS( SELECT [DEBITARTICLE_GUID_ID] FROM [dbo].[T_DEBITARTICLE_PERIOD] 
													 WHERE [DEBITARTICLE_GUID_ID] = @DEBITARTICLE_GUID 
														AND [BEGIN_DATE] = @BEGIN_DATE
														AND [END_DATE] = @END_DATE )
							INSERT INTO [dbo].[T_DEBITARTICLE_PERIOD]( DEBITARTICLE_GUID_ID, BEGIN_DATE, END_DATE )
							VALUES( @DEBITARTICLE_GUID, @BEGIN_DATE, @END_DATE );
						
						-- статья - служба - тип расходов - проект
						IF NOT EXISTS ( SELECT TOP 1 DEBITARTICLE_GUID_ID FROM dbo.T_DEBITARTICLEBUDGETDEP 
							WHERE ( ( DEBITARTICLE_GUID_ID = @DEBITARTICLE_GUID ) AND ( BUDGETDEP_GUID_ID = @BUDGETDEP_GUID ) ) )
							BEGIN
								INSERT INTO dbo.T_DEBITARTICLEBUDGETDEP( DEBITARTICLE_GUID_ID, BUDGETDEP_GUID_ID, BUDGETEXPENSETYPE_GUID, BUDGETPROJECT_GUID )
								VALUES( @DEBITARTICLE_GUID, @BUDGETDEP_GUID, @BUDGETEXPENSETYPE_GUID, @BUDGETPROJECT_GUID );
							END
						ELSE 
							UPDATE dbo.T_DEBITARTICLEBUDGETDEP  SET BUDGETEXPENSETYPE_GUID = @BUDGETEXPENSETYPE_GUID, BUDGETPROJECT_GUID = @BUDGETPROJECT_GUID
							WHERE DEBITARTICLE_GUID_ID = @DEBITARTICLE_GUID
								AND BUDGETDEP_GUID_ID = @BUDGETDEP_GUID;
						
						UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET [IMPORTISOK] = 1,  [IMPORTRESULT_DESCRIPTION] = 'Успешное завершение импорта строки' 
						WHERE [DEBITARTICLE_ID] = @DEBITARTICLE_ID;
					
					END

				FETCH next FROM crSynch INTO @BUDGETITEM_NUM, @BUDGETITEM_NAME, @DEBITARTICLE_ID, @PARENT_DEBITARTICLE_ID;
			END
		CLOSE crSynch;
		DEALLOCATE crSynch;

		DROP  TABLE #DEBITARTICLE_2;
		DROP  TABLE #DEBITARTICLE;

		IF EXISTS( SELECT IMPORTISOK FROM [dbo].[T_IMPORTDATA_DEBITARTICLE] WHERE [IMPORTISOK] <> 1)
			BEGIN
				SET @ERROR_NUM = 1;
				SET @ERROR_MES = 'Ошибка импорта данных.';
			END
		
		UPDATE [dbo].[T_IMPORTDATA_DEBITARTICLE] SET IMPORTISOK = 0 WHERE [IMPORTISOK] <> 1;

		SELECT ACCOUNTPLAN_1C_CODE, BUDGETITEM_NUM, BUDGETITEM_NAME, BUDGETDEP_NAME, BUDGETEXPENSETYPE_NAME, 
			BUDGETPROJECT_NAME, DEBITARTICLE_ID, DEBITARTICLE_PARENTID, ACCOUNTPLAN_GUID, BUDGETPROJECT_GUID, 
			BUDGETDEP_GUID, BUDGETEXPENSETYPE_GUID, IMPORTISOK, IMPORTRESULT_DESCRIPTION
		FROM [dbo].[T_IMPORTDATA_DEBITARTICLE]
		ORDER BY DEBITARTICLE_ID;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION UpdateData;

    SET @ERROR_NUM = ERROR_NUMBER();
    SET @ERROR_MES = ERROR_MESSAGE();
	END CATCH;

	IF( @ERROR_NUM = 0 )
		BEGIN
			COMMIT TRANSACTION UpdateData
			SET @ERROR_MES = 'Успешное завершение операции.';
		END

	RETURN @ERROR_NUM;
END

GO
