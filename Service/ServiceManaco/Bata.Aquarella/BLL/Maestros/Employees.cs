using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Employees
    {
        #region < Atributos >
        
        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region <  Métodos estaticos >

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_profesion"></param>
        /// <returns></returns>
        public static DataTable getEmployeesByCharge(string _company, string _profesion, string _idWarehouse)
        {
            DataTable dtResult = new DataTable();
            try
            {
                /// CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///String sqlCommand = "sp_getHdrOrder";
                ///
                string sqlCommand = "MAESTROS.sp_getEmployeesByCharge";
                   
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _profesion, _idWarehouse, results);
                //
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Consultar la bodega a la cual pertenece un empleado
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idUser"></param>
        /// <returns></returns>
        public static DataTable getEmployeeWarehouse(string _company, decimal _idUser)
        {
            DataTable dtResult = new DataTable();
            try
            {
                /// CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "maestros.sp_getemployeewarehouse";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idUser, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idUser"></param>
        /// <returns></returns>
        public static DataTable getEmployeeByPK(string _company, decimal _idUser)
        {
            DataTable dtResult = new DataTable();
            try
            {
                /// CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "maestros.sp_getemployeebypk";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idUser, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }



        #endregion
    }
}