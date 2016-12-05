using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Order_Hdr
    {
        #region < Atributos >

        public int _qtys { get; set; }
        public decimal _subTotal { get; set; }
        public string _subTotalDesc { get; set; }
        public decimal _taxes { get; set; }
        public string _taxesDesc { get; set; }
        public decimal _grandTotal { get; set; }
        public string _grandTotalDesc { get; set; }
        public string _totalCommDesc { get; set; }
        /// <summary>
        /// Gran total promotor
        /// </summary>
        public string _grandTotalPromDesc { get; set; }
        /// <summary>
        /// Total comision sugerida a promotor
        /// </summary>
        public string _totalCommPromDesc { get; set; }

        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        /// <summary>
        /// Guardar una lista de pedido, cabecera y detalle
        /// </summary>
        /// <param name="_co">Comapñia</param>
        /// <param name="_idCust">Id promotor asociado a la lista de pedido</param>
        /// <param name="_reference"></param>
        /// <param name="_discCommPctg"></param>
        /// <param name="_discCommValue"></param>
        /// <param name="_shipTo"></param>
        /// <param name="_specialInstr"></param>
        /// <param name="_itemsDetail"></param>
        /// <returns></returns>
        public static string saveCompleteOrder(string _co,decimal _idCust,string _reference,decimal _discCommPctg,
                                                decimal _discCommValue,string _shipTo,string _specialInstr,List<Order_Dtl> _itemsDetail)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.sp_add_order_hdr";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "p_ohv_co", DbType.String, _co);

            // Output parameters specify the size of the return data.            
            db.AddOutParameter(dbCommandWrapper, "p_ohv_order_no", DbType.String, 12);

            // Id del promotor
            db.AddInParameter(dbCommandWrapper, "p_ohn_customer_id", DbType.Decimal, _idCust);

            db.AddInParameter(dbCommandWrapper, "p_ohv_reference", DbType.String, _reference);
            db.AddInParameter(dbCommandWrapper, "p_ohn_disc_comm_pctg", DbType.Decimal, _discCommPctg);
            db.AddInParameter(dbCommandWrapper, "p_ohn_disc_comm_value", DbType.Decimal, _discCommValue);

            db.AddInParameter(dbCommandWrapper, "p_ohv_ship_to", DbType.String, _shipTo);
            db.AddInParameter(dbCommandWrapper, "p_ohv_special_instr", DbType.String, _specialInstr);

            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);

                    resultDoc = (string)db.GetParameterValue(dbCommandWrapper, "p_ohv_order_no");

                    int i=1;
                    // Recorrer todas las lineas adicionadas al detalle
                    foreach (Order_Dtl item in _itemsDetail)
                    {
                        sqlCommand = "logistica.sp_add_order_dtl";
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                            _co,
                            resultDoc,
                            i,// Linea
                            item._code,
                            item._size,
                            item._qty,
                            item._qtyCancel,
                            item._dsctoPerc,// % de descuento
                            item._dscto,// Valor de descuento
                            item._commissionPctg,// % de comision
                            item._commission//valor de comision
                            );
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        i++;
                    }
                    // Commit the transaction.
                    // La motivacion es algo personal 
                    transaction.Commit();
                }
                catch
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc = "0";
                }
                connection.Close();
            }
            return resultDoc;
        }

        public static string modifyCompleteOrder(string _co, string _noOrder, string _reference, decimal _discCommPctg,
            decimal _discCommValue, string _shipTo, string _specialInstr, List<Order_Dtl> _itemsDetail)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            string resultDoc = "";
            string sqlCommand = "logistica.sp_modifyorder";

            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);

            // Recoleccion de la informacion necesaria para crear el registro de la cabecera del pedido
            db.AddInParameter(dbCommandWrapper, "p_odv_co", DbType.String, _co);

            db.AddInParameter(dbCommandWrapper, "p_odv_order_no", DbType.String, _noOrder);

            db.AddInParameter(dbCommandWrapper, "p_ohv_reference", DbType.String, _reference);
            db.AddInParameter(dbCommandWrapper, "p_OHN_DISC_COMM_PCTG", DbType.Decimal, _discCommPctg);
            db.AddInParameter(dbCommandWrapper, "p_OHN_DISC_COMM_VALUE", DbType.Decimal, _discCommValue);


            db.AddInParameter(dbCommandWrapper, "p_OHV_SHIP_TO", DbType.String, _shipTo);
            db.AddInParameter(dbCommandWrapper, "p_OHV_SPECIAL_INSTR", DbType.String, _specialInstr);


            // Comenzar una transaccion y si todo resulta bien cerrarla realizando los cambios en la base de datos
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    int i = 1;
                    // Recorrer todas las lineas adicionadas al detalle
                    foreach (Order_Dtl item in _itemsDetail)
                    {
                        sqlCommand = "LOGISTICA.SP_ADD_ORDER_DTL";
                        dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,
                            _co,
                            _noOrder,
                            i,
                            item._code,
                            item._size,
                            item._qty,
                            item._qtyCancel,
                            item._dsctoPerc,// % de descuento
                            item._dscto,// Valor de descuento
                            item._commissionPctg,// % de comision
                            item._commission//valor de comision
                            );
                        db.ExecuteNonQuery(dbCommandWrapper, transaction);
                        i++;
                    }
                    // Commit the transaction.
                    transaction.Commit();
                    resultDoc = "1";
                }
                catch
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    resultDoc = "0";
                }
                connection.Close();
            }
            return resultDoc;
        }
    }
}