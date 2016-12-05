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
    public abstract class SubCategoryBase
    {

        public static string _conn = Constants.OrcleStringConn;

        #region < VARIABLES >
        private string _SCV_CO;
        private string _SCV_SUBCAT_ID;
        private string _SCV_DESCRIPTION;
        private bool _isNew;
        #endregion

        #region < CONSTRUCTORES >
        protected SubCategoryBase()
        {
            _SCV_CO = string.Empty;
            _SCV_SUBCAT_ID = string.Empty;
            _SCV_DESCRIPTION = string.Empty;
        }

        protected SubCategoryBase(string SCV_CO, string SCV_SUBCAT_ID)
            : this()
        {
            _SCV_CO = SCV_CO;
            _SCV_SUBCAT_ID = SCV_SUBCAT_ID;
        }

        protected SubCategoryBase(string SCV_CO, string SCV_SUBCAT_ID, string SCV_DESCRIPTION)
            : this()
        {
            _SCV_CO = SCV_CO;
            _SCV_SUBCAT_ID = SCV_SUBCAT_ID;
            _SCV_DESCRIPTION = SCV_DESCRIPTION;
        }

        #endregion

        #region < PROPIEDADES >

        public string SCV_CO
        {
            get { return _SCV_CO; }
            set { _SCV_CO = value; }
        }

        public string SCV_SUBCAT_ID
        {
            get { return _SCV_SUBCAT_ID; }
            set { _SCV_SUBCAT_ID = value; }
        }

        public string SCV_DESCRIPTION
        {
            get { return _SCV_DESCRIPTION; }
            set { _SCV_DESCRIPTION = value; }
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

        #region < METODOS  PUBLICOS >

        /// <summary>
        /// Populates current instance of the object with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="SCV_CO"></param>
        /// <param name="SCV_SUBCAT_ID"></param>
        public virtual void Load(string SCV_CO, string SCV_SUBCAT_ID)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(SCV_CO, SCV_SUBCAT_ID);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _SCV_CO = (string)row["SCV_CO"];
                _SCV_SUBCAT_ID = (string)row["SCV_SUBCAT_ID"];
                _SCV_DESCRIPTION = row.IsNull("SCV_DESCRIPTION") ? string.Empty : (string)row["SCV_DESCRIPTION"];
            }
            catch
            {
                _SCV_CO = string.Empty;
                _SCV_SUBCAT_ID = string.Empty;
                _SCV_DESCRIPTION = string.Empty;
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
            DataSet ds = this.LoadByPrimaryKey(_SCV_CO, _SCV_SUBCAT_ID);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }        

        #endregion

        #region < METODOS  PRIVADOS >

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="SCV_CO"></param>
        /// <param name="SCV_SUBCAT_ID"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string SCV_CO, string SCV_SUBCAT_ID)
        {
            // CURSOR REF
            object results = new object[1];
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADPK_SUB_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, SCV_CO, SCV_SUBCAT_ID, results);
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
            string sqlCommand = "SP_ADD_SUB_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _SCV_CO, _SCV_SUBCAT_ID, SetNullValue((_SCV_DESCRIPTION == string.Empty), _SCV_DESCRIPTION));
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
        public bool Update()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_ADD_SUB_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _SCV_CO, _SCV_SUBCAT_ID, SetNullValue((_SCV_DESCRIPTION == string.Empty), _SCV_DESCRIPTION));
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

        #region < METODOS  ESTATICOS >

        /// <summary>
        /// Verifica la existencia de la subcategoria especificada.
        /// </summary>
        /// <param name="SCV_CO"></param>
        /// <param name="SCV_SUBCAT_ID"></param>
        /// <returns></returns>
        public static bool Existe(string SCV_CO, string SCV_SUBCAT_ID)
        {
            SubCategory sc = new SubCategory();
            sc.Load(SCV_CO, SCV_SUBCAT_ID);
            if (!sc.SCV_SUBCAT_ID.Equals(string.Empty) && !sc.SCV_CO.Equals(string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Retorna todas las subcategorias del Sistema de Información.
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllSUB_CATEGORY()
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_LOADALL_SUBCATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }
        /// <summary>
        /// Retorna todas las subcategorias del Sistema de Información.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubCategory()
        {
            ///
            DataTable ret = GetAllSUB_CATEGORY().Tables[0];
            ///
            return ret;
        }
        /// <summary>
        /// Rerorna las subcategorias de la categoria y gran categoria especificadas.
        /// </summary>
        /// <param name="scv_co"></param>
        /// <param name="msv_macat_id"></param>
        /// <param name="scv_category"></param>
        /// <returns></returns>
        public static DataTable GetSubCategory(String scv_co, String msv_macat_id, String scv_category)
        {
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_SUBCATEXCATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, scv_co, msv_macat_id, scv_category, results);
            DataTable ret = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            ///
            return ret;
        }
        /// <summary>
        /// Guarda una tabla de subcategorias.
        /// </summary>
        /// <param name="tabla"></param>
        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("SP_LOADAll_SUBCATEGORY");
            da.InsertCommand = db.GetStoredProcCommand("SP_ADD_SUB_CATEGORY");
            da.UpdateCommand = db.GetStoredProcCommand("SP_ADD_SUB_CATEGORY");
            da.DeleteCommand = db.GetStoredProcCommand("SP_DELETE_SUB_CATEGORY");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "SCV_CO", DbType.AnsiString, "SCV_CO", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "SCV_SUBCAT_ID", DbType.AnsiString, "SCV_SUBCAT_ID", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "SCV_DESCRIPTION", DbType.AnsiString, "SCV_DESCRIPTION", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "SCV_CO", DbType.AnsiString, "SCV_CO", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "SCV_SUBCAT_ID", DbType.AnsiString, "SCV_SUBCAT_ID", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "SCV_DESCRIPTION", DbType.AnsiString, "SCV_DESCRIPTION", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "SCV_CO", DbType.AnsiString, "SCV_CO", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "SCV_SUBCAT_ID", DbType.AnsiString, "SCV_SUBCAT_ID", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        /// <summary>
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="SCV_CO"></param>
        /// <param name="SCV_SUBCAT_ID"></param>
        public static void Remove(string SCV_CO, string SCV_SUBCAT_ID)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_DELETE_SUB_CATEGORY";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_SCV_CO", DbType.AnsiString, SCV_CO);
            db.AddInParameter(dbCommandWrapper, "P_SCV_SUBCAT_ID", DbType.AnsiString, SCV_SUBCAT_ID);
            ///
            db.ExecuteNonQuery(dbCommandWrapper);
        }

        #endregion
    }
}