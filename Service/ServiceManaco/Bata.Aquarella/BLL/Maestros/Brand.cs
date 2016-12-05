using System;
using System.Data.Common;
using System.Collections.Generic;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.IO;


namespace Bata.Aquarella.BLL.Maestros
{
    public class BRANDS
    {

        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < VARIABLES >
        private string _BRV_CO;
        private string _BRV_BRAND_ID;
        private string _BRV_DESCRIPTION;
        private bool _isNew;
        #endregion

        #region < CONSTRUCTORES >
        public BRANDS()
        {
            _BRV_BRAND_ID = string.Empty;
            _BRV_CO = string.Empty;
            _BRV_DESCRIPTION = string.Empty;
        }

        public BRANDS(string BRV_BRAND_ID, string BRV_CO)
            : this()
        {
            _BRV_BRAND_ID = BRV_BRAND_ID;
            _BRV_CO = BRV_CO;
        }

        public BRANDS(string BRV_CO, string BRV_BRAND_ID, string BRV_DESCRIPTION)
            : this()
        {
            _BRV_CO = BRV_CO;
            _BRV_BRAND_ID = BRV_BRAND_ID;
            _BRV_DESCRIPTION = BRV_DESCRIPTION;
        }

        #endregion


        #region < PROPIEDADES >

        public string BRV_CO
        {
            get { return _BRV_CO; }
            set { _BRV_CO = value; }
        }

        public string BRV_BRAND_ID
        {
            get { return _BRV_BRAND_ID; }
            set { _BRV_BRAND_ID = value; }
        }

        public string BRV_DESCRIPTION
        {
            get { return _BRV_DESCRIPTION; }
            set { _BRV_DESCRIPTION = value; }
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

        #region Utilerias
        public static bool Existe(string BRV_BRAND_ID, string BRV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "SP_EXISTS_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add in parameters
            db.AddInParameter(dbCommandWrapper, "P_BRV_BRAND_ID", DbType.AnsiString, BRV_BRAND_ID);
            db.AddInParameter(dbCommandWrapper, "P_BRV_CO", DbType.AnsiString, BRV_CO);
            db.AddOutParameter(dbCommandWrapper, "P_RECCOUNT_BRANDS", DbType.Int16, 4);

            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            bool ret = false;
            db.ExecuteScalar(dbCommandWrapper);
            int num = Convert.ToInt16(db.GetParameterValue(dbCommandWrapper, "P_RECCOUNT_BRANDS"));
            ret = num > 0;
            return ret;
        }
        /*
        public static DataTable GetAllBRANDS()
        {
            // CURSOR REF
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase();

            string sqlCommand = "SP_LOADALL_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,results);
            DataTable ret = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
			
            ret.PrimaryKey = new DataColumn[] { ret.Columns["BRV_BRAND_ID"], ret.Columns["BRV_CO"] };
			
            return ret;
        }
        */
        public static DataSet GetAllBRANDS()
        {
            // CURSOR REF
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADALL_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("SP_LOADALL_BRANDS");
            da.InsertCommand = db.GetStoredProcCommand("SP_ADD_BRANDS");
            da.UpdateCommand = db.GetStoredProcCommand("SP_ADD_BRANDS");
            da.DeleteCommand = db.GetStoredProcCommand("daab_DeleteBRANDS");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "COMPANY", DbType.AnsiString, "compañia", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "ID", DbType.AnsiString, "codigo", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "DESCRIPTION", DbType.AnsiString, "descripcion", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "COMPANY", DbType.AnsiString, "compañia", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "ID", DbType.AnsiString, "codigo", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "DESCRIPTION", DbType.AnsiString, "descripcion", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "BRV_BRAND_ID", DbType.AnsiString, "BRV_BRAND_ID", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "BRV_CO", DbType.AnsiString, "BRV_CO", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        #endregion

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="BRV_BRAND_ID"></param>
        /// <param name="BRV_CO"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string BRV_BRAND_ID, string BRV_CO)
        {
            // CURSOR REF
            object results = new object[1];

            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADBYPK_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, BRV_CO, BRV_BRAND_ID, results);
            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        /// <summary>
        /// Populates current instance of the object with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="BRV_BRAND_ID"></param>
        /// <param name="BRV_CO"></param>
        public void Load(string BRV_BRAND_ID, string BRV_CO)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(BRV_BRAND_ID, BRV_CO);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _BRV_CO = (string)row["BRV_CO"];
                _BRV_BRAND_ID = (string)row["BRV_BRAND_ID"];
                _BRV_DESCRIPTION = row.IsNull("BRV_DESCRIPTION") ? string.Empty : (string)row["BRV_DESCRIPTION"];

            }
            catch
            {
                _BRV_BRAND_ID = string.Empty;
                _BRV_CO = string.Empty;
                _BRV_DESCRIPTION = string.Empty;

            }
        }

