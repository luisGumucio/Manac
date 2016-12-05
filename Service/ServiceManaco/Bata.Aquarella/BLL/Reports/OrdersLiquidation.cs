using System;

namespace Bata.Aquarella.BLL.Reports
{
    public class OrdersLiquidation
    {
        #region < ATRIBUTOS >
        /// <summary>
        /// Atributos, información del coordinador
        /// </summary>
        public string CON_COORDINATOR_ID;
        public string bdv_email;
        public string completeNameCustomer;
        public string BDV_DOCUMENT_NO;
        public string BDV_ADDRESS;
        public string BDV_PHONE;
        public string ubicationCustomer;


        /// <summary>
        /// Atributos de la cabecera de la liquidacion
        /// </summary>
        public string LHV_CO;
        public string LHV_LIQUIDATION_NO;
        public DateTime LHD_DATE;
        public decimal LHN_CUSTOMER;
        public decimal LHN_DISSCOUNT;
        public decimal LHN_COMMISSION;
        public decimal LHN_TAX_RATE;
        public decimal LHN_HANDLING;
        public decimal LHN_VAL_INIT;        

        /// <summary>
        /// Atributos del detalle de la liquidacion
        /// </summary>
        public string ARV_NAME;
        public string LDV_ARTICLE;
        public string LDV_SIZE;
        public int LDN_QTY;
        public decimal LDN_DISSCOUNT;
        public decimal LDN_COMMISSION;
        public decimal LDN_SELL_PRICE;
        public decimal valorNetoLinea;

        /// <summary>
        /// Atributos, totalizacion de la liquidacion
        /// </summary>
        public decimal subtotal;
        public decimal dctoGeneralTotal;
        public decimal valorIva;
        public decimal valorTotalPagar;
        public decimal comisionLineal; 
        public decimal dsctosLineales;
        

        /// <summary>
        /// Atributos utilizados para la creacion de la tabla de tallas del formato bata.
        /// </summary>
        public string columna;
        public string fila;
        public string tallaDisplay;
        ///
        public string filaArticulo;


        /// <summary>
        /// Atributos promotor
        /// </summary>
        public string idPromotor;
        public string namePromotor;
        public string cedulaPromotor;
        public string instrEspeciales;

        /// <summary>
        /// Cantidades pedidas originalmente por consumidor
        /// </summary>
        public string articlePedido;
        public string tallaArtPedido;
        public int qtyArtPedido;

        /// <summary>
        /// Decuento general de cada pedido de promotor
        /// </summary>
        public decimal descuentoPorPedidoPromotor;

        /// <summary>
        /// Numero de orden pertenecientce a un promotor
        /// </summary>
        public string LOV_ORDER;

        /// <summary>
        /// Fecha de realizacion del pedido del promotor
        /// </summary>
        public DateTime fechaPedidoPromotor;

        #endregion

