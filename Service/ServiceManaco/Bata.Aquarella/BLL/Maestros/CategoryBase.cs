using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.IO;


namespace Bata.Aquarella.BLL.Maestros
{
    public abstract class CategoryBase
    {
        public static string _conn = Constants.OrcleStringConn;

         #region < VARIABLES >

        private string _CAV_CO;
        private string _CAV_CAT_ID;
        private string _CAV_DESCRIPTION;
        private bool _isNew;

        #endregion

        #region < CONSTRUCTORES >

        protected CategoryBase()
        {
            _CAV_CAT_ID = string.Empty;
            _CAV_CO = string.Empty;
            _CAV_DESCRIPTION = string.Empty;
        }

        protected CategoryBase(string CAV_CAT_ID, string CAV_CO)
            : this()
        {
            _CAV_CAT_ID = CAV_CAT_ID;
            _CAV_CO = CAV_CO;
        }

        protected CategoryBase(string CAV_CO, string CAV_CAT_ID, string CAV_DESCRIPTION)
            : this()
        {
            _CAV_CO = CAV_CO;
            _CAV_CAT_ID = CAV_CAT_ID;
            _CAV_DESCRIPTION = CAV_DESCRIPTION;
        }

        #endregion

        #region < PROPIEDADES >

        public string CAV_CO
        {
            get { return _CAV_CO; }
            set { _CAV_CO = value; }
        }

        public string CAV_CAT_ID
        {
            get { return _CAV_CAT_ID; }
            set { _CAV_CAT_ID = value; }
        }

        public string CAV_DESCRIPTION
        {
            get { return _CAV_DESCRIPTION; }
            set { _CAV_DESCRIPTION = value; }
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
        /// <param name="CAV_CAT_ID"></param>
        /// <param name="CAV_CO"></param>
        public virtual void Load(string CAV_CAT_ID, string CAV_CO)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(CAV_CAT_ID, CAV_CO);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _CAV_CO = (string)row["CAV_CO"];
                _CAV_CAT_ID = (string)row["CAV_CAT_ID"];
                _CAV_DESCRIPTION = row.IsNull("CAV_DESCRIPTION") ? string.Empty : (string)row["CAV_DESCRIPTION"];
            }
            catch
            {
                _CAV_CAT_ID = string.Empty;
                _CAV_CO = string.Empty;
                _CAV_DESCRIPTION = string.Empty;
            }
        }
        /// <summary>
        /// Adds or updates information in the database depending on the primary key stored in the object instance.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        public bool Save()
        {
            if (this.IsNew)
                return Insert();
            else
                return Update();
        }
        /// <summary>
        /// Serializes the current instance data to an Xml string.
        /// </summary>
        /// <returns>A string containing the Xml representation of the object.</returns>
        virtual public string ToXml()
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(_CAV_CAT_ID, _CAV_CO);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }

        #endregion

        #region < METODOS PRIVADOS >

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="CAV_CAT_ID"></param>
        /// <param name="CAV_CO"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string CAV_CAT_ID, string CAV_CO)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADPK_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, CAV_CAT_ID, CAV_CO, results);
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
            string sqlCommand = "SP_ADD_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _CAV_CO, _CAV_CAT_ID, SetNullValue((_CAV_DESCRIPTION == string.Empty), _CAV_DESCRIPTION));
            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
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
            string sqlCommand = "SP_ADD_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _CAV_CO, _CAV_CAT_ID, SetNullValue((_CAV_DESCRIPTION == string.Empty), _CAV_DESCRIPTION));
            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
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
        /// Verifica la existencia de la categoria especificada en el Sistema de Información.
        /// </summary>
        /// <param name="CAV_CAT_ID"></param>
        /// <param name="CAV_CO"></param>
        /// <returns>true se es encontrada, false en caso contrario.</returns>
        public static bool Existe(string CAV_CAT_ID, string CAV_CO)
        {
            Category c = new Category();
            c.Load(CAV_CAT_ID, CAV_CO);
            if (!c.CAV_CAT_ID.Equals(string.Empty) && !c.CAV_CO.Equals(string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Retorna todas las categorias del Sistema de Información.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCategory()
        {
            ///
            DataTable ret = GetAllCATEGORY().Tables[0];
            ///
            return ret;
        }
        /// <summary>
        /// Retorna todas las categorias del Sistema de Información.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllCATEGORY()
        {
            // CURSOR REF
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADALL_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }
        /// <summary>
        /// Retorna las categorias de la Major Categoria especificada.
        /// </summary>
        /// <param name="msv_co"></param>
        /// <param name="msv_macat_id"></param>
        /// <returns></returns>
        public static DataTable GetCategory(String msv_co, String msv_macat_id)
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_CATEXMAJCATE";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, msv_co, msv_macat_id, results);
            DataTable ret = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            ///
            return ret;
        }
        /// <summary>
        /// Guarda una tabla con categorias.
        /// </summary>
        /// <param name="tabla"></param>
        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("SP_LOADAll_CATEGORY");
            da.InsertCommand = db.GetStoredProcCommand("SP_ADD_CATEGORY");
            da.UpdateCommand = db.GetStoredProcCommand("SP_ADD_CATEGORY");
            da.DeleteCommand = db.GetStoredProcCommand("SP_DELETE_CATEGORY");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "CAV_CO", DbType.AnsiString, "CAV_CO", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "CAV_CAT_ID", DbType.AnsiString, "CAV_CAT_ID", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "CAV_DESCRIPTION", DbType.AnsiString, "CAV_DESCRIPTION", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "CAV_CO", DbType.AnsiString, "CAV_CO", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "CAV_CAT_ID", DbType.AnsiString, "CAV_CAT_ID", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "CAV_DESCRIPTION", DbType.AnsiString, "CAV_DESCRIPTION", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "CAV_CAT_ID", DbType.AnsiString, "CAV_CAT_ID", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "CAV_CO", DbType.AnsiString, "CAV_CO", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        /// <summary>
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="CAV_CAT_ID"></param>
        /// <param name="CAV_CO"></param>
        public static void Remove(string CAV_CAT_ID, string CAV_CO)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_DELETE_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_CAV_CAT_ID", DbType.AnsiString, CAV_CAT_ID);
            db.AddInParameter(dbCommandWrapper, "P_CAV_CO", DbType.AnsiString, CAV_CO);

            db.ExecuteNonQuery(dbCommandWrapper);
        }

        #endregion
    }
}