using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Bata.Aquarella.BLL.Util;

namespace Bata.Aquarella.BLL.Logistica
{
    public class Production
    {

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;


        /// <summary>
        /// Consultar produccion del schema de lepalacio
        /// </summary>
        /// <param name="noTransfer"></param>
        /// <returns></returns>
        public static DataSet getHdrProductionInterface()
        {
            try
            {                
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "lepalacio.get_productionhdr";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }


        /// <summary>
        /// Consultar detalle de un traspaso de produccion en el schema de lepalacio
        /// </summary>
        /// <param name="noSeccion"></param>
        /// <returns></returns>
        public static DataSet getDtlProductionInterface(string noSeccion, string date)
        {
            try
            {
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "lepalacio.get_productiondtl";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, noSeccion, date, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Cargar produccion en las tablas de traspaso
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_seccion"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static String addProductionFromCoins(string _company, string _seccion, string _date)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.add_productionfromcoins";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_trv_co", DbType.String, _company);
            db.AddInParameter(dbCommandWrapper, "p_trv_docreferencia", DbType.String, _seccion);
            db.AddInParameter(dbCommandWrapper, "pfecha", DbType.String, _date);
            // Output parameters specify the size of the return data.
            db.AddOutParameter(dbCommandWrapper, "p_trv_document", DbType.String, 12);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    resultDoc = (string)db.GetParameterValue(dbCommandWrapper, "p_trv_document");
                    // Commit the transaction.
                    transaction.Commit();
                }
                catch
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc = "-1";
                }
                connection.Close();
            }
            return resultDoc;
        }

    }
}