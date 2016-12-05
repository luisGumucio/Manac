using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;
using System.IO;

namespace Bata.Aquarella.BLL.Maestros
{
    public class Prices
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < VARIABLES >

        private string _PRV_CO;
        private string _PRV_ARTICLE;
        private decimal _PRN_PUBLIC_PRICE;
        private DateTime _PRD_DATE;
        private string _PRV_CIRCULAR;
        private bool _isNew;
        /// <summary>
        /// Variables de identificacón del usuario en el Log.
        /// </summary>
        Decimal _USER;
        String _MACHINE;

        #endregion

        #region < CONSTRUCTORES >
        public Prices()
        {
            _PRV_ARTICLE = string.Empty;
            _PRV_CIRCULAR = string.Empty;
            _PRV_CO = string.Empty;
        }

        public Prices(string PRV_ARTICLE, string PRV_CIRCULAR, string PRV_CO)
            : this()
        {
            _PRV_ARTICLE = PRV_ARTICLE;
            _PRV_CIRCULAR = PRV_CIRCULAR;
            _PRV_CO = PRV_CO;
        }

        public Prices(string PRV_CO, string PRV_ARTICLE, decimal PRN_PUBLIC_PRICE, DateTime PRD_DATE, string PRV_CIRCULAR)
            : this()
        {
            _PRV_CO = PRV_CO;
            _PRV_ARTICLE = PRV_ARTICLE;
            _PRN_PUBLIC_PRICE = PRN_PUBLIC_PRICE;
            _PRD_DATE = PRD_DATE;
            _PRV_CIRCULAR = PRV_CIRCULAR;
        }

        #endregion

        #region < PROPIEDADES >

        public string PRV_CO
        {
            get { return _PRV_CO; }
            set { _PRV_CO = value; }
        }

        public string PRV_ARTICLE
        {
            get { return _PRV_ARTICLE; }
            set { _PRV_ARTICLE = value; }
        }

        public decimal PRN_PUBLIC_PRICE
        {
            get { return _PRN_PUBLIC_PRICE; }
            set { _PRN_PUBLIC_PRICE = value; }
        }

        public DateTime PRD_DATE
        {
            get { return _PRD_DATE; }
            set { _PRD_DATE = value; }
        }

