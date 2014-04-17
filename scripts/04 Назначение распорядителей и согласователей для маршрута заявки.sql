USE [ERP_Budget]
GO

ALTER TABLE [dbo].[T_BUDGETDEP_MANAGERLIST] ADD [Rights_ID]	dbo.D_ID NULL
GO

UPDATE [dbo].[T_BUDGETDEP_MANAGERLIST] SET [Rights_ID] = 3	-- распорядитель бюджета
GO

ALTER TABLE [dbo].[T_BUDGETDEP_MANAGERLIST]  WITH CHECK ADD  CONSTRAINT [FK_T_BUDGETDEP_MANAGERLIST_Rights] FOREIGN KEY([Rights_ID])
REFERENCES [dbo].[Rights] ([iID])
GO

ALTER TABLE [dbo].[T_BUDGETDEP_MANAGERLIST] CHECK CONSTRAINT [FK_T_BUDGETDEP_MANAGERLIST_Rights]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_BUDGET_MANAGERLIST](
	[BUDGET_GUID_ID]	[dbo].[D_GUID] NOT NULL,
	[USER_ID]					[dbo].[D_ID] NOT NULL,
	[Rights_ID]				[dbo].[D_ID] NOT NULL

) ON [PRIMARY]

GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST]  WITH CHECK ADD  CONSTRAINT [FK_T_BUDGET_MANAGERLIST_T_BUDGET] FOREIGN KEY([BUDGET_GUID_ID])
REFERENCES [dbo].[T_BUDGET] ([GUID_ID])
GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST] CHECK CONSTRAINT [FK_T_BUDGET_MANAGERLIST_T_BUDGET]
GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST]  WITH CHECK ADD  CONSTRAINT [FK_T_BUDGET_MANAGERLIST_UsersID] FOREIGN KEY([USER_ID])
REFERENCES [dbo].[UsersID] ([ulID])
GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST] CHECK CONSTRAINT [FK_T_BUDGET_MANAGERLIST_UsersID]
GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST]  WITH CHECK ADD  CONSTRAINT [FK_T_BUDGET_MANAGERLIST_Rights] FOREIGN KEY([Rights_ID])
REFERENCES [dbo].[Rights] ([iID])
GO

ALTER TABLE [dbo].[T_BUDGET_MANAGERLIST] CHECK CONSTRAINT [FK_T_BUDGET_MANAGERLIST_Rights]
GO


