﻿
using System;
namespace Bata.Aquarella.BLL.Reports
{
    public class Liquidation
    {
        #region < Atributos >

        /// <summary>
        /// Informacion de la bodega
        /// </summary>
        public string _wavId { get; set; }
        public string _wavDes { get; set; }
        public string _wavAdd { get; set; }
        public string _wavPhone { get; set; }
        public string _wavUbication { get; set; }

        /// <summary>
        /// Cliente
        /// </summary>
        public string _cusId { get; set; }
        public string _cusDoc { get; set; }
        public string _cusName { get; set; }
        public string _cusAdd { get; set; }
        public string _cusPhone { get; set; }
        public string _cusCellPhone { get; set; }
        public string _cusMail { get; set; }
        public string _cusUbication { get; set; }

        /// <summary>
        /// Liquidacion cabecera
        /// </summary>
        public string _liqNo { get; set; }
        public DateTime _liqDateCreate { get; set; }
        public DateTime _liqDateExp { get; set; }
        public string _liqStatus { get; set; }
        public decimal _liqDctogeneral { get; set; }
        public decimal _liqTaxRate { get; set; }
        public decimal _liqTaxValue { get; set; }
        public decimal _liqHandling { get; set; }

        /// <summary>
        /// Liquidacion detalle
        /// </summary>
        public string _artCode { get; set; }        
        public string _artBrand { get; set; }
        public string _artColor { get; set; }
        public string _artName { get; set; }
        public string _artSize { get; set; }
        public decimal _artQty { get; set; }
        public decimal _artPrice { get; set; }
        public decimal _artComm { get; set; }
        public decimal _artDiss { get; set; }

        /// <summary>
        /// Campo descriptivo de sitio o entrega para un tercero.
        /// </summary>
        public string _shipTo { get; set; }

        public Liquidation(string wavId, string wavDes, string wavAdd, string wavPhone, string wavUbication, string cusId,
        string cusDoc, string cusName, string cusAdd, string cusPhone, string cusCellPhone, string cusMail, string cusUbication,
            string liqNo, DateTime liqDateCreate, DateTime liqDateExp, string liqStatus, decimal liqDctogeneral, decimal liqTaxRate,
            decimal liqTaxValue, decimal liqHandling, string artCode, string artBrand, string artColor, string artName,
            string artSize, decimal artQty, decimal artPrice, decimal artComm, decimal artDiss, string shipTo)
        {
            _wavId = wavId;
            _wavDes = wavDes;
            _wavAdd = wavAdd;
            _wavPhone = wavPhone;
            _wavUbication = wavUbication;

            _cusId = cusId;
            _cusDoc = cusDoc;
            _cusName = cusName;
            _cusAdd = cusAdd;
            _cusPhone = cusPhone;
            _cusCellPhone = cusCellPhone;
            _cusMail = cusMail;
            _cusUbication = cusUbication;
            _liqNo = liqNo;
            _liqDateCreate = liqDateCreate;
            _liqDateExp = liqDateExp;
            _liqStatus = liqStatus;
            _liqDctogeneral = liqDctogeneral;
            _liqTaxRate = liqTaxRate;
            _liqTaxValue = liqTaxValue;
            _liqHandling = liqHandling;

            _artCode = artCode;
            _artBrand = artBrand;
            _artColor = artColor;
            _artName = artName;
            _artSize = artSize;
            _artQty = artQty;
            _artPrice = artPrice;
            _artComm = artComm;
            _artDiss = artDiss;

            _shipTo = shipTo;
        }

        #endregion

    }
}