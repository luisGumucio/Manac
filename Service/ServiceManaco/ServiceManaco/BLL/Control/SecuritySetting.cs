using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Bata.Aquarella.BLL
{
    public class SecuritySetting
    {
        #region < Atributos >

        /// <summary>
        /// Número de intentos.
        /// </summary>
        public int _sen_attempts { set; get; }
        /// <summary>
        ///  Dias de Caducidad de la contraseña.
        /// </summary>
        public int _sen_expirationTime { set; get; }
        /// <summary>
        /// Horas en que se desbloqueara la cuenta.
        /// </summary>
        public int _sen_releaseTime { set; get; }
        /// <summary>
        /// Estado que indica si se captura log de intento de credenciales invalidas.
        /// </summary>
        public string _sev_captureLog { set; get; }
        /// <summary>
        /// Esto que indica si se envia correo para indicar caducidad de contraseña.
        /// </summary>
        public string _sev_sendmail { set; get; }
        /// <summary>
        /// Tipo de usuario que se le aplicará caducidad de la contraseña.
        /// </summary>
        public string _sev_usertype { set; get; }
       

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos publicos >

     

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Consultar la configuración de seguridad
        /// </summary>
        /// <param name="usv_username"></param>
        /// <returns></returns>
        public static DataSet getSecuritySetting()
        {
            try
            {
                string sqlCommand = "control_aq.sp_get_securitysetting";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        

        #endregion
    }
}