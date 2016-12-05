using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Maestros
{
    public class Odv
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

         #region < VARIABLES >

        private string _ODV_CO;
        private string _ODV_ARTICLE;
        private string _ODV_DOCUMENT;
        private DateTime _ODD_DATE;
        private decimal _ODN_ODV;
        private bool _isNew;
        /// <summary>
        /// Variables de identificacion del usuario para el log.
        /// </summary>
        Decimal _USER;
        String _MACHINE;

        #endregion

        #region < CONSTRUCTORES >
        
        public Odv()
        {
            _ODV_ARTICLE = string.Empty;
            _ODV_CO = string.Empty;
            _ODV_DOCUMENT = string.Empty;
            _ODD_DATE = DateTime.Parse("01/01/1900");
            _ODN_ODV = 0;
        }

        public Odv(string ODV_ARTICLE, string ODV_CO, string ODV_DOCUMENT)
            : this()
        {
            _ODV_ARTICLE = ODV_ARTICLE;
            _ODV_CO = ODV_CO;
            _ODV_DOCUMENT = ODV_DOCUMENT;
        }

        public Odv(string ODV_CO, string ODV_ARTICLE, string ODV_DOCUMENT, DateTime ODD_DATE, decimal ODN_ODV)
            : this()
        {
            _ODV_CO = ODV_CO;
            _ODV_ARTICLE = ODV_ARTICLE;
            _ODV_DOCUMENT = ODV_DOCUMENT;
            _ODD_DATE = ODD_DATE;
            _ODN_ODV = ODN_ODV;
        }

        #endregion

        #region < PROPIEDADES >

        public string ODV_CO
        {
            get { return _ODV_CO; }
            set { _ODV_CO = value; }
        }

        public string ODV_ARTICLE
        {
            get { return _ODV_ARTICLE; }
            set { _ODV_ARTICLE = value; }
        }

        public string ODV_DOCUMENT
        {
            get { return _ODV_DOCUMENT; }
            set { _ODV_DOCUMENT = value; }
        }

        public DateTime ODD_DATE
        {
            get { return _ODD_DATE; }
            set { _ODD_DATE = value; }
        }

        public decimal ODN_ODV
        {
            get { return _ODN_ODV; }
            set { _ODN_ODV = value; }
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
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        public void Load(string ODV_ARTICLE, string ODV_CO, string ODV_DOCUMENT)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(ODV_ARTICLE, ODV_CO, ODV_DOCUMENT);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _ODV_CO = (string)row["ODV_CO"];
                _ODV_ARTICLE = (string)row["ODV_ARTICLE"];
                _ODV_DOCUMENT = (string)row["ODV_DOCUMENT"];
                _ODD_DATE = row.IsNull("ODD_DATE") ? DateTime.Parse("01/01/1900") : (DateTime)row["ODD_DATE"];
                _ODN_ODV = row.IsNull("ODN_ODV") ? 0 : (decimal)row["ODN_ODV"];
            }
            catch
            {
                _ODV_ARTICLE = string.Empty;
                _ODV_CO = string.Empty;
                _ODV_DOCUMENT = string.Empty;
                _ODD_DATE = DateTime.Parse("01/01/1900");
                _ODN_ODV = 0;
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
            ///
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
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(_ODV_ARTICLE, _ODV_CO, _ODV_DOCUMENT);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }

        #endregion

        #region < METODOS PRIVADOS >

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string ODV_ARTICLE, string ODV_CO, string ODV_DOCUMENT)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADPK_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, ODV_ARTICLE, ODV_CO, ODV_DOCUMENT, results);
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
            string sqlCommand = "SP_ADD_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _ODV_CO, _ODV_ARTICLE, _ODV_DOCUMENT, SetNullValue((_ODD_DATE == DateTime.Parse("01/01/1900")), _ODD_DATE), _ODN_ODV);
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
            string sqlCommand = "SP_ADD_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _ODV_CO, _ODV_ARTICLE, _ODV_DOCUMENT, SetNullValue((_ODD_DATE == DateTime.Parse("01/01/1900")), _ODD_DATE), _ODN_ODV);
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
        /// Verifica la existencia del docuemnto especificado para ponerle precio al articulo especificado.
        /// </summary>
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <returns></returns>
        public static bool Existe(string ODV_ARTICLE, string ODV_CO, string ODV_DOCUMENT)
        {
            Odv X = new Odv();
            //
            X.Load(ODV_ARTICLE, ODV_CO, ODV_DOCUMENT);
            //
            if (!(X.ODV_ARTICLE == string.Empty) && !(X.ODV_CO == string.Empty) && !(X.ODV_DOCUMENT == string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Retorna los articulos que seles puso precio con el documento especificado.
        /// </summary>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <returns></returns>
        public static DataTable getByDocument(string ODV_CO, string ODV_DOCUMENT)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "SP_GETBYDOCUMENT_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, ODV_CO, ODV_DOCUMENT, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            DataTable t = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            t.Columns["ODV_CO"].DefaultValue = ODV_CO;
            t.Columns["ODV_DOCUMENT"].DefaultValue = ODV_DOCUMENT;
            ///
            return t;
        }
        /// <summary>
        /// Retorna los articulos que aun no tienen el precio del documento especificado activo.
        /// </summary>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <returns></returns>
        public static DataTable GetByDocumentInActive(string ODV_CO, string ODV_DOCUMENT)
        {
            //
            DataTable dt = getByDocument(ODV_CO, ODV_DOCUMENT);
            //
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                DateTime date = DateTime.Parse(dr["ODD_DATE"].ToString());
                if (date > DateTime.Now)
                {
                    return dt;
                }
            }
            return null;
        }
        /// <summary>
        /// Verifica la existencia del docuemnto especifico en el Sistema de Información
        /// </summary>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <returns></returns>
        public static Boolean ExistDOCUMENT(string ODV_CO, string ODV_DOCUMENT)
        {
            //
            DataTable dt = getByDocument(ODV_CO, ODV_DOCUMENT);
            //
            try
            {
                return dt.Rows.Count > 0;
            }
            catch { return false; }
        }
        /// <summary>
        /// Retorna todos los Articulos con ODV Vigente.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllODV()
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADALL_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }
        /// <summary>
        /// Guarda odv's de uan tabla.
        /// </summary>
        /// <param name="tabla"></param>
        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("SP_LOADAll_ODV");
            da.InsertCommand = db.GetStoredProcCommand("SP_ADD_ODV");
            da.UpdateCommand = db.GetStoredProcCommand("SP_ADD_ODV");
            da.DeleteCommand = db.GetStoredProcCommand("SP_DELETE_ODV");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "ODV_CO", DbType.AnsiString, "ODV_CO", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "ODV_ARTICLE", DbType.AnsiString, "ODV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "ODV_DOCUMENT", DbType.AnsiString, "ODV_DOCUMENT", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "ODD_DATE", DbType.DateTime, "ODD_DATE", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "ODN_ODV", DbType.Decimal, "ODN_ODV", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "ODV_CO", DbType.AnsiString, "ODV_CO", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "ODV_ARTICLE", DbType.AnsiString, "ODV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "ODV_DOCUMENT", DbType.AnsiString, "ODV_DOCUMENT", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "ODD_DATE", DbType.DateTime, "ODD_DATE", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "ODN_ODV", DbType.Decimal, "ODN_ODV", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "ODV_ARTICLE", DbType.AnsiString, "ODV_ARTICLE", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "ODV_CO", DbType.AnsiString, "ODV_CO", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "ODV_DOCUMENT", DbType.AnsiString, "ODV_DOCUMENT", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        /// <summary>
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_DOCUMENT"></param>
        public static void Remove(string ODV_ARTICLE, string ODV_CO, string ODV_DOCUMENT, Decimal user, String machine)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_DELETE_ODV";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_ODV_ARTICLE", DbType.AnsiString, ODV_ARTICLE);
            db.AddInParameter(dbCommandWrapper, "P_ODV_CO", DbType.AnsiString, ODV_CO);
            db.AddInParameter(dbCommandWrapper, "P_ODV_DOCUMENT", DbType.AnsiString, ODV_DOCUMENT);
            ///
            db.ExecuteNonQuery(dbCommandWrapper);
        }
        /// <summary>
        /// Metodo Estatico para la Actualización de un registro
        /// Nota: No intente agragar un nuevo registro con esta metodo esta valídado.
        /// </summary>
        /// <param name="ODV_CO"></param>
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODV_DOCUMENT"></param>
        /// <param name="ODD_DATE"></param>
        /// <param name="ODN_ODV"></param>
        /// <returns> actalizo(true) o no actualizo(false)</returns>
        static public bool actualizarODV(string ODV_CO, string ODV_ARTICLE, string ODV_DOCUMENT, DateTime ODD_DATE, decimal ODN_ODV, Decimal user, String machine)
        {
            //Intanciación de ODV
            Odv X = new Odv(ODV_CO, ODV_ARTICLE, ODV_DOCUMENT, ODD_DATE, ODN_ODV);
            //Validadcion de existencia de ODV
            if (Odv.Existe(X.ODV_ARTICLE, X.ODV_CO, X.ODV_DOCUMENT))
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
        /// retorna la tablatemporarl guarada en la session del browser con los odv's en construccion.
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        static public DataTable getTableODVTemp(String nameTable)
        {
            //
            DataTable dt = SessionDataTable.getTable(nameTable);
            //
            if (dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Actualiza el odv de un articulo guardado en la tabla qeu esta guardada en la session.
        /// </summary>
        /// <param name="ODV_ARTICLE"></param>
        /// <param name="ODN_ODV"></param>
        /// <returns></returns>
        static public Boolean upDateTableODVTemp(string ODV_CO, string ODV_ARTICLE, string ODV_DOCUMENT, decimal ODN_ODV)
        {
            return SessionDataTable.upDateRowODV(SessionDataTable.getNameTableOdV(), ODV_ARTICLE, ODN_ODV);
        }
        /// <summary>
        /// Retorna una nueva fila para insertar articulos con odv en una tabla.
        /// </summary>
        /// <returns></returns>
        static public DataTable getSchemeTableODV()
        {
            DataTable dt = new Odv().LoadByPrimaryKey("", "", "").Tables[0];
            return dt;
        }
        /// <summary>
        /// Retorna la fecha del docuemento especificado.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string getDateDocument(string ODV_CO, string ODV_DOCUMENT)
        {
            //
            DataTable t = getByDocument(ODV_CO, ODV_DOCUMENT);
            //
            if (t != null && t.Rows.Count > 0)
            {
                DataRow dr = t.Rows[0];
                return dr["ODD_DATE"].ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Retorna el ustimo costo de un articulo o cero si nunca a tenido.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public static decimal getLastCost(string ODV_CO, string ODV_ARTICLE)
        {
            //CURSOR REF
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
            //            
            t.Columns["ARV_CO"].DefaultValue = ODV_CO;
            t.Columns["ARV_ARTICLE"].DefaultValue = ODV_ARTICLE;
            //
            try
            {
                return Convert.ToDecimal((t.Rows[0])["ODN_ODV"]);
            }
            catch
            {
                return 0;
            }
        }
        # endregion
    }
}