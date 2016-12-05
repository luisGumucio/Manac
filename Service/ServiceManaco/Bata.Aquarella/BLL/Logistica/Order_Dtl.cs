
using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace Bata.Aquarella.BLL
{
    public class Order_Dtl
    {
        #region < Atributos >

        /// <summary>
        /// Código de articulo
        /// </summary>
        public string _code { get; set; }
        /// <summary>
        /// Nombre de articulo
        /// </summary>
        public string _artName { get; set; }
        /// <summary>
        /// Marca
        /// </summary>
        public string _brand { get; set; }
        /// <summary>
        /// Imagen de la marca
        /// </summary>
        public string _brandImg { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public string _color { get; set; }
        /// <summary>
        /// Talla
        /// </summary>
        public string _size { get; set; }
        /// <summary>
        /// Unidades
        /// </summary>
        public int _qty { get; set; }
        /// <summary>
        /// Unidades canceladas
        /// </summary>
        public int _qtyCancel { get; set; }
        /// <summary>
        /// Mayor categoria
        /// </summary>
        public string _majorCat { get; set; }
        /// <summary>
        /// Categoria
        /// </summary>
        public string _cat { get; set; }
        /// <summary>
        /// Sub-categoria
        /// </summary>
        public string _subcat { get; set; }
        /// <summary>
        /// Origen
        /// </summary>
        public string _origin { get; set; }
        /// <summary>
        /// Descripcion origen
        /// </summary>
        public string _originDesc { get; set; }
        /// <summary>
        /// Comision de articulo, 1-> si, 0-> No
        /// </summary>
        public int _comm { get; set; }
        /// <summary>
        /// Url de fotografia
        /// </summary>
        public string _uriPhoto { get; set; }
        /// <summary>
        /// Precio publico
        /// </summary>
        public decimal _price { get; set; }
        /// <summary>
        /// Formato moneda del precio publico
        /// </summary>
        public string _priceDesc { get; set; }
        /// <summary>
        /// Precio publico con impuesto
        /// </summary>
        public decimal _priceTax { get; set; }
        /// <summary>
        /// Formato moneda del precio publico con impuesto
        /// </summary>
        public string _priceTaxDesc { get; set; }
        /// <summary>
        /// Comision valor
        /// </summary>
        public decimal _commission { get; set; }
        /// <summary>
        /// % de comision
        /// </summary>
        public decimal _commissionPctg { get; set; }
        /// <summary>
        /// Formato de moneda del valor de la comision
        /// </summary>
        public string _commissionDesc { get; set; }
        /// <summary>
        /// Valor descuento sobre el item
        /// </summary>
        public decimal _dscto { get; set; }
        /// <summary>
        /// Formato moneda del valor del descuento item
        /// </summary>
        public string _dsctoDesc { get; set; }
        /// <summary>
        /// % de descuento item
        /// </summary>
        public decimal _dsctoPerc { get; set; }
        /// <summary>
        /// Valor de descuento item
        /// </summary>
        public decimal _dsctoVale { get; set; }
        /// <summary>
        /// Formato moneda del valor del descuento
        /// </summary>
        public string _dsctoValeDesc { get; set; }
        /// <summary>
        /// Mensaje del descuento
        /// </summary>
        public string _dsctoMsg { get; set; }
        /// <summary>
        /// Total neto de la linea
        /// </summary>
        public decimal _lineTotal { get; set; }
        /// <summary>
        /// Formato moneda del total de la linea
        /// </summary>
        public string _lineTotDesc { get; set; }
        /// <summary>
        /// Unidades
        /// </summary>
        public int _units { get; set; }
        /// <summary>
        /// Tiene unidades
        /// </summary>
        public bool _hasStock { get; set; }



        /// <summary>
        /// Nombre de conexion a bd
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;
        
        #endregion

        #region < Metodos estaticos >
        
        /// <summary>
        /// Crear una nueva linea de pedido
        /// </summary>
        /// <param name="dtArt"></param>
        /// <param name="dtDisscArt"></param>
        /// <returns></returns>
        public static Order_Dtl getNewLineOrder(DataTable dtArt, DataTable dtDisscArt)
        {
            Order_Dtl order = new Order_Dtl();

            if (dtArt == null || dtArt.Rows.Count == 0)
                return null;

            DataRow dr = dtArt.Rows[0];

            order = new Order_Dtl
            {
                _code = dr["arv_article"].ToString(),
                _artName = dr["arv_name"].ToString(),
                _brand = dr["brv_description"].ToString(),
                _brandImg = dr["brv_image"].ToString(),
                _color = dr["cov_description"].ToString(),
                _majorCat = dr["mcv_description"].ToString(),
                _cat = dr["cav_description"].ToString(),
                _subcat = dr["scv_description"].ToString(),
                _origin = dr["arv_origin"].ToString(),
                _originDesc = dr["arv_origin"].ToString().Equals(Constants.IdOriginImported) ? "Artículo importado" : "Artículo nacional",
                _comm = (int)Convert.ToInt16(dr["arn_commission"]),
                _uriPhoto = dr["arv_photo"].ToString(),
                _price = Convert.ToDecimal(dr["prn_public_price"]),
                _priceDesc = Convert.ToDecimal(dr["prn_public_price"]).ToString("C0"),
                _dsctoDesc = order._dscto.ToString("C0")
            };

            if (dtDisscArt == null || dtDisscArt.Rows.Count == 0)
                return order;

            dr = dtDisscArt.Rows[0];

            order._dscto = Convert.ToDecimal(dr["valdiscount"]);
            order._dsctoDesc = order._dscto.ToString("C0");
            order._dsctoPerc = Convert.ToDecimal(dr["porcentaje"]);
            order._dsctoVale = order._price - order._dscto; 
            order._dsctoValeDesc = order._dsctoVale.ToString("C0");
            order._dsctoMsg = dr["div_message"].ToString();

            return order;
        }

        public static Order_Dtl getNewLineOrder(DataRow dr)
        {
            Order_Dtl order = new Order_Dtl();

            order = new Order_Dtl
            {
                _code = dr["arv_article"].ToString(),
                _artName = dr["arv_name"].ToString(),
                _brand = dr["brv_description"].ToString(),
                _brandImg = dr["brv_image"].ToString(),
                _color = dr["cov_description"].ToString(),
                _majorCat = dr["mcv_description"].ToString(),
                _cat = dr["cav_description"].ToString(),
                _subcat = dr["scv_description"].ToString(),
                _origin = dr["arv_origin"].ToString(),
                _originDesc = dr["arv_origin"].ToString().Equals(Constants.IdOriginImported) ? "Artículo importado" : "Artículo nacional",
                _comm = (int)Convert.ToInt16(dr["arn_commission"]),
                _uriPhoto = dr["arv_photo"].ToString(),
                _price = Convert.ToDecimal(dr["prn_public_price"]),
                _priceDesc = Convert.ToDecimal(dr["prn_public_price"]).ToString("C0"),
                _dsctoDesc = order._dscto.ToString("C0"),
                _units = int.Parse(dr["son_qty"].ToString())
            };

            return order;
        }

        /// <summary>
        /// Consultar detalle de un pedido
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_noOrder"></param>
        /// <returns></returns>
        public static DataTable getDtlOrder(string _co,string _noOrder)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "logistica.sp_get_orderdtl";

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _noOrder, _co, results);

                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        #endregion
                
    }
}