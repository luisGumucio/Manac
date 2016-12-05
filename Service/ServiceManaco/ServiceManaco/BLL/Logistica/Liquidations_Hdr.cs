using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Bata.Aquarella.BLL
{
    public class Liquidations_Hdr
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Separar pedidos borrador y generar liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_ordersChain">Pedidos en borrador: 1,2 etc</param>
        /// <returns></returns>
        public static string[] createLiquidation(string _co, string _ordersChain, Transporters_Guides shipping, string _newStatus)
        {
            string[] resultDoc = new string[2];
            string sqlCommand = "logistica.sp_reserve_stock_order_list";
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
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ordersChain);
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);


                        if (!string.IsNullOrEmpty(_newStatus))
                        {
                            sqlCommand = "logistica.sp_liquidation_for_hall";//"logistica.sp_updateStatusLiquidation";
                            //
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ordersChain, _newStatus, 12);
                            db.ExecuteNonQuery(dbCommandWrapper, transaction);
                            //
                            resultDoc[0] = db.GetParameterValue(dbCommandWrapper, "p_lhv_liquidation_no").ToString();
                        }
                        else
                        {
                            // Crear Liquidacion
                            sqlCommand = "logistica.sp_liquidation";

                            //
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ordersChain, 12);
                            db.ExecuteNonQuery(dbCommandWrapper, transaction);
                            //
                            resultDoc[0] = db.GetParameterValue(dbCommandWrapper, "p_lhv_liquidation_no").ToString();
                        }

                        /// Quitar cuando el sistema funcione con los cambios en la tabla transporters_guides
                        if (shipping._configShipping)
                        {
                            /* Solo para cuando este activo la nueva creacion de guias*/
                            // Registro info destinatario
                            sqlCommand = "logistica.sp_addguide_shipping";
                            //
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, 12, DBNull.Value, shipping._tgn_transport, shipping._tgv_name_cust, shipping._tgv_phone_cust,
                                shipping._tgv_movil_cust, shipping._tgv_shipp_add, shipping._tgv_shipp_block, shipping._tgv_city, shipping._tgv_depto, resultDoc[0]);
                            db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        }
                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction. 
                        transaction.Rollback();
                        resultDoc[0] = "-1";
                        resultDoc[1] = ex.Message;//"-1";
                        //return ex.Message;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                resultDoc[0] = "-1";
                resultDoc[1] = ex.Message;;
            }
            return resultDoc;
        }

        /// <summary>
        /// Consulta de liquidacion e informacion adicional
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getLiquidationHdrInfo(string _co, string _noLiq)
        {
            try
            {
                string sqlCommand = "logistica.sp_getliquidationhdrinfo";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,_noLiq, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static DataSet getLiquidationForEfecty(string _co, string _noLiq)
        {
            try
            {
                string sqlCommand = "LOGISTICA.sp_getliquidation_efecty";
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
        /// Consulta de liquidacion pra pagos Online
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idLiq"></param>
        /// <returns></returns>
        public static DataTable getInfoLiquiForPaysOnline(string _co, string _idLiq)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_get_liquidation_info";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idLiq, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Actualizar estado de la cabecera de liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiquidation"></param>
        /// <param name="_newStatus"></param>
        /// <returns></returns>
        public static string updateStatusLiquidation(string _co, string _noLiquidation, string _newStatus)
        {
            try
            {
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.sp_updateStatusLiquidation";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _newStatus, _noLiquidation);                                
                ///
                db.ExecuteNonQuery(dbCommandWrapper);
                ///
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Consulta de liquidaciones para marcar
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_ware"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getLiquidationPicking(string _co, string _ware,string _area)
        {
            try
            {
                string sqlCommand = "logistica.sp_getliquid_picking";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _area, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar liquidaciones en estado activo en bodega, osea liquidaciones separadas, en marcacion o para facturacion.
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_status"></param>
        /// <param name="_ware"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getLiqActives(string _co, string _ware, string _area)
        {
            try
            {
                string sqlCommand = "logistica.sp_getliquidActives";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _area, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Liquidaciones con finalizacion de marcacion y para entregar en hall
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_ware"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getLiquidationForHall(string _co, string _ware, string _area)
        {
            try
            {
                string sqlCommand = "logistica.sp_getliquid_forhall";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _area, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /*
        public static void paralellism()
        {
            // Use an event to wait for the children
            using (var mre = new ManualResetEvent(false))
            {
                int count = 2;

                // Process the left child asynchronously
                ThreadPool.QueueUserWorkItem(delegate
                {
                    //Process(tree.Left, action);
                    insert(count.ToString() + " task1 Async" + DateTime.Now.ToLongTimeString());                    
                    if (Interlocked.Decrement(ref count) == 0)
                        mre.Set();
                });                
                // Process the right child asynchronously
                ThreadPool.QueueUserWorkItem(delegate
                {
                    //Process(tree.Right, action);
                    insert(count.ToString() + " task2 Async" + DateTime.Now.ToLongTimeString());
                    if (Interlocked.Decrement(ref count) == 0)
                        mre.Set();
                });

                // Process the current node synchronously
                //action(tree.Data);

                // Wait for the children
                mre.WaitOne();
            }

            insert("task1 Sync" + DateTime.Now.ToLongTimeString());
            insert("task2 Sync" + DateTime.Now.ToLongTimeString());

        }


        public static void insert(string i)
        {            
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_insert_curso";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, i);
                db.ExecuteNonQuery(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
        */


        /// <summary>
        /// Consulta de liquidaciones separadas
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idWarehouse"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getSeparateLiquidations(string _company, string _idWarehouse, string _area)
        {            
            try
            {
                // CURSOR REF
                object results = new object[1];
                string _status = Util.ValuesDB.acronymStatusOrdersSeparated;
                Database db = DatabaseFactory.CreateDatabase(Liquidations_Hdr._conn);
                string sqlCommand = "logistica.sp_getliquidationWithQtyNoLiq";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _status, _idWarehouse, _area, results);
                return db.ExecuteDataSet(dbCommandWrapper);                
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar estado de pedidos, por liquidacion o numero de guia
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_liqGuide"></param>
        /// <returns></returns>
        public static DataSet getLiquidations(string _co, string _liqGuide)
        {
            try
            {
                object obj = (object)new object[1];
                Database database = DatabaseFactory.CreateDatabase(Liquidations_Hdr._conn);
                string str2 = "logistica.sp_get_liquidations";
                DbCommand storedProcCommand = database.GetStoredProcCommand(str2, _co,_liqGuide,obj);
                return database.ExecuteDataSet(storedProcCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string updateExpirationDateOnLiq(string _company, string _noLiquidation, decimal _idUser, int _typeOperation)
        {
            try
            {
                Database database = DatabaseFactory.CreateDatabase(Liquidations_Hdr._conn);
                string str = "logistica.sp_update_expirationdateliq";
                DbCommand storedProcCommand = database.GetStoredProcCommand(str, _company, _noLiquidation, _idUser, _typeOperation);
                database.ExecuteNonQuery(storedProcCommand);
                return "1";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public static DataSet get_Liquidations_separated(string _company, string _search)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                string _status = Util.ValuesDB.acronymStatusOrdersSeparated;
                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "logistica.sp_liquidations_separated";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _search, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Cambio de estado de una liquidacion, para recoleccion presencial
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noLiquidation"></param>
        /// <param name="_newStatus"></param>
        /// <returns></returns>
        public static string updateStatusLiquidationInCedi(string _company, string _noLiquidation, string _newStatus)
        {
            try
            {
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.sp_updatestatusliqui_inCEDI";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noLiquidation, _newStatus);
                ///
                db.ExecuteNonQuery(dbCommandWrapper);
                ///
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static DataSet get_liquidations_Hall(String _company, DateTime _startDate, DateTime _endDate)
        {
            ///
            DataSet dsResult = new DataSet();
            try
            {
                // CURSOR REF
                object results = new object[1];
                object results1 = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_get_liquidations_hall";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, results, results1);
                ///
                dsResult = db.ExecuteDataSet(dbCommandWrapper);
                ///
                return dsResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static string cancel_Liquidation_Hall(String _company, String _noLiquidation)
        {
            try
            {
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.sp_cancel_liquidation_inhall";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noLiquidation);
                ///
                db.ExecuteNonQuery(dbCommandWrapper);
                ///
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Crear una liquidacion Nueva version octubre 2012
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_ordersChain"></param>
        /// <param name="shipping"></param>
        /// <param name="_newStatus"></param>
        /// <returns></returns>
        public static string[] liquidation(string _co, string _ordersChain, Transporters_Guides shipping, string _newStatus, decimal _bondWeek, decimal _bondMonth)
        {
            string[] resultDoc = new string[2];
            string sqlCommand = "logistica.sp_createliquidation";
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
                        /*PARAMETERS*/
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ordersChain, DBNull.Value, _newStatus, _bondWeek, _bondMonth);
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);

                        resultDoc[0] = db.GetParameterValue(dbCommandWrapper, "p_lhv_liquidation_no").ToString();

                        //** Quitar cuando el sistema funcione con los cambios en la tabla transporters_guides
                        if (shipping._configShipping)
                        {
                            /* Solo para cuando este activo la nueva creacion de guias*/
                            // Registro info destinatario
                            sqlCommand = "logistica.sp_addguide_shipping";
                            //
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, 12, DBNull.Value, shipping._tgn_transport, shipping._tgv_name_cust, shipping._tgv_phone_cust,
                                shipping._tgv_movil_cust, shipping._tgv_shipp_add, shipping._tgv_shipp_block, shipping._tgv_city, shipping._tgv_depto, shipping._adjuntDocInvoice, resultDoc[0]);
                            db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        }
                        // Commit the transaction.
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction. 
                        transaction.Rollback();
                        resultDoc[0] = "-1";
                        resultDoc[1] = ex.Message;//"-1";
                        //return ex.Message;
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                resultDoc[0] = "-1";
                resultDoc[1] = ex.Message; ;
            }
            return resultDoc;
        }

        #endregion
    }        
}