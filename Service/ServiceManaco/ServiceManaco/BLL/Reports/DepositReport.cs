using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bata.Aquarella.BLL.Reports
{
    public class DepositReport
    {
        public string _pav_co { get; set; }
        public string _cov_warehouseid { get; set; }
        public string _pav_payment_id { get; set; }
        public decimal _pan_coord_id { get; set; }
        public string _bdv_document_no { get; set; }
        public string _bdv_email { get; set; }
        public string _coordinator { get; set; }
        public string _pav_bank_id { get; set; }
        public string _bav_description { get; set; }
        public string _bav_money { get; set; }
        public string _pav_num_consignacion { get; set; }
        public DateTime _pad_pay_date { get; set; }
        public DateTime _pad_curr_date { get; set; }
        public decimal _pan_amount { get; set; }
        public string _pav_comments { get; set; }
        public string _pav_status { get; set; }
        public string _stv_description { get; set; }
        public string _arv_description { get; set; }

        public DepositReport(string pav_co, string cov_warehouseid, string pav_payment_id, decimal pan_coord_id,
                            string bdv_document_no, string bdv_email, string coordinator, string pav_bank_id,
                            string bav_description, string bav_money, string pav_num_consignacion, DateTime pad_pay_date,
                            DateTime pad_curr_date, decimal pan_amount, string pav_comments, string pav_status,
                            string stv_description, string arv_description)
        {
            _pav_co = pav_co;
            _cov_warehouseid = cov_warehouseid;
            _pav_payment_id = pav_payment_id;
            _pan_coord_id = pan_coord_id;
            _bdv_document_no = bdv_document_no;
            _bdv_email = bdv_email;
            _coordinator = coordinator;
            _pav_bank_id = pav_bank_id;
            _bav_description = bav_description;
            _bav_money = bav_money;
            _pav_num_consignacion = pav_num_consignacion;
            _pad_pay_date = pad_pay_date;
            _pad_curr_date = pad_curr_date;
            _pan_amount = pan_amount;
            _pav_comments = pav_comments;
            _pav_status = pav_status;
            _stv_description = stv_description;
            _arv_description = arv_description;
        }
    }
}