using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Clear
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Metodos estaticos >

        /// <summary>
        /// Crear el pre clear, cruce de pagos y liquidaciones
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_list_liquidations"></param>
        /// <param name="_list_documentrans"></param>
        /// <returns></returns>
        //Para los pedidos que seran enviados a domicilio
        public static string setPreClear(string _company, string _list_liquidations, string _list_documentrans)
        {
            string clearId = string.Empty;
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //                
                string sqlCommand = "financiera.sp_pre_clear";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _list_liquidations, _list_documentrans, clearId);
                //
                db.ExecuteNonQuery(dbCommandWrapper);                
                clearId = db.GetParameterValue(dbCommandWrapper, "p_clv_clear_id").ToString();

                return clearId;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

       //Para los pedidos que seran entregados en CEDI
        public static string setPreClearHall(string _company, string _list_liquidations, string _list_documentrans)
        {
            string clearId = string.Empty;
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //                
                string sqlCommand = "FINANCIERA.SP_PRE_CLEAR_POS";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _list_liquidations, _list_documentrans, clearId);
                //
                db.ExecuteNonQuery(dbCommandWrapper);
                clearId = db.GetParameterValue(dbCommandWrapper, "p_clv_clear_id").ToString();

                return clearId;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Cruce financiero de documentos
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_list_documentrans"></param>
        /// <returns></returns>
        public static string setClearingDoc(string _company, string _list_documentrans)
        {
            string clearId = string.Empty;
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "financiera.sp_clearing_doctrans";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _list_documentrans, clearId);
                db.ExecuteNonQuery(dbCommandWrapper);
                clearId = Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_clv_clear_id"));

                return clearId;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}