﻿using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class History_Password
    {

        #region < Atributos >

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Verificar si el nuevo password ya habia sido registrado alguna vez
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_pass"></param>
        /// <param name="_co"></param>
        /// <returns></returns>
        public static bool isNewPassword(decimal _userId, string _pass, string _co)
        {
            bool ret = true;
            string newPass = Cryptographic.decrypt(_pass);
            string oldPass = "";

            DataTable pass = History_Password.getHistoryPasswords(_co, _userId).Tables[0];

            foreach (DataRow p in pass.Rows)
            {
                oldPass = Cryptographic.decrypt(p["hpv_password"].ToString());
                if (oldPass.Equals(newPass))
                {
                    ret = false;
                }
            }
            //return id history pass where company and userid search equals
            return ret;
        }

        #region < Metodos estaticos >

        /// <summary>
        /// Retorna la historia de passwords del usuario especificado.
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public static DataSet getHistoryPasswords(string _co, decimal _userId)
        {
            try
            {
                string sqlCommand = "control_aq.sp_getbyusers_hp";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///                
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _userId, _co, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static bool addPassword(string co,decimal userId, string pass, DateTime date)
        {            
            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "control_aq.SP_INSERT_HISTORY_PASSWORD";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                    co,
                    12,
                    userId,
                    pass,
                    date
                    );
                db.ExecuteNonQuery(dbCommandWrapper);                
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}