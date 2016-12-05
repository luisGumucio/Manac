using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Ventas
{
    public class Returns_Hdr
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        //private String RHV_CO;
        //private String RHV_RETURN_NO;
        //private DateTime RHD_DATE;
        //private String RHV_TRANSACTION;
        //private Decimal RHN_PERSON;
        //private Decimal RHN_COORDINATOR;
        //private Decimal RHN_EMPLOYEE;

        #endregion
        
        #region < Metodos estaticos >


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_statusid"></param>
        /// <param name="_areaid"></param>
        /// <param name="_warehouse"></param>
        /// <returns></returns>
        public static DataSet getReturnsHdrByStatus(string _co, string _statusid, string _warehouse)
        {
            try
            {
                string sqlCommand = "ventas.sp_get_returns_bystatus";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _statusid, _warehouse, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }



        /// <summary>
        /// Separar pedidos borrador y generar liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_ordersChain">Pedidos en borrador: 1,2 etc</param>
        /// <returns></returns>
        public static object[] approvalReturnsList(string _co, string _return_no, decimal _employee)
        {
            object[] result = new object[2];
            result[0] = false;
            result[1] = "";
            string sqlCommand = "ventas.sp_approval_returns_list";
            try
            {
                //
                Database db = DatabaseFactory.CreateDatabase(_conn);
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

                // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos 
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _return_no, _employee);
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        // Commit the transaction.
                        transaction.Commit();
                        result[0] = true;
                        result[1] = "";
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction. 
                        transaction.Rollback();
                        result[0] = false;
                        result[1] = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result[0] = false;
                result[1] = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="RHV_CO"></param>
        /// <param name="RHV_RETURN_NO"></param>
        /// <param name="RHD_DATE"></param>
        /// <param name="RHV_TRANSACTION"></param>
        /// <param name="RHN_PERSON"></param>
        /// <param name="RHN_COORDINATOR"></param>
        /// <param name="RHN_EMPLOYEE"></param>
        /// <param name="listArticlesReturned"></param>
        /// <returns></returns>
        public static String[] saveReturnOrder(string RHV_CO,
            string RHN_COORDINATOR, string RHN_EMPLOYEE, string STV_WAREHOUSE, List<Returns_Dtl> listArticlesReturned)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string storageDevol = "";
            string noTransaccion = "";

            /// Nombre del procedimiento
            string sqlCommand = "ventas.SP_ADD_RETURNSHDR";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "P_RHV_CO", DbType.String, RHV_CO);
            ///
            ///db.AddInParameter(dbCommandWrapper, "P_RHN_PERSON", DbType.Decimal, RHN_PERSON);
            ///
            db.AddInParameter(dbCommandWrapper, "P_RHN_COORDINATOR", DbType.String, RHN_COORDINATOR);
            ///
            db.AddInParameter(dbCommandWrapper, "P_RHN_EMPLOYEE", DbType.Decimal, RHN_EMPLOYEE);
            ///
            db.AddInParameter(dbCommandWrapper, "P_STV_WAREHOUSE", DbType.String, STV_WAREHOUSE);

            // Output parameters specify the size of the return data.            
            db.AddOutParameter(dbCommandWrapper, "P_RHV_RETURN_NO", DbType.String, 12);
            // Storage a donde se enviaron los articulos devueltos
            db.AddOutParameter(dbCommandWrapper, "P_STORAGE_DEVOL", DbType.String, 12);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);

                    /// Recuperar el parametro de salida
                    resultDoc = (String)db.GetParameterValue(dbCommandWrapper, "P_RHV_RETURN_NO");
                    storageDevol = (String)db.GetParameterValue(dbCommandWrapper, "P_STORAGE_DEVOL");

                    // Recorrer todas las lineas adicionadas al detalle
                    foreach (Returns_Dtl item in listArticlesReturned)
                    {
                        /// Procedimiento que adiciona las lineas de detalle
                        sqlCommand = "ventas.SP_ADD_RETURNSDTL_V2";

                        // Cambio para el manejo de conceptos en la devolucion
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                            RHV_CO,
                            resultDoc,
                            item._RDV_INVOICE,
                            item._RDV_ARTICLE,
                            item._RDV_SIZE,
                            item._RDN_QTY,
                            item._RDV_CONCEPT_ID);
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    }

                    ///////////////////////////////////////////////////////////////////////////////////////////
                    /// Procedimiento que adiciona las lineas de detalle
                    sqlCommand = "ventas.SP_ADD_DOCTRANS_RETURNS";
                    ///
                    dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                        RHV_CO,
                        resultDoc, 12);
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);///
                    /// Recuperar el parametro de salida
                    noTransaccion = (String)db.GetParameterValue(dbCommandWrapper, "P_DTV_TRANSDOC");
                    ///////////////////////////////////////////////////////////////////////////////////////////

                    // Commit the transaction.
                    transaction.Commit();

                    String[] results = new String[3];
                    results[0] = resultDoc;
                    results[1] = noTransaccion;
                    results[2] = storageDevol;

                    connection.Close();
                    return results;

                }
                catch
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc = "0";
                    connection.Close();
                    return null;
                }
            }
        }


        public static String[] addReturnOrder(String RHV_CO,
         String RHN_COORDINATOR, String RHN_EMPLOYEE, String STV_WAREHOUSE, List<Returns_Dtl> listArticlesReturned)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            String resultDoc = "";
            String storageDevol = "";
            String noTransaccion = "";

            /// Nombre del procedimiento
            String sqlCommand = "ventas.SP_ADD_RETURNSHDR_DEA";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "P_RHV_CO", DbType.String, RHV_CO);
            ///
            ///db.AddInParameter(dbCommandWrapper, "P_RHN_PERSON", DbType.Decimal, RHN_PERSON);
            ///
            db.AddInParameter(dbCommandWrapper, "P_RHN_COORDINATOR", DbType.String, RHN_COORDINATOR);
            ///
            db.AddInParameter(dbCommandWrapper, "P_RHN_EMPLOYEE", DbType.Decimal, RHN_EMPLOYEE);
            ///
            db.AddInParameter(dbCommandWrapper, "P_STV_WAREHOUSE", DbType.String, STV_WAREHOUSE);

            // Output parameters specify the size of the return data.            
            db.AddOutParameter(dbCommandWrapper, "P_RHV_RETURN_NO", DbType.String, 12);
            // Storage a donde se enviaron los articulos devueltos
            db.AddOutParameter(dbCommandWrapper, "P_STORAGE_DEVOL", DbType.String, 12);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);

                    /// Recuperar el parametro de salida
                    resultDoc = (String)db.GetParameterValue(dbCommandWrapper, "P_RHV_RETURN_NO");
                    storageDevol = (String)db.GetParameterValue(dbCommandWrapper, "P_STORAGE_DEVOL");

                    // Recorrer todas las lineas adicionadas al detalle
                    foreach (Returns_Dtl item in listArticlesReturned)
                    {
                        /// Procedimiento que adiciona las lineas de detalle
                        sqlCommand = "ventas.sp_add_returnsdtl_dea";

                        ///
                        if (item._RDV_STORAGE.Equals(""))
                        {
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                                RHV_CO,
                                resultDoc,
                                item._RDV_INVOICE,
                                item._RDV_ARTICLE,
                                item._RDV_SIZE,
                                item._RDN_QTY, storageDevol);
                        }
                        else
                        {
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                            RHV_CO,
                            resultDoc,
                            item._RDV_INVOICE,
                            item._RDV_ARTICLE,
                            item._RDV_SIZE,
                            item._RDN_QTY, item._RDV_STORAGE);
                        }
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    }

                    // Commit the transaction.
                    transaction.Commit();

                    String[] results = new String[3];
                    results[0] = resultDoc;
                    results[1] = noTransaccion;
                    results[2] = storageDevol;

                    connection.Close();
                    return results;

                }
                catch
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc = "0";
                    connection.Close();
                    return null;
                }
            }
        }

        /// <summary>
        /// Consulta de cabecera de devolucion
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noReturn"></param>
        /// <returns></returns>
        public static DataTable getRetunrHdr(string _company, string _noReturn)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_getreturnhdr";

                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noReturn, results);

                ///
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar cabecera de devolucion en espera de aprobacion
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noReturn"></param>
        /// <returns></returns>
        public static DataTable getRetunrHdrDea(string _company, string _noReturn)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_getreturnhdr_dea";

                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noReturn, results);

                ///
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_customer"></param>
        /// <returns></returns>
        public static DataTable getReturnsByCoord(String _company, String _customer)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.getreturnsbycoord";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _customer, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch
            {
                ///
                return null;
            }
        }

        /// <summary>
        /// Consulta de devoluciones por fecha
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_ware"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getReturnsByDate(string _co, string _startDate, string _endDate, string _ware, string _area)
        {
            //         
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "ventas.sp_get_returns_bydate ";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _startDate, _endDate, _ware, _area, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}