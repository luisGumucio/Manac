using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Stock
    {
        #region < Atributos >

        public string _codeItem { get; set; }
        public string _nameItem { get; set; }
        public string _brandItem { get; set; }

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < Métodos estaticos >

        /// <summary>
        /// Obtiene el Articulo determinado del Storage determinado y el Warehouse detereminado.
        /// </summary>
        /// <param name="arv_article"></param>
        /// <returns>Todos los datos del Artículo</returns>
        public static DataSet getByArticle(String company, string arv_article, String warehouse, String storage)
        {
            ///
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "SP_GETARTICLE_STOCKV_ART";
            ///
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, arv_article, warehouse, storage, results);
            DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
            return ret;
        }

        /// <summary>
        /// Obtiene el Articulo determinado con todas sus tallas en cero y costo y precio publico.
        /// </summary>
        /// <param name="arv_article"></param>
        /// <returns>Todos los datos del Artículo</returns>
        public static DataSet getByArticle(String company, string arv_article)
        {
            ///
            object results = new object[1];
            ///
            Database db = DatabaseFactory.CreateDatabase(_conn);
            ///
            string sqlCommand = "MAESTROS.SP_GETVARTICLE_FOR_SIZES";
            ///
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, arv_article, results);
            DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
            return ret;
        }


        /// <summary>
        /// Consultar el stock de un articulo; Se muestran todas las tallas del articulo y las cantidades de estas en el stock
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_idWare"></param>
        /// <param name="_idSto"> Puede enviar el _idStorage en blanco y se tomara por defecto el storage de estanterias</param>
        /// <param name="_art"></param>
        /// <returns></returns>
        public static DataSet getStockByArtWithAllSizes(string _co, string _idWare, string _idSto, string _art, string _storage)
        {
            try
            {
                //
                if (string.IsNullOrEmpty(_idSto))
                    _idSto = ValuesDB.idStorageEstanterias;

                //
                object results = new object[1];
                //
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "logistica.sp_getstockxartwithallsizes";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _idWare, _idSto, _art, _storage, results);
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idWarehouse"></param>
        /// <param name="_idStorage"></param>
        /// <returns></returns>
        public static DataTable getStockOnlyArticlesByWareAndStorage(String _company, String _idWarehouse, String _idStorage)
        {
            try
            {
                ///
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.sp_getstockarticlessbyware";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idWarehouse, _idStorage, results);
                DataTable ret = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_idWarehouse"></param>
        /// <param name="_idStorage"></param>
        /// <param name="_article"></param>
        /// <returns></returns>
        public static DataTable getStockArticleByWareAndStorage(String _company, String _idWarehouse, String _idStorage, String _article)
        {
            try
            {
                ///
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.sp_getstockarticlexwarestorage";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _idWarehouse, _idStorage, _article, results);
                DataTable ret = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar las cantidades en stock de una articulo en una talla especifica
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_code"></param>
        /// <param name="_size"></param>
        /// <param name="_ware"></param>
        /// <returns></returns>
        public static DataSet getStockArticleSize(string _co, string _code, string _size, string _ware, string _listStorages)
        {
            try
            {
                string sqlCommand = "logistica.sp_getstockarticlesize";
                // CURSOR REF
                object results = new object[1];
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _code, _size, _ware, _listStorages, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        /// <summary>
        /// Obtiene la cantidad del Articulo por tallas y todos sus datos.
        /// </summary>
        /// <param name="sov_co">Codigo de la Compañia</param>
        /// <param name="sov_warehouse">Codigo de la Bodega</param>
        /// <param name="sov_article">Codigo del articulo, puede utilizar el comodin %</param>
        /// <param name="stv_publicacces">T storages de acceso publico, F storages no publicos, TF ambos</param>
        /// <returns>Todos los datos del Artículo, tallas y cantidad</returns>
        public static DataSet getArticleStock(string sov_co, string sov_warehouse, string sov_article, string stv_publicacces, string storages)
        {
            try
            {
                ///
                object results = new object[1];
                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "logistica.SP_GETARTICLESTOCK";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, sov_co, sov_warehouse, sov_article, stv_publicacces, storages, results);
                DataSet ret = db.ExecuteDataSet(dbCommandWrapper);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
        
    }
}