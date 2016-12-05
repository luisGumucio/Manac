using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Promoter
    {
        #region < Atributos >

        public decimal _idPromoter { get; set; }
        public decimal _idCoord { get; set; }
        public string _name { get; set; }
        public string _address { get; set; }
        public string _document { get; set; }

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;


        #endregion
        
        #region < Metodos estaticos >

        /// <summary>
        /// Consultar todos los promotores asociados a un coordinador
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idCoord"></param>
        /// <returns></returns>
        public static DataTable getAllPromotersByCoordinator(string _co, decimal _idCoord)
        {
            try
            {
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "admonred.sp_loadallpromoterByCoord";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idCoord, results);
                DataTable dtaTableResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                // Retornar el datatable
                return dtaTableResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static List<Promoter> setListPromoters(DataTable dt)
        {
            List<Promoter> lstPromoters = new List<Promoter>();

            if (dt == null || dt.Rows.Count == 0)
                return lstPromoters;

            foreach (DataRow dr in dt.Rows)
            {
                lstPromoters.Add(new Promoter
                {
                    _idPromoter = Convert.ToDecimal(dr["prn_promoter_id"]),
                    _idCoord = Convert.ToDecimal(dr["prn_promoter_id"]),
                    _name = dr["nombrecompleto"].ToString(),
                    _address = dr["bdv_phone"].ToString(),
                    _document = dr["bdv_document_no"].ToString()
                });
            }

            return lstPromoters;
        }

        /// <summary>
        /// Inactivar un promotor
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_promoter"></param>
        /// <param name="_coordinator"></param>
        /// <returns></returns>
        public static string deletePromoter(string _co, decimal _promoter, decimal _coordinator)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                //
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "admonred.sp_deletepromoter";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _promoter, _coordinator);                
                db.ExecuteNonQuery(dbCommandWrapper);
                return "1";
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idProm"></param>
        /// <param name="_idCoor"></param>
        /// <returns></returns>
        public static string addPromoter(string _co, decimal _idProm, decimal _idCoor)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);            
            string sqlCommand = "admonred.sp_add_promoter";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "P_PRV_CO", DbType.String, _co);
            ///
            db.AddInParameter(dbCommandWrapper, "P_PRN_COORDINATOR_ID", DbType.Decimal, _idCoor);
            ///
            db.AddInParameter(dbCommandWrapper, "P_PRN_PROMOTER_ID", DbType.Decimal, _idProm);

            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    // Commit the transaction.
                    transaction.Commit();
                    return "1";
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    throw new Exception(e.Message, e.InnerException);
                }
                //connection.Close();
            }

        }

        /// <summary>
        /// Consulta de promotores por cedula
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_doc"></param>
        /// <returns></returns>
        public static DataTable getPromoter(string _co, string _doc)
        {
            try
            {
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "admonred.sp_getpromoter";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _doc, results);
                DataTable dtaTableResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                // Retornar el datatable
                return dtaTableResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Cambiar promotor de coordinador
        /// </summary>
        /// <param name="company"></param>
        /// <param name="promoter"></param>
        /// <param name="newCoord"></param>
        /// <returns></returns>
        public static void updatePromCoord(string company, decimal promoter, decimal newCoord)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "admonred.sp_updatepromoter";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, promoter, newCoord);
                ///DbCommand dbCommandWrapper = db.GetSqlStringCommand(sqlCommand);
                db.ExecuteNonQuery(dbCommandWrapper);                
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Convertir un promotor en un cliente directo
        /// </summary>
        /// <param name="company"></param>
        /// <param name="promoter"></param>
        /// <param name="typeCoord"></param>
        /// <param name="area"></param>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static void upgradePromoter(string company, decimal promoter, string typeCoord, string area, string mail)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = " ADMONRED.SP_UPGRADEPROMOTER";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, promoter, typeCoord, area, mail);
                ///DbCommand dbCommandWrapper = db.GetSqlStringCommand(sqlCommand);
                db.ExecuteNonQuery(dbCommandWrapper);                
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion

    }
}