        #region < CONSTRUCTORES >

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CON_COORDINATOR_ID"></param>
        /// <param name="bdv_email"></param>
        /// <param name="completeNameCustomer"></param>
        /// <param name="BDV_DOCUMENT_NO"></param>
        /// <param name="BDV_ADDRESS"></param>
        /// <param name="BDV_PHONE"></param>
        /// <param name="ubicationCustomer"></param>
        /// <param name="LHV_CO"></param>
        /// <param name="LHV_LIQUIDATION_NO"></param>
        /// <param name="LHD_DATE"></param>
        /// <param name="LHN_CUSTOMER"></param>
        /// <param name="LHN_DISSCOUNT"></param>
        /// <param name="LHN_COMMISSION"></param>
        /// <param name="LHN_TAX_RATE"></param>
        /// <param name="LHN_HANDLING"></param>
        /// <param name="LHN_VAL_INIT"></param>
        /// <param name="LDV_ARTICLE"></param>
        /// <param name="ARV_NAME"></param>
        /// <param name="LDV_SIZE"></param>
        /// <param name="LDN_QTY"></param>
        /// <param name="LDN_SELL_PRICE"></param>
        /// <param name="LDN_DISSCOUNT"></param>
        /// <param name="LDN_COMMISSION"></param>
        /// <param name="valorNetoLinea"></param>
        /// <param name="subtotal"></param>
        /// <param name="dctoGeneralTotal"></param>
        /// <param name="valorIva"></param>
        /// <param name="valorTotalPagar"></param>
        /// <param name="dsctosLineales"></param>
        /// <param name="comisionLineal"></param>
        /// <param name="idPromotor"></param>
        /// <param name="namePromotor"></param>
        /// <param name="cedulaPromotor"></param>
        /// <param name="instrEspeciales"></param>
        /// <param name="articlePedido"></param>
        /// <param name="tallaArtPedido"></param>
        /// <param name="qtyArtPedido"></param>
        /// <param name="descuentoPorPedidoPromotor"></param>
        /// <param name="LOV_ORDER"></param>
        /// <param name="fechaPedidoPromotor"></param>
        public OrdersLiquidation(string CON_COORDINATOR_ID,
                                    string bdv_email,
                                    string completeNameCustomer,
                                    string BDV_DOCUMENT_NO,
                                    string BDV_ADDRESS,
                                    string BDV_PHONE,
                                    string ubicationCustomer,


                                    string LHV_CO, string LHV_LIQUIDATION_NO, DateTime LHD_DATE,
                                    decimal LHN_CUSTOMER, decimal LHN_DISSCOUNT,
                                    decimal LHN_COMMISSION, decimal LHN_TAX_RATE,
                                    decimal LHN_HANDLING, decimal LHN_VAL_INIT,

                                    string LDV_ARTICLE, string ARV_NAME, string LDV_SIZE,
                                    int LDN_QTY, decimal LDN_SELL_PRICE, decimal LDN_DISSCOUNT,
                                    decimal LDN_COMMISSION, decimal valorNetoLinea,

                                    decimal subtotal,
                                    decimal dctoGeneralTotal,
                                    decimal valorIva,
                                    decimal valorTotalPagar,
                                    decimal dsctosLineales,
                                    decimal comisionLineal,
                                    
                                    string idPromotor,string namePromotor,
                                    string cedulaPromotor,string instrEspeciales,

                                    string articlePedido,
                                    string tallaArtPedido,
                                    int qtyArtPedido,
                                    
                                    decimal descuentoPorPedidoPromotor,
                                    string LOV_ORDER,
                                    DateTime fechaPedidoPromotor
                                    )
        {
            /// Informacion del cliente
            /// 
            //string CON_COORDINATOR_ID;
            _bdv_email = bdv_email;
            _completeNameCustomer = completeNameCustomer;
            _BDV_DOCUMENT_NO = BDV_DOCUMENT_NO;
            _BDV_ADDRESS = BDV_ADDRESS;
            _BDV_PHONE = BDV_PHONE;
            _ubicationCustomer = ubicationCustomer;

            /// Carga de informacion de la cabecera de la liquidacion
            /// 
            _LHV_CO = LHV_CO;
            _LHV_LIQUIDATION_NO = LHV_LIQUIDATION_NO;
            _LHD_DATE = LHD_DATE;
            _LHN_CUSTOMER = LHN_CUSTOMER;
            _LHN_DISSCOUNT = LHN_DISSCOUNT;
            _LHN_COMMISSION = LHN_COMMISSION;
            _LHN_TAX_RATE = LHN_TAX_RATE;
            _LHN_HANDLING = LHN_HANDLING;
            _LHN_VAL_INIT = LHN_VAL_INIT;

            /// Carga de informacion del detalle de la liquidacion
            /// 
            _LDV_ARTICLE = LDV_ARTICLE;
            _ARV_NAME = ARV_NAME;
            _LDV_SIZE = LDV_SIZE;
            _LDN_QTY = LDN_QTY;
            _LDN_SELL_PRICE = LDN_SELL_PRICE;
            _LDN_DISSCOUNT = LDN_DISSCOUNT;
            _LDN_COMMISSION = LDN_COMMISSION;
            _valorNetoLinea = valorNetoLinea;

            /// Totales
            /// 
            _subtotal = subtotal;
            _dctoGeneralTotal = dctoGeneralTotal;
            _valorIva = valorIva;
            _valorTotalPagar = valorTotalPagar;
            _comisionLineal = comisionLineal;
            _dsctosLineales = dsctosLineales;


            /// Info del promotor
            /// 
            _idPromotor = idPromotor;
            _namePromotor = namePromotor;
            _cedulaPromotor = cedulaPromotor;
            _instrEspeciales = instrEspeciales;

            /// Cantidades originalmente pedidas por el cliente
            /// 
            _articlePedido = articlePedido;
            _tallaArtPedido = tallaArtPedido;
            _qtyArtPedido = qtyArtPedido;

            ///
            _descuentoPorPedidoPromotor = descuentoPorPedidoPromotor;

            /// Numero de orden de un promotor
            /// 
            _LOV_ORDER = LOV_ORDER;

            /// Fecha del registro del pedido del promotor
            /// 
            _fechaPedidoPromotor = fechaPedidoPromotor;


        }

        /// <summary>
        /// Setters and Getters de la informacion del coordinador
        /// </summary>
        public string _CON_COORDINATOR_ID
        {
            get { return CON_COORDINATOR_ID; }
            set { CON_COORDINATOR_ID = value; }
        }

        public string _bdv_email
        {
            get { return bdv_email; }
            set { bdv_email = value; }
        }

        public string _completeNameCustomer
        {
            get { return completeNameCustomer; }
            set { completeNameCustomer = value; }
        }

        public string _BDV_DOCUMENT_NO
        {
            get { return BDV_DOCUMENT_NO; }
            set { BDV_DOCUMENT_NO = value; }
        }

        public string _BDV_ADDRESS
        {
            get { return BDV_ADDRESS; }
            set { BDV_ADDRESS = value; }
        }

        public string _BDV_PHONE
        {
            get { return BDV_PHONE; }
            set { BDV_PHONE = value; }
        }

        public string _ubicationCustomer
        {
            get { return ubicationCustomer; }
            set { ubicationCustomer = value; }
        }

