using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Logistica
{
    public class WhTransfer
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Consultar en el schema de lepalacio los traspasos aun sin cargar
        /// </summary>
        /// <returns></returns>
        public static DataSet getTransfersToCatalog()
        {
            try
            {
                object results = new object[1];
                //
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "lepalacio.get_transferstocatalog";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar ajuste del schema de lepalacio
        /// </summary>
        /// <param name="noTransfer"></param>
        /// <returns></returns>
        public static DataSet getAjuste(string noTransfer)
        {
            try
            {
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "lepalacio.get_Ajuste";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, noTransfer, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}