        /// <summary>
        /// Adds or updates information in the database with parameters.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        public static bool Save(string BRV_CO, string BRV_BRAND_ID, string BRV_DESCRIPTION)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_ADD_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add parameters
            db.AddInParameter(dbCommandWrapper, "P_BRV_CO", DbType.AnsiString, BRV_CO);
            db.AddInParameter(dbCommandWrapper, "P_BRV_BRAND_ID", DbType.AnsiString, BRV_BRAND_ID);
            db.AddInParameter(dbCommandWrapper, "P_BRV_DESCRIPTION", DbType.AnsiString, BRV_DESCRIPTION);

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
        /// Inserts the current instance data into the database.
        /// </summary>
        /// <returns>Returns True if saved successfully, False otherwise.</returns>
        private bool Insert()
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_ADD_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _BRV_CO, _BRV_BRAND_ID, SetNullValue((_BRV_DESCRIPTION == string.Empty), _BRV_DESCRIPTION));

            // Add parameters
            //db.AddInParameter(dbCommandWrapper, "COMPANY", DbType.AnsiString, _BRV_CO);
            //db.AddInParameter(dbCommandWrapper, "ID", DbType.AnsiString, _BRV_BRAND_ID);
            //db.AddInParameter(dbCommandWrapper, "DESCRIPTION", DbType.AnsiString, _BRV_DESCRIPTION);

            db.ExecuteNonQuery(dbCommandWrapper);

            return true;
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

            string sqlCommand = "SP_ADD_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _BRV_CO, _BRV_BRAND_ID, SetNullValue((_BRV_DESCRIPTION == string.Empty), _BRV_DESCRIPTION));

            // Add parameters
            db.AddInParameter(dbCommandWrapper, "P_BRV_CO", DbType.AnsiString, _BRV_CO);
            db.AddInParameter(dbCommandWrapper, "P_BRV_BRAND_ID", DbType.AnsiString, _BRV_BRAND_ID);
            db.AddInParameter(dbCommandWrapper, "P_BRV_DESCRIPTION", DbType.AnsiString, SetNullValue((_BRV_DESCRIPTION == string.Empty), _BRV_DESCRIPTION));

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
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="BRV_BRAND_ID"></param>
        /// <param name="BRV_CO"></param>
        public static void Remove(string BRV_BRAND_ID, string BRV_CO)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_DELETE_BRANDS";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_BRV_BRAND_ID", DbType.AnsiString, BRV_BRAND_ID);
            db.AddInParameter(dbCommandWrapper, "P_BRV_CO", DbType.AnsiString, BRV_CO);

            db.ExecuteNonQuery(dbCommandWrapper);
        }

        /// <summary>
        /// Serializes the current instance data to an Xml string.
        /// </summary>
        /// <returns>A string containing the Xml representation of the object.</returns>
        virtual public string ToXml()
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(_BRV_BRAND_ID, _BRV_CO);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
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
    }
}