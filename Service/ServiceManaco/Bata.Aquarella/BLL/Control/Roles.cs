using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Bata.Aquarella.BLL.Control
{
    public class Roles
    {
        
        #region <Atributos>

        private string _ROV_CO { get; set; }
        private decimal _RON_ID { get; set; }
        private string _ROV_NAME { get; set; }
        private string _ROV_DESCRIPTION { get; set; }      

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;
        #endregion
        #region <Metodos Estaticos>
        /// <summary> Insaertar un Rol
        /// </summary>
        /// <param name="ROV_CO">identificador de la compañia</param>
        /// <param name="ROV_NAME">Nombre del rol</param>
        /// <param name="ROV_DESCRIPTION">descripcion</param>
        /// <returns>bool</returns>
        public static bool insertRole(string ROV_CO, string ROV_NAME, string ROV_DESCRIPTION)
        {   
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "control_aq.sp_insert_roles";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, ROV_CO, ROV_NAME, ROV_DESCRIPTION);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception){ return false; }
        }

        /// <summary> Actualiza informacion del rol
        /// </summary>
        /// <param name="ROV_CO">company</param>
        /// <param name="RON_ID">id rol</param>
        /// <param name="ROV_NAME">nombre Rol</param>
        /// <param name="ROV_DESCRIPTION">Descripcion Rol</param>
        /// <returns>bool estado del update. </returns>
        public static bool updateRole(string ROV_CO, int RON_ID, string ROV_NAME, string ROV_DESCRIPTION)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string sqlCommand = "SP_UPDATE_ROLES";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "P_ROV_CO", DbType.String, ROV_CO);
            db.AddInParameter(dbCommandWrapper, "P_RON_ID", DbType.Decimal, RON_ID);
            db.AddInParameter(dbCommandWrapper, "P_ROV_NAME", DbType.String, ROV_NAME.Trim());
            db.AddInParameter(dbCommandWrapper, "P_ROV_DESCRIPTION", DbType.String, ROV_DESCRIPTION.Trim());

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>Obtener lista de roles
        /// </summary>
        /// <returns>Data Set con los roles</returns>
        public static DataSet getRoles()
        {
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "control_aq.SP_LOADALL_ROLES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        /// <summary> Obtener los roles por usuarios
        /// </summary>
        /// <param name="USN_USERID"></param>
        /// <param name="USV_CO"></param>
        /// <returns>Data Set con roles de usuario</returns>
        public static DataSet GetRolesByUser(decimal USN_USERID, string USV_CO)
        {
            object results = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_LOADROLES_US";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,USN_USERID,USV_CO,results);
            return db.ExecuteDataSet(dbCommandWrapper);
        }

        /// <summary> Insertar User role
        /// </summary>
        /// <param name="_URN_ROLEID"></param>
        /// <param name="_URN_USERID"></param>
        /// <param name="_URV_CO"></param>
        /// <returns>bool estado de la insercion</returns>
        public static bool insertUserRole(decimal _URN_ROLEID, decimal _URN_USERID, string _URV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_ADD_USERS_ROLES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _URN_ROLEID, _URN_USERID, _URV_CO);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception) { return false; }

        }

        public static bool deleteUserRole(decimal _URN_ROLEID, decimal _URN_USERID, string _URV_CO)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlCommand = "control_aq.SP_DELETE_USERS_ROLES";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _URN_ROLEID, _URN_USERID, _URV_CO);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception) { return false; }
        }
        #endregion


    }
}