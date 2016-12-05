using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Regimes
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consultar todos los regimenes
        /// </summary>
        /// <param name="_co"></param>
        /// <returns></returns>
        public static DataSet getAllRegimes(string _co)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase();
                string sqlCommand = "maestros.sp_load_all_regimes";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }
}