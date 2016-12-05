﻿using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Admonred
{
    public class Coordinator_Type
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion


        #region < Métodos estaticos >

        /// <summary>
        /// Método de consulta de comisiones de los coordinadores sobre los diferentes articulos
        /// </summary>
        /// <returns></returns>
        public static DataSet getAllCoordinatorType(string _company)
        {
            try
            {
                /// CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "sp_getallcoordinatorstype";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Metodo de consulta de tipo de clientes solo promotor directo
        /// </summary>
        /// <param name="_company"></param>
        /// <returns></returns>
        public static DataSet getCoordinatorsPromoterType(string _company)
        {
            try
            {
                /// CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ADMONRED.SP_GETCOORD_PROM_TYPE";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
    }
}