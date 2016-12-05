using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Status
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Consultar estados por módulo
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_module"></param>
        /// <returns></returns>
        public static DataSet getStatusByModule(string _co, string _module)
        {
            // CURSOR REF
            object results = new object[1];
            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "control_aq.sp_getstatusbymodule";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _module, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static DataSet getStatusCedi(string _co)
        {
            // CURSOR REF
            object results = new object[1];
            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "control_aq.SP_GETSTATUS_CEDI";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
        #endregion
    }
}