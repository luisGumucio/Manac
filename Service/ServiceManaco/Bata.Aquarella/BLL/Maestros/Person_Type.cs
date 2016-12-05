using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bata.Aquarella.BLL.Util;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Bata.Aquarella.BLL
{
    public class Person_Type
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consultar todos los tipos de personas
        /// </summary>
        /// <param name="_co"></param>
        /// <returns></returns>
        public static DataSet getAllPersonType(string _co)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "maestros.sp_getAllPersonTypes";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }
}