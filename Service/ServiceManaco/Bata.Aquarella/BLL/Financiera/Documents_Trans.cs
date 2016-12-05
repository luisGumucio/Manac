using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Documents_Trans
    {
        #region < Atributos >

        public bool _check { get; set; }
        public bool _enabled { get; set; }
        public string _docNo { get; set; }
        public string _conceptid { get; set; }
        public string _conceptDesc { get; set; }
        public string _date { get; set; }
        public decimal _value { get; set; }
        public decimal _increase { get; set; }
        public string _type { get; set; }



        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < VARIABLES >
        private string _dtv_co;
        private string _dtv_transdoc_id;
        private int _dtn_coord_id;
        private string _dtv_concept_id;
        private string _dtv_document_no;
        private DateTime _dtd_document_date;
        private decimal _dtn_tax_base;
        private string _dtv_clear;
        private string _dtv_comments;
        private decimal _dtn_tax;
        private decimal _dtn_autorete;
        private string _dtv_warehouse;
        #endregion

        #region < CONSTRUCTORES >
        public Documents_Trans()
        {
            _dtv_co = string.Empty;
            _dtv_transdoc_id = string.Empty;
            _dtn_coord_id = 0;
            _dtv_concept_id = string.Empty;
            _dtv_document_no = string.Empty;
            _dtd_document_date = DateTime.Now;
            _dtn_tax_base = 0;
            _dtv_clear = string.Empty;
            _dtv_comments = string.Empty;
            _dtn_tax = 0;
            _dtn_autorete = 0;
            _dtv_warehouse = string.Empty;
        }

        public Documents_Trans(string dtv_co, string dtv_transdoc_id, int dtn_coord_id,
            string dtv_concept_id,
            string dtv_document_no,
            DateTime dtd_document_date,
            decimal dtn_tax_base,
            string dtv_clear, string dtv_comments, decimal dtn_tax, decimal dtn_autorete, string dtv_warehouse)
        {
            _dtv_co = dtv_co;
            _dtv_transdoc_id = dtv_transdoc_id;
            _dtn_coord_id = dtn_coord_id;
            _dtv_concept_id = dtv_concept_id;
            _dtv_document_no = dtv_document_no;
            _dtd_document_date = dtd_document_date;
            _dtn_tax_base = dtn_tax_base;
            _dtv_clear = dtv_clear;
            _dtv_comments = dtv_comments;
            _dtn_tax = dtn_tax;
            _dtn_autorete = dtn_autorete;
            _dtv_warehouse = dtv_warehouse;
        }
        #endregion

        #region < PROPIEDADES >
        public string dtv_Co
        {
            get { return _dtv_co; }
            set { _dtv_co = value; }
        }

        public string dtv_TransDoc_Id
        {
            get { return _dtv_transdoc_id; }
            set { _dtv_transdoc_id = value; }
        }

        public int dtn_Coord_Id
        {
            get { return _dtn_coord_id; }
            set { _dtn_coord_id = value; }
        }

        public string dtv_Concept_Id
        {
            get { return _dtv_concept_id; }
            set { _dtv_concept_id = value; }
        }

        public string dtv_Document_No
        {
            get { return _dtv_document_no; }
            set { _dtv_document_no = value; }
        }

        public DateTime dtd_Document_Date
        {
            get { return _dtd_document_date; }
            set { _dtd_document_date = value; }
        }

        public decimal dtn_Tax_Base
        {
            get { return _dtn_tax_base; }
            set { _dtn_tax_base = value; }
        }

        public string dtv_Clear
        {
            get { return _dtv_clear; }
            set { _dtv_clear = value; }
        }

        public string dtv_Comments
        {
            get { return _dtv_comments; }
            set { _dtv_comments = value; }
        }

        public decimal dtn_Tax
        {
            get { return _dtn_tax; }
            set { _dtn_tax = value; }
        }

        public decimal dtn_Autorete
        {
            get { return _dtn_autorete; }
            set { _dtn_autorete = value; }
        }

        public string dtv_Warehouse
        {
            get { return _dtv_warehouse; }
            set { _dtv_warehouse = value; }
        }

        public decimal dtn_autorete_cree { get; set; }
        #endregion

        #region < METODOS PUBLICOS >
        public string Save()
        {
            return Documents_Trans.SaveDoctrans(this.dtv_Co,
                this.dtn_Coord_Id, this.dtv_Concept_Id, this.dtv_Document_No,
                this.dtn_Tax_Base, this.dtv_Comments, this.dtn_Tax, this.dtn_Autorete, this.dtv_Warehouse, this.dtn_autorete_cree);
        }

        #endregion

        #region < METODOS PRIVADOS >

        #endregion

        #region < METODOS ESTATICOS >
        /// <summary>
        /// Metodo para guardar un Document trans
        /// </summary>
        /// <param name="dtv_Co">Compañia</param>
        /// <param name="dtn_Coord_Id">IdCoordinador</param>
        /// <param name="dtv_Concept_Id">Concepto de la nota</param>
        /// <param name="dtv_Document_No">No de voucher</param>
        /// <param name="dtn_Tax_Base">Valor</param>
        /// <param name="dtv_Comments">Comentario</param>
        /// <param name="dtn_Tax">Impuestos</param>
        /// <param name="dtn_Autorete">Autorretencion</param>
        /// <param name="dtv_Warehouse">WareHouse coordinator</param>
        /// <param name="dtn_autorete_cree">Autorretencion Cree</param>
        /// <returns>String con el numero de</returns>
        public static string SaveDoctrans(string dtv_Co, 
            decimal dtn_Coord_Id,
            string dtv_Concept_Id,
            string dtv_Document_No,
            decimal dtn_Tax_Base,
            string dtv_Comments,
            decimal dtn_Tax,
            decimal dtn_Autorete,
            string dtv_Warehouse,
            decimal dtn_autorete_cree
            )
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string result = "";
            string sqlCommand = "FINANCIERA.SP_ADD_DOCTRANS2";
            string _DocTrans_Id = string.Empty;
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, dtv_Co,
                 _DocTrans_Id, dtn_Coord_Id, dtv_Concept_Id, dtv_Document_No,
                 dtn_Tax_Base, dtv_Comments, dtn_Tax, dtn_Autorete, dtv_Warehouse, dtn_autorete_cree);
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

                    result = _DocTrans_Id;
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
        /// Consultar cruce de pagos y liquidaciones de un cliente
        /// </summary>
        /// <param name="dtv_co"></param>
        /// <param name="dtn_coord_id"></param>
        /// <returns></returns>
        static public DataSet get_DocTranLiquiByCoordinator(string _co, int _idCust)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "financiera.sp_loadpayliqui_x_coordinator";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCust, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        static public DataSet get_DocTranLiquiByCoordinatorHall(string _co, int _idCust)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.SP_LOADPAYLIQUI_X_HALL";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCust, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        static public DataSet get_DocTransByConcept(string dtv_co, string warehouse, DateTime dtd_date_start, DateTime dtd_date_end)
        {
            // CURSOR REF
            object results = new object[1];

            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADOCTRANS_X_CONCEPT";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, dtv_co, warehouse, dtd_date_start, dtd_date_end, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        static public DataSet get_BalanceByCoordinator(string company, string warehouse, string areaId)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.SP_GET_BALANCE_X_COORDINATOR";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, warehouse, areaId, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener los saldos de los coordinadores
        /// </summary>
        /// <param name="company">compañia</param>
        /// <param name="warehouse">bodega</param>
        /// <param name="regionId">Filtra por region</param>
        /// <returns></returns>
        static public DataSet get_BalanceByCoordinators(string company, string warehouse, string regionId)
        {
            try
            {
                object refcursor = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.sp_get_balanceCoord_by_region";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, warehouse, regionId, refcursor);

                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch 
            {
                return null;
            }
        
        }

        /// <summary>
        /// Consultar saldos y montos en pedidos del cliente
        /// </summary>
        /// <param name="company"></param>
        /// <param name="warehouse"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        static public DataSet getBalanceCoordById(string company, string customer)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "financiera.sp_get_balance_coor_byid";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, customer, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
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
        /// <param name="_idCoord"></param>
        /// <param name="_noDoc"></param>
        /// <returns></returns>
        static public DataTable getClearDocTransByDoc(String _company, String _idCoord, String _noDoc)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "financiera.sp_get_cleardoctransbydoc";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idCoord, _noDoc, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
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
        /// <param name="_idCoord"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        static public DataTable getClearDocTransByDate(String _company, String _idCoord, String _startDate, String _endDate)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "financiera.sp_get_cleardoctransbydate";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idCoord, _startDate, _endDate, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar notas creditos y debitos cargadas a un cliente
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public static DataSet getDocTransCust(String _company, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "financiera.sp_getdoctranscust";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Consulta clear de facturas
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_type">1->Clear solo de facturas de Pos, otro: cualquier factura</param>
        /// <param name="_document"></param>
        /// <param name="_invoice"></param>
        /// <param name="_dateStart"></param>
        /// <param name="_dateEnd"></param>
        /// <returns></returns>
        public static DataSet getClear(string _co, int _type, string _document, string _invoice, string _dateStart, string _dateEnd)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "financiera.sp_get_clear";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _type, _document, _invoice, _dateStart, _dateEnd, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}