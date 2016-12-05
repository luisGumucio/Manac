using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Payments
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Realizar el registro de un recaudo financiero
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_custId"></param>
        /// <param name="_bank"></param>
        /// <param name="_noConsig"></param>
        /// <param name="_datePay"></param>
        /// <param name="_amount"></param>
        /// <param name="_comm"></param>        
        /// <returns></returns>
        public static bool savePayment(string _co, decimal _custId, string _bank, string _noConsig, DateTime _datePay, decimal _amount,
            string _comm)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            bool result = false;
            string sqlCommand = "financiera.sp_add_payment";
            string _Payment_Id = string.Empty;
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,
                                _Payment_Id, _custId, _bank, _noConsig, _datePay, _amount, _comm);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    _Payment_Id = Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_pav_payment_id"));

                    // Commit the transaction.
                    transaction.Commit();

                    result = true;
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    connection.Close();
                    //throw new Exception(e.Message, e.InnerException);
                }
                connection.Close();
                return result;
            }
        }

        //Guardamos el pago realizado por Tigo Money
        public static string savePaymentTigo(string _cedula, decimal _importe, string _operacion)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "FINANCIERA.sp_add_payment_efecty";
            string _DocTrans_Id = string.Empty;
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _cedula, _importe, _operacion, _DocTrans_Id);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    _DocTrans_Id = Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_dtv_transdoc_id"));

                    // Commit the transaction.
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _DocTrans_Id = "-1";
                    // Roll back the transaction. 
                    transaction.Rollback();
                    connection.Close();
                    //throw new Exception(e.Message, e.InnerException);
                }
                connection.Close();
                return _DocTrans_Id;
            }
        }



        /// <summary>
        /// Consultar pagos por estado, bodega y area
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_status"></param>
        /// <param name="_idWare"></param>
        /// <returns></returns>
        public static DataSet loadPaymentsByWarehouseAndStatus(string _co, string _status, string _idWare, string _idRegion)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                string sqlCommand = "financiera.sp_getpaystatandwarehouse";

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _status, _idWare, _idRegion, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                DataSet datos = db.ExecuteDataSet(dbCommandWrapper);

                return datos;

            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Actualizar estado de un pago
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_paymentId"></param>
        /// <param name="_status"></param>
        /// <returns></returns>
        public static bool updatePayment(string _co, string _paymentId, string _status)
        {
            bool result = false;
            string sqlCommand = "financiera.sp_update_payments";

            Database db = DatabaseFactory.CreateDatabase(_conn);
            
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,
                 _paymentId, _status);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);

                    // Commit the transaction.
                    transaction.Commit();

                    result = true;
                }
                catch (Exception)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                }
                connection.Close();
                return result;
            }
        }
        
        /// <summary>
        /// Consultar el listado de pagos de un cliente
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idCust"></param>
        /// <returns></returns>
        static public DataSet getPaymentsByCoordinator(string _co, int _idCust)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.SP_LOADPAYMENTS_X_COORDINATOR";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCust, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        static public string GetTypeChange()
        {
            try
            {
                string _valor = "";
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "FINANCIERA.SP_GET_TYPECHANGE";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);         
                /// 
                db.AddOutParameter(dbCommandWrapper, "p_typechange", DbType.String, 12);

                db.ExecuteNonQuery(dbCommandWrapper);
                ///
                _valor = (String)db.GetParameterValue(dbCommandWrapper, "p_typechange");
                ///
                return _valor;
            }
            catch (Exception e)
            {
                return "N";
            }
        }

        static public DataSet getPayments(string _co, string _idPayment)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.SP_GETPAYMENT";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idPayment, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion

    }
}