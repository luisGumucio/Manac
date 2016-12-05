using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Coordinator
    {
        #region < Atributos >

        /// <summary>
        /// Compañia
        /// </summary>
        public string _co { get; set; }
        /// <summary>
        /// Identificador
        /// </summary>
        public decimal _idCust { get; set; }
        /// <summary>
        /// Comision
        /// </summary>
        public decimal _commission { get; set; }
        /// <summary>
        /// Bodega
        /// </summary>
        public string _idWare { get; set; }
        /// <summary>
        /// Porcenta. de impuesto
        /// </summary>
        public decimal _taxRate { get; set; }
        /// <summary>
        /// Tipo de cliente
        /// </summary>
        public string _typeCust { get; set; }
        /// <summary>
        /// Numero de documento
        /// </summary>
        public string _NoDocumento { get; set; }
        /// <summary>
        /// Nombre Completo
        /// </summary>
        public string _nombre { get; set; }
        /// <summary>
        /// Nombre storages
        /// </summary>
        public string _storages { get; set; }

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        public static decimal Obtener_Decuento_Coordinador(string _articulo, string _coordinador, string _warehouse)
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "VENTAS.SP_GET_PERCENTAGE";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _articulo,_coordinador,_warehouse, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["porcentaje"]);
            }
            catch { ret = 0; }

            return ret;
        }
        /// Consultar coordinadores
        /// </summary>
        public static DataSet getCoordinators(string _co, string _idWare, string _areaId)
        {
            try
            {
                string sqlCommand = "admonred.sp_getAllCoordinators";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,_idWare,_areaId, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static DataSet getCoordinatorsSimple(string _co, string _idWare, string _areaId)
        {
            try
            {
                string sqlCommand = "admonred.sp_getAllCoordinatorsSimple";
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idWare, _areaId, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static DataTable getCoordinatorsByLetters(string _co, string _idWare, string _regionId, string _Letters_tosearch) {
            try
            {
                string sqlCommand = "admonred.sp_getcoordinatorsbyletters";
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idWare, _regionId, _Letters_tosearch, results);
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar coordinador por id
        /// </summary>
        public static DataSet getCoordinatorByPk(string _co, decimal _idCoord)
        {
            try
            {
                string sqlCommand = "admonred.sp_getCoordinatorByPrimaryKey";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCoord, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consulta de pedidos en borrador, historial de liquidaciones y devoluciones
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idCust"></param>
        /// <returns>3 datasets 0-> Pedidos borrador, 1-> Liquidaciones, 2-> Devoluciones</returns>
        public static DataSet getOrdLiqAnsRet(string _co, decimal _idCust)
        {
            try
            {
                string sqlCommand = "logistica.sp_getorderscustomer";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCust, results, results, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Crear un nuevo cliente
        /// </summary>
        /// <param name="BDV_CO"></param>
        /// <param name="BDV_FIRST_NAME"></param>
        /// <param name="BDV_MIDDLE_NAME"></param>
        /// <param name="BDV_FIRST_SURNAME"></param>
        /// <param name="BDV_SECOND_SURNAME"></param>
        /// <param name="BDD_BIRTHDAY"></param>
        /// <param name="BDV_DOCUMENT_NO"></param>
        /// <param name="BDV_VERIF_DIGIT_NO"></param>
        /// <param name="BDV_DOCUMENT_TYPE_ID"></param>
        /// <param name="BDV_PERSON_TYPE_ID"></param>
        /// <param name="BDV_ADDRESS"></param>
        /// <param name="BDV_PHONE"></param>
        /// <param name="BDV_FAX"></param>
        /// <param name="BDV_MOVIL_PHONE"></param>
        /// <param name="BDV_EMAIL"></param>
        /// <param name="BDV_CITY_CD"></param>
        /// <param name="BDV_STATUS"></param>
        /// <param name="BDV_LANGUAGE_ID"></param>
        /// <param name="BDV_AREA_ID"></param>
        /// <param name="BDV_CREATE_USER"></param>
        /// <param name="BDV_UPDATE_USER"></param>
        /// <param name="BDV_SEX"></param>
        /// <param name="p_COV_COORDINATOR_TYPE"></param>
        /// <param name="p_CON_TERM_PAY_ID"></param>
        /// <param name="p_COV_DELIVERY_TEM_ID"></param>
        /// <param name="p_COV_CURRENCY_ID"></param>
        /// <param name="p_COV_WAREHOUSEID"></param>
        /// <param name="p_COV_BANK_ID"></param>
        /// <param name="p_COV_BANK_ACCOUNT_NO"></param>
        /// <param name="p_COV_APPROVAL_NAME"></param>
        /// <param name="p_COV_HANDLING_ID"></param>
        /// <param name="p_COV_CREDIT_FLAG"></param>
        /// <param name="p_CON_CREDIT_LIMIT"></param>
        /// <param name="p_COV_TAX_ID"></param>
        /// <param name="p_COV_AUTORETAINER_FLAG"></param>
        /// <param name="p_COV_GREAT_TAXPAYERS_FLAG"></param>
        /// <param name="p_con_perso_invoice"></param>
        /// <param name="con_regime"></param>
        /// <returns></returns>
        public static string addCoordinador
            (
                    string BDV_CO,
                    string BDV_FIRST_NAME,
                    string BDV_MIDDLE_NAME,
                    string BDV_FIRST_SURNAME,
                    string BDV_SECOND_SURNAME,
                    DateTime BDD_BIRTHDAY,
                    string BDV_DOCUMENT_NO,
                    string BDV_VERIF_DIGIT_NO,
                    string BDV_DOCUMENT_TYPE_ID,
                    string BDV_PERSON_TYPE_ID,
                    string BDV_ADDRESS,
                    string BDV_PHONE,
                    string BDV_FAX,
                    string BDV_MOVIL_PHONE,
                    string BDV_EMAIL,
                    string BDV_CITY_CD,
                    string BDV_STATUS,
                    string BDV_LANGUAGE_ID,
                    string BDV_AREA_ID,
                    string BDV_CREATE_USER,
                    string BDV_UPDATE_USER,
                    string BDV_SEX,
                    // Atributos de coordinador
                    string p_COV_COORDINATOR_TYPE,
                    string p_CON_TERM_PAY_ID,
                    string p_COV_DELIVERY_TEM_ID,
                    string p_COV_CURRENCY_ID,
                    string p_COV_WAREHOUSEID,
                    string p_COV_BANK_ID,
                    string p_COV_BANK_ACCOUNT_NO,
                    string p_COV_APPROVAL_NAME,
                    string p_COV_HANDLING_ID,
                    string p_COV_CREDIT_FLAG,
                    string p_CON_CREDIT_LIMIT,
                    string p_COV_TAX_ID,
                    string p_COV_AUTORETAINER_FLAG,
                    string p_COV_GREAT_TAXPAYERS_FLAG,
                    decimal p_con_perso_invoice,
                    string con_regime,
                    string BDV_LOCALITY,
                    string BDV_NEIGHBORHOOD
            )
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "maestros.sp_add_basic_data_v2";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "p_BDV_CO", DbType.String, BDV_CO);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_FIRST_NAME", DbType.String, BDV_FIRST_NAME);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_MIDDLE_NAME", DbType.String, BDV_MIDDLE_NAME);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_FIRST_SURNAME", DbType.String, BDV_FIRST_SURNAME);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_SECOND_SURNAME", DbType.String, BDV_SECOND_SURNAME);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDD_BIRTHDAY", DbType.DateTime, BDD_BIRTHDAY);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_DOCUMENT_NO", DbType.String, BDV_DOCUMENT_NO);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_VERIF_DIGIT_NO", DbType.String, BDV_VERIF_DIGIT_NO);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_DOCUMENT_TYPE_ID", DbType.String, BDV_DOCUMENT_TYPE_ID);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_PERSON_TYPE_ID", DbType.String, BDV_PERSON_TYPE_ID);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_ADDRESS", DbType.String, BDV_ADDRESS);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_PHONE", DbType.String, BDV_PHONE);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_FAX", DbType.String, BDV_FAX);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_MOVIL_PHONE", DbType.String, BDV_MOVIL_PHONE);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_EMAIL", DbType.String, BDV_EMAIL);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_CITY_CD", DbType.String, BDV_CITY_CD);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_STATUS", DbType.String, BDV_STATUS);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_LANGUAGE_ID", DbType.String, BDV_LANGUAGE_ID);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_AREA_ID", DbType.String, BDV_AREA_ID);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_CREATE_USER", DbType.String, BDV_CREATE_USER);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_UPDATE_USER", DbType.String, BDV_UPDATE_USER);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_SEX", DbType.String, BDV_SEX);
            ///

            db.AddInParameter(dbCommandWrapper, "p_BDV_LOCALITY", DbType.String, BDV_LOCALITY);
            ///
            db.AddInParameter(dbCommandWrapper, "p_BDV_NEIGHBORHOOD", DbType.String, BDV_NEIGHBORHOOD);

            /// Output parameters specify the size of the return data.            
            /// 
            db.AddOutParameter(dbCommandWrapper, "p_BDN_ID", DbType.Decimal, 12);

            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    ///
                    resultDoc = (String)Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_BDN_ID"));

                    /// Verificar si el coordinador desea que sea a el a quien se le facture
                    /// Si es igual a -2 quiere decir que el coordinador desea que sea a el a quien se le facture 
                    if (p_con_perso_invoice == -2)
                    {
                        ///
                        p_con_perso_invoice = Convert.ToDecimal(resultDoc);
                    }

                    sqlCommand = "admonred.SP_ADD_COORDINATOR";
                    dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                        BDV_CO,
                        resultDoc,
                        p_COV_COORDINATOR_TYPE,
                        p_CON_TERM_PAY_ID,
                        p_COV_DELIVERY_TEM_ID,
                        p_COV_CURRENCY_ID,
                        p_COV_WAREHOUSEID,
                        p_COV_BANK_ID,
                        p_COV_BANK_ACCOUNT_NO,
                        p_COV_APPROVAL_NAME,
                        p_COV_HANDLING_ID,
                        p_COV_CREDIT_FLAG,
                        p_CON_CREDIT_LIMIT,
                        p_COV_TAX_ID,
                        p_COV_AUTORETAINER_FLAG,
                        p_COV_GREAT_TAXPAYERS_FLAG,
                        p_con_perso_invoice, con_regime);
                    //
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);

                    // Commit the transaction.
                    transaction.Commit();
                    //
                    return resultDoc;
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        /// <summary>
        /// Actualizacion de coordinador
        /// </summary>
        public static string updateCoordinator(string _company, string _idPerson,
                    string p_COV_COORDINATOR_TYPE,
                    string p_COV_WAREHOUSEID,
                    string p_COV_HANDLING_ID,
                    string p_COV_TAX_ID,
                    decimal p_con_perso_invoice,
                    string p_con_regime)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "admonred.sp_updatecoordinator";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idPerson, p_COV_COORDINATOR_TYPE, p_COV_WAREHOUSEID,
                    p_COV_HANDLING_ID, p_COV_TAX_ID, p_con_perso_invoice, p_con_regime);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                //
                return "1";
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar atributos del coordinador
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idCoord"></param>
        /// <returns></returns>
        public static DataSet getCoordinator(string _co, decimal _idCoord)
        {
            try
            {
                string sqlCommand = "admonred.sp_getcoordinator";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCoord, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consulta de venta de un cliente, entre rangos de fechas y por cedula
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_document"></param>
        /// <param name="_dateStart"></param>
        /// <param name="_dateEnd"></param>
        /// <returns></returns>
        public static DataSet getSalesByCustomer(string _co, string _document, string _dateStart, string _dateEnd)
        {
            try
            {
                string sqlCommand = "ventas.sp_getsales_bycustomer";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _document,_dateStart, _dateEnd, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar informacion del cliente por documento
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_document"></param>
        /// <returns></returns>
        public static DataSet getCustomerByDoc(string _co, string _document)
        {
            try
            {
                string sqlCommand = "admonred.sp_getcoordinatorbydoc";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _document, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Actualizar campos especiales de cliente
        /// </summary>
        /// <param name="_co">Company</param>
        /// <param name="_customerId">Customer id for update</param>
        /// <param name="_type">New Type customer</param>
        /// <param name="_area">New Area</param>
        /// <param name="_ware">New Warehouse</param>
        /// <param name="_status">New Status</param>
        /// <returns></returns>
        public static string updateCoord(string _co, string _customerId, string _type, string _area, string _ware, string _status)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "admonred.sp_updatecoord";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _customerId, _type, _area, _ware, _status);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                //
                return "1";
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static decimal Obtener_Bono_Coordinador(string _coordinador, string _type)
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "ADMONRED.sp_get_account_coordinator";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,  _coordinador, _type, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["importe"]);
            }
            catch { ret = 0; }

            return ret;
        }

        public static decimal Obtener_Bono_Coordinador_v2(string _liquidation, string _type)
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "ADMONRED.sp_get_account_coordinator_v2";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _liquidation, _type, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["importe"]);
            }
            catch { ret = 0; }

            return ret;
        }

        public static string Verifica_Bonos_Utilizar(decimal _idCordinator, string _noLiq, decimal _bono, decimal _bonoPremio)
        {

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "LOGISTICA.SP_VERIFY_USE_BONDS";

                string resultDoc = "";

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

                // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
                db.AddInParameter(dbCommandWrapper, "p_user", DbType.Decimal, _idCordinator);
                ///
                db.AddInParameter(dbCommandWrapper, "p_liquidation", DbType.String, _noLiq);
                ///
                db.AddInParameter(dbCommandWrapper, "p_bono", DbType.Decimal, _bono);
                //
                db.AddInParameter(dbCommandWrapper, "p_bono_pre", DbType.Decimal, _bonoPremio);

                db.AddOutParameter(dbCommandWrapper, "p_result", DbType.String, 96);

                // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);

                        resultDoc = (String)db.GetParameterValue(dbCommandWrapper, "p_result");
                        // Commit the transaction.
                        transaction.Commit();
                        
                    }
                    catch (Exception e)
                    {
                        // Roll back the transaction. 
                        transaction.Rollback();
                        resultDoc = "La transacción no se completo correctamente.";
                        //throw new Exception(e.Message, e.InnerException);
                    }
                    connection.Close();
                }
                
                ///
                return resultDoc;

        }

        static public decimal Obtener_Porcentaje_Maximo()
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "ADMONRED.sp_get_max_percentage";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["rbn_amount"]);
            }
            catch { ret = 0; }

            return ret;
        }

        static public decimal Obtener_BonoPremio_Usado()
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "ADMONRED.sp_get_account_use";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["rbn_amount"]);
            }
            catch { ret = 0; }

            return ret;
        }

        static public decimal Obtener_Importe_Total_Pedido(string _nroOrders)
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "LOGISTICA.sp_get_importe_total";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _nroOrders, results);

            decimal ret;
            try
            {
                DataRow user = db.ExecuteDataSet(dbCommandWrapper).Tables[0].Rows[0];
                ret = Convert.ToDecimal(user["IMPORTE"]);
            }
            catch { ret = 0; }

            return ret;
        }

        static public DataSet Obtener_Movimiento_Bonos(string _coordi, string _fecha_ini,string _fecha_fin,string _tipo_bono)
        {
            try
            {
                string sqlCommand = "ADMONRED.sp_get_all_bond_move";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _coordi, _fecha_ini, _fecha_fin, _tipo_bono, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}