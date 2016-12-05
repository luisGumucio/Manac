using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Hall_Liquidations
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Entregar una liquidación para Hall
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <param name="_idEmp"></param>
        /// <returns></returns>
        public static string addLiquidationHall(string _co, string _noLiq, decimal _idEmp)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "logistica.sp_add_hall_liquidation";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, _idEmp);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                //
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        #endregion
    }
}