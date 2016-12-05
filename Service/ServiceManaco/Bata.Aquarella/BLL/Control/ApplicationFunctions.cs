using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;

namespace Bata.Aquarella.BLL.Control
{
    public class ApplicationFunctions
    {
        public int _id { set; get; }
        public string _name { set; get; }
        public string _description { set; get; }
        public int _idpadre { set; get; }
        public string _image { set; get; }
        public string _url { set; get; }
        public string _comments { set; get; }

        public static string _conn = Constants.OrcleStringConn;

        private static DataSet _data;
        /// <summary>
        /// Retorna todos los roles del Usurio especificado.
        /// </summary>
        /// <param name="USN_USERID"></param>
        /// <param name="USV_CO"></param>
        /// <returns></returns>
        public static List<ApplicationFunctions> getFunctions_tree(string p_type, string p_co,decimal p_userid)
        {
            try
            {
            
            List<ApplicationFunctions> colappfunctions = new List<ApplicationFunctions>();
            _data = new DataSet();
            // CURSOR REF
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "CONTROL_AQ.sp_getfunctions_tree";
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, p_type, p_co, p_userid, results);
            //return ALL APPLICATIONS COMPANY AND FUCTION REQUIRED 
            _data = db.ExecuteDataSet(dbCommandWrapper);
            foreach(DataRow row in _data.Tables[0].Rows)
            {
             
            colappfunctions.Add(
                new ApplicationFunctions{
                    _id = Int32.Parse(row["fun_id"].ToString()),
                    _name = row["fuv_name"].ToString(),
                    _description = row["fuv_description"].ToString(),
                    _idpadre = Int32.Parse(row["fun_father"].ToString()),
                    _image = row["fun_image"].ToString(),
                    _url = row["apv_url"].ToString(),
                    _comments = row["apv_comments"].ToString()
                  }
                );
            }

            return colappfunctions;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }
    }


    
}