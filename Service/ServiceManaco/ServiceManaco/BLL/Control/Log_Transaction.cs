using System;
using System.Data.Common;
using System.Threading.Tasks;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

// Adicionar paquete : Microsoft.CompilerServices.AsyncTargetingPack en nugetpacks


namespace Bata.Aquarella.BLL
{
    public class Log_Transaction
    {
        #region < Atributos >

        /// <summary>
        /// Client ip
        /// </summary>
        public string _ip { get; set; }

        /// <summary>
        /// Client pc name
        /// </summary>
        public string _pcName { get; set; }

        /// <summary>
        /// Client country
        /// </summary>
        public string _country { get; set; }

        /// <summary>
        /// Client region
        /// </summary>
        public string _region { get; set; }

        /// <summary>
        /// Client city
        /// </summary>
        public string _city { get; set; }


        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion
        
        public Log_Transaction(string iP, string pcName, string country, string region, string city)
        {
            this._ip = iP;
            this._pcName = pcName;
            this._country = country;
            this._region = region;
            this._city = city;
        }


        public Log_Transaction(string iP)
        {
            this._ip = iP;
        }

        public Log_Transaction()
        { }

        #region < Metodos estaticos >

        /// <summary>
        /// Insertar un log de transaccion de actividad del cliente
        /// </summary>
        /// <param name="co"></param>
        /// <param name="userId"></param>
        /// <param name="dateTx"></param>
        /// <param name="log"></param>
        /// <param name="machine"></param>
        /// <returns></returns>
        public static bool insertLogTransaction(string co,decimal userId, DateTime dateTx, string log, string machine )
        {   
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "control_aq.sp_insert_log_transaction";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, co, 12, userId, dateTx, log, machine);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Async Tasks
        /// </summary>
        public static void registerUserInfo(Users user, string log)
        {
            Task.Run(async () =>
            {
                // Inicio de una tarea que se completara despues del tiempo de vencimiento especificado (mil)
                await Task.Delay(1);

                string logPlus = " GEOINFO-> USER:" + user._usv_username.ToUpper();

                if (!string.IsNullOrEmpty(user._logTx._country))
                    logPlus += " COUNTRY:" + user._logTx._country.ToUpper() + " REGION:" + user._logTx._region.ToUpper() + " CITY:" + user._logTx._city.ToUpper();

                // Registrar el logueo
                Log_Transaction.insertLogTransaction(user._usv_co, user._usn_userid, DateTime.Now, log + logPlus, user._logTx._ip);
            });
        }


        #endregion

    }
}