using System;
using System.Data.Common;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Logistica
{
    public abstract class Transaction
    {

        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion


        #region < VARIABLES >
        protected string _trv_co = "";
        protected string _trv_document = "";
        protected DateTime _trd_date = DateTime.Now;
        protected string _trv_storage = "";
        protected string _trv_concept = "";
        protected string _trv_docreferencia = "";
        protected string _trv_status = "";
        protected int _totalCantidad = 0;
        protected decimal _totalODV = 0;
        protected decimal _totalPrecioPublico = 0;
        protected List<Transaction_det> _items = new List<Transaction_det>();
        #endregion

        #region < PROPIEDADES >
        public string trv_co
        {
            get { return _trv_co; }
            set { _trv_co = value; }
        }

        public string trv_document
        {
            get { return _trv_document; }
        }

        public DateTime trd_date
        {
            get { return _trd_date; }
        }

        public string trv_storage
        {
            get { return _trv_storage; }
            set { _trv_storage = value; }
        }

        public string trv_concept
        {
            get { return _trv_concept; }
            set { _trv_concept = value; }
        }

        public string trv_docreferencia
        {
            get { return _trv_docreferencia; }
            set { _trv_docreferencia = value; }
        }

        public string trv_Status
        {
            get { return _trv_status; }
            set { _trv_status = value; }
        }

        public decimal totalCantidad
        {
            get
            {
                int sum = 0;
                foreach (Transaction_det item in _items)
                    sum += item.tdn_Qty;
                _totalCantidad = sum;
                return _totalCantidad;
            }
        }

        public decimal totalODV
        {
            get
            {
                decimal sum = 0.0m;
                foreach (Transaction_det item in _items)
                    sum += item.tdn_Qty * item.tdn_Odv;
                _totalODV = sum;
                return _totalODV;
            }
        }

        public decimal totalPrecioPublico
        {
            get
            {
                decimal sum = 0.0m;
                foreach (Transaction_det item in _items)
                    sum += item.tdn_Qty * item.tdn_Public_Price;
                _totalPrecioPublico = sum;
                return _totalPrecioPublico;
            }
        }

        public List<Transaction_det> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        #endregion

        public Transaction_det Transaction_det
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region < CONSTRUCTORES >

        #endregion

        #region < METODOS >
        public abstract string Save();

        public abstract bool Anull();


        public void InsertItem(int tdn_line_no, string tdv_article, string tdv_size, int tdn_qty, decimal tdn_odv, decimal tdn_public_price)
        {
            Transaction_det item = new Transaction_det(tdn_line_no, tdv_article, tdv_size, tdn_qty, tdn_odv, tdn_public_price);

            if (this._items.Count >= tdn_line_no)
            {
                this._items[tdn_line_no - 1].tdv_Article = tdv_article;
                this._items[tdn_line_no - 1].tdv_Size = tdv_size;
                this._items[tdn_line_no - 1].tdn_Qty = tdn_qty;
                this._items[tdn_line_no - 1].tdn_Odv = tdn_odv;
                this._items[tdn_line_no - 1].tdn_Public_Price = tdn_public_price;
            }
            else
                this._items.Add(item);
        }
        #endregion

        #region < METODOS ESTATICOS >


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noDucument"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_concept"></param>
        /// <param name="_idWarehouse"></param>
        /// <returns></returns>
        public static DataTable searchTransactionsDocs(String _company, String _noDucument,
            String _startDate, String _endDate, String _concept, String _idWarehouse)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "LOGISTICA.sp_searchTransactionsDocs";
                ///       
                /// Verificar los filtros de búsqueda  de documentos
                ///

                ///
                String _sentenceNoDocument = _noDucument != null && !_noDucument.Equals(String.Empty) ? " and upper(trv_document) like (upper('" + _noDucument + "'))" : "";
                ///
                String _sentenceConcept = !_concept.Equals("-1") && _concept != null ? " and  TRV_CONCEPT = '" + _concept + "' " : "";
                /// 
                //String _sentenceIdWarehouse = !_idWarehouse.Equals("-1") && _idWarehouse != null ? "and trV_storage in (SELECT STV_STORAGESID FROM LOGISTICA.STORAGES where STV_CO = '" + _company + "' and STV_WAREHOUSE = '" + _idWarehouse + "' ')" : "";

                String _sentenceIdWarehouse = !_idWarehouse.Equals("-1") && _idWarehouse != null ? "and STV_WAREHOUSE = '" + _idWarehouse + "'" : "";
                ///
                String _sentenceFecha = "";

                /// Verificar si la fecha inicial no esta vacia
                if (_startDate != null && !_startDate.Equals(String.Empty))
                {
                    /// Verificar si la fecha final no esta vacia
                    /// 
                    if (_endDate != null && !_endDate.Equals(String.Empty))
                    {
                        /// Armar consulta entre las fechas dadas
                        _sentenceFecha = " AND TRD_DATE  BETWEEN TO_DATE ('" + _startDate + "',  " +
                                         "          'dd/mm/yyyy'         " +
                                         "         )                                " +
                                         "  AND TO_DATE (concat('" + _endDate + "','23:59:00'),        " +
                                         "          'dd/mm/yyyy hh24:mi: ss'         " +
                                         "         )";
                    }
                    else
                    {
                        /// Armar consulta para los ducumentos mayores a la fecha de inicio
                        /// 
                        _sentenceFecha = " AND TRD_DATE  >= TO_DATE ('" + _startDate + "')";
                    }
                }
                /// Verificar si la fecha final no esta vacia
                else if (_endDate != null && !_endDate.Equals(String.Empty))
                {
                    /// Armar consulta para los documentos menores a la fecha final
                    /// 
                    _sentenceFecha = " AND TRD_DATE  <= TO_DATE (concat('" + _endDate + "','23:59:00'),        " +
                                         "          'dd/mm/yyyy hh24:mi: ss'         " +
                                         "         )";
                }

                /// 
                /// Armar la sentencia WHERE
                /// 
                String sqlWhereCommand = "   TRV_CO = '" + _company + "' " +
                              _sentenceNoDocument + _sentenceFecha + _sentenceConcept + _sentenceIdWarehouse;

                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, sqlWhereCommand, results);
                ///
                ///DbCommand dbCommandWrapper = db.GetSqlStringCommand(sqlCommand);
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception e)
            {
                Console.Write("Error : " + e.ToString());
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_warehouse"></param>
        /// <param name="_fecStart"></param>
        /// <param name="_fecEnd"></param>
        /// <returns></returns>
        public static DataTable getArticlesTransferToCatalog(String _company, DateTime _fecStart, DateTime _fecEnd)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_getartstransfertocatalog";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _fecStart, _fecEnd, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_seccion"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public static String addTransferFromCoins(String _company, String _docReference)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.add_transferfromcoins";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_trv_co", DbType.String, _company);
            db.AddInParameter(dbCommandWrapper, "p_trv_docreferencia", DbType.String, _docReference);
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

        public static DataTable getArticlesWidthLinkCoins(String _co, String _idInv)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_get_widths_major";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idInv, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }


        public static String loadStockForInventRetail(String _co, String _ware)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_load_stock_inv_retail";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware);
                ///
                return db.ExecuteNonQuery(dbCommandWrapper).ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Adicionar un traspaso a la tabla de ajuste del schema de lepalacio
        /// </summary>
        /// <param name="lstTxDt"></param>
        public static void addAjuste(List<Transaction_det> lstTxDt)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);            
            string sqlCommand = "lepalacio.sp_add_ajuste";

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (var item in lstTxDt)
                    {
                        DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
                        db.AddInParameter(dbCommandWrapper, "p_ajuste_no", DbType.String, item._idTx);
                        db.AddInParameter(dbCommandWrapper, "p_item_no", DbType.String, item._item);
                        db.AddInParameter(dbCommandWrapper, "p_item_desc", DbType.String, item._article);
                        db.AddInParameter(dbCommandWrapper, "p_brand_cd", DbType.String, item._brand);
                        db.AddInParameter(dbCommandWrapper, "p_size_display", DbType.String, item._size);
                        db.AddInParameter(dbCommandWrapper, "p_pares", DbType.Decimal, item._units);
                        db.AddInParameter(dbCommandWrapper, "p_line_odv_unit", DbType.Decimal, item.tdn_Odv);
                        db.AddInParameter(dbCommandWrapper, "p_standard_price", DbType.Decimal, item.tdn_Public_Price);
                        db.AddInParameter(dbCommandWrapper, "p_fecha", DbType.Date, item._dateTx);
                        db.AddInParameter(dbCommandWrapper, "p_codigo_interno", DbType.String, string.Empty);
                        db.AddInParameter(dbCommandWrapper, "p_referencia", DbType.String, string.Empty);
                        db.AddInParameter(dbCommandWrapper, "p_bodega_coins", DbType.String, item._wareOrig);

                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    }
                    // Commit the transaction.
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    connection.Close();
                    throw new Exception(e.Message, e.InnerException);
                }
                connection.Close();
            }            
        }

        public static DataSet getCustomerClaims(string co, DateTime fecStart, DateTime fecEnd)
        {   
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_customer_claims";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, co, fecStart, fecEnd, results, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region < METODOS PRIVADOS >

        #endregion
    }
}