using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Area
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consultar todos las areas
        /// </summary>
        /// <param name="_co"></param>
        /// <returns></returns>
        public static DataSet getAllAreas(string _co)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "maestros.sp_getaallareas";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,_co, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar 
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_region"></param>
        /// <returns></returns>
        public static DataSet getAreasByRegion(string _co, string _region)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "maestros.sp_getareasByRegion";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,_region, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }


    }
}