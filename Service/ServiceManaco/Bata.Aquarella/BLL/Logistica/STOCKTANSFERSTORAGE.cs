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
    public class STOCKTANSFERSTORAGE : Transaction
    {

        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conection = Constants.OrcleStringConn;
       
        #endregion

        #region < VARIABLES >
        string _trv_storage_in;
        string _trv_storage_out;
        /// <summary>
        /// Para IP de la maquina que utiliza la clase
        /// </summary>
        //string _MACHINE;
        /// <summary>
        /// Para Usuario autenticado en el sistema que usa la clase.
        /// </summary>
        //decimal _USER;
        #endregion

        #region < CONSTRUCTORES >

        public STOCKTANSFERSTORAGE()
        {
            //Variables de la SuperClase.
            _trv_co = string.Empty;
            _trv_document = string.Empty;
            _trd_date = DateTime.Now;
            _trv_storage_in = string.Empty;
            _trv_storage_out = string.Empty;
            _trv_concept = string.Empty;
            _trv_docreferencia = string.Empty;
            _trv_status = string.Empty;
        }

        public STOCKTANSFERSTORAGE(string TRV_CO, string TRV_DOCUMENT, DateTime TRD_DATE,
            String TRV_STORAGE_IN, String TRV_STORAGE_OUT, string TRV_CONCEPT, string TRV_DOCREFERENCE, String TRV_STATUS,
            List<Transaction_det> items)
            : this()
        {
            //Variables de la SuperClase.
            _trv_co = TRV_CO;
            _trv_document = TRV_DOCUMENT;
            _trd_date = TRD_DATE;
            _trv_storage_in = TRV_STORAGE_IN;
            _trv_storage_out = TRV_STORAGE_OUT;
            _trv_concept = TRV_CONCEPT;
            _trv_docreferencia = TRV_DOCREFERENCE;
            _trv_status = TRV_STATUS;
            _items = items;
        }

        #endregion

        #region < PROPIEDADES >

        string STORAGE_IN
        {
            get { return _trv_storage_in; }
            set { _trv_storage_in = value; }
        }

        string STORAGE_OUT
        {
            get { return _trv_storage_out; }
            set { _trv_storage_out = value; }
        }

        #endregion

        #region < METODOS PUBLICOS >

        /// <summary>
        /// Debe estar porque es un metodo abstracto. Permite gusrada un intercabio entre estorage. 
        /// </summary>
        /// <returns>Los docuentos separados por linea. ejemp:(doc1-doc2).</returns>
        public override string Save()
        {
            ///////////////////////
            //Guardar en el log
            ///////////////////////            
            ////
            string result = SaveTransfer(_trv_co, STORAGE_OUT, STORAGE_IN, _trv_docreferencia, _trv_status, _items).Trim();
            //
            return result;
        }
        /// <summary>
        /// Permite anular una tranferencia. no implementado. Ya que se debe colocar porqeu es abstracto en la super clase. 
        /// </summary>
        /// <returns></returns>
        public override bool Anull()
        {
            ///////////////////////
            //Guardar en el log
            ///////////////////////
            return STOCKTANSFERSTORAGE.AnullTransfer(this.trv_co, this.trv_document);           
        }

        #endregion

        #region < METODOS PRIVADOS >

        /// <summary>
        /// Utility function that returns a DBNull.Value if requested. The comparison is done inline
        /// in the Insert() and Update() functions.
        /// </summary>
        private object SetNullValue(bool isNullValue, object value)
        {
            if (isNullValue)
                return DBNull.Value;
            else
                return value;
        }

        #endregion

        #region < METODOS ESTATICOS >

        /// <summary>
        /// Guarda un el traspaso con los parametros especificados en la base de datos.
        /// </summary>
        /// <returns>Cadena con los numeros de los dos documentos generados separados por raya(doc1-doc2).</returns>
        static public string SaveTransfer(string TRV_CO, String TRV_STORAGE_OUT, String TRV_STORAGE_IN,
            string TRV_DOCREFERENCE, String TRV_STATUS, List<Transaction_det> items)
        {
            ///
            string result = exchangeStorage.saveDocExchangeStorage(TRV_CO, TRV_STORAGE_OUT,
                TRV_STORAGE_IN, TRV_DOCREFERENCE, TRV_STATUS, items).Trim();
            ///
            return result;
        }
        /// <summary>
        /// Not implement method.
        /// </summary>
        /// <param name="trv_co"></param>
        /// <param name="trv_document"></param>
        /// <returns>False forever</returns>
        public static bool AnullTransfer(string trv_co, string trv_document)
        {
            /////
            return false;
        }
        /// <summary>
        /// Método utilizado para retornar los DETALLES de un documento.
        /// </summary>
        /// <param name="tvr_co"></param>
        /// <param name="doc_production"></param>
        /// <returns></returns>
        static public DataSet GetDetailsTransfer(String tdv_co, String tdv_docTransfer)
        {
            ///
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conection);
            ///
            string sqlCommand = "SP_GET_DETAILSTRANFERXDOC";
            ///
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, tdv_co, tdv_docTransfer, results);
            ///
            DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
            ///
            return ret;
        }
        /// <summary>
        /// Método utilizado para obtener la talla de un articulo y la cantidad existente, para una 
        /// linea de DETALLE de traspaso.
        /// </summary>
        /// <param name="tvr_art"></param>
        /// <param name="doc_production"></param>
        /// <returns></returns>
        static public DataSet GetArtSizesQtyFromDetTransferStorage(String asv_co, String asv_article, String sov_storage)
        {
            ///
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conection);
            ///
            string sqlCommand = "SP_GETARTSIZQTY_STOCKTRANSFER";
            ///
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, asv_co, asv_article, sov_storage, results);
            ///
            DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
            ///
            return ret;
        }
        /// <summary>
        /// Hace una busqueda con los parametos que llegan.<br />
        /// Son oblegatorios company, warehouse y storage, los demas parametros pueden llegar vacios ("").
        /// </summary>
        /// <param name="company"></param>
        /// <param name="wareHouse"></param>
        /// <param name="storage_id"></param>
        /// <param name="brand_id"></param>
        /// <param name="majorCategory_id"></param>
        /// <param name="category_id"></param>
        /// <param name="subCategory_id"></param>
        /// <param name="origin_id"></param>
        /// <param name="type_id"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public List<stockStorage> searchStock(String company, String wareHouse, String storage_id, String brand_id, String majorCategory_id, string category_id,
            String subCategory_id, String origin_id, String type_id, string article_id, String _supplierId)
        {
            ///
            String _article = article_id.Equals(string.Empty) ? "%%" : article_id;
            ///
            String _brand = !brand_id.Equals(string.Empty) ? " AND UPPER(ARV_BRAND_ID) = UPPER('" + brand_id + "')" : "";
            ///
            String _majorCategory = !majorCategory_id.Equals(string.Empty) ? " AND UPPER(ARV_MAJOR_CATEGORY) = UPPER('" + majorCategory_id + "')" : "";
            ///
            String _category = !category_id.Equals(string.Empty) ? " AND UPPER(ARV_CATEGORY) = UPPER('" + category_id + "')" : "";
            ///
            String _subCategory = !subCategory_id.Equals(string.Empty) ? " AND UPPER(ARV_SUB_CATEGORY) = UPPER('" + subCategory_id + "')" : "";
            ///
            String _origin = !origin_id.Equals(string.Empty) ? " AND UPPER(ARV_ORIGIN) = UPPER('" + origin_id + "')" : "";
            ///
            String _type = !type_id.Equals(string.Empty) ? " AND UPPER(ARV_TYPE) = UPPER('" + type_id + "')" : "";
            ///            
            /// Linea adicionada para habilitar la busqueda por un proveedor
            String _supplier = !_supplierId.Equals("-1") ? " AND ASV_SUPPLIERS = '" + _supplierId + "' and UPPER(ASV_CO) = UPPER(ARV_CO) AND UPPER(ASV_ARTICLE) = UPPER(SOV_ARTICLE)" : "";
            ///
            ///
            String sentence = " ARV_ARTICLE LIKE '" + _article + "'" +
            " AND UPPER(ARV_CO) = UPPER('" + company + "')" +
            " AND UPPER(SOV_WAREHOUSE) = UPPER('" + wareHouse + "')" +
            " AND UPPER(SOV_STORAGE) = UPPER('" + storage_id + "')" +
            _brand + _majorCategory + _category + _subCategory + _origin + _type + _supplier;
            ///
            object results = new object[1];
            object results1 = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conection);
            ///
            string sqlCommand = "LOGISTICA.sp_search_stocktransferAll";
            ///
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, sentence, results, results1);
            ///
            DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
            ///

            List<stockStorage> _stockstorageList = new List<stockStorage>();
            
            if (ret != null)
            {
                if (ret.Tables.Count > 0)
                {
                    foreach (DataRow row in ret.Tables[0].Rows)
                    {
                        stockStorage stockstor = new stockStorage();
                        stockstor._arv_article = row["ARV_ARTICLE"].ToString();
                        stockstor._arv_name = row["ARV_NAME"].ToString();
                        stockstor._odn_odv = decimal.Parse(row["ODN_ODV"].ToString());
                        stockstor._public_price = decimal.Parse(row["PRN_PUBLIC_PRICE"].ToString());
                        stockstor._qty = int.Parse(row["SON_QTY"].ToString());
                        stockstor._size = row["SOV_SIZE"].ToString();
                        stockstor._qtyExists = int.Parse(row["SON_QTY"].ToString());
                        _stockstorageList.Add(stockstor);                        
                    }

                    foreach (DataRow row1 in ret.Tables[1].Rows)
                    {                       
                            stockStorage stockstor1 = new stockStorage();
                            stockstor1._arv_article = row1["asv_article"].ToString();
                            stockstor1._arv_name = row1["ARV_NAME"].ToString();
                            stockstor1._odn_odv = decimal.Parse(row1["ODN_ODV"].ToString());
                            stockstor1._public_price = decimal.Parse(row1["PRN_PUBLIC_PRICE"].ToString());
                            stockstor1._qty = 0;
                            stockstor1._qtyExists = 0;
                            stockstor1._size = row1["asv_size_display"].ToString();
                            _stockstorageList.Add(stockstor1);                      
                    }
                }
            }
         
            return _stockstorageList;
        }

        public static DataSet checkTransit(String _company, String _typeStorage)
        {
            try
            {
                ///
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conection);
                ///
                string sqlCommand = " LOGISTICA.sp_checktransit";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _typeStorage, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
                ///                
            }
            catch { return null; }
        }


        public static DataSet get_listSearchStock()
        {
            try
            {
                ///
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conection);
                ///
                string sqlCommand = " MAESTROS.sp_get_listSearchStock";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results, results, results, results, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
                ///                
            }
            catch { return null; }
        }

        #endregion
    }

    public class stockStorage {

        public string _arv_article { set; get; }
        public string _arv_name { set; get; }
        public decimal _odn_odv { set; get; }
        public decimal _public_price { set; get; }
        public int _qty { set; get; }
        public int _qtyExists { set; get; }
        public string _size { set; get; }
        public int _pares { set; get; }
        public decimal _costoTotal { set; get; }
        public decimal _ppTotal { set; get; }
        public bool _check { set; get; }
    }
}