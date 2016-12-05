using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Maestros
{
    public class Concepts
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Consulta de conceptos para devolucion
        /// </summary>
        /// <param name="co"></param>
        /// <returns></returns>
        public static DataSet getReturnConcepts(string co)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "MAESTROS.sp_get_conceptsreturn";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,co, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consulta de conceptos para devolucion por tipo
        /// </summary>
        /// <param name="_co">Id compañia</param>
        /// <param name="_type">Tipo</param>
        /// <returns></returns>
        public static DataTable getConceptsByType(string _co, string _type) {
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "MAESTROS.sp_load_conceptsbytype";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _type, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }
}