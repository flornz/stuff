using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClassLibrary1
{
    public class db_fld
        {

        /// <summary>
        /// return fieldnames ArrayList given connection string and table name
        /// </summary>
        /// <param name="flds"></param>
        /// <param name="cs"></param>
        /// <param name="tbl_name"></param>
        public static void return_field_names(ref ArrayList flds, string cs, string tbl_name)
        {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"select top 1 * from " + tbl_name;

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
            {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                {
                    objRdr = objCmd.ExecuteReader();
                    for (int ctr = 0; ctr < objRdr.FieldCount; ctr++)
                    {
                        flds.Add(objRdr.GetName(ctr));
                    }
                }
            }
        } // void return_field_names

        public static void return_field_names_types(ref ArrayList flds, ref ArrayList types, string cs, string tbl_name)
        {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"select top 1 * from " + tbl_name;

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
            {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                {
                    objRdr = objCmd.ExecuteReader();
                    for (int ctr = 0; ctr < objRdr.FieldCount; ctr++)
                    {
                        flds.Add(objRdr.GetName(ctr));
                        types.Add(objRdr.GetDataTypeName(ctr));
                    }
                }
            }
        } // void return_field_names

        /// <summary>
        /// rewrite to just return dt, then handle dt
        /// </summary>
        /// <param name="flds"></param>
        /// <param name="types"></param>
        /// <param name="sizes"></param>
        /// <param name="cs"></param>
        /// <param name="tbl_name"></param>
        public static System.Data.DataTable return_field_names_types_sizes(ref ArrayList flds, ref ArrayList types, ref ArrayList sizes, string cs, string tbl_name)
        {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"select top 1 * from " + tbl_name;
            System.Data.DataTable dt = null;//reader.GetSchemaTable(); 

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
            {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                {
                    objRdr = objCmd.ExecuteReader();
                    dt = objRdr.GetSchemaTable();


                    
                    //for (int ctr = 0; ctr < objRdr.FieldCount; ctr++)
                    //{
                    //    flds.Add(objRdr.GetName(ctr));
                    //    types.Add(objRdr.GetDataTypeName(ctr));
                    //    objRdr.GetSchemaTable
                    //}
                }
            }

            return(dt);
        } // void return_field_names

        public static System.Data.DataTable return_schema_table(string cs, string tbl_name)
            {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"select top 1 * from " + tbl_name;
            System.Data.DataTable dt = null;//reader.GetSchemaTable(); 

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
                {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                    {
                    objRdr = objCmd.ExecuteReader();
                    dt = objRdr.GetSchemaTable();
                    }
                }

            return (dt);
            }

        /// <summary>
        /// to do: http://stackoverflow.com/questions/1464883/how-can-i-easily-convert-datareader-to-listt (reade to linq)
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="tbl_name"></param>
        /// <returns></returns>
        public static System.Data.DataTable return_schema_table_tsql(string cs, string tbl_name)
            {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"
                SELECT
	                sysobjects.name AS TABLE_NAME
	                --, syscolumns.id
	                , syscolumns.name AS COLUMN_NAME
	                , systypes.name AS DATA_TYPE
	                , syscolumns.length AS CHARACTER_MAXIMUM_LENGTH
	                --, sysproperties.value AS COLUMN_DESCRIPTION
	                , syscomments.text AS COLUMN_DEFAULT
	                , syscolumns.isnullable AS IS_NULLABLE
                	, IS_PK = coalesce(tc.CONSTRAINT_NAME, 'NOT_A_PK')
	
                FROM
	                syscolumns 
	                INNER JOIN systypes ON syscolumns.xtype = systypes.xtype 
	                LEFT OUTER JOIN sysobjects ON syscolumns.id = sysobjects.id 
	                LEFT OUTER JOIN syscomments ON syscolumns.cdefault = syscomments.id
	                left join (
	                    INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.Constraint_name
	                ) on tc.CONSTRAINT_TYPE = 'Primary Key' and tc.table_name = sysobjects.name and syscolumns.name = ccu.COLUMN_NAME
	
                WHERE
	                (systypes.name <> 'sysname')
	                and (sysobjects.xtype = 'U')
            ";
            sql += " and sysobjects.name like '" + tbl_name + "'";
            sql += " order by SysColumns.colid";

            System.Data.DataTable dt = null;//reader.GetSchemaTable(); 

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
                {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                    {
                    objRdr = objCmd.ExecuteReader();
                    dt = new System.Data.DataTable();
                    dt.Load(objRdr);
                    }
                }

            return (dt);
            }
        
        /// SQL: set @fld=value
        /// </summary>
        /// <param name="flds"></param>
        /// <param name="cs"></param>
        /// <param name="tbl_name"></param>
        public static void return_set_field_equals_value(string cs, string tbl_name)
        {
            System.Data.SqlClient.SqlDataReader objRdr = null;
            string sql = @"select top 1 * from " + tbl_name;
            string _str = "";
            int ctr = 0;
            string data_type = "";
            string data_value = "";

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
            {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                {
                    objRdr = objCmd.ExecuteReader();

                    objRdr.Read();
                    for (ctr = 0; ctr < objRdr.FieldCount; ctr++)
                    {
                        data_type = objRdr.GetDataTypeName(ctr);

                        if (ctr == 9)
                        {
                            //debug
                            //Console.Write(data_type);
                        }

                        if (data_type.IndexOf("date") > -1)
                        {
                            //_str = _str + "set @" + objRdr.GetName(ctr) + " = getdate()";
                            data_value = "getdate()";
                        }
                        else if (data_type.IndexOf("Decimal") > -1)
                        {
                            //_str = _str + "set @" + objRdr.GetName(ctr) + " = 0.0";
                            data_value = "0.0";
                        }
                        else if (data_type.IndexOf("char") > -1)
                        {
                            try
                            {
                                data_value = "'" + objRdr.GetString(ctr) + "'";
                            }
                            catch
                            {
                                data_value = "''";
                            }
                        }
                        else
                        {
                            //_str = _str + "set @" + objRdr.GetName(ctr) + " = 0.0";
                            data_value = "0";
                        }

                        _str = _str + "\nset @" + objRdr.GetName(ctr) + " = " + data_value + "";

                    }
                }
            }
        } // void return_field_names

        public static void exec_sql(string cs, string t_sql)
            {

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
                {
                objConn1.Open();
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(t_sql, objConn1))
                    {
                    objCmd.ExecuteNonQuery();
                    }
                }
            } //
        
        //        public static string create_tmp_upd_hist_table_from_sql(string cs, string t_sql, string tbl_name)
        public static string create_tmp_upd_hist_table_from_sql(string cs)
            {
            string drop_tbl_sql = @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[_hw_uh_tmp]') AND type in (N'U')) DROP TABLE [dbo].[_hw_uh_tmp]";
            string sql = @"
--USE [eProfileDev]
--GO
--/****** Object:  StoredProcedure [dbo].[sp_HardwareAsset_Update_History_Search_ById]    Script Date: 11/10/2011 11:15:45 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER OFF
--GO
---- =============================================
---- Author:		FSoliva
---- Create date: 2011-Nov-07
---- Description:	[sp_HardwareAsset_Update History_Search_ById]
---- =============================================
--ALTER PROCEDURE [dbo].[sp_HardwareAsset_Update_History_Search_ById] (
--	@HARDWARE_ASSET_ID int
--)

--AS

SELECT     top 4
	dbo.T_Audit.AUDIT_ID
	, dbo.T_HardwareAsset.HARDWARE_ASSET_ID
	, dbo.T_Audit.AUDIT_USER_ID
	, dbo.T_AuditColumn.AUDIT_COLUMN_ID

	, AUDIT_DATE = convert(char(11), dbo.T_Audit.AUDIT_DATE, 109) + ' ' + convert(char(5), dbo.T_Audit.AUDIT_DATE, 114)
	
	, dbo.T_HardwareAsset.UPDATE_REFERENCE
	, UPDATE_TYPE = upd_type.REF_VALUE_DESC
	--, dbo.T_HardwareAsset.UPDATE_TYPE
--	, dbo.T_Employee.FIRST_NAME	, dbo.T_Employee.LAST_NAME
	, dbo.T_Login.USERNAME
	
	--, dbo.T_AuditColumn.TABLE_NAME
	, dbo.T_AuditColumn.COLUMN_NAME
	, dbo.T_AuditColumn.[DESCRIPTION]

	-- this colums will have the rollback links in update history, and will roll back associated child colums (see below)
	, IS_PARENT_COLUMN = CASE
		WHEN dbo.T_AuditColumn.COLUMN_NAME IN ('COST_CENTRE_ID', 'AUTHORIZED_EMPLOYEE_ID', 'ASSET_LOCATION_ID') then 1
		ELSE 0
	END

	, IS_CHILD_COLUMN = CASE
		WHEN dbo.T_AuditColumn.COLUMN_NAME IN (
			'COST_CENTRE_NUMBER', 'BUSINESS_UNIT_NAME'
			, 'FIRST_NAME', 'LAST_NAME', 'USERNAME'
			, 'STREET_ADDRESS', 'CITY', 'PROV_STATE_CODE', 'POSTAL_CODE') 
			THEN 1
		ELSE 0
	END
                    	
	--, dbo.T_AuditValue.OLD_VALUE
	, OLD_VALUE = case dbo.T_AuditColumn.COLUMN_NAME
		when 'HOMEUSE' then
			case dbo.T_AuditValue.OLD_VALUE when 'Y' then 'True' else 'False' end
		when 'IS_BREAK_FIX_ONLY' then
			case dbo.T_AuditValue.OLD_VALUE when 'T' then 'True' else 'False' end
		when 'MAINTENANCE_CONTRACT' then
			case dbo.T_AuditValue.OLD_VALUE when 'T' then 'True' else 'False' end
		--when 'AUTHORIZED_EMPLOYEE_ID' then 
		--	dbo.[fn_HardwareAsset_Audit_Get_Previous_Values_ById] (dbo.T_Audit.AUDIT_ID)
		else dbo.T_AuditValue.OLD_VALUE
	end
	
	--, dbo.T_AuditValue.NEW_VALUE
	, NEW_VALUE = case dbo.T_AuditColumn.COLUMN_NAME
		when 'HOMEUSE' then
			case dbo.T_AuditValue.NEW_VALUE when 'Y' then 'True' else 'False' end
		when 'IS_BREAK_FIX_ONLY' then
			case dbo.T_AuditValue.NEW_VALUE when 'T' then 'True' else 'False' end
		when 'MAINTENANCE_CONTRACT' then
			case dbo.T_AuditValue.NEW_VALUE when 'T' then 'True' else 'False' end
		--when 'AUTHORIZED_EMPLOYEE_ID' then 
		--	dbo.[fn_HardwareAsset_Audit_Get_Current_Values_ById] (dbo.T_Audit.AUDIT_ID)
		else dbo.T_AuditValue.NEW_VALUE
	end

	, dbo.T_AuditColumn.IS_AUDIT_DISPLAY
	
	into _hw_uh_tmp

FROM         
	dbo.T_Audit 
	INNER JOIN dbo.T_Employee ON dbo.T_Audit.AUDIT_USER_ID = dbo.T_Employee.EMPLOYEE_ID	and dbo.T_Employee.IS_LOGICAL_DELETED = 'F'
	left join dbo.T_Login ON dbo.T_Login.EMPLOYEE_ID = dbo.T_Employee.EMPLOYEE_ID  AND dbo.T_Login.IS_LOGICAL_DELETED = 'F'
	INNER JOIN dbo.T_AuditValue ON dbo.T_Audit.AUDIT_ID = dbo.T_AuditValue.AUDIT_ID --and dbo.T_AuditValue.IS_LOGICAL_DELETED = 'F'
	INNER JOIN dbo.T_AuditColumn ON dbo.T_AuditValue.AUDIT_COLUMN_ID = dbo.T_AuditColumn.AUDIT_COLUMN_ID --and dbo.T_AuditColumn.IS_LOGICAL_DELETED = 'F'
	INNER JOIN dbo.T_HardwareAsset ON dbo.T_Audit.HARDWARE_ASSET_ID = dbo.T_HardwareAsset.HARDWARE_ASSET_ID and dbo.T_HardwareAsset.IS_LOGICAL_DELETED = 'F'
	INNER JOIN dbo.T_ReferenceValue upd_type ON dbo.T_HardwareAsset.UPDATE_TYPE = upd_type.REF_CODE
order by
		dbo.T_Audit.AUDIT_ID desc
		, dbo.T_AuditColumn.SEQUENCE_NUMBER

                ";

            using (System.Data.SqlClient.SqlConnection objConn1 = new System.Data.SqlClient.SqlConnection(cs))
                {
                objConn1.Open();
                
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(drop_tbl_sql, objConn1))
                    {
                    objCmd.ExecuteNonQuery();
                    }
                using (System.Data.SqlClient.SqlCommand objCmd = new System.Data.SqlClient.SqlCommand(sql, objConn1))
                    {
                    objCmd.ExecuteNonQuery();
                    }
                }

            return("_hw_uh_tmp");
            } // void return_field_names
        
        }
}
