using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebitArticle
{
    class CDebitArticleImportRow
    {
	    public System.Int32 DEBITARTICLE_ID { get; set; }
	    public System.String ACCOUNTPLAN_1C_CODE { get; set; }
	    public System.String BUDGETITEM_NUM { get; set; }
	    public System.String BUDGETITEM_NAME { get; set; }
	    public System.String BUDGETDEP_NAME { get; set; }
	    public System.String BUDGETEXPENSETYPE_NAME { get; set; }
	    public System.String BUDGETPROJECT_NAME { get; set; }
	    public System.Guid ACCOUNTPLAN_GUID	 { get; set; }
	    public System.Guid BUDGETPROJECT_GUID	 { get; set; }
	    public System.Guid BUDGETDEP_GUID	 { get; set; }
        public System.Guid BUDGETEXPENSETYPE_GUID { get; set; }
	    public System.Boolean IMPORTISOK { get; set; }
        public System.String IMPORTRESULT_DESCRIPTION { get; set; }

        public CDebitArticleImportRow()
        {
            DEBITARTICLE_ID = 0;
            ACCOUNTPLAN_1C_CODE = System.String.Empty;
            BUDGETITEM_NUM = System.String.Empty;
            BUDGETITEM_NAME = System.String.Empty;
            BUDGETDEP_NAME = System.String.Empty;
            BUDGETEXPENSETYPE_NAME = System.String.Empty;
            BUDGETPROJECT_NAME = System.String.Empty;

            ACCOUNTPLAN_GUID = System.Guid.Empty;
            BUDGETPROJECT_GUID = System.Guid.Empty;
            BUDGETDEP_GUID = System.Guid.Empty;
            BUDGETEXPENSETYPE_GUID = System.Guid.Empty;
            IMPORTISOK = false;
            IMPORTRESULT_DESCRIPTION = System.String.Empty;
        }

    }

    class CDebitArticleDatabaseModel
    {
        /// <summary>
        /// Удаляет указанный финансовый период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="BEGIN_DATE">начало периода</param>
        /// <param name="END_DATE">окончание периода</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean DeleteDebitArticlePeriodFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime BEGIN_DATE,  System.DateTime END_DATE, 
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteDebitArticlePeriod]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BEGIN_DATE", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@END_DATE", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@BEGIN_DATE"].Value = BEGIN_DATE;
                cmd.Parameters["@END_DATE"].Value = END_DATE;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);


            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        
        /// <summary>
        /// Удаляет информация в промежуточной таблице для импорта статей расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false -  ошибка</returns>
        public static System.Boolean ClearImportDebitArticleListFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteRowsFromImportDataDebitArticle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);


            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Загружает в базу данным список импортируемых статей расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="DebitArticleImportRowList">импортируемый список</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddDebitArticleListToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
             List<CDebitArticleImportRow> DebitArticleImportRowList,
             ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (DebitArticleImportRowList == null) { return bRet; }

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddRowToImportDataDebitArticle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DEBITARTICLE_ID", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ACCOUNTPLAN_1C_CODE", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETITEM_NUM", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETITEM_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETDEP_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETEXPENSETYPE_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETPROJECT_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ACCOUNTPLAN_GUID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETPROJECT_GUID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETDEP_GUID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BUDGETEXPENSETYPE_GUID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                foreach (CDebitArticleImportRow objDebitArticleImportRow in DebitArticleImportRowList)
                {
                    cmd.Parameters["@DEBITARTICLE_ID"].Value = objDebitArticleImportRow.DEBITARTICLE_ID;
                    cmd.Parameters["@ACCOUNTPLAN_1C_CODE"].Value = objDebitArticleImportRow.ACCOUNTPLAN_1C_CODE;
                    cmd.Parameters["@BUDGETITEM_NUM"].Value = objDebitArticleImportRow.BUDGETITEM_NUM;
                    cmd.Parameters["@BUDGETITEM_NAME"].Value = objDebitArticleImportRow.BUDGETITEM_NAME;
                    cmd.Parameters["@BUDGETDEP_NAME"].Value = objDebitArticleImportRow.BUDGETDEP_NAME;
                    cmd.Parameters["@BUDGETEXPENSETYPE_NAME"].Value = objDebitArticleImportRow.BUDGETEXPENSETYPE_NAME;
                    cmd.Parameters["@BUDGETPROJECT_NAME"].Value = objDebitArticleImportRow.BUDGETPROJECT_NAME;
                    cmd.Parameters["@ACCOUNTPLAN_GUID"].Value = objDebitArticleImportRow.ACCOUNTPLAN_GUID;
                    cmd.Parameters["@BUDGETPROJECT_GUID"].Value = objDebitArticleImportRow.BUDGETPROJECT_GUID;
                    cmd.Parameters["@BUDGETDEP_GUID"].Value = objDebitArticleImportRow.BUDGETDEP_GUID;
                    cmd.Parameters["@BUDGETEXPENSETYPE_GUID"].Value = objDebitArticleImportRow.BUDGETEXPENSETYPE_GUID;
                    
                    cmd.ExecuteNonQuery();
                    
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                    if (iRes != 0) { break; }
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Импорт данных в справочник статей расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="ClearDebitArticleperiod">признак "удалить статьи расходов указанного финансового перода"</param>
        /// <param name="BEGIN_DATE">начало финансового периода</param>
        /// <param name="END_DATE">окончание финансового периода</param>
        /// <param name="DebitArticleImportRowList">список импортруемых записей</param>
        /// <param name="DebitArticleImportResultRowList">список результирующих записей</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public static System.Boolean ImportDebitArticleListToDB(UniXP.Common.CProfile objProfile, System.Boolean ClearDebitArticleperiod,
            System.DateTime BEGIN_DATE, System.DateTime END_DATE,
            List<CDebitArticleImportRow> DebitArticleImportRowList, 
            ref List<CDebitArticleImportRow> DebitArticleImportResultRowList,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (DebitArticleImportRowList == null) { return bRet; }
                if (DebitArticleImportResultRowList == null) { DebitArticleImportResultRowList = new List<CDebitArticleImportRow>(); }

                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                // данные загружаются в промежуточную таблицу в БД для последующей обработки
                System.Boolean bPrevResult = false;
                if (ClearDebitArticleperiod == true)
                {
                    if (ClearImportDebitArticleListFromDB(objProfile, cmd, ref strErr, ref iRes) == true)
                    {
                        bPrevResult = AddDebitArticleListToDB(objProfile, cmd, DebitArticleImportRowList, ref strErr, ref iRes);
                    }
                    else { bPrevResult = false; }
                }
                else
                {
                    bPrevResult = AddDebitArticleListToDB(objProfile, cmd, DebitArticleImportRowList, ref strErr, ref iRes);
                }

                if (bPrevResult == false)
                {
                    // откатываем транзакцию
                    if (DBTransaction != null)
                    {
                        DBTransaction.Rollback();
                    }
                    if (DBConnection != null)
                    {
                        DBConnection.Close();
                    }

                    return bRet;
                }
                else
                {
                    if (DBTransaction != null)
                    {
                        DBTransaction.Commit();
                    }
                }

                cmd.Parameters.Clear();
                cmd.Transaction = null;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_ImportDebitArticleList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BEGIN_DATE", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@END_DATE", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClearDebitArticleperiod", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@BEGIN_DATE"].Value = BEGIN_DATE;
                cmd.Parameters["@END_DATE"].Value = END_DATE;
                cmd.Parameters["@ClearDebitArticleperiod"].Value = ClearDebitArticleperiod;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CDebitArticleImportRow newRow = null;

                    while (rs.Read())
                    {

                        newRow = new CDebitArticleImportRow();

                        newRow.DEBITARTICLE_ID = ((rs["DEBITARTICLE_ID"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["DEBITARTICLE_ID"])); ;
                        newRow.ACCOUNTPLAN_1C_CODE = ((rs["ACCOUNTPLAN_1C_CODE"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ACCOUNTPLAN_1C_CODE"]));
                        newRow.BUDGETITEM_NUM = ((rs["BUDGETITEM_NUM"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["BUDGETITEM_NUM"]));
                        newRow.BUDGETITEM_NAME = ((rs["BUDGETITEM_NAME"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["BUDGETITEM_NAME"]));
                        newRow.BUDGETDEP_NAME = ((rs["BUDGETDEP_NAME"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["BUDGETDEP_NAME"]));
                        newRow.BUDGETEXPENSETYPE_NAME = ((rs["BUDGETEXPENSETYPE_NAME"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["BUDGETEXPENSETYPE_NAME"]));
                        newRow.BUDGETPROJECT_NAME = ((rs["BUDGETPROJECT_NAME"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["BUDGETPROJECT_NAME"]));

                        newRow.ACCOUNTPLAN_GUID = ((rs["ACCOUNTPLAN_GUID"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["ACCOUNTPLAN_GUID"]);
                        newRow.BUDGETPROJECT_GUID = ((rs["ACCOUNTPLAN_GUID"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["ACCOUNTPLAN_GUID"]);
                        newRow.BUDGETDEP_GUID = ((rs["ACCOUNTPLAN_GUID"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["ACCOUNTPLAN_GUID"]);
                        newRow.BUDGETEXPENSETYPE_GUID = ((rs["ACCOUNTPLAN_GUID"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["ACCOUNTPLAN_GUID"]);
                        newRow.IMPORTISOK = ((rs["IMPORTISOK"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IMPORTISOK"]));
                        newRow.IMPORTRESULT_DESCRIPTION = ((rs["IMPORTRESULT_DESCRIPTION"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IMPORTRESULT_DESCRIPTION"]));

                        DebitArticleImportResultRowList.Add(newRow);
                    }

                }

                strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                iRes = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                rs.Close();
                rs.Dispose();
                DBConnection.Close();

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
    }
}
