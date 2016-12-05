using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Discounts
    {

        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Métodos estaticos >

        /// <summary>
        /// Consultar el descuento sobre un articulo en especifico
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_code"></param>
        /// <returns></returns>
        public static DataSet getArticleDiscount(string _co, string _code)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                //
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "ventas.sp_articlediscount";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _code, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch { return null; }
        }

        #endregion        
    }
}