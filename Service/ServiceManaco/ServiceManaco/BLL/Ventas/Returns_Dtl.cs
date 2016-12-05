﻿using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Ventas
{
    public class Returns_Dtl
    {

        #region < ATRIBUTOS >

        /// <summary>
        /// Nombre de cadena de conexion en el web.config
        /// </summary>
        public static string _conn = Constants.OrcleStringConn;

        public bool _checked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string RDV_CO;
        
        /// <summary>
        /// 
        /// </summary>
        string RDV_RETURN;

        /// <summary>
        /// 
        /// </summary>
        decimal RDN_LINE;

        /// <summary>
        /// 
        /// </summary>
        string RDV_INVOICE;

        /// <summary>
        /// 
        /// </summary>
        string RDV_ARTICLE;
        public string RDV_ARTICLE_NAME { get; set; }
        public string RDV_BRAND { get; set; }
        public string RDV_ARTICLE_COLOR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string RDV_SIZE;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_QTY;
        public decimal _QTYINVOICE { get; set; }
        public decimal _QTYINLINE { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_SELLPRICE;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_DISSCOUNT_LIN;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_COMMISSION;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_HANDLING;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_DISSCOUNT_GEN;
        
        /// <summary>
        /// 
        /// </summary>
        decimal RDN_TAXES;

        /// <summary>
        /// 
        /// </summary>
        string RDV_STORAGE;

        public string _RDV_CONCEPT_ID { get; set; }
        public string _RDV_CONCEPT_DESC { get; set; }

        #endregion


        #region < CONSTRUCTORES >


        public Returns_Dtl()
        { 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_RDV_CO"></param>
        /// <param name="_RDV_RETURN"></param>
        /// <param name="_RDN_LINE"></param>
        /// <param name="_RDV_INVOICE"></param>
        /// <param name="_RDV_ARTICLE"></param>
        /// <param name="_RDV_SIZE"></param>
        /// <param name="_RDN_QTY"></param>
        /// <param name="_RDN_SELLPRICE"></param>
        /// <param name="_RDN_DISSCOUNT_LIN"></param>
        /// <param name="_RDN_COMMISSION"></param>
        /// <param name="_RDN_HANDLING"></param>
        /// <param name="_RDN_DISSCOUNT_GEN"></param>
        /// <param name="_RDN_TAXES"></param>
        public Returns_Dtl(string RDV_CO,string RDV_RETURN,decimal RDN_LINE,string RDV_INVOICE,
            string RDV_ARTICLE,string RDV_SIZE,decimal RDN_QTY,decimal RDN_SELLPRICE,
            decimal RDN_DISSCOUNT_LIN,decimal RDN_COMMISSION,decimal RDN_HANDLING,
            decimal RDN_DISSCOUNT_GEN,decimal RDN_TAXES)
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
            this.RDV_CO = RDV_CO;
            this.RDV_RETURN = RDV_RETURN;
            this.RDN_LINE = RDN_LINE;
            this.RDV_INVOICE = RDV_INVOICE;
            this.RDV_ARTICLE = RDV_ARTICLE;
            this.RDV_SIZE = RDV_SIZE;
            this.RDN_QTY = RDN_QTY;
            this.RDN_SELLPRICE = RDN_SELLPRICE;
            this.RDN_DISSCOUNT_LIN = RDN_DISSCOUNT_LIN;
            this.RDN_COMMISSION = RDN_COMMISSION;
            this.RDN_HANDLING = RDN_HANDLING;
            this.RDN_DISSCOUNT_GEN = RDN_DISSCOUNT_GEN;
            this.RDN_TAXES = RDN_TAXES;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RDV_ARTICLE"></param>
        /// <param name="RDV_SIZE"></param>
        /// <param name="RDN_QTY"></param>
        public Returns_Dtl(string RDV_INVOICE, string RDV_ARTICLE, string RDV_SIZE, decimal RDN_QTY)
        {
            this._RDV_INVOICE = RDV_INVOICE;
            this._RDV_ARTICLE = RDV_ARTICLE;
            this._RDV_SIZE = RDV_SIZE;
            this._RDN_QTY = RDN_QTY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RDV_ARTICLE"></param>
        /// <param name="RDV_SIZE"></param>
        /// <param name="RDN_QTY"></param>
        public Returns_Dtl(string RDV_INVOICE, string RDV_ARTICLE, string RDV_SIZE, decimal RDN_QTY,string RDV_STORAGE)
        {
            this._RDV_INVOICE = RDV_INVOICE;
            this._RDV_ARTICLE = RDV_ARTICLE;
            this._RDV_SIZE = RDV_SIZE;
            this._RDN_QTY = RDN_QTY;
            this._RDV_STORAGE = RDV_STORAGE;
        }


        #endregion


        #region < PROPIEDADES >
        /// <summary>
        /// 
        /// </summary>
        public string _RDV_CO
        {
            get { return RDV_CO; }
            set { RDV_CO = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _RDV_RETURN
        {
            get { return RDV_RETURN; }
            set { RDV_RETURN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_LINE
        {
            get { return RDN_LINE; }
            set { RDN_LINE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _RDV_INVOICE
        {
            get { return RDV_INVOICE; }
            set { RDV_INVOICE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _RDV_ARTICLE
        {
            get { return RDV_ARTICLE; }
            set { RDV_ARTICLE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _RDV_SIZE
        {
            get { return RDV_SIZE; }
            set { RDV_SIZE = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_QTY
        {
            get { return RDN_QTY; }
            set { RDN_QTY = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_SELLPRICE
        {
            get { return RDN_SELLPRICE; }
            set { RDN_SELLPRICE = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_DISSCOUNT_LIN
        {
            get { return RDN_DISSCOUNT_LIN; }
            set { RDN_DISSCOUNT_LIN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_COMMISSION
        {
            get { return RDN_COMMISSION; }
            set { RDN_COMMISSION = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_HANDLING
        {
            get { return RDN_HANDLING; }
            set { RDN_HANDLING = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_DISSCOUNT_GEN
        {
            get { return RDN_DISSCOUNT_GEN; }
            set { RDN_DISSCOUNT_GEN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal _RDN_TAXES
        {
            get { return RDN_TAXES; }
            set { RDN_TAXES = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _RDV_STORAGE
        {
            get { return RDV_STORAGE; }
            set { RDV_STORAGE = value; }
        }

        #endregion

        #region < METODOS ESTATICOS - PUBLICOS >

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_company"></param>
        /// <param name="_noReturn"></param>
        /// <returns></returns>
        public static DataTable getRetunrDtl(string _company, string _noReturn)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                DataTable dtResult = new DataTable();

                ///
                Database db = DatabaseFactory.CreateDatabase(_conn);
                string sqlCommand = "ventas.sp_getretunrdtl";

                ///
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand,_company,_noReturn, results);
                
                ///
                dtResult = db.ExecuteDataSet(dbCommandWrapper).Tables[0];
                return dtResult;
            }
            catch
            {
                ///
                return null;
            }
        }

        /// <summary>
        /// Devoluciones al detalle, por concepto y entre dos fechas
        /// </summary>
        /// <param name="_co"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_area"></param>
        /// <returns></returns>
        public static DataSet getReturnsDtlByDate(string _co, string _startDate, string _endDate, string _area)
        {
            //         
            try
            {
                // CURSOR REF
                object results = new object[1];

                Database db = DatabaseFactory.CreateDatabase(_conn);
                //
                string sqlCommand = "ventas.sp_get_returnsdtl_bydate";
                //
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _area, _startDate, _endDate, results);
                //
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }


        #endregion
    }
}