CREATE NONCLUSTERED INDEX [INDX_T_BUDGET_MANAGERLIST_BUDGET] ON [dbo].[T_BUDGET_MANAGERLIST]
(
	[BUDGET_GUID_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_BUDGET_MANAGERLIST_USER] ON [dbo].[T_BUDGET_MANAGERLIST]
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE NONCLUSTERED INDEX [INDX_T_BUDGET_MANAGERLIST_RIGHT] ON [dbo].[T_BUDGET_MANAGERLIST]
(
	[Rights_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO

CREATE UNIQUE NONCLUSTERED INDEX [INDX_T_BUDGET_MANAGERLIST_ALLCOLUMNS] ON [dbo].[T_BUDGET_MANAGERLIST]
(
	[BUDGET_GUID_ID] ASC,
	[USER_ID] ASC,
	[Rights_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [INDEX]

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор динамического права для распорядителя бюджета

CREATE FUNCTION [dbo].[GetDynamicRightManagerID] ()
returns int
with execute as caller
as
begin

  DECLARE @RIGHT_ID int;
  SET @RIGHT_ID = NULL;

	SELECT @RIGHT_ID = [iID] FROM [dbo].[Rights] WHERE [strName] = 'Распорядитель бюджета';

  RETURN @RIGHT_ID;

end

GO
GRANT EXECUTE ON [dbo].[GetDynamicRightManagerID] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор динамического права для согласователя бюджета

CREATE FUNCTION [dbo].[GetDynamicRightCoordinatorID] ()
returns int
with execute as caller
as
begin

  DECLARE @RIGHT_ID int;
  SET @RIGHT_ID = NULL;

	SELECT @RIGHT_ID = [iID] FROM [dbo].[Rights] WHERE [strName] = 'Согласователь бюджета';

  RETURN @RIGHT_ID;

end

GO
GRANT EXECUTE ON [dbo].[GetDynamicRightCoordinatorID] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет запись в список дополнительных распорядителей для бюджетного подразделения ( dbo.T_BUDGETDEP_MANAGERLIST )
--
-- Входные параметры:
--
--  @BUDGETDEP_GUID_ID		- уникальный идентификатор бюджетного подразделения
--  @USER_ID							- уникальный идентификатор пользователя
--		@Right_Name					- уникальный идентификатор динамического права
--
-- Выходные параметры:
--
--		@ERROR_NUMBER				- код ошибки
--		@ERROR_MESSAGE			- текст ошибки
--
-- Результат:
--
--		0 - успешное завершение
--		<>0 - ошибка удаления
--
ALTER PROCEDURE [dbo].[sp_AddBudgetDepAdvancedManager] 
  @BUDGETDEP_GUID_ID	D_GUID,
  @USER_ID						D_ID,
	@Rights_ID					D_ID = NULL,
  
  @ERROR_NUMBER				int output,
  @ERROR_MESSAGE			nvarchar(4000) output

AS

BEGIN
  
  SET @ERROR_NUMBER = 0;
  SET @ERROR_MESSAGE = '';

	IF( @Rights_ID IS NULL )
		SET @Rights_ID = ( SELECT dbo.GetDynamicRightManagerID() );

	DECLARE @Right_Name	varchar(50);
	SET @Right_Name = NULL;

	BEGIN TRY

    IF NOT EXISTS( SELECT GUID_ID FROM dbo.T_BUDGETDEP WHERE GUID_ID = @BUDGETDEP_GUID_ID )
      BEGIN
        SET @ERROR_NUMBER = 1;
        SET @ERROR_MESSAGE = 'Не найдено бюджетное подразделение с заданным идентификатором:' + CONVERT( nvarchar(30), @BUDGETDEP_GUID_ID ) ;
		    RETURN @ERROR_NUMBER;
      END

    IF NOT EXISTS( SELECT [iID] FROM [dbo].[Rights] WHERE [iID] = @Rights_ID )
      BEGIN
        SET @ERROR_NUMBER = 1;
        SET @ERROR_MESSAGE = 'Не найдено динамическое право с заданным идентификатором: ' + CONVERT( nvarchar(20), @Rights_ID ) ;
		    RETURN @ERROR_NUMBER;
      END

    IF NOT EXISTS( SELECT [ulID] FROM [dbo].[UsersID] WHERE [ulID] = @USER_ID )
      BEGIN
        SET @ERROR_NUMBER = 1;
        SET @ERROR_MESSAGE = 'Не найден пользователь с заданным идентификатором: ' + CONVERT( nvarchar(20), @USER_ID ) ;
		    RETURN @ERROR_NUMBER;
      END
		
		SELECT @Right_Name = [strName] FROM [dbo].[Rights] WHERE [iID] = @Rights_ID;


    IF EXISTS( SELECT * FROM [dbo].[UserRights] WHERE ulUserID = @USER_ID AND [iRightsID] = @Rights_ID AND bState = 1 )
      BEGIN
        IF NOT EXISTS( SELECT * FROM dbo.T_BUDGETDEP_MANAGERLIST
											 WHERE ( BUDGETDEP_GUID_ID = @BUDGETDEP_GUID_ID ) AND ( [USER_ID] = @USER_ID ) AND ( [Rights_ID] = @Rights_ID ) )
					BEGIN
						INSERT INTO dbo.T_BUDGETDEP_MANAGERLIST( [BUDGETDEP_GUID_ID], [USER_ID], [Rights_ID] )
						VALUES( @BUDGETDEP_GUID_ID, @USER_ID, @Rights_ID );
					END
      END
    ELSE
      BEGIN
        SET @ERROR_NUMBER = 2;
        SET @ERROR_MESSAGE = 'Пользователю не назначено право: ' + @Right_Name;
		    
				RETURN @ERROR_NUMBER;
      END  
      
	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();
		
		RETURN @ERROR_NUMBER;
	END CATCH;

  IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';


	RETURN @ERROR_NUMBER;
END

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Возвращает признак того, имеет ли пользователь в указанном подразделении заданное динамическое право

CREATE FUNCTION [dbo].[IsUserIncludeInBudgetDepManagerList] ( @BudgetDep_Guid D_GUID, @User_Id D_ID, @Right_Id D_ID )
returns D_BIT
with execute as caller
as
begin

  DECLARE @IsContaining D_BIT;
  SET @IsContaining = 0;

  IF EXISTS ( SELECT * FROM [dbo].[T_BUDGETDEP_MANAGERLIST] 
							WHERE ( [BUDGETDEP_GUID_ID] = @BudgetDep_Guid ) AND ( [USER_ID] = @User_Id ) AND ( [Rights_ID] = @Right_Id ) )
    BEGIN
      SET @IsContaining = 1;
    END

  RETURN @IsContaining;

end

GO
GRANT EXECUTE ON [dbo].[IsUserIncludeInBudgetDepManagerList] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список дополнительных распорядителей бюджета с признаком "входит в состав распорядителей бюджета"
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор бюджетного подразделения
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepAdvancedManagerList] 
  @GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY
		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );

		WITH UserTable (strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID)
		AS
		(
			SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID
			FROM dbo.UserView INNER JOIN [dbo].[UserRights] 
				ON dbo.UserView.ErpBudgetUserID = [dbo].[UserRights].[ulUserID] 
					AND ( ( [dbo].[UserRights].[iRightsID] = @ManagerRight_Id AND [dbo].[UserRights].[bState] = 1) 
					OR ( [dbo].[UserRights].[iRightsID] = @CoordinatorRight_Id  AND [dbo].[UserRights].[bState] = 1) )
		)
    SELECT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
			dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @ManagerRight_Id ) as IsManager,
			dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsCoordinator
    FROM UserTable
    ORDER BY strLastName;  

	END TRY
	BEGIN CATCH
		RETURN 2;
	END CATCH;

	RETURN 0;
END

GRANT EXECUTE ON [dbo].[sp_GetBudgetDepAdvancedManagerList] TO [public]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает состав бюджетного подразделения ( dbo.T_BUDGETDEPDECLARATION )
--
-- Входящие параметры:
--  @BUDGETDEP_GUID_ID - уникальный идентификатор бюджетного подразделения
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepDeclaration] 
  @BUDGETDEP_GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY
    -- выбираем сотрудников заданного бюджетного подразделения
    IF( @BUDGETDEP_GUID_ID IS NOT NULL )
      BEGIN
        SELECT BUDGETDEP_GUID_ID, ulUserID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER, 
          strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID
        FROM dbo.BudgetDepDeclarationView
        WHERE BUDGETDEP_GUID_ID = @BUDGETDEP_GUID_ID
        ORDER BY strLastName;
      END
	END TRY
	BEGIN CATCH
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO

USE [UniXP]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает признак того, заблокирован ли ли пользователь
--
-- Входные параметры
--
-- @ulUserID - уи пользователя
--
-- 

CREATE FUNCTION [dbo].[IsUserBlocked] ( @ulUserID int )
returns bit
with execute as caller
as
begin

  DECLARE @IsBlocked bit;
  SET @IsBlocked = 0;

	SELECT @IsBlocked = [bBlock] FROM [dbo].[ClientRights] WHERE [ulUserID] = @ulUserID;

	IF( @IsBlocked IS NULL ) SET @IsBlocked = 0;
	
  RETURN @IsBlocked;

end


GO
GRANT EXECUTE ON [dbo].[IsUserBlocked] TO [public]
GO



USE [ERP_Budget]
GO

/****** Object:  View [dbo].[UserView]    Script Date: 13.03.2014 20:19:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[UserView]
AS
SELECT     TOP (100) PERCENT UniXP.dbo.[User].strLogonName, UniXP.dbo.[User].strFirstName, UniXP.dbo.[User].strMiddleName, UniXP.dbo.[User].strLastName, 
                      UniXP.dbo.[User].strDescription, UniXP.dbo.[User].uuidOptionsID, dbo.UsersID.ulUniXPID AS UniXPUserID, dbo.UsersID.ulID AS ErpBudgetUserID, 
                      UniXP.dbo.IsUserBlocked(dbo.UsersID.ulUniXPID) AS IsUserBlocked
FROM         UniXP.dbo.[User] INNER JOIN
                      dbo.UsersID ON UniXP.dbo.[User].ulSQLUserID = dbo.UsersID.ulUniXPID

GO

/****** Object:  StoredProcedure [dbo].[sp_GetUser]    Script Date: 13.03.2014 20:07:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список пользователей ( dbo.UsersID )
--
-- Входящие параметры:
--  @ulUserID - уникальный идентификатор пользователя
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetUser] 
  @ulUserID D_ID = NULL
AS

BEGIN
  
	BEGIN TRY
    -- если @ulUserID указан, выбираем информацию по заданному пользователю
    IF( @ulUserID IS NOT NULL )
      BEGIN
        SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, 
          dbo.UserView.strLastName, dbo.UserView.strDescription, 
          dbo.UserView.uuidOptionsID, dbo.UserView.UniXPUserID, 
					dbo.UserView.ErpBudgetUserID, dbo.UserView.[IsUserBlocked]
        FROM dbo.UserView
        WHERE dbo.UserView.ErpBudgetUserID = @ulUserID;
      END
    ELSE
      BEGIN
        SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, 
          dbo.UserView.strLastName, dbo.UserView.strDescription, 
          dbo.UserView.uuidOptionsID, dbo.UserView.UniXPUserID, 
					dbo.UserView.ErpBudgetUserID, dbo.UserView.[IsUserBlocked]
        FROM dbo.UserView
        ORDER BY dbo.UserView.strLastName;
      END
	END TRY
	BEGIN CATCH
		RETURN 2;
	END CATCH;

	RETURN 0;
END

GO

/****** Object:  View [dbo].[BudgetDepView]    Script Date: 13.03.2014 20:24:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[BudgetDepView]
AS
SELECT     dbo.T_BUDGETDEP.GUID_ID, dbo.T_BUDGETDEP.PARENT_GUID_ID, dbo.T_BUDGETDEP.BUDGETDEP_NAME, dbo.T_BUDGETDEP.BUDGETDEP_MANAGER, 
                      dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, dbo.UserView.UniXPUserID, 
                      dbo.UserView.ErpBudgetUserID, dbo.T_BUDGETDEP.BUDGETDEP_ID, 
											dbo.UserView.IsUserBlocked
FROM         dbo.T_BUDGETDEP LEFT OUTER JOIN
                      dbo.UserView ON dbo.T_BUDGETDEP.BUDGETDEP_MANAGER = dbo.UserView.ErpBudgetUserID

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список бюджетных подразделений ( dbo.T_BUDGETDEP )
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDep] 
  @GUID_ID D_GUID = NULL
AS

BEGIN
  
	BEGIN TRY
    -- если @GUID_ID указан, выбираем одну запись
    IF( @GUID_ID IS NOT NULL )
      BEGIN
        SELECT GUID_ID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLogonName, 
          strFirstName, strMiddleName, strLastName, IsUserBlocked
        FROM dbo.BudgetDepView
        WHERE GUID_ID = @GUID_ID;
      END
    ELSE
    -- выбираем весь список бюджетных подразделений
      BEGIN
				-- 20130719
				-- очень накладно проверять, заветилось ли бюджетное подразделение в заявках
				-- выборка будет происходить непосредственно из BudgetDepView
        --SELECT t.GUID_ID, t.PARENT_GUID_ID, t.BUDGETDEP_NAME, t.BUDGETDEP_MANAGER, t.strLogonName, 
        --  t.strFirstName, t.strMiddleName, t.strLastName, cast( t.ISREADONLY as bit ) as ISREADONLY, 
        --  dbo.HasBudgetDepChildren( t.GUID_ID ) as HASCHILDREN
        --FROM 
        --(
        --  SELECT GUID_ID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLogonName, 
        --    strFirstName, strMiddleName, strLastName, 0 as ISREADONLY
        --  FROM dbo.BudgetDepView
        --  WHERE 
        --        GUID_ID NOT IN ( SELECT DISTINCT( BUDGETDEP_GUID_ID ) FROM dbo.BudgetDocView )
        --    AND GUID_ID NOT IN ( SELECT BUDGETDEP_GUID_ID FROM dbo.BudgetView )

        --  UNION

        --  SELECT GUID_ID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLogonName, 
        --    strFirstName, strMiddleName, strLastName, 1 as ISREADONLY
        --  FROM dbo.BudgetDepView
        --  WHERE 
        --        GUID_ID IN ( SELECT BUDGETDEP_GUID_ID FROM dbo.BudgetDocView )
        --    OR GUID_ID IN ( SELECT DISTINCT( BUDGETDEP_GUID_ID ) FROM dbo.BudgetView )
        --) as t
        --ORDER BY t.BUDGETDEP_NAME

				SELECT GUID_ID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLogonName, 
            strFirstName, strMiddleName, strLastName, cast( 1 as bit ) as ISREADONLY,
				dbo.HasBudgetDepChildren( GUID_ID ) as HASCHILDREN, IsUserBlocked
        FROM dbo.BudgetDepView
				ORDER BY BUDGETDEP_NAME;

      END
	END TRY
	BEGIN CATCH
		PRINT ERROR_MESSAGE();
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список дополнительных распорядителей бюджета с признаком "входит в состав распорядителей бюджета"
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор бюджетного подразделения
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepAdvancedManagerList] 
  @GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY
		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );

		WITH UserTable (strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
			IsManager, IsCoordinator, IsUserBlocked)
		AS
		(
			SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @ManagerRight_Id ) as IsManager,
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsCoordinator,
				IsUserBlocked
			FROM dbo.UserView INNER JOIN [dbo].[UserRights] 
				ON dbo.UserView.ErpBudgetUserID = [dbo].[UserRights].[ulUserID] 
					AND ( ( [dbo].[UserRights].[iRightsID] = @ManagerRight_Id AND [dbo].[UserRights].[bState] = 1  ) 
					OR ( [dbo].[UserRights].[iRightsID] = @CoordinatorRight_Id  AND [dbo].[UserRights].[bState] = 1) )
		)
    SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, 
			ErpBudgetUserID, IsManager, IsCoordinator, IsUserBlocked
    FROM UserTable
		WHERE ( ( IsUserBlocked = 0 ) OR ( IsCoordinator = 1 ) OR ( IsManager = 1 ) )
    ORDER BY strLastName;  

	END TRY
	BEGIN CATCH
		RETURN 2;
	END CATCH;

	RETURN 0;
END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает признак того, имеет ли пользователь в указанном бюджете заданное динамическое право

CREATE FUNCTION [dbo].[IsUserIncludeInBudgetManagerList] ( @Budget_Guid D_GUID, @User_Id D_ID, @Right_Id D_ID )
returns D_BIT
with execute as caller
as
begin

  DECLARE @IsContaining D_BIT;
  SET @IsContaining = 0;

  IF EXISTS ( SELECT * FROM [dbo].[T_BUDGET_MANAGERLIST] 
							WHERE ( [BUDGET_GUID_ID] = @Budget_Guid ) AND ( [USER_ID] = @User_Id ) AND ( [Rights_ID] = @Right_Id ) )
    BEGIN
      SET @IsContaining = 1;
    END

  RETURN @IsContaining;

end

GO
GRANT EXECUTE ON [dbo].[IsUserIncludeInBudgetManagerList] TO [public]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список дополнительных распорядителей и согласователей бюджета
--
-- Входные параметры:
--
--  @Budget_Guid - уникальный идентификатор бюджета
--
-- Выходные параметры:
--
--		@ERROR_NUM		код ошибки
--		@ERROR_MES		текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <> 0 - ошибка выполнения запроса в базу данных

CREATE PROCEDURE [dbo].[usp_GetBudgetAdvancedManagerList] 
  @Budget_Guid		D_GUID,
  
	@ERROR_NUMBER			int output,
  @ERROR_MESSAGE			nvarchar( 4000 ) output

AS

BEGIN
  
	BEGIN TRY

    SET @ERROR_NUMBER = 0;
    SET @ERROR_MESSAGE = '';

		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;
		DECLARE @BudgetDep_Guid				D_GUID;		

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );
		SELECT @BudgetDep_Guid = [BUDGETDEP_GUID_ID] FROM [dbo].[T_BUDGET] WHERE [GUID_ID] = @Budget_Guid;

		WITH BudgetDepManagerList( strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
			IsUserBlocked, IsManager, IsCoordinator )
		AS
		(
			SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
				dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked,
				dbo.IsUserIncludeInBudgetManagerList ( @Budget_Guid, ErpBudgetUserID, @ManagerRight_Id ) as IsManager,
				dbo.IsUserIncludeInBudgetManagerList ( @Budget_Guid, ErpBudgetUserID, @CoordinatorRight_Id ) as IsCoordinator
			FROM [dbo].[T_BUDGETDEP_MANAGERLIST] AS BUDGETDEP_MANAGERLIST INNER JOIN dbo.UserView 
				ON BUDGETDEP_MANAGERLIST.[USER_ID] = dbo.UserView.ErpBudgetUserID INNER JOIN [dbo].[UserRights] 
				ON dbo.UserView.ErpBudgetUserID = [dbo].[UserRights].[ulUserID] 
					AND ( ( [dbo].[UserRights].[iRightsID] = @ManagerRight_Id AND [dbo].[UserRights].[bState] = 1  ) 
					OR ( [dbo].[UserRights].[iRightsID] = @CoordinatorRight_Id  AND [dbo].[UserRights].[bState] = 1) )
			WHERE BUDGETDEP_MANAGERLIST.[BUDGETDEP_GUID_ID] = @BudgetDep_Guid
		)
    SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, 
			ErpBudgetUserID, IsManager, IsCoordinator, IsUserBlocked
    FROM BudgetDepManagerList
		WHERE ( IsUserBlocked = 0 )
    ORDER BY strLastName;  

	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();

		RETURN @ERROR_NUMBER;
	END CATCH;

	IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';

	RETURN @ERROR_NUMBER;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetBudgetAdvancedManagerList] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Удаляет список дополнительных распорядителей для бюджета
--
-- Входные параметры:
--
--  @Budget_Guid		уникальный идентификатор бюджета
--
-- Выходные параметры:
--
--		@ERROR_NUM		код ошибки
--		@ERROR_MES		текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <> 0 - ошибка выполнения запроса в базу данных

CREATE PROCEDURE [dbo].[usp_DeleteBudgetAdvancedManagerList] 
  @Budget_Guid		D_GUID,

  @ERROR_NUMBER int output,
  @ERROR_MESSAGE nvarchar(4000) output

AS

BEGIN
  
  SET @ERROR_NUMBER = 0;
  SET @ERROR_MESSAGE = '';

	BEGIN TRY

    DELETE FROM [dbo].[T_BUDGET_MANAGERLIST] WHERE [BUDGET_GUID_ID] = @Budget_Guid;
      
	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();
		
		RETURN @ERROR_NUMBER;
	END CATCH;

  IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';

	RETURN @ERROR_NUMBER;
END

GO
GRANT EXECUTE ON [dbo].[usp_DeleteBudgetAdvancedManagerList] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Добавляет запись в список согласователей и дополнительных распорядителей бюджета ( dbo.T_BUDGET_MANAGERLIST )
--
-- Входные параметры:
--
--  @Budget_Guid					- уникальный идентификатор бюджета
--  @USER_ID							- уникальный идентификатор пользователя
--		@Rights_ID					- уникальный идентификатор динамического права
--
-- Выходные параметры:
--
--		@ERROR_NUMBER				- код ошибки
--		@ERROR_MESSAGE			- текст ошибки
--
-- Результат:
--
--		0 - успешное завершение
--		<>0 - ошибка удаления
--
CREATE PROCEDURE [dbo].[sp_AddBudgetAdvancedManager] 
  @Budget_Guid				D_GUID,
  @USER_ID						D_ID,
	@Rights_ID					D_ID,
  
  @ERROR_NUMBER				int output,
  @ERROR_MESSAGE			nvarchar(4000) output

AS

BEGIN
  
  SET @ERROR_NUMBER = 0;
  SET @ERROR_MESSAGE = '';

	DECLARE @Right_Name	varchar(50);
	SET @Right_Name = NULL;

	BEGIN TRY

    IF NOT EXISTS( SELECT GUID_ID FROM dbo.T_BUDGET WHERE GUID_ID = @Budget_Guid )
      BEGIN
        SET @ERROR_NUMBER = 1;
        SET @ERROR_MESSAGE = 'Не найден бюджет с заданным идентификатором:' + CONVERT( nvarchar(30), @Budget_Guid ) ;
		    RETURN @ERROR_NUMBER;
      END

    IF NOT EXISTS( SELECT [iID] FROM [dbo].[Rights] WHERE [iID] = @Rights_ID )
      BEGIN
        SET @ERROR_NUMBER = 2;
        SET @ERROR_MESSAGE = 'Не найдено динамическое право с заданным идентификатором: ' + CONVERT( nvarchar(20), @Rights_ID ) ;
		    RETURN @ERROR_NUMBER;
      END

    IF NOT EXISTS( SELECT [ulID] FROM [dbo].[UsersID] WHERE [ulID] = @USER_ID )
      BEGIN
        SET @ERROR_NUMBER = 3;
        SET @ERROR_MESSAGE = 'Не найден пользователь с заданным идентификатором: ' + CONVERT( nvarchar(20), @USER_ID ) ;
		    RETURN @ERROR_NUMBER;
      END
		
		SELECT @Right_Name = [strName] FROM [dbo].[Rights] WHERE [iID] = @Rights_ID;


    IF EXISTS( SELECT * FROM [dbo].[UserRights] WHERE ulUserID = @USER_ID AND [iRightsID] = @Rights_ID AND bState = 1 )
      BEGIN
        IF NOT EXISTS( SELECT * FROM dbo.T_BUDGET_MANAGERLIST
											 WHERE ( BUDGET_GUID_ID = @Budget_Guid ) AND ( [USER_ID] = @USER_ID ) AND ( [Rights_ID] = @Rights_ID ) )
					BEGIN
						INSERT INTO dbo.T_BUDGET_MANAGERLIST( [BUDGET_GUID_ID], [USER_ID], [Rights_ID] )
						VALUES( @Budget_Guid, @USER_ID, @Rights_ID );
					END
      END
    ELSE
      BEGIN
        SET @ERROR_NUMBER = 2;
        SET @ERROR_MESSAGE = 'Пользователю не назначено право: ' + @Right_Name;
		    
				RETURN @ERROR_NUMBER;
      END  
      
	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();
		
		RETURN @ERROR_NUMBER;
	END CATCH;

  IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';


	RETURN @ERROR_NUMBER;
END

GO
GRANT EXECUTE ON [dbo].[sp_AddBudgetAdvancedManager] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Проверяет, существует ли для заданного бюджета список распорядителей и согласователей
--
-- Входные параметры
-- 
-- @Budget_Guid - уи бюджета
--
CREATE FUNCTION [dbo].[IsBudgetAccessLimited] ( @Budget_Guid D_GUID )
returns D_BIT
with execute as caller
as
begin

  DECLARE @IsContaining D_BIT;
  SET @IsContaining = 0;

  IF EXISTS ( SELECT * FROM [dbo].[T_BUDGET_MANAGERLIST] WHERE [BUDGET_GUID_ID] = @Budget_Guid )
    BEGIN
      SET @IsContaining = 1;
    END

  RETURN @IsContaining;

end

GO
GRANT EXECUTE ON [dbo].[IsBudgetAccessLimited] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает бюджеты ( dbo.T_BUDGET )
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор бюджета
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudget] 
  @GUID_ID D_GUID = NULL
AS

BEGIN
  
	BEGIN TRY
    -- выбираем информацию для конкретного бюджета
    IF( @GUID_ID IS NOT NULL )
      BEGIN
        SELECT GUID_ID, BUDGETDEP_GUID_ID, BUDGET_NAME, BUDGET_DATE, PARENT_GUID_ID, 
          BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLastName, strFirstName, UniXPUserID,
          cast( BUDGET_ACCEPT as bit ) AS BUDGET_ACCEPT, BUDGET_ACCEPTDATE, CURRENCY_GUID_ID, CURRENCY_CODE, CURRENCY_NAME, 
					OFFBUDGET_EXPENDITURES, 	
					BUDGETPROJECT_GUID, BUDGETPROJECT_NAME, BUDGETPROJECT_DESCRIPTION, 
					BUDGETPROJECT_ACTIVE, BUDGETPROJECT_1C_CODE, 
					BUDGETTYPE_GUID, BUDGETTYPE_NAME, BUDGETTYPE_DESCRIPTION, BUDGETTYPE_ACTIVE, 
					dbo.IsBudgetAccessLimited( GUID_ID ) AS IsBudgetAccessLimited
        FROM dbo.BudgetView, dbo.UserView
        WHERE 
             dbo.BudgetView.GUID_ID = @GUID_ID
         AND dbo.BudgetView.BUDGETDEP_MANAGER = dbo.UserView.ErpBudgetUserID;
      END
    ELSE
      BEGIN
        -- выбираем все записи
        SELECT GUID_ID, BUDGETDEP_GUID_ID, BUDGET_NAME, BUDGET_DATE, PARENT_GUID_ID, 
          BUDGETDEP_NAME, BUDGETDEP_MANAGER, strLastName, strFirstName, UniXPUserID,
          cast( BUDGET_ACCEPT as bit ) AS BUDGET_ACCEPT, BUDGET_ACCEPTDATE, CURRENCY_GUID_ID, CURRENCY_CODE, CURRENCY_NAME,
					OFFBUDGET_EXPENDITURES, 	
					BUDGETPROJECT_GUID, BUDGETPROJECT_NAME, BUDGETPROJECT_DESCRIPTION, 
					BUDGETPROJECT_ACTIVE, BUDGETPROJECT_1C_CODE, 
					BUDGETTYPE_GUID, BUDGETTYPE_NAME, BUDGETTYPE_DESCRIPTION, BUDGETTYPE_ACTIVE,
					dbo.IsBudgetAccessLimited( GUID_ID ) AS IsBudgetAccessLimited
        FROM dbo.BudgetView, dbo.UserView
        WHERE dbo.BudgetView.BUDGETDEP_MANAGER = dbo.UserView.ErpBudgetUserID
--          AND Year(dbo.BudgetView.BUDGET_DATE) = Year( GetDate() )
        ORDER BY BUDGET_DATE, BUDGET_NAME;
      END
	END TRY
	BEGIN CATCH
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор действия "Подтвердить"

CREATE FUNCTION [dbo].[GetBudgetDocEventConfirmGuid] ()
returns D_GUID
with execute as caller
as
begin

  DECLARE @BudgetDocEvent_Guid D_GUID;
  SET @BudgetDocEvent_Guid = NULL;

	SELECT Top 1 @BudgetDocEvent_Guid = [GUID_ID] 
	FROM [dbo].[T_BUDGETDOCEVENT] 
	WHERE [BUDGETDOCEVENT_NAME] = 'Подтвердить';

  RETURN @BudgetDocEvent_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetBudgetDocEventConfirmGuid] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор действия "Подтвердить дополнительно"

CREATE FUNCTION [dbo].[GetBudgetDocEventConfirmAddGuid] ()
returns D_GUID
with execute as caller
as
begin

  DECLARE @BudgetDocEvent_Guid D_GUID;
  SET @BudgetDocEvent_Guid = NULL;

	SELECT Top 1 @BudgetDocEvent_Guid = [GUID_ID] 
	FROM [dbo].[T_BUDGETDOCEVENT] 
	WHERE [BUDGETDOCEVENT_NAME] = 'Подтвердить (доп.)';

  RETURN @BudgetDocEvent_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetBudgetDocEventConfirmAddGuid] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор действия "Пройти контроль"

CREATE FUNCTION [dbo].[GetBudgetDocEventControlGuid] ()
returns D_GUID
with execute as caller
as
begin

  DECLARE @BudgetDocEvent_Guid D_GUID;
  SET @BudgetDocEvent_Guid = NULL;

	SELECT Top 1 @BudgetDocEvent_Guid = [GUID_ID] 
	FROM [dbo].[T_BUDGETDOCEVENT] 
	WHERE [BUDGETDOCEVENT_NAME] = 'Пройти контроль';

  RETURN @BudgetDocEvent_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetBudgetDocEventControlGuid] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор действия "Согласовать"

CREATE FUNCTION [dbo].[GetBudgetDocEventReconcileGuid] ()
returns D_GUID
with execute as caller
as
begin

  DECLARE @BudgetDocEvent_Guid D_GUID;
  SET @BudgetDocEvent_Guid = NULL;

	SELECT Top 1 @BudgetDocEvent_Guid = [GUID_ID] 
	FROM [dbo].[T_BUDGETDOCEVENT] 
	WHERE [BUDGETDOCEVENT_NAME] = 'Согласовать';

  RETURN @BudgetDocEvent_Guid;

end

GO
GRANT EXECUTE ON [dbo].[GetBudgetDocEventReconcileGuid] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает идентификатор динамического права для контролера бюджета

CREATE FUNCTION [dbo].[GetDynamicRightControllerID] ()
returns int
with execute as caller
as
begin

  DECLARE @RIGHT_ID int;
  SET @RIGHT_ID = NULL;

	SELECT @RIGHT_ID = [iID] FROM [dbo].[Rights] WHERE [strName] = 'Контролер бюджета';

  RETURN @RIGHT_ID;

end

GO
GRANT EXECUTE ON [dbo].[GetDynamicRightControllerID] TO [public]
GO


/****** Object:  View [dbo].[UserBudgetDocEventView]    Script Date: 16.03.2014 19:47:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[UserBudgetDocEventView]
AS
SELECT     dbo.T_BUDGETDOCEVENTRIGHT.BUDGETDOCEVENT_GUID_ID, dbo.T_BUDGETDOCEVENTRIGHT.RIGHT_ID, 
                      dbo.T_BUDGETDOCEVENT.BUDGETDOCEVENT_NAME, dbo.Rights.strName, dbo.UserRights.ulUserID, dbo.UserRights.bState, 
                      dbo.UserView.strFirstName, dbo.UserView.strLastName, dbo.UserView.UniXPUserID, dbo.UserView.[IsUserBlocked]
FROM         dbo.T_BUDGETDOCEVENTRIGHT INNER JOIN
                      dbo.T_BUDGETDOCEVENT ON dbo.T_BUDGETDOCEVENTRIGHT.BUDGETDOCEVENT_GUID_ID = dbo.T_BUDGETDOCEVENT.GUID_ID INNER JOIN
                      dbo.Rights ON dbo.T_BUDGETDOCEVENTRIGHT.RIGHT_ID = dbo.Rights.iID INNER JOIN
                      dbo.UserRights ON dbo.Rights.iID = dbo.UserRights.iRightsID LEFT OUTER JOIN
                      dbo.UserView ON dbo.UserRights.ulUserID = dbo.UserView.ErpBudgetUserID

GO


/****** Object:  StoredProcedure [dbo].[sp_GetBudgetDocEventUserList]    Script Date: 16.03.2014 19:46:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список пользователей, имеющих доступ к указанному событию 
--
-- Входящие параметры:
-- @BUDGETDOCEVENT_GUID_ID - уникальный идентификатор события бюджетного документа
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDocEventUserList] 
  @BUDGETDOCEVENT_GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY

    SELECT ulUserID, UniXPUserID, strLastName, strFirstName, RIGHT_ID, strName
    FROM dbo.UserBudgetDocEventView
    WHERE ( BUDGETDOCEVENT_GUID_ID = @BUDGETDOCEVENT_GUID_ID ) AND ( bState = 1 )
			AND UniXPUserID IS NOT NULL
			AND ulUserID IS NOT NULL 
			AND IsUserBlocked = 0
    ORDER BY strLastName;

	END TRY
	BEGIN CATCH
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список пользователей, имеющих доступ к указанному действию
--
-- Входные параметры:
--
--  @BudgetDocEvent_Guid		- уникальный идентификатор действия
--  @BudgetDep_Guid				- уникальный идентификатор бюджетного подразделения
--  @Budget_Guid						- уникальный идентификатор бюджета
--
-- Выходные параметры:
--
--		@ERROR_NUM		код ошибки
--		@ERROR_MES		текст ошибки
--
-- Результат:
--    0 - успешное завершение
--    <> 0 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[usp_GetBudgetDocEventUserList] 
  @BudgetDocEvent_Guid	D_GUID,
  @BudgetDep_Guid				D_GUID = NULL,
  @Budget_Guid					D_GUID = NULL,
  
	@ERROR_NUMBER					int output,
  @ERROR_MESSAGE				nvarchar( 4000 ) output

AS

BEGIN
  
	BEGIN TRY

    SET @ERROR_NUMBER = 0;
    SET @ERROR_MESSAGE = '';

		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;
		DECLARE @ControllerRight_Id		D_ID;

		DECLARE @BudgetDocEventConfirm_Guid			D_GUID;
		DECLARE @BudgetDocEventConfirmAdd_Guid	D_GUID;
		DECLARE @BudgetDocEventControl_Guid			D_GUID;
		DECLARE @BudgetDocEventReconcile_Guid		D_GUID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );
		SET @ControllerRight_Id = ( SELECT dbo.GetDynamicRightControllerID() );

		SET @BudgetDocEventConfirm_Guid = ( SELECT dbo.GetBudgetDocEventConfirmGuid() );
		SET @BudgetDocEventConfirmAdd_Guid = ( SELECT dbo.GetBudgetDocEventConfirmAddGuid() );
		SET @BudgetDocEventControl_Guid = ( SELECT dbo.GetBudgetDocEventControlGuid() );
		SET @BudgetDocEventReconcile_Guid = ( SELECT dbo.GetBudgetDocEventReconcileGuid() );

		-- контроль
		IF( @BudgetDocEvent_Guid = @BudgetDocEventControl_Guid )
			BEGIN
				IF( @BudgetDep_Guid IS NOT NULL )
					BEGIN
						SELECT DISTINCT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
							dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
						FROM [dbo].[T_USERBUDGETRIGHTS] AS UserBudgetRights INNER JOIN dbo.UserView 
								ON UserBudgetRights.[ulUserID] = dbo.UserView.ErpBudgetUserID
						WHERE UserBudgetRights.[iRightsID] = @ControllerRight_Id
							AND UserBudgetRights.[BUDGETDEP_GUID_ID] = @BudgetDep_Guid
							AND UserBudgetRights.[bState] = 1
						ORDER BY dbo.UserView.strLastName; 
					END
			END

		-- подтверждение
		IF( ( @BudgetDocEvent_Guid = @BudgetDocEventConfirm_Guid ) OR  ( @BudgetDocEvent_Guid = @BudgetDocEventConfirmAdd_Guid ) )
			BEGIN
				IF( ( @Budget_Guid IS NOT NULL ) AND 
							EXISTS( SELECT [USER_ID] FROM [dbo].[T_BUDGET_MANAGERLIST] AS BudgetManagerList 
											WHERE BudgetManagerList.[Rights_ID] = @ManagerRight_Id
												AND BudgetManagerList.[BUDGET_GUID_ID] = @Budget_Guid  ) )
					BEGIN
						WITH BudgetManagerList( strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, IsUserBlocked )
						AS
						(
							SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
								dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
							FROM [dbo].[T_BUDGET_MANAGERLIST] AS BudgetManagerList INNER JOIN dbo.UserView 
									ON BudgetManagerList.[USER_ID] = dbo.UserView.ErpBudgetUserID
							WHERE BudgetManagerList.[Rights_ID] = @ManagerRight_Id
								AND BudgetManagerList.[BUDGET_GUID_ID] = @Budget_Guid
							
							UNION 

							SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
								dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
							FROM [dbo].[T_BUDGET] INNER JOIN [dbo].[T_BUDGETDEP] 
								ON [dbo].[T_BUDGET].[BUDGETDEP_GUID_ID] = [dbo].[T_BUDGETDEP].[GUID_ID] INNER JOIN dbo.UserView 
									ON [dbo].[T_BUDGETDEP].[BUDGETDEP_MANAGER] = dbo.UserView.ErpBudgetUserID
							WHERE [dbo].[T_BUDGET].[GUID_ID] = @Budget_Guid
						)
						SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, IsUserBlocked
						FROM BudgetManagerList
						ORDER BY strLastName; 
					END
				ELSE IF( @BudgetDep_Guid IS NOT NULL )
					BEGIN
						WITH BudgetDepManagerList( strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, IsUserBlocked )
						AS
						(
							SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
								dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
							FROM [dbo].[T_BUDGETDEP_MANAGERLIST] AS BudgetDepManagerList INNER JOIN dbo.UserView 
									ON BudgetDepManagerList.[USER_ID] = dbo.UserView.ErpBudgetUserID
							WHERE BudgetDepManagerList.[Rights_ID] = @ManagerRight_Id
								AND BudgetDepManagerList.[BUDGETDEP_GUID_ID] = @BudgetDep_Guid
							
							UNION 

							SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
								dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
							FROM [dbo].[T_BUDGETDEP]  INNER JOIN dbo.UserView 
									ON [dbo].[T_BUDGETDEP].[BUDGETDEP_MANAGER] = dbo.UserView.ErpBudgetUserID
							WHERE [dbo].[T_BUDGETDEP].[GUID_ID] = @BudgetDep_Guid
						)
						SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, IsUserBlocked
						FROM BudgetDepManagerList
						ORDER BY strLastName; 
					END
			END
		
		-- согласование
		IF( @BudgetDocEvent_Guid = @BudgetDocEventReconcile_Guid )
			BEGIN
				IF( ( @Budget_Guid IS NOT NULL ) AND 
							EXISTS( SELECT [USER_ID] FROM [dbo].[T_BUDGET_MANAGERLIST] AS BudgetManagerList 
											WHERE BudgetManagerList.[Rights_ID] = @ManagerRight_Id
												AND BudgetManagerList.[BUDGET_GUID_ID] = @Budget_Guid  ) )
					BEGIN
						SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
							dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
						FROM [dbo].[T_BUDGET_MANAGERLIST] AS BudgetManagerList INNER JOIN dbo.UserView 
								ON BudgetManagerList.[USER_ID] = dbo.UserView.ErpBudgetUserID
						WHERE BudgetManagerList.[Rights_ID] = @CoordinatorRight_Id
							AND BudgetManagerList.[BUDGET_GUID_ID] = @Budget_Guid
						ORDER BY dbo.UserView.strLastName; 
					END
				ELSE IF( @BudgetDep_Guid IS NOT NULL )
					BEGIN
						SELECT dbo.UserView.strLogonName, dbo.UserView.strFirstName, dbo.UserView.strMiddleName, dbo.UserView.strLastName, 
							dbo.UserView.UniXPUserID, dbo.UserView.ErpBudgetUserID, dbo.UserView.IsUserBlocked
						FROM [dbo].[T_BUDGETDEP_MANAGERLIST] AS BudgetDepManagerList INNER JOIN dbo.UserView 
								ON BudgetDepManagerList.[USER_ID] = dbo.UserView.ErpBudgetUserID
						WHERE BudgetDepManagerList.[Rights_ID] = @CoordinatorRight_Id
							AND BudgetDepManagerList.[BUDGETDEP_GUID_ID] = @BudgetDep_Guid
						ORDER BY dbo.UserView.strLastName; 
					END
			END

	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();

		RETURN @ERROR_NUMBER;
	END CATCH;

	IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';

	RETURN @ERROR_NUMBER;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetBudgetDocEventUserList] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- Возвращает состав шаблона маршрута ( dbo.T_ROUTEDECLARATION )
--
-- Входящие параметры:
--  @ROUTE_GUID_ID - уникальный идентификатор шаблона маршрута
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetRouteDeclaration] 
  @ROUTE_GUID_ID D_GUID = NULL
AS

BEGIN
  IF( @ROUTE_GUID_ID IS NOT NULL )
		BEGIN
			WITH RouteDeclaration( ROUTE_GUID_ID, BUDGETDOCSTATE_IN_GUID_ID, BUDGETDOCSTATE_OUT_GUID_ID, BUDGETDOCEVENT_GUID_ID, 
				POINT_ENTER, POINT_EXIT, ROUTEPOINTGROUP_GUID_ID, POINT_SHOW )
			AS
			(
				SELECT ROUTE_GUID_ID, BUDGETDOCSTATE_IN_GUID_ID, BUDGETDOCSTATE_OUT_GUID_ID, BUDGETDOCEVENT_GUID_ID, 
				POINT_ENTER, POINT_EXIT, ROUTEPOINTGROUP_GUID_ID, POINT_SHOW
				FROM [dbo].[T_ROUTEDECLARATION]
				 WHERE ROUTE_GUID_ID = @ROUTE_GUID_ID
			)
			SELECT ROUTEDECLARATION.BUDGETDOCSTATE_IN_GUID_ID, ROUTEDECLARATION.BUDGETDOCSTATE_OUT_GUID_ID, 
				ROUTEDECLARATION.BUDGETDOCEVENT_GUID_ID, 
				ROUTEDECLARATION.POINT_ENTER, ROUTEDECLARATION.POINT_EXIT, BUDGETDOCEVENT.BUDGETDOCEVENT_NAME, 
				BUDGETDOCSTATE_IN.BUDGETDOCSTATE_NAME AS BUDGETDOCSTATE_IN_NAME, 
				BUDGETDOCSTATE_OUT.BUDGETDOCSTATE_NAME AS BUDGETDOCSTATE_OUT_NAME, 
				ROUTEDECLARATION.ROUTEPOINTGROUP_GUID_ID, dbo.T_ROUTEPOINTGROUP.ROUTEPOINTGROUP_NAME, ROUTEDECLARATION.POINT_SHOW, BUDGETDOCEVENT.BUDGETDOCEVENT_ID
			FROM   RouteDeclaration AS ROUTEDECLARATION INNER JOIN dbo.T_BUDGETDOCEVENT AS BUDGETDOCEVENT 
				ON ROUTEDECLARATION.BUDGETDOCEVENT_GUID_ID = BUDGETDOCEVENT.GUID_ID LEFT OUTER JOIN dbo.T_BUDGETDOCSTATE AS BUDGETDOCSTATE_IN 
				ON ROUTEDECLARATION.BUDGETDOCSTATE_IN_GUID_ID = BUDGETDOCSTATE_IN.GUID_ID INNER JOIN dbo.T_BUDGETDOCSTATE AS BUDGETDOCSTATE_OUT 
				ON ROUTEDECLARATION.BUDGETDOCSTATE_OUT_GUID_ID = BUDGETDOCSTATE_OUT.GUID_ID INNER JOIN dbo.T_ROUTEPOINTGROUP 
				ON ROUTEDECLARATION.ROUTEPOINTGROUP_GUID_ID = dbo.T_ROUTEPOINTGROUP.GUID_ID
			ORDER BY BUDGETDOCEVENT_ID;
		END
	ELSE
		BEGIN
			SELECT BUDGETDOCSTATE_IN_GUID_ID, BUDGETDOCSTATE_OUT_GUID_ID, BUDGETDOCEVENT_GUID_ID, 
				POINT_ENTER, POINT_EXIT, BUDGETDOCEVENT_NAME, BUDGETDOCSTATE_IN_NAME, BUDGETDOCSTATE_OUT_NAME, 
				ROUTEPOINTGROUP_GUID_ID, ROUTEPOINTGROUP_NAME, POINT_SHOW, BUDGETDOCEVENT_ID, [ROUTE_GUID_ID]
			FROM dbo.RouteDeclarationView
	    ORDER BY BUDGETDOCEVENT_ID;		
		END
		
		--SELECT BUDGETDOCSTATE_IN_GUID_ID, BUDGETDOCSTATE_OUT_GUID_ID, BUDGETDOCEVENT_GUID_ID, 
  --    POINT_ENTER, POINT_EXIT, BUDGETDOCEVENT_NAME, BUDGETDOCSTATE_IN_NAME, BUDGETDOCSTATE_OUT_NAME, 
  --    ROUTEPOINTGROUP_GUID_ID, ROUTEPOINTGROUP_NAME, POINT_SHOW, BUDGETDOCEVENT_ID
  --  FROM dbo.RouteDeclarationView
  --  WHERE ROUTE_GUID_ID = @ROUTE_GUID_ID
  --  ORDER BY BUDGETDOCEVENT_ID;

	RETURN 0;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список пользователей, имеющих доступ к указанному событию 
--
-- Входящие параметры:
-- @BUDGETDOCEVENT_GUID_ID - уникальный идентификатор события бюджетного документа
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDocEventUserList] 
  @BUDGETDOCEVENT_GUID_ID D_GUID = NULL
AS

BEGIN
  
	BEGIN TRY

    IF( @BUDGETDOCEVENT_GUID_ID IS NOT NULL )
			BEGIN
				SELECT ulUserID, UniXPUserID, strLastName, strFirstName, RIGHT_ID, strName
				FROM dbo.UserBudgetDocEventView
				WHERE ( BUDGETDOCEVENT_GUID_ID = @BUDGETDOCEVENT_GUID_ID ) AND ( bState = 1 )
					AND UniXPUserID IS NOT NULL
					AND ulUserID IS NOT NULL 
					AND IsUserBlocked = 0
				ORDER BY strLastName;
			END
		ELSE
			BEGIN
				SELECT BudgetDocEventRight.[BUDGETDOCEVENT_GUID_ID], BudgetDocEventRight.[RIGHT_ID], dbo.Rights.[strName],
					dbo.UserRights.ulUserID, dbo.UserView.UniXPUserID, dbo.UserView.strLastName, dbo.UserView.strFirstName
				FROM [dbo].[T_BUDGETDOCEVENTRIGHT] AS BudgetDocEventRight INNER JOIN dbo.Rights 
					ON BudgetDocEventRight.RIGHT_ID = dbo.Rights.iID INNER JOIN dbo.UserRights 
					ON dbo.Rights.iID = dbo.UserRights.iRightsID AND dbo.UserRights.[bState] = 1 INNER JOIN dbo.UserView 
					ON dbo.UserRights.ulUserID = dbo.UserView.ErpBudgetUserID AND dbo.UserView.IsUserBlocked = 0
				ORDER BY dbo.UserView.strLastName;
			END


	END TRY
	BEGIN CATCH
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список пользователей, имеющих доступ к указанному событию 
--
-- Входящие параметры:
-- @BUDGETDOCEVENT_GUID_ID - уникальный идентификатор события бюджетного документа
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDocEventUserList] 
  @BUDGETDOCEVENT_GUID_ID D_GUID = NULL
AS

BEGIN
  
	BEGIN TRY

    IF( @BUDGETDOCEVENT_GUID_ID IS NOT NULL )
			BEGIN
				SELECT ulUserID, UniXPUserID, strLastName, strFirstName, RIGHT_ID, strName
				FROM dbo.UserBudgetDocEventView
				WHERE ( BUDGETDOCEVENT_GUID_ID = @BUDGETDOCEVENT_GUID_ID ) AND ( bState = 1 )
					AND UniXPUserID IS NOT NULL
					AND ulUserID IS NOT NULL 
					AND IsUserBlocked = 0
				ORDER BY strLastName;
			END
		ELSE
			BEGIN
				SELECT BudgetDocEventRight.[BUDGETDOCEVENT_GUID_ID], BudgetDocEventRight.[RIGHT_ID], dbo.Rights.[strName],
					dbo.UserRights.ulUserID, dbo.UserView.UniXPUserID, dbo.UserView.strLastName, dbo.UserView.strFirstName, 
					[dbo].[T_BUDGETDOCEVENT].[BUDGETDOCEVENT_NAME], [dbo].[T_BUDGETDOCEVENT].[BUDGETDOCEVENT_ID]
				FROM [dbo].[T_BUDGETDOCEVENTRIGHT] AS BudgetDocEventRight INNER JOIN dbo.Rights 
					ON BudgetDocEventRight.RIGHT_ID = dbo.Rights.iID INNER JOIN [dbo].[T_BUDGETDOCEVENT]
					ON BudgetDocEventRight.[BUDGETDOCEVENT_GUID_ID] = [dbo].[T_BUDGETDOCEVENT].[GUID_ID] INNER JOIN dbo.UserRights 
					ON dbo.Rights.iID = dbo.UserRights.iRightsID AND dbo.UserRights.[bState] = 1 INNER JOIN dbo.UserView 
					ON dbo.UserRights.ulUserID = dbo.UserView.ErpBudgetUserID AND dbo.UserView.IsUserBlocked = 0
				ORDER BY BudgetDocEventRight.[BUDGETDOCEVENT_GUID_ID], dbo.UserView.strLastName;
			END


	END TRY
	BEGIN CATCH
		RETURN ERROR_NUMBER();
	END CATCH;

	RETURN 0;
END

GO

/****** Object:  StoredProcedure [dbo].[sp_GetBudgetDepAdvancedManagerList]    Script Date: 18.03.2014 14:45:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список дополнительных распорядителей бюджета с признаком "входит в состав распорядителей бюджета"
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор бюджетного подразделения
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepAdvancedManagerList] 
  @GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY
		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;
		DECLARE @ControllerRight_Id	D_ID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );
		SET @ControllerRight_Id = ( SELECT dbo.GetDynamicRightControllerID() );

		WITH UserTable (strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
			IsManager, IsCoordinator, IsController, IsUserBlocked)
		AS
		(
			SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @ManagerRight_Id ) as IsManager,
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsCoordinator,
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsController,
				IsUserBlocked
			FROM dbo.UserView INNER JOIN [dbo].[UserRights] 
				ON dbo.UserView.ErpBudgetUserID = [dbo].[UserRights].[ulUserID] 
					AND ( ( [dbo].[UserRights].[iRightsID] = @ManagerRight_Id AND [dbo].[UserRights].[bState] = 1  ) 
					OR ( [dbo].[UserRights].[iRightsID] = @CoordinatorRight_Id  AND [dbo].[UserRights].[bState] = 1) )
		)
    SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, 
			ErpBudgetUserID, IsManager, IsCoordinator, IsController, IsUserBlocked
    FROM UserTable
		WHERE ( ( IsUserBlocked = 0 ) OR ( IsCoordinator = 1 ) OR ( IsManager = 1 ) )
    ORDER BY strLastName;  

	END TRY
	BEGIN CATCH
		RETURN 2;
	END CATCH;

	RETURN 0;
END

GO

USE [ERP_Budget]
GO

DECLARE @ControllerRight_Id	D_ID;
SET @ControllerRight_Id = ( SELECT dbo.GetDynamicRightControllerID() );

DELETE FROM [dbo].[T_BUDGETDEP_MANAGERLIST] WHERE [Rights_ID] = @ControllerRight_Id;

INSERT INTO [dbo].[T_BUDGETDEP_MANAGERLIST]( BUDGETDEP_GUID_ID, [USER_ID], Rights_ID )
SELECT [BUDGETDEP_GUID_ID], [ulUserID], [iRightsID]
FROM [dbo].[T_USERBUDGETRIGHTS]
WHERE [iRightsID] = @ControllerRight_Id
	AND [bState] = 1;


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Возвращает список бюджетов для отображения в профайле
--
-- Входные параметры:
--
--  @UniXPUserID		уникальный пользователя в БД "UniXP"
--
-- Выходные параметры:
--
--		@ERROR_NUM		код ошибки
--		@ERROR_MES		текст ошибки
--
-- Результат:
--
--    0 - успешное завершение
--    <> 0 - ошибка выполнения запроса в базу данных
--

CREATE PROCEDURE [dbo].[usp_GetBudgetListForProfile] 
  @UniXPUserID					D_ID,
  
	@ERROR_NUMBER					int output,
  @ERROR_MESSAGE				nvarchar( 4000 ) output

AS

BEGIN
  
	BEGIN TRY

    SET @ERROR_NUMBER = 0;
    SET @ERROR_MESSAGE = '';

		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;
		DECLARE @ControllerRight_Id		D_ID;
		DECLARE @ErpBudgetUserID			D_ID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );
		SET @ControllerRight_Id = ( SELECT dbo.GetDynamicRightControllerID() );
		SELECT @ErpBudgetUserID = [ulID] FROM [dbo].[UsersID] WHERE [ulUniXPID] = @UniXPUserID;

		CREATE TABLE #BudgetList( Budget_Guid uniqueidentifier, Right_Id int );

		-- контроль
		IF EXISTS( SELECT [BUDGET_GUID_ID] FROM [dbo].[T_BUDGET_MANAGERLIST] 
								WHERE [USER_ID] = @ErpBudgetUserID AND [Rights_ID] = @ControllerRight_Id )
			BEGIN
				INSERT INTO #BudgetList( Budget_Guid, Right_Id )
				SELECT [dbo].[T_BUDGET_MANAGERLIST].[BUDGET_GUID_ID], @ControllerRight_Id
				FROM [dbo].[T_BUDGET_MANAGERLIST] INNER JOIN [dbo].[T_BUDGET]
					ON [dbo].[T_BUDGET_MANAGERLIST].[BUDGET_GUID_ID] = [dbo].[T_BUDGET].GUID_ID AND [dbo].[T_BUDGET_MANAGERLIST].[USER_ID] = @ErpBudgetUserID AND [dbo].[T_BUDGET_MANAGERLIST].[Rights_ID] = @ControllerRight_Id 
			END
		ELSE
			BEGIN
				INSERT INTO #BudgetList( Budget_Guid, Right_Id )
				SELECT [dbo].[T_BUDGET].[GUID_ID], @ControllerRight_Id
				FROM [dbo].[T_BUDGETDEP_MANAGERLIST] INNER JOIN [dbo].[T_BUDGETDEP]
					ON [dbo].[T_BUDGETDEP_MANAGERLIST].[BUDGETDEP_GUID_ID] = [dbo].[T_BUDGETDEP].GUID_ID AND [dbo].[T_BUDGETDEP_MANAGERLIST].[USER_ID] = @ErpBudgetUserID AND [dbo].[T_BUDGETDEP_MANAGERLIST].[Rights_ID] = @ControllerRight_Id INNER JOIN T_BUDGET
					ON [dbo].[T_BUDGETDEP].GUID_ID = T_BUDGET.[BUDGETDEP_GUID_ID]
			END

		-- подтверждение
		INSERT INTO #BudgetList( Budget_Guid, Right_Id )
		SELECT [dbo].[T_BUDGET].[GUID_ID], @ManagerRight_Id
		FROM [dbo].[T_BUDGET] INNER JOIN [dbo].[T_BUDGETDEP] 
			ON [dbo].[T_BUDGET].[BUDGETDEP_GUID_ID] = [dbo].[T_BUDGETDEP].[GUID_ID] AND [dbo].[T_BUDGETDEP].[BUDGETDEP_MANAGER] = @ErpBudgetUserID

		-- согласование
		IF EXISTS( SELECT [BUDGET_GUID_ID] FROM [dbo].[T_BUDGET_MANAGERLIST] 
								WHERE [USER_ID] = @ErpBudgetUserID AND [Rights_ID] = @CoordinatorRight_Id )
			BEGIN
				INSERT INTO #BudgetList( Budget_Guid, Right_Id )
				SELECT [dbo].[T_BUDGET_MANAGERLIST].[BUDGET_GUID_ID], @CoordinatorRight_Id
				FROM [dbo].[T_BUDGET_MANAGERLIST] INNER JOIN [dbo].[T_BUDGET]
					ON [dbo].[T_BUDGET_MANAGERLIST].[BUDGET_GUID_ID] = [dbo].[T_BUDGET].GUID_ID AND [dbo].[T_BUDGET_MANAGERLIST].[USER_ID] = @ErpBudgetUserID AND [dbo].[T_BUDGET_MANAGERLIST].[Rights_ID] = @CoordinatorRight_Id 
			END
		ELSE IF EXISTS( SELECT [BUDGETDEP_GUID_ID] FROM [dbo].[T_BUDGETDEP_MANAGERLIST] 
				WHERE [USER_ID] = @ErpBudgetUserID AND [Rights_ID] = @CoordinatorRight_Id )
			BEGIN
				INSERT INTO #BudgetList( Budget_Guid, Right_Id )
				SELECT [dbo].[T_BUDGET].[GUID_ID], @CoordinatorRight_Id
				FROM [dbo].[T_BUDGETDEP_MANAGERLIST] INNER JOIN [dbo].[T_BUDGETDEP]
					ON [dbo].[T_BUDGETDEP_MANAGERLIST].[BUDGETDEP_GUID_ID] = [dbo].[T_BUDGETDEP].GUID_ID AND [dbo].[T_BUDGETDEP_MANAGERLIST].[USER_ID] = @ErpBudgetUserID AND [dbo].[T_BUDGETDEP_MANAGERLIST].[Rights_ID] = @CoordinatorRight_Id INNER JOIN T_BUDGET
					ON [dbo].[T_BUDGETDEP].GUID_ID = T_BUDGET.[BUDGETDEP_GUID_ID];
			END
		ELSE
			BEGIN
				IF EXISTS( SELECT [ulUserID] FROM [dbo].[UserRights] 
										WHERE [ulUserID] = @ErpBudgetUserID AND [iRightsID] = @CoordinatorRight_Id AND [bState] = 1 )
					BEGIN
						INSERT INTO #BudgetList( Budget_Guid, Right_Id )
						SELECT [dbo].[T_BUDGET].[GUID_ID], @CoordinatorRight_Id
						FROM [dbo].[T_BUDGET]; 
					END
			END	
		
		SELECT DISTINCT #BudgetList.Budget_Guid, #BudgetList.Right_Id, 
			[dbo].[T_BUDGET].[BUDGET_NAME], [dbo].[T_BUDGET].[BUDGET_DATE], 
			[dbo].[T_BUDGET].[BUDGETDEP_GUID_ID], [dbo].[T_BUDGETDEP].[PARENT_GUID_ID],
			[dbo].[T_BUDGETDEP].[BUDGETDEP_NAME], [dbo].[T_BUDGETDEP].[BUDGETDEP_MANAGER]
		FROM #BudgetList INNER JOIN [dbo].[T_BUDGET]
			ON #BudgetList.Budget_Guid = [dbo].[T_BUDGET].[GUID_ID] INNER JOIN [dbo].[T_BUDGETDEP]
			ON [dbo].[T_BUDGET].[BUDGETDEP_GUID_ID] = [dbo].[T_BUDGETDEP].[GUID_ID]
		ORDER BY [dbo].[T_BUDGET].[BUDGET_NAME];

		DROP TABLE #BudgetList;

	END TRY
	BEGIN CATCH
    SET @ERROR_NUMBER = ERROR_NUMBER();
    SET @ERROR_MESSAGE = ERROR_MESSAGE();

		RETURN @ERROR_NUMBER;
	END CATCH;

	IF( @ERROR_NUMBER = 0 )
		SET @ERROR_MESSAGE = 'Успешное завершение операции.';

	RETURN @ERROR_NUMBER;
END

GO
GRANT EXECUTE ON [dbo].[usp_GetBudgetListForProfile] TO [public]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetBudgetDepAdvancedManagerList]    Script Date: 19.03.2014 20:50:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список дополнительных распорядителей бюджета с признаком "входит в состав распорядителей бюджета"
--
-- Входящие параметры:
--  @GUID_ID - уникальный идентификатор бюджетного подразделения
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepAdvancedManagerList] 
  @GUID_ID D_GUID
AS

BEGIN
  
	BEGIN TRY
		DECLARE @ManagerRight_Id			D_ID;
		DECLARE @CoordinatorRight_Id	D_ID;
		DECLARE @ControllerRight_Id	D_ID;

		SET @ManagerRight_Id = ( SELECT dbo.GetDynamicRightManagerID() );
		SET @CoordinatorRight_Id = ( SELECT dbo.GetDynamicRightCoordinatorID() );
		SET @ControllerRight_Id = ( SELECT dbo.GetDynamicRightControllerID() );

		WITH UserTable (strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
			IsManager, IsCoordinator, IsController, IsUserBlocked)
		AS
		(
			SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, ErpBudgetUserID, 
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @ManagerRight_Id ) as IsManager,
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsCoordinator,
				dbo.IsUserIncludeInBudgetDepManagerList ( @GUID_ID, ErpBudgetUserID, @CoordinatorRight_Id ) as IsController,
				IsUserBlocked
			FROM dbo.UserView INNER JOIN [dbo].[UserRights] 
				ON dbo.UserView.ErpBudgetUserID = [dbo].[UserRights].[ulUserID] 
					AND ( ( [dbo].[UserRights].[iRightsID] = @ManagerRight_Id AND [dbo].[UserRights].[bState] = 1  ) 
					OR ( [dbo].[UserRights].[iRightsID] = @CoordinatorRight_Id  AND [dbo].[UserRights].[bState] = 1) 
					OR ( [dbo].[UserRights].[iRightsID] = @ControllerRight_Id  AND [dbo].[UserRights].[bState] = 1) )
		)
    SELECT DISTINCT strLogonName, strFirstName, strMiddleName, strLastName, UniXPUserID, 
			ErpBudgetUserID, IsManager, IsCoordinator, IsController, IsUserBlocked
    FROM UserTable
		WHERE ( ( IsUserBlocked = 0 ) OR ( IsCoordinator = 1 ) OR ( IsManager = 1 ) )
    ORDER BY strLastName;  

	END TRY
	BEGIN CATCH
		RETURN 2;
	END CATCH;

	RETURN 0;
END

GO

/****** Object:  StoredProcedure [dbo].[sp_GetBudgetDepForBudgetDoc]    Script Date: 23.03.2014 11:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Возвращает список бюджетных подразделений ( dbo.T_BUDGETDEP )
--
-- Входящие параметры:
--
-- Выходные параметры:
--
-- Результат:
--    0 - успешное завершение
--    2 - ошибка выполнения запроса в базу данных

ALTER PROCEDURE [dbo].[sp_GetBudgetDepForBudgetDoc] 
AS

BEGIN
  
	--BEGIN TRY

		WITH BudgetDep (BUDGETDEP_GUID_ID)
			AS
			(
					SELECT DISTINCT BUDGETDEP_GUID_ID 
					FROM dbo.T_BUDGET 
          WHERE Year( BUDGET_DATE ) = Year( Getdate() ) 
						AND BUDGET_ACTIVE = 1
			)
		SELECT dbo.T_BUDGETDEP.GUID_ID, dbo.T_BUDGETDEP.PARENT_GUID_ID, 
			dbo.T_BUDGETDEP.BUDGETDEP_NAME, dbo.T_BUDGETDEP.BUDGETDEP_MANAGER, [dbo].[UsersID].[ulUniXPID]
		FROM dbo.T_BUDGETDEP INNER JOIN BudgetDep
			ON dbo.T_BUDGETDEP.GUID_ID = BudgetDep.BUDGETDEP_GUID_ID INNER JOIN  [dbo].[UsersID]
			ON dbo.T_BUDGETDEP.BUDGETDEP_MANAGER = [dbo].[UsersID].[ulID]
		ORDER BY dbo.T_BUDGETDEP.BUDGETDEP_NAME;

  --SELECT GUID_ID, PARENT_GUID_ID, BUDGETDEP_NAME, BUDGETDEP_MANAGER
  --FROM dbo.T_BUDGETDEP
  --WHERE 
  --  GUID_ID IN ( SELECT DISTINCT BUDGETDEP_GUID_ID FROM dbo.T_BUDGET 
  --               WHERE Year( BUDGET_DATE ) = Year( Getdate() ) AND BUDGET_ACTIVE = 1  )
  --               --WHERE Year( BUDGET_DATE ) = 2013 AND BUDGET_ACTIVE = 1  )

  --ORDER BY BUDGETDEP_NAME;

	--END TRY
	--BEGIN CATCH
	--	RETURN ERROR_NUMBER();
	--END CATCH;

	RETURN 0;
END

GO