        /// <summary>
        /// Setters and Getters de la cabecera de la liquidacion
        /// </summary>
        public string _LHV_CO
        {
            get { return LHV_CO; }
            set { LHV_CO = value; }
        }
        public string _LHV_LIQUIDATION_NO
        {
            get { return LHV_LIQUIDATION_NO; }
            set { LHV_LIQUIDATION_NO = value; }
        }

        public DateTime _LHD_DATE
        {
            get { return LHD_DATE; }
            set { LHD_DATE = value; }
        }
        public decimal _LHN_CUSTOMER
        {
            get { return LHN_CUSTOMER; }
            set { LHN_CUSTOMER = value; }
        }

        public decimal _LHN_DISSCOUNT
        {
            get { return LHN_DISSCOUNT; }
            set { LHN_DISSCOUNT = value; }
        }

        public decimal _LHN_COMMISSION
        {
            get { return LHN_COMMISSION; }
            set { LHN_COMMISSION = value; }
        }

        public decimal _LHN_TAX_RATE
        {
            get { return LHN_TAX_RATE; }
            set { LHN_TAX_RATE = value; }
        }

        public decimal _LHN_HANDLING
        {
            get { return LHN_HANDLING; }
            set { LHN_HANDLING = value; }
        }

        public decimal _LHN_VAL_INIT
        {
            get { return LHN_VAL_INIT; }
            set { LHN_VAL_INIT = value; }
        }
        /// <summary>
        /// Setters and Getters del detalle de la liquidacion
        /// </summary>    
        public string _ARV_NAME
        {
            get { return ARV_NAME; }
            set { ARV_NAME = value; }
        }
        public string _LDV_ARTICLE
        {
            get { return LDV_ARTICLE; }
            set { LDV_ARTICLE = value; }
        }
        public string _LDV_SIZE
        {
            get { return LDV_SIZE; }
            set { LDV_SIZE = value; }
        }
        public int _LDN_QTY
        {
            get { return LDN_QTY; }
            set { LDN_QTY = value; }
        }
        public decimal _LDN_SELL_PRICE
        {
            get { return LDN_SELL_PRICE; }
            set { LDN_SELL_PRICE = value; }
        }
        public decimal _LDN_DISSCOUNT
        {
            get { return LDN_DISSCOUNT; }
            set { LDN_DISSCOUNT = value; }
        }
        public decimal _LDN_COMMISSION
        {
            get { return LDN_COMMISSION; }
            set { LDN_COMMISSION = value; }
        }

        public decimal _valorNetoLinea
        {
            get { return valorNetoLinea; }
            set { valorNetoLinea = value; }
        }
        /// <summary>
        /// Setters and Getters del totalizado de la liquidacion
        /// </summary>
        public decimal _comisionLineal 
        {
            get { return comisionLineal; }
            set { comisionLineal = value; }
        }
        public decimal _dsctosLineales
        {
            get { return dsctosLineales; }
            set { dsctosLineales = value; }
        }
        public decimal _subtotal
        {
            get { return subtotal; }
            set { subtotal = value; }
        }
        
        public decimal _dctoGeneralTotal
        {
            get { return dctoGeneralTotal; }
            set { dctoGeneralTotal = value; }
        }
            
        public decimal _valorIva
        {
            get { return valorIva; }
            set {valorIva = value; }
        }
            
        public decimal _valorTotalPagar
        {
            get { return valorTotalPagar; }
            set { valorTotalPagar = value; }
        }

        /// <summary>
        /// Setters and Getters de la formacion del promotor.
        /// </summary>
        public string _idPromotor
        {
            get { return idPromotor; }
            set { idPromotor = value; }
        }
        public string _namePromotor
        {
            get { return namePromotor; }
            set { namePromotor = value; }
        }
        public string _cedulaPromotor
        {
            get { return cedulaPromotor; }
            set { cedulaPromotor = value; }
        }

        public string _instrEspeciales
        {
            get { return instrEspeciales; }
            set { instrEspeciales = value; }
        }

        /// <summary>
        /// Cantidades pedidas
        /// </summary>
        public string _articlePedido
        {
            get { return articlePedido; }
            set { articlePedido = value; }
        }
        public string _tallaArtPedido
        {
            get { return tallaArtPedido; }
            set { tallaArtPedido = value; }
        }

        public int _qtyArtPedido
        {
            get { return qtyArtPedido; }
            set { qtyArtPedido = value; }
        }

        public decimal _descuentoPorPedidoPromotor
        {
            get { return descuentoPorPedidoPromotor; }
            set { descuentoPorPedidoPromotor = value; }
        }

        /// <summary>
        /// Numero de orden del promotor
        /// </summary>
        public string _LOV_ORDER
        {
            get { return LOV_ORDER; }
            set { LOV_ORDER = value; }
        }

        /// <summary>
        /// Fecha de realizacion del pedido del promotor
        /// </summary>
        public DateTime _fechaPedidoPromotor
        {
            get { return fechaPedidoPromotor; }
            set { fechaPedidoPromotor = value; }
        }


        #endregion
    }
}