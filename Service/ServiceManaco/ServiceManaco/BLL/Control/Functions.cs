using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;

namespace Bata.Aquarella.BLL.Control
{
    public class Functions
    {
        public string _FUV_CO { get; set; }
        public decimal _FUN_ID { get; set; }
        public string _FUV_NAME { get; set; }
        public string _FUV_DESCRIPTION { get; set; }
        public decimal? _FUN_ORDER { get; set; }
        public decimal? _FUN_FATHER { get; set; }
        public decimal _FUN_SYSTEM { get; set; }
        public string _FUN_URL { get; set; }

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        public bool InsertFunction()
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "control_aq.SP_INSERT_FUNCTIONS2";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand,_FUV_CO,_FUN_ID,_FUV_NAME,_FUV_DESCRIPTION,_FUN_ORDER,_FUN_FATHER,_FUN_SYSTEM);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region <Metodos estaticos>
        public static DataTable GetAllFunctions() {

            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "control_aq.SP_LOADALL_FUNCTIONS_SYS3";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return (db.ExecuteDataSet(dbCommandWrapper)).Tables[0];
        }

        public static DataSet GetAllFunctionsDS()
        {

            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "control_aq.SP_LOADALL_FUNCTIONS_SYS3";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        public static DataSet GetFunctionsByRol(string ROV_CO, decimal RON_ID)
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADFUNCTIONS_RO";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, RON_ID, ROV_CO, results);
            ///return ALL APPLICATIONS COMPANY AND FUCTION REQUIRED 
            try
            {
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception)
            {   
                throw;
            }
            
        }

        public static bool updateFunction(string FUV_CO, decimal FUN_ID, string FUV_NAME, string FUV_DESCRIPTION, decimal? FUN_ORDER, decimal? FUN_FATHER) {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "control_aq.SP_UPDATE_FUNCTIONS";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand);

            db.AddInParameter(dbCommandWrapper, "P_FUV_CO", DbType.String, FUV_CO);
            db.AddInParameter(dbCommandWrapper, "P_FUN_ID", DbType.Decimal, FUN_ID);
            db.AddInParameter(dbCommandWrapper, "P_FUV_NAME", DbType.String, FUV_NAME);
            db.AddInParameter(dbCommandWrapper, "P_FUV_DESCRIPTION", DbType.String, FUV_DESCRIPTION);
            db.AddInParameter(dbCommandWrapper, "P_FUN_ORDER", DbType.Decimal, FUN_ORDER);
            db.AddInParameter(dbCommandWrapper, "P_FUN_FATHER", DbType.Decimal, FUN_FATHER);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool InsertRoleFunction(decimal _RFN_FUNCTIONID, decimal _RFN_ROLEID, string _RFV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "SP_ADD_ROLES_FUNCTIONS";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _RFN_FUNCTIONID, _RFN_ROLEID, _RFV_CO);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRoleFunction(decimal _RFN_FUNCTIONID, decimal _RFN_ROLEID, string _RFV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "SP_DELETE_ROLES_FUNCTIONS";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _RFN_FUNCTIONID, _RFN_ROLEID, _RFV_CO);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion
    }
}