using System;
using System.Data.Common;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Logistica
{
    public class exchangeStorage: Transaction
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conection = Constants.OrcleStringConn;
        
        #endregion

        #region < VARIABLES >
        String _trv_storage_out = "";
        String _trv_storage_in = "";
        # endregion

        public exchangeStorage()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        #region < PROPIEDADES >
        public string trv_storage_out
        {
            get { return _trv_storage_out; }
            set { _trv_storage_out = value; }
        }

        public string trv_storage_in
        {
            get { return _trv_storage_in; }
            set { _trv_storage_in = value; }
        }
        #endregion

        #region < METODOS >

        /// <summary>
        /// Instanciacion de la funcion estatica de salvado de documentos implicados en un cambio de 
        /// storage de un manojo de articulos.
        /// </summary>
        /// <returns></returns>
        public override String Save()
        {
            return exchangeStorage.saveDocExchangeStorage(this.trv_co,
                    this.trv_storage_out, this.trv_storage_in,
                    this._trv_docreferencia, this._trv_status, this.Items);

        }
        public override bool Anull()
        {
            return exchangeStorage.AnullExchangeStorage(this.trv_co,
                    this.trv_document);
        }

        #endregion

        #region < METODOS ESTATICOS >

        /// <summary>
        /// Método de salvado de los documentos implicados en el cambio de Storage de articulos.
        /// [0] Documento Salida, [1] Documento Entrada
        /// </summary>
        /// <param name="trv_co"></param>
        /// <param name="trv_storage"></param>
        /// <param name="trv_docreferencia"></param>
        /// <param name="trv_status"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static String saveDocExchangeStorage(string trv_co,
                    String trv_storage_out, String trv_storage_in,
                    String trv_docreferencia, String trv_status,
                    List<Transaction_det> items)
        {
            Database db = DatabaseFactory.CreateDatabase(_conection);
            String[] resultDoc = new String[2];
            string sqlCommand = "SP_ADD_EXCHANGESTORAGE";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_trv_co", DbType.String, trv_co);

            // Output parameters specify the size of the return data.
            db.AddOutParameter(dbCommandWrapper, "p_trv_document_out", DbType.String, 12);
            db.AddOutParameter(dbCommandWrapper, "p_trv_document_in", DbType.String, 12);

            // Storage de documento de salida
            db.AddInParameter(dbCommandWrapper, "p_trv_storage_out", DbType.String, trv_storage_out);
            // Storage de documento de entrada
            db.AddInParameter(dbCommandWrapper, "p_trv_storage_in", DbType.String, trv_storage_in);

            db.AddInParameter(dbCommandWrapper, "p_trv_docreferencia", DbType.String, trv_docreferencia);
            db.AddInParameter(dbCommandWrapper, "p_trv_status", DbType.String, trv_status);

           

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    // Rescatar el nuevo codigo dado al documento de salida
                    resultDoc[0] = (string)db.GetParameterValue(dbCommandWrapper, "p_trv_document_out");
                    // Rescatar el nuevo codigo dado al documento de entrada
                    resultDoc[1] = (string)db.GetParameterValue(dbCommandWrapper, "p_trv_document_in");

                    // insert the items for the two documents
                    for (int k = 0; k < resultDoc.Length; k++)
                    {
                        foreach (Transaction_det item in items)
                        {
                            sqlCommand = "SP_ADD_TRANSDETAILS";
                            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                                trv_co, resultDoc[k], item.tdv_Article, item.tdv_Size, item.tdn_Qty);
                            db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        }

                    }
                    // Commit the transaction.
                    transaction.Commit();
                }
                catch 
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc[0] = "0";
                    resultDoc[1] = "0";
                }
                connection.Close();
            }
            return resultDoc[0] + " - " + resultDoc[1];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trv_co"></param>
        /// <param name="trv_document"></param>
        /// <returns></returns>
        public static bool AnullExchangeStorage(string trv_co, string trv_document)
        {
            /*Database db = DatabaseFactory.CreateDatabase();
            bool result = false;
            string sqlCommand = "SP_ANULL_PRODUCTION";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            db.AddInParameter(dbCommandWrapper, "p_trv_co", DbType.String, trv_co);
            db.AddInParameter(dbCommandWrapper, "p_trv_document", DbType.String, trv_document);

            db.ExecuteNonQuery(dbCommandWrapper);

            result = true;

            return result;
             * */
            return false;
        }
        #endregion
    }
}