        public string PRV_CIRCULAR
        {
            get { return _PRV_CIRCULAR; }
            set { _PRV_CIRCULAR = value; }
        }

        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            set { _isNew = value; }
        }
        #endregion

        #region < METODOS PUBLICOS >

        /// <summary>
        /// Populates current instance of the object with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <param name="PRV_CO"></param>
        public void Load(string PRV_ARTICLE, string PRV_CIRCULAR, string PRV_CO)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(PRV_ARTICLE, PRV_CIRCULAR, PRV_CO);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _PRV_CO = (string)row["PRV_CO"];
                _PRV_ARTICLE = (string)row["PRV_ARTICLE"];
                _PRN_PUBLIC_PRICE = (decimal)row["PRN_PUBLIC_PRICE"];
                _PRD_DATE = (DateTime)row["PRD_DATE"];
                _PRV_CIRCULAR = (string)row["PRV_CIRCULAR"];
            }
            catch 
            {
                _PRV_ARTICLE = string.Empty;
                _PRV_CIRCULAR = string.Empty;
                _PRV_CO = string.Empty;
            }
        }
        /// <summary>
        /// Adds or updates information in the database depending on the primary key stored in the object instance.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        public bool Save(Decimal user, String machine)
        {
            _USER = user;
            _MACHINE = machine;
            if (this.IsNew)
                return Insert();
            else
                return Update();
        }
        /// <summary>
        /// Serializes the current instance data to an Xml string.
        /// </summary>
        /// <returns>A string containing the Xml representation of the object.</returns>
        public string ToXml()
        {
            /// DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(_PRV_ARTICLE, _PRV_CIRCULAR, _PRV_CO);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }

        #endregion

        #region < METODOS PRIVADOS >

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <param name="PRV_CO"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string PRV_ARTICLE, string PRV_CIRCULAR, string PRV_CO)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADPK_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, PRV_ARTICLE, PRV_CIRCULAR, PRV_CO, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            return db.ExecuteDataSet(dbCommandWrapper);
        }
        /// <summary>
        /// Inserts the current instance data into the database.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        private bool Insert()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_ADD_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _PRV_CO, _PRV_ARTICLE, _PRN_PUBLIC_PRICE, _PRD_DATE, _PRV_CIRCULAR);
            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
                /////////////////////
                //Guardar en el log
                /////////////////////
            }
            catch 
            {
                return false;
            }
        }
        /// <summary>
        /// Updates the current instance data in the database.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        private bool Update()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_ADD_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _PRV_CO, _PRV_ARTICLE, _PRN_PUBLIC_PRICE, _PRD_DATE, _PRV_CIRCULAR);
            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
                //////////////////////
                //Guardar en el log
                //////////////////////
            }
            catch 
            {
                return false;
            }
        }
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
        /// Verifica la existencia de la circular con el articulo especificado ponerle precio publico.
        /// </summary>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <param name="PRV_CO"></param>
        /// <returns></returns>
        public static bool Existe(string PRV_ARTICLE, string PRV_CIRCULAR, string PRV_CO)
        {
            Prices X = new Prices();
            //
            X.Load(PRV_ARTICLE, PRV_CIRCULAR, PRV_CO);
            //
            if (!(X.PRV_ARTICLE == string.Empty) && !(X.PRV_CIRCULAR == string.Empty) && !(X.PRV_CO == string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Busca todos los articulos a los qeu se les puso precio con la circular especificada.
        /// </summary>
        /// <param name="PRV_CO"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <returns></returns>
        public static DataTable getByCircular(string PRV_CO, string PRV_CIRCULAR)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_GETBYCIRCULAR_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, PRV_CO, PRV_CIRCULAR, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            DataTable t = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            t.Columns["PRV_CO"].DefaultValue = PRV_CO;
            t.Columns["PRV_CIRCULAR"].DefaultValue = PRV_CIRCULAR;
            //
            return t;
        }
        /// <summary>
        /// Verifica la existencia en el Sistema de Información de la cisrcular especificada.
        /// </summary>
        /// <param name="PRV_CO"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <returns></returns>
        public static Boolean ExistCIRCULAR(string PRV_CO, string PRV_CIRCULAR)
        {
            DataTable dt = getByCircular(PRV_CO, PRV_CIRCULAR);
            //
            try
            {
                return dt.Rows.Count > 0;
            }
            catch { return false; }
        }
        /// <summary>
        /// Retorna todos los articulos con el ultimo precio actualizado.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllPRICES()
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADALL_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }
        /// <summary>
        /// Guarda una tabla de precios publicos para un articulo con una vigencia definida.
        /// </summary>
        /// <param name="tabla"></param>
        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("SP_LOADAll_PRICES");
            da.InsertCommand = db.GetStoredProcCommand("SP_ADD_PRICES");
            da.UpdateCommand = db.GetStoredProcCommand("SP_ADD_PRICES");
            da.DeleteCommand = db.GetStoredProcCommand("SP_DELETE_PRICES");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "PRV_CO", DbType.AnsiString, "PRV_CO", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "PRV_ARTICLE", DbType.AnsiString, "PRV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "PRN_PUBLIC_PRICE", DbType.Decimal, "PRN_PUBLIC_PRICE", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "PRD_DATE", DbType.DateTime, "PRD_DATE", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "PRV_CIRCULAR", DbType.AnsiString, "PRV_CIRCULAR", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "PRV_CO", DbType.AnsiString, "PRV_CO", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "PRV_ARTICLE", DbType.AnsiString, "PRV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "PRN_PUBLIC_PRICE", DbType.Decimal, "PRN_PUBLIC_PRICE", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "PRD_DATE", DbType.DateTime, "PRD_DATE", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "PRV_CIRCULAR", DbType.AnsiString, "PRV_CIRCULAR", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "PRV_ARTICLE", DbType.AnsiString, "PRV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "PRV_CIRCULAR", DbType.AnsiString, "PRV_CIRCULAR", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "PRV_CO", DbType.AnsiString, "PRV_CO", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        /// <summary>
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <param name="PRV_CO"></param>
        public static void Remove(string PRV_ARTICLE, string PRV_CIRCULAR, string PRV_CO, Decimal user, String machine)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_DELETE_PRICES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_PRV_ARTICLE", DbType.AnsiString, PRV_ARTICLE);
            db.AddInParameter(dbCommandWrapper, "P_PRV_CIRCULAR", DbType.AnsiString, PRV_CIRCULAR);
            db.AddInParameter(dbCommandWrapper, "P_PRV_CO", DbType.AnsiString, PRV_CO);
            ///
            db.ExecuteNonQuery(dbCommandWrapper);
        }
        /// <summary>
        /// Metodo Estatico para la Actualización de un registro
        /// Nota: No intente agragar un nuevo registro con esta metodo esta valídado.
        /// </summary>
        /// <param name="PRV_CO"></param>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRN_PUBLIC_PRICE"></param>
        /// <param name="PRD_DATE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <returns> actalizo(true) o no actualizo(false)</returns>
        static public bool actualizarPRICES(string PRV_CO, string PRV_ARTICLE, decimal PRN_PUBLIC_PRICE, DateTime PRD_DATE, string PRV_CIRCULAR, Decimal user, String machine)
        {
            //Intanciación de PRICES
            Prices X = new Prices(PRV_CO, PRV_ARTICLE, PRN_PUBLIC_PRICE, PRD_DATE, PRV_CIRCULAR);
            //Validadcion de existencia de PRICES
            if (Prices.Existe(X.PRV_ARTICLE, X.PRV_CIRCULAR, X.PRV_CO))
            {
                //Update
                X.IsNew = false;
                //Guardar
                if (X.Save(user, machine))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// Retorna una fila vacia y lista para insertar un precio para agreagar a una tabla de precios.
        /// </summary>
        /// <returns></returns>
        static public DataTable getSchemeTablePrices()
        {
            DataTable dt = new Prices().LoadByPrimaryKey("", "", "").Tables[0];
            return dt;
        }
        /// <summary>
        /// Retorna la tabla de precios gruardad en la session cuando se estan poniendo o editando precios.
        /// </summary>
        /// <returns></returns>
        public static DataTable getTablePricesTemp()
        {
            return SessionDataTable.getTable(SessionDataTable.getNameTablePrices());
        }
        /// <summary>
        /// Retorna el ultimo precio publico del articulo especificad ó cero(0) si nuncaa tenido.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public static decimal getLastPricePublic(string ODV_CO, string ODV_ARTICLE)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADPK_V_ARTICLES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, ODV_CO, ODV_ARTICLE, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            DataTable t = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            t.Columns["ARV_CO"].DefaultValue = ODV_CO;
            t.Columns["ARV_ARTICLE"].DefaultValue = ODV_ARTICLE;
            try
            {
                return Convert.ToDecimal((t.Rows[0])["PRN_PUBLIC_PRICE"]);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Actuializa una la fila del articulo especificado en la tabla de precsios de la seccion cuando se estan guardando precios o estos se estan editando.
        /// </summary>
        /// <param name="PRV_CO"></param>
        /// <param name="PRV_ARTICLE"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <param name="PRN_PUBLIC_PRICE"></param>
        /// <returns></returns>
        static public Boolean upDateTablePUBLIC_PRICETemp(string PRV_CO, string PRV_ARTICLE, string PRV_CIRCULAR, Decimal PRN_PUBLIC_PRICE)
        {
            return SessionDataTable.upDateRowPUBLICS_PRICES(SessionDataTable.getNameTablePrices(), PRV_ARTICLE, PRN_PUBLIC_PRICE);
        }
        /// <summary>
        /// Retorana la fecha apartir de la cual qeu da vigente la circular especificada.
        /// </summary>
        /// <param name="PRV_CO"></param>
        /// <param name="PRV_CIRCULAR"></param>
        /// <returns></returns>
        public static string getDateCircular(string PRV_CO, string PRV_CIRCULAR)
        {
            ///
            DataTable t = getByCircular(PRV_CO, PRV_CIRCULAR);
            ///
            if (t != null && t.Rows.Count > 0)
            {
                DataRow dr = t.Rows[0];
                return dr["PRD_DATE"].ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}