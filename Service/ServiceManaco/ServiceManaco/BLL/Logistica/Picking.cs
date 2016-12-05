using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL
{
    public class Picking
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Adicionar registro de inicio de marcacion para una liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <param name="_idEmp"></param>
        /// <returns></returns>
        /*public static string addOrderToPicking(string _co, string _noLiq, string _idEmp)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "logistica.sp_add_picking";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, _idEmp, DBNull.Value);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                //
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }*/

        public static string addOrderToPicking(string _co, string _noLiq, string _idEmp)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.sp_add_picking";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_piv_co", DbType.String, _co);
            db.AddInParameter(dbCommandWrapper, "p_piv_liquidation", DbType.String, _noLiq);
            db.AddInParameter(dbCommandWrapper, "p_pin_employee", DbType.String, _idEmp);
            db.AddInParameter(dbCommandWrapper, "p_pid_end_date", DbType.String, DBNull.Value);
            // Output parameters specify the size of the return data.
            //db.AddOutParameter(dbCommandWrapper, "p_trv_document", DbType.String, 12);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    resultDoc = "1";//(string)db.GetParameterValue(dbCommandWrapper, "p_trv_document");
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

        /// <summary>
        /// Finalizar la marcacion de una liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <param name="_status"></param>
        /// <returns></returns>
        /*public static string finalizePicking(string _co, string _noLiq)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "logistica.sp_finalize_picking";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                //
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }            
        }*/

        public static string finalizePicking(string _co, string _noLiq)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.sp_finalize_picking";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_piv_co", DbType.String, _co);
            db.AddInParameter(dbCommandWrapper, "p_piv_liquidation", DbType.String, _noLiq);
            // Output parameters specify the size of the return data.
            //db.AddOutParameter(dbCommandWrapper, "p_trv_document", DbType.String, 12);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    resultDoc = "1";//(string)db.GetParameterValue(dbCommandWrapper, "p_trv_document");
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

        /// <summary>
        /// Consultar informacion de marcado de una liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getInfoLiqPicking(string _co, string _noLiq)
        {
            try
            {
                string sqlCommand = "logistica.sp_get_info_picking";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getDtlPicking(string _co, string _noLiq)
        {
            try
            {
                string sqlCommand = "logistica.sp_get_picking";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }


        public static DataSet getInfoPickingEmp(string _co, string _ware)
        {
            try
            {
                string sqlCommand = "logistica.sp_get_info_pickingEmpl";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}