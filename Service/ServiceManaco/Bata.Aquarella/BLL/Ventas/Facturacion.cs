using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Ventas
{
    public class Facturacion
    {
        #region < Atributos >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        #endregion

        #region < MÉTODOS ESTATICOS >

        /// <summary>
        /// Generacion de factura.
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noLiquidation"></param>
        /// <param name="_pointOfSale"></param>
        /// <returns></returns>
        public static string generarFactura(String _company, String _noLiquidation, Decimal _pointOfSale)
        {
            try
            {
                String _idFactura = "";
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.SP_INVOICING";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommandWrapper, "P_IHV_CO", DbType.String, _company);
                db.AddInParameter(dbCommandWrapper, "P_IHV_LIQUIDATION", DbType.String, _noLiquidation);
                db.AddInParameter(dbCommandWrapper, "P_IHN_POINTSALE", DbType.Decimal, _pointOfSale);

                // Output parameters specify the size of the return data.            
                db.AddOutParameter(dbCommandWrapper, "P_IHV_INVOICE_NO", DbType.String, 12);
                db.ExecuteNonQuery(dbCommandWrapper);                
                _idFactura = (String)db.GetParameterValue(dbCommandWrapper, "P_IHV_INVOICE_NO");
                
                return _idFactura;
            }
            catch
            {
                return "-1";
            }
        }

        /// <summary>
        /// Consultar la cabecera de una factura
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noInvoice"></param>
        /// <param name="_noLiquidation"></param>
        /// <returns></returns>
        public static DataTable getInvoiceHdr(String _company, String _noInvoice, String _noLiquidation)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_getinvoicehdr";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noInvoice, _noLiquidation, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar detalles de la factura
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noInvoice"></param>
        /// <returns></returns>
        public static DataTable getInvoiceDtl(String _company, String _noInvoice)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_getinvoice_dtl";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noInvoice, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar Ventas por fechas agrupadas por marca y tipo de articulo, muestra tambien el stock
        /// </summary>
        /// <param name="company">Codigo Compañia</param>
        /// <param name="warehouse">Codigo de la Bodega</param>
        /// <param name="fecStart">Fecha de Inicio de la Consulta</param>
        /// <param name="fecEnd">Fecha Final de la Consulta</param>
        /// <returns></returns>
        public static DataTable getSalesForDate(string company, string warehouse, string area, DateTime fecStart, DateTime fecEnd)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "VENTAS.sp_getsales_for_warebrandtype";///"ventas.SP_GETSALES_FOR_WAREBRANDTYPE";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, warehouse, area, fecStart, fecEnd, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static string updateNoPrintsInvoice(String _company, String _noInvoice)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_updatenoprintsinvoice";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommandWrapper, "p_ihv_co", DbType.String, _company);
                db.AddInParameter(dbCommandWrapper, "p_ihv_invoice_no", DbType.String, _noInvoice);
                db.ExecuteNonQuery(dbCommandWrapper);
                
                return "1";
            }
            catch
            {
                return "-1";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable getLinesOrdersVrsLiquiVrsInvoiced(String _company, String _noLiquidation)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_get_ordersvrsliquidvrsinvoi";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noLiquidation, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Consultar la informacion de la cabecera de la factura de acuerdo a un numero de liquidacion dado
        /// </summary>
        /// <returns></returns>
        public static DataTable getHdrInvoiceByLiquidation(String _company, String _noLiquidation)
        {

            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_gethdrinvoicebyliquidation";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noLiquidation, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable searchArticleInvoice(String _company, String _noInvoice, String _article, String _size, String _customer)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_searcharticleinvoice";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noInvoice, _article, _size, _customer, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
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
        /// <param name="_noInvoice"></param>
        /// <returns></returns>
        public static DataTable getInvoiceHdrByNoInvoice(String _company, String _noInvoice)
        {   
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_gethdrinvoicebynoinvoice";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noInvoice, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable getSalesByMonthForBrand(String _company, String _warehouseId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsales_bymonthforbrand";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _warehouseId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable getSalesByWeekForBrand(String _company, String _warehouseId, String _startDate, String _endDate)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsales_byweekforbrand";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _warehouseId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta de ventas por coordinador
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_warehouseId"></param>
        /// <param name="_date_start"></param>
        /// <param name="_date_end"></param>
        /// <returns></returns>
        public static DataTable getSalesByCoordinator(String _company, String _warehouseId, String _areaId, DateTime _date_start, DateTime _date_end)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.SP_GETSALES_FOR_COORDINATOR";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _warehouseId, _areaId, _date_start, _date_end, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar ventas netas, ventas factura, margen y cantidades de los cedis, segmentado por semana.
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public static DataTable getAalesCediByWeek(String _company, String _startDate, String _endDate)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsalescedibyweek";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar articulos adquiridos por el cliente, filtrando por cliente, articulo etc.
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noInvoice"></param>
        /// <param name="_idCustomer"></param>
        /// <param name="_article"></param>
        /// <param name="_size"></param>
        /// <returns></returns>
        public static DataTable getShippedDetail(String _company, String _noInvoice, String _idCustomer,
            String _article, String _size, String _noDocReturn)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getshippeddetail";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _noInvoice, _idCustomer, _article, _size, _noDocReturn, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Consultar Ventas por fechas agrupadas por marca y tipo de articulo, muestra tambien el stock
        /// </summary>
        /// <param name="company">Codigo Compañia</param>
        /// <param name="warehouse">Codigo de la Bodega</param>
        /// <param name="fecStart">Fecha de Inicio de la Consulta</param>
        /// <param name="fecEnd">Fecha Final de la Consulta</param>
        /// <returns></returns>
        public static DataTable getSalesByCategOrArticle(String company, String warehouse, DateTime fecStart, DateTime fecEnd, int _typeReport)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsales_byopcions";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, warehouse, fecStart, fecEnd, _typeReport, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta de comportamiento de coordinadores por mes
        /// </summary>
        /// <param name="company"></param>
        /// <param name="warehouse"></param>
        /// <param name="fecStart"></param>
        /// <param name="fecEnd"></param>
        /// <returns></returns>
        public static DataTable getSalesCoordByMonth(String company, String warehouse, String areaId, DateTime fecStart, DateTime fecEnd)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsalescoordbymonth";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, company, warehouse, areaId, fecStart, fecEnd, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Detalle de ventas por coordinador 
        /// </summary>
        public static DataTable loadDetailSalesForCoordinator(String _company, String _fecStart, String _fecEnd, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "VENTAS.sp_getsales_det_coord";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _fecStart, _fecEnd, _coordId, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Sir de aquarella
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_fecStart"></param>
        /// <returns></returns>
        public static DataTable loadSirAquarella(String _company, DateTime _fecStart)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_loadsiraquarella";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _fecStart, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar ventas y devoluciones por marca y tipo de articulo
        /// </summary>       
        public static DataTable getSalesWeekBrand(String _company, String _warehouseId, String _areaId, String _startDate, String _endDate)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsales_weekbrand";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _warehouseId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta las compras y devoluciones realizadas por un coordinador, detallada por articulo y talla.
        /// </summary>        
        public static DataTable getSalesDevolByCoord(String _company, String _coordId, String _article, String _size)
        {
            ///
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getsalesdevolbycoord";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _coordId, _article, _size, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                ///
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable getSalesPromoter(String _company, String _startDate, String _endDate, String _idCoord, String _idProm)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "ventas.sp_getsalespromoter";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, _idCoord, _idProm, results);
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene las ventas totales del canal 
        /// acumulado a lo que va del dia y a lo que va de la semana
        /// Este procedimiento retorna 4 cursores
        /// </summary>
        /// <returns>Dataset con los 4 cursores de resultados</returns>
        public static DataSet getSalesChannelByCategory()
        {
            DataSet dsResult = new DataSet();
            try 
	        {
	            // CURSOR REF
                object result = new object[1];
                Database db = DatabaseFactory.CreateDatabase(_conn);
                String sqlCommand = "VENTAS.sp_getsales_channel_bycat";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, result, result, result, result);
                dsResult = db.ExecuteDataSet(dbCommandWrapper);
                return dsResult;
	        }
	        catch 
	        {		
		        return null;
	        }
        }

        /// <summary>
        /// Consulta de performance de la red de coordinadores, nacional o por region
        /// </summary>       
        public static DataTable getPerformNetworkCoord(String _company, String _wareId, String _regionId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getperfomnetworkcoord";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _regionId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
       
        public static DataTable getPerformNetworkPromoter(String _company, String _regionId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getperfomnetworkpromoter";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _regionId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPerfomNetPrombyCoord(String _company, String _startDate, String _endDate, String _coord_id)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getperfomnetprombycoord";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, _coord_id, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable getCoordAbstinence(String _company, String _wareId, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordabstinence";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
        
        public static DataTable getCoordNewVinculations(String _company, String _wareId, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordnewvinculations";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

 
        public static DataTable getCoordReentry(String _company, String _wareId, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordReentry";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }
                
        public static DataTable getCoordInactive(String _company, String _wareId, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordinactive";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getCoordiPossibleDesertion(String _company, String _wareId, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordipossibledesertion";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getCoordiDesertion(string _company, string _wareId, string _areaId, string _startDate, string _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoorddesertion";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getCoordiPrevDesertion(string _company, string _wareId, string _areaId, string _startDate, string _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getcoordprevdesertion";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _wareId, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }


        public static DataTable getPromStencil(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromstencil";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromAbstinence(String _company, String _endDate, String _coordId) {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromabstinence";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromVinculaciones(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromvinculaciones";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromReingresos(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromreingresos";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromInactive(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getprominactive ";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromPosBaja(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromposbajas";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromBajas(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getprombajas";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromActive(String _company, String _endDate, String _coordId)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromactive ";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _endDate, _coordId, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar desercion de promotores
        /// </summary>
        /// <returns></returns>
        public static DataTable getPromDesertion(String _company, String _areaId, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getpromdesertion";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _areaId, _startDate, _endDate, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consultar unn promotor, su informacion y su coordinador
        /// </summary>
        public static DataTable getPromoter(String _company, String _areaId, String _searchValue)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getPromoter";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _areaId, _searchValue, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable getPromInfo(String _company, String _areaId, String _varConsult)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "ventas.sp_getprominfo";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _areaId, _varConsult, results);
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                //
                return dtResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Realizar una cosula de un datatable mediante linq y algunos campos especificos
        /// </summary>
        public static DataTable getFilterLinq(Object dtObj, String filter)
        {
            try
            {
                DataTable dt = (DataTable)dtObj;
                ///
                return (from x in dt.AsEnumerable()
                        where
                        x.Field<String>("nombrecompleto").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("nombrecompletoc").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("area").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("ubicationcustomer").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("ubicationcustomerc").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("cedulac").ToUpper().Contains(filter.Trim().ToUpper()) ||
                        x.Field<String>("cedulap").ToUpper().Contains(filter.Trim().ToUpper())
                        select x).CopyToDataTable();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Consulta de ventas y devoluciones por separadas.
        /// </summary>
        /// <returns>Varios cursores en un dataset</returns>
        public static DataSet getAuditSalesReturns(String _company, String _startDate, String _endDate)
        {
            DataTable dtResult = new DataTable();
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                String sqlCommand = "VENTAS.sp_getAuditSalesReturns";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _company, _startDate, _endDate, results, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Consulta de ventas por semana y categoria
        /// </summary>
        public static DataSet getSalesForCatByWeek(string _co, string _ware, string _area,string _region, DateTime _startDate, DateTime _endDate)
        {
            ///            
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "ventas.sp_getsales_for_catbyweek";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _area,_region, _startDate, _endDate, results);
                ///
               return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Consultar las ventas por gran-categoria y categoria, por semana y año.
        /// </summary>
        public static DataSet getSalesCategoWeekYear(string _co, DateTime _startDate)
        {
            ///            
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                ///
                string sqlCommand = "ventas.sp_getsalescatego_weekyear";
                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _startDate, results, results);
                ///
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Consulta de ventas generales o solo hall
        /// </summary>
        /// <param name="type">1-> Solo ventas Hall, 2-> Ventas totales generales</param>
        /// <returns></returns>
        public static DataSet getSalesHallOrGral(string _co, string _ware, string _area,string _region, string _startDate, string _endDate, int type)
        {
            //         
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "ventas.sp_getsales_hallorgral";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _area,_region, _startDate, _endDate, type, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Consulta de ventas Coordinador o promotor. 
        /// </summary>
        /// <param name="_type">1 - Coordinador, 2 - Promotor</param>
        /// <returns>DataSet Consulta</returns>
        public static DataTable getSalesCustByWeek(string _co, string _ware, string _startDate, string _endDate, int _type) {
            try
            {
                object result = new Object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                
                string sqlCommand = "ventas.sp_getsales_custbyweek";

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _ware, _startDate, _endDate, _type, result);
                

                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception ex)
            {   
                throw ex;
            }
        }

        /// <summary>
        /// Consulta los clientes ha ser recategorizados.
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_startDate">fecha de inicio</param>
        /// <param name="_endDate">fecha final</param>
        /// <returns>DataTable con los usuarios recategorizados</returns>
        public static DataTable getCategorizeCustomersByDates(string _co, string _startDate, string _endDate) {
            try
            {
                object result = new Object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                
                string sqlCommand = "ventas.sp_get_categorizecustomer";

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _startDate, _endDate, result);

                return db.ExecuteDataSet(dbCommandWrapper).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Realiza la recategorizacion por fechas
        /// </summary>
        /// <param name="_co">id de la compañia</param>
        /// <param name="_startDate">Fecha de inicio</param>
        /// <param name="_endDate">Fecha final</param>
        /// <returns>bool estado de la actualizacion.</returns>
        public static bool ReCategorizeCustomersByDates(string _co, string _startDate, string _endDate) {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(_conn);
                
                string sqlCommand = "ventas.sp_categorizecoordinators_v2";

                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _startDate, _endDate);

                int result = db.ExecuteNonQuery(dbCommandWrapper);

                if (result > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion 
    }
}