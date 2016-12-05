using System;
using System.Data;
using System.Data.Common;
using System.IO;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Logistica
{
    public class warehouses
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < VARIABLES >
        private string _WAV_CO;
        private string _WAV_WAREHOUSEID;
        private string _WAV_DESCRIPTION;
        private string _WAV_ADDRESS;
        private string _WAV_TELEPHONES;
        private string _WAV_CITY;
        private string _WAV_STATUS;
        private string _WAN_CAPACITY;

        private bool _isNew;
        #endregion

        #region < CONSTRUCTORES >
        protected warehouses()
        {
            _WAV_CO = string.Empty;
            _WAV_WAREHOUSEID = string.Empty;
            _WAV_DESCRIPTION = string.Empty;
            _WAV_ADDRESS = string.Empty;
            _WAV_TELEPHONES = string.Empty;
            _WAV_CITY = string.Empty;
            _WAV_STATUS = string.Empty;
            _WAN_CAPACITY = string.Empty;
        }

        public warehouses(string WAV_CO, string WAV_WAREHOUSEID)
            : this()
        {
            _WAV_CO = WAV_CO;
            _WAV_WAREHOUSEID = WAV_WAREHOUSEID;
        }

        public warehouses(string WAV_CO, string WAV_WAREHOUSEID, string WAV_DESCRIPTION, string WAV_ADDRESS, string WAV_TELEPHONES, string WAV_CITY, string WAV_STATUS, string WAN_CAPACITY)
            : this()
        {
            _WAV_CO = WAV_CO;
            _WAV_WAREHOUSEID = WAV_WAREHOUSEID;
            _WAV_DESCRIPTION = WAV_DESCRIPTION;
            _WAV_ADDRESS = WAV_ADDRESS;
            _WAV_TELEPHONES = WAV_TELEPHONES;
            _WAV_CITY = WAV_CITY;
            _WAV_STATUS = WAV_STATUS;
            _WAN_CAPACITY = WAN_CAPACITY;
        }

        public warehouses(string WAV_CO, string WAV_WAREHOUSEID, string WAV_DESCRIPTION)
            : this()
        {
            _WAV_CO = WAV_CO;
            _WAV_WAREHOUSEID = WAV_WAREHOUSEID;
            _WAV_DESCRIPTION = WAV_DESCRIPTION;

        }

        #endregion

        #region < PROPIEDADES >

        public string WAV_CO
        {
            get { return _WAV_CO; }
            set { _WAV_CO = value; }
        }

        public string WAV_WAREHOUSEID
        {
            get { return _WAV_WAREHOUSEID; }
            set { _WAV_WAREHOUSEID = value; }
        }

        public string WAV_DESCRIPTION
        {
            get { return _WAV_DESCRIPTION; }
            set { _WAV_DESCRIPTION = value; }
        }

        public string WAV_ADDRESS
        {
            get { return _WAV_ADDRESS; }
            set { _WAV_ADDRESS = value; }
        }

        public string WAV_TELEPHONES
        {
            get { return _WAV_TELEPHONES; }
            set { _WAV_TELEPHONES = value; }
        }

        public string WAV_CITY
        {
            get { return _WAV_CITY; }
            set { _WAV_CITY = value; }
        }

        public string WAV_STATUS
        {
            get { return _WAV_STATUS; }
            set { _WAV_STATUS = value; }
        }

        public string WAN_CAPACITY
        {
            get { return _WAN_CAPACITY; }
            set { _WAN_CAPACITY = value; }
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
        /// <param name="WAV_CO"></param>
        /// <param name="WAV_WAREHOUSEID"></param>
        public virtual void Load(string WAV_CO, string WAV_WAREHOUSEID)
        {
            // DataSet that will hold the returned results		
            DataSet ds = this.LoadByPrimaryKey(WAV_CO, WAV_WAREHOUSEID);
            try
            {
                // Load member variables from datarow
                DataRow row = ds.Tables[0].Rows[0];
                _WAV_CO = (string)row["WAV_CO"];
                _WAV_WAREHOUSEID = (string)row["WAV_WAREHOUSEID"];
                _WAV_DESCRIPTION = row.IsNull("WAV_DESCRIPTION") ? string.Empty : (string)row["WAV_DESCRIPTION"];
            }
            catch
            {
                _WAV_CO = string.Empty;
                _WAV_WAREHOUSEID = string.Empty;
                _WAV_DESCRIPTION = string.Empty;
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
            DataSet ds = this.LoadByPrimaryKey(_WAV_CO, _WAV_WAREHOUSEID);
            StringWriter writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }

        /// <summary>
        /// Realizar la consulta, sobre un metodo privado, de todas la informacion
        /// de las bodegas.
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idWareHouse"></param>
        /// <returns></returns>
        public DataTable getWarehouseByPk()
        {
            ///
            try
            {
                ///
                return this.LoadByPrimaryKey(_WAV_CO, _WAV_WAREHOUSEID).Tables[0];
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region < METODOS PRIVADOS >

        /// <summary>
        /// Populates a dataset with info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="WAV_CO"></param>
        /// <param name="WAV_WAREHOUSEID"></param>
        /// <returns>A DataSet containing the results of the query</returns>
        private DataSet LoadByPrimaryKey(string WAV_CO, string WAV_WAREHOUSEID)
        {
            // CURSOR REF
            object results = new object[1];

            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADBYPRIMARYKEY_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, WAV_CO, WAV_WAREHOUSEID, results);
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

            string sqlCommand = "SP_ADD_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add parameters
            db.AddInParameter(dbCommandWrapper, "P_WAV_CO", DbType.AnsiString, _WAV_CO);
            db.AddInParameter(dbCommandWrapper, "P_WAV_WAREHOUSEID", DbType.AnsiString, _WAV_WAREHOUSEID);
            db.AddInParameter(dbCommandWrapper, "P_WAV_DESCRIPTION", DbType.AnsiString, SetNullValue((_WAV_DESCRIPTION == string.Empty), _WAV_DESCRIPTION));
            db.AddInParameter(dbCommandWrapper, "P_WAV_ADDRESS", DbType.AnsiString, WAV_ADDRESS);
            db.AddInParameter(dbCommandWrapper, "P_WAV_TELEPHONES", DbType.AnsiString, WAV_TELEPHONES);
            db.AddInParameter(dbCommandWrapper, "P_WAV_CITY", DbType.AnsiString, WAV_CITY);
            db.AddInParameter(dbCommandWrapper, "P_WAV_STATUS", DbType.AnsiString, WAV_STATUS);
            db.AddInParameter(dbCommandWrapper, "P_WAN_CAPACITY", DbType.AnsiString, WAN_CAPACITY);

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
            ///
            string sqlCommand = "SP_ADD_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
            // Add parameters
            db.AddInParameter(dbCommandWrapper, "P_WAV_CO", DbType.AnsiString, _WAV_CO);
            db.AddInParameter(dbCommandWrapper, "P_WAV_WAREHOUSEID", DbType.AnsiString, _WAV_WAREHOUSEID);
            db.AddInParameter(dbCommandWrapper, "P_WAV_DESCRIPTION", DbType.AnsiString, SetNullValue((_WAV_DESCRIPTION == string.Empty), _WAV_DESCRIPTION));
            db.AddInParameter(dbCommandWrapper, "P_WAV_ADDRESS", DbType.AnsiString, WAV_ADDRESS);
            db.AddInParameter(dbCommandWrapper, "P_WAV_TELEPHONES", DbType.AnsiString, WAV_TELEPHONES);
            db.AddInParameter(dbCommandWrapper, "P_WAV_CITY", DbType.AnsiString, WAV_CITY);
            db.AddInParameter(dbCommandWrapper, "P_WAV_STATUS", DbType.AnsiString, WAV_STATUS);
            db.AddInParameter(dbCommandWrapper, "P_WAN_CAPACITY", DbType.AnsiString, WAN_CAPACITY);
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

        #endregion

        #region < METODOS ESTATICOS >

        public static bool Existe(string WAV_CO, string WAV_WAREHOUSEID)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "SP_EXISTS_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add in parameters
            db.AddInParameter(dbCommandWrapper, "P_WAV_CO", DbType.AnsiString, WAV_CO);
            db.AddInParameter(dbCommandWrapper, "P_WAV_WAREHOUSEID", DbType.AnsiString, WAV_WAREHOUSEID);
            db.AddOutParameter(dbCommandWrapper, "P_RECCOUNT_WAREHOUSES", DbType.Int16, 4);

            // DataSet that will hold the returned results		
            // Note: connection closed by ExecuteDataSet method call 
            bool ret = false;
            db.ExecuteScalar(dbCommandWrapper);
            int num = Convert.ToInt16(db.GetParameterValue(dbCommandWrapper, "P_RECCOUNT_WAREHOUSES"));
            ret = num > 0;
            return ret;
        }

        public static DataSet GetAllWAREHOUSES()
        {
            // CURSOR REF
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_LOADALL_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);

        }

        public static bool Save(string WAV_CO, string WAV_WAREHOUSEID, string WAV_DESCRIPTION, string WAV_ADDRESS, string WAV_TELEPHONES, string WAV_CITY, string WAV_STATUS, string WAN_CAPACITY)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_ADD_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add parameters
            db.AddInParameter(dbCommandWrapper, "P_WAV_CO", DbType.AnsiString, WAV_CO);
            db.AddInParameter(dbCommandWrapper, "P_WAV_WAREHOUSEID", DbType.AnsiString, WAV_WAREHOUSEID);
            db.AddInParameter(dbCommandWrapper, "P_WAV_DESCRIPTION", DbType.AnsiString, WAV_DESCRIPTION);
            db.AddInParameter(dbCommandWrapper, "P_WAV_ADDRESS", DbType.AnsiString, WAV_ADDRESS);
            db.AddInParameter(dbCommandWrapper, "P_WAV_TELEPHONES", DbType.AnsiString, WAV_TELEPHONES);
            db.AddInParameter(dbCommandWrapper, "P_WAV_CITY", DbType.AnsiString, WAV_CITY);
            db.AddInParameter(dbCommandWrapper, "P_WAV_STATUS", DbType.AnsiString, WAV_STATUS);
            db.AddInParameter(dbCommandWrapper, "P_WAN_CAPACITY", DbType.AnsiString, WAN_CAPACITY);
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

        public static void SaveAll(DataTable tabla)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            DbDataAdapter da = db.GetDataAdapter();

            da.SelectCommand = db.GetStoredProcCommand("daab_GetAllWAREHOUSES");
            da.InsertCommand = db.GetStoredProcCommand("daab_AddWAREHOUSES");
            da.UpdateCommand = db.GetStoredProcCommand("daab_UpdateWAREHOUSES");
            da.DeleteCommand = db.GetStoredProcCommand("daab_DeleteWAREHOUSES");

            #region Parametros de InsertCommand
            db.AddInParameter(da.InsertCommand, "WAV_CO", DbType.AnsiString, "WAV_CO", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_WAREHOUSEID", DbType.AnsiString, "WAV_WAREHOUSEID", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_DESCRIPTION", DbType.AnsiString, "WAV_DESCRIPTION", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_ADDRESS", DbType.AnsiString, "WAV_ADDRESS", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_TELEPHONES", DbType.AnsiString, "WAV_TELEPHONES", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_CITY", DbType.AnsiString, "WAV_CITY", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAV_STATUS", DbType.AnsiString, "WAV_STATUS", DataRowVersion.Default);
            db.AddInParameter(da.InsertCommand, "WAN_CAPACITY", DbType.AnsiString, "WAN_CAPACITY", DataRowVersion.Default);

            #endregion

            #region Parametros de UpdateCommand
            db.AddInParameter(da.UpdateCommand, "WAV_CO", DbType.AnsiString, "WAV_CO", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "WAV_WAREHOUSEID", DbType.AnsiString, "WAV_WAREHOUSEID", DataRowVersion.Default);
            db.AddInParameter(da.UpdateCommand, "WAV_DESCRIPTION", DbType.AnsiString, "WAV_DESCRIPTION", DataRowVersion.Default);

            #endregion

            #region Parametros de DeleteCommand
            db.AddInParameter(da.DeleteCommand, "WAV_CO", DbType.AnsiString, "WAV_CO", DataRowVersion.Default);
            db.AddInParameter(da.DeleteCommand, "WAV_WAREHOUSEID", DbType.AnsiString, "WAV_WAREHOUSEID", DataRowVersion.Default);

            #endregion

            db.UpdateDataSet(tabla.DataSet, tabla.TableName, da.InsertCommand, da.UpdateCommand, da.DeleteCommand, UpdateBehavior.Standard);
        }
        /// <summary>
        /// Removes info from the database, based on the requested primary key.
        /// </summary>
        /// <param name="WAV_CO"></param>
        /// <param name="WAV_WAREHOUSEID"></param>
        public static void Remove(string WAV_CO, string WAV_WAREHOUSEID)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "SP_DELETE_WAREHOUSES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Add primary keys to command wrapper.
            db.AddInParameter(dbCommandWrapper, "P_WAV_CO", DbType.AnsiString, WAV_CO);
            db.AddInParameter(dbCommandWrapper, "P_WAV_WAREHOUSEID", DbType.AnsiString, WAV_WAREHOUSEID);

            db.ExecuteNonQuery(dbCommandWrapper);
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

        /// <summary>
        /// Consultar todas las bodegas deacuerdo a una compañia
        /// </summary>
        /// <param name="_company"></param>
        /// <returns></returns>
        public static DataSet getWarehousesByCo(String _company)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "SP_GET_WAREHOUSES_BYCO";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, results);
                ///
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
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static DataTable nearestGeographicCedis(String _company, String latitude, String longitude)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_get_nearestcedisgs";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, latitude, longitude, results);
                ///
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
        /// <param name="_type">2,3,4 -> tipos de bodega</param>
        /// <returns></returns>
        public static DataTable getAllWaresByType(String _company)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "logistica.sp_getwarehousesbytype";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch
            {
                return null;
            }
        }


        #endregion
    }
}