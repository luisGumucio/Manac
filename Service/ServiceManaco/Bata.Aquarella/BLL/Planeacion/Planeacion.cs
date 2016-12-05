using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data.Common;

namespace Bata.Aquarella.BLL.Planning
{
    public class Planeacion
    {
        public static string _conn = Constants.OrcleStringConn;

        public static string addPreOrder(string _co, string _coord_id, string _article, string _cant)
        {
            try
            {
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "PLANEACION.SP_ADD_PREORDER";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _coord_id, _article, _cant);
                ///
                db.ExecuteNonQuery(dbCommandWrapper);
                ///
                return "1";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}