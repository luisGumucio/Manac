using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;

namespace Bata.Aquarella.BLL
{
    public class Liquidation_Dtl
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consultar detalle de liquidacion
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noLiq"></param>
        /// <returns></returns>
        public static DataSet getLiquidationDtl(string _co, string _noLiq)
        {
            try
            {
                string sqlCommand = "logistica.sp_get_liquidation_dtl";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static List<string> getMailLiquidation(string _co, string _noLiq) {

            List<string> MailLiquidation = new List<string>();
            try
            {
                string sqlCommand = "logistica.sp_mail_liquidation";

                object result = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _noLiq, result);

                DataTable mailMessageLiquidation = db.ExecuteDataSet(dbCommandWrapper).Tables[0];

                MailLiquidation.Add(mailMessageLiquidation.Rows[0][0].ToString());
                MailLiquidation.Add(mailMessageLiquidation.Rows[0][1].ToString());
                MailLiquidation.Add(mailMessageLiquidation.Rows[0][2].ToString());
                MailLiquidation.Add(mailMessageLiquidation.Rows[0][3].ToString());

                return MailLiquidation;
            }
            catch (Exception e){ throw new Exception(e.Message, e.InnerException);}
        }

        /// <summary>
        /// Método que retorna toda la informacion referente a una liquidación, 
        /// como lo pedido frente a lo liquidado por el sistema.
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noOrder"></param>
        /// <returns></returns>
        public static DataSet getOrdersVrsLiquidated(string _company, string _noLiquidation)
        {            
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "logistica.SP_GET_ORDERSVRSLIQUIDATED";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noLiquidation, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch
            {
                return null;
            }
        }
    }
}