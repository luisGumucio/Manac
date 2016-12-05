using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bata.Aquarella.BLL.Util;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Bata.Aquarella.BLL.Control
{
    public class ApplicationClass
    {
        public string _APV_CO { get; set; }
        public decimal _APN_ID { get; set; }
        public string _APV_NAME { get; set; }
        public string _APV_TYPE { get; set; }
        public string _APV_URL { get; set; }
        public decimal _APN_ORDER { get; set; }
        public string _APV_IMAGE { get; set; }
        public string _APV_STATUS { get; set; }
        public string _APV_HELP { get; set; }
        public string _APV_COMMENTS { get; set; }

        public static string _conn = Constants.OrcleStringConn;


        #region <Metodos Publicos>

        public bool InsertApplication() {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_INSERT_APPLICATION";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _APV_CO, _APN_ID, _APV_NAME, _APV_TYPE,_APV_URL,_APN_ORDER,_APV_IMAGE,_APV_STATUS,_APV_HELP,_APV_COMMENTS);
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

        #region <Metodos Estaticos>

        public static DataSet ApplicationByFunc(decimal FUN_ID, string FUV_CO) {
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "control_aq.SP_LOADAPPLICATIONS_FU";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, FUN_ID, FUV_CO, results);
            //return ALL APPLICATIONS COMPANY AND FUCTION REQUIRED 
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        public static DataTable GetAllAplications() {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "control_aq.SP_LOADALL_APPLICATION";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
        }

        public static bool insertAppFunction(decimal _AFN_APLIID, decimal _AFN_FUNCTIONID, string _AFV_CO){
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_ADD_APPLICATION_FUNCTIONS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _AFN_APLIID, _AFN_FUNCTIONID, _AFV_CO);

            try 
	        {	        
		        db.ExecuteNonQuery(dbCommandWrapper);
                return true;
	        }
	        catch
	        {
		        return false;
	        }
        }

        public static bool deleteAppFunction(decimal _AFN_APLIID, decimal _AFN_FUNCTIONID, string _AFV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_DELETE_APPLICATION_FUNCTION";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _AFN_APLIID, _AFN_FUNCTIONID, _AFV_CO);

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

        public static DataTable GetApplicationType() {
            
            object result = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_LOADALL_APPLICATION_TYPE";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,result);

            return db.ExecuteDataSet(dbCommandWrapper).Tables[0];

        }

        public static bool UpdateApplication(string APV_CO, decimal APN_ID, string APV_NAME, string APV_TYPE, string APV_URL, decimal APN_ORDER, string APV_STATUS, string APV_HELP, string APV_COMMENTS)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_UPDATE_APPLICATION";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, APV_CO, APN_ID, APV_NAME, APV_TYPE, APV_URL, APN_ORDER, null, APV_STATUS, APV_HELP, APV_COMMENTS);

            try{
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception){return false;}
                
        }

        public static DataSet GetUserAppliFunction(string UAV_CO, decimal UAN_USERID) {
            object result = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_USERLOAD_USER_APPLIFUNC2";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,UAV_CO,UAN_USERID,"A",result);

            return db.ExecuteDataSet(dbCommandWrapper);
        }

        public static bool insetUserAppliFunction(string _UAV_CO,decimal _UAN_USERID,decimal _UAN_APLIID, decimal _UAN_FUN_ID){
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_ADD_USERS_APPLIFUNCTIONS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _UAV_CO,_UAN_USERID,_UAN_APLIID, "A",_UAN_FUN_ID,"N");

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool deleteUserAppliFunction(decimal UAN_APPLIID, decimal UAN_FUN_ID, decimal UAN_USERID, string UAV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_DELETE_USERS_APPLIFUNCTIONS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, UAN_APPLIID, UAN_FUN_ID, UAN_USERID, UAV_CO);

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

        public static decimal GetIDFunctionByApp(decimal _AFN_APLIID) {
            object result = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_GETFUNCTION_APP";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _AFN_APLIID, result);

            DataSet data = db.ExecuteDataSet(dbCommandWrapper);

            // Retorna el primer Identificador de funcion que encuentre. 
            if (data.Tables[0].Rows.Count > 0)
                return Convert.ToDecimal(data.Tables[0].Rows[0][0]);
            else
                return -1;
            
        }
        #endregion


    }
}