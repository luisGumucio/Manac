using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Taxes
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consultar todos impuestos
        /// </summary>
        /// <param name="_co"></param>
        /// <returns></returns>
        public static DataSet getAllTaxes()
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "maestros.sp_loadall_taxes";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }
}