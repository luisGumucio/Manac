using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL.Control
{
    public class NewsLetters
    {
        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        /// <summary>
        /// Adicionar una nueva Entrada
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_tittle">Titulo de la entrada</param>
        /// <param name="_content">Contenido html de la entrada</param>
        /// <param name="_post_type">tipo de entrada</param>
        /// <param name="_post_user">usuario que publica la entrada</param>
        /// <returns>estado de la insercion</returns>
        public static bool InsertNewsLetter(string _co, string _tittle, string _content,string _estado, string _post_type, decimal _post_user, string _category) {

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "CONTROL_AQ.SP_ADD_NEWSLETTER";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _co, _tittle, _content, _estado, _post_type, _post_user, _category);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtener una entrada por su id
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_id">id de la entrada</param>
        /// <returns>DataSet con los datos de la entrada</returns>
        public static DataSet GetNewsLetterById(string _co, string _id) {

            object result = new object[1];
            
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "CONTROL_AQ.SP_GET_NEWSLETTER_BYID";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _co, _id, result);

            try
            {
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception)
            {
                throw;
            }
        
        }
        
        /// <summary>
        /// Trae un Dataset con todas las entradas
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <returns>Dataset con todas las entradas</returns>
        public static DataSet GetAllNewsLetters(string _co) {
            
            object result = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "CONTROL_AQ.SP_GETALL_NEWSLETTER";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _co, result);

            try
            {
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception)
            {
                throw;
            }        
        }

        /// <summary>
        /// Actualiza un comunicado
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_id">id de la entrada</param>
        /// <param name="_tittle">titulo</param>
        /// <param name="_content">contenido de la entrada contenido html</param>
        /// <param name="_status">estado de la entrada</param>
        /// <param name="_post_type">tipo de entrada</param>
        /// <param name="_post_user">usuario que actualiza la entrada</param>
        /// <returns></returns>
        public static bool UpdateNewsLetter(string _co, string _id, string _tittle, string _content, string _status, string _post_type, decimal _post_user, string _category) {
            
            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "CONTROL_AQ.SP_UPDATE_NEWSLETTER";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _co, _id, _tittle, _content, _status, _post_type, _post_user, _category);

            try
            {
                db.ExecuteNonQuery(dbCommandWrapper);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtener la entradas de determinada categoria
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_category">id de la categoria</param>
        /// <returns>Todas las entradas de esa categoria</returns>
        public static DataSet GetNewsLettersByCat(string _co, string _category)
        {

            object result = new object[1];

            Database db = DatabaseFactory.CreateDatabase(_conn);

            string sqlComand = "CONTROL_AQ.SP_GET_NEWSLETTERS_BYCAT";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlComand, _co, _category, result);

            try
            {
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}