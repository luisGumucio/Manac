using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
namespace Bata.Aquarella.BLL.Util
{
    public static class Export
    {
        #region < VARIABLES >
        #endregion

        #region < CONSTRUCTORES >
        #endregion

        #region < PROPIEDADES >
        /// <summary>
        /// Enumeracion que define los formatos para los cuales esta preparada la clase
        /// </summary>
        public enum ExportFormat
        {
            XML,
            XLS,
            HTML,
            CSV,
            CUSTOM,
            TSV
        }
        #endregion

        #region < METODOS PUBLICOS >
        #endregion

        #region < METODOS PRIVADOS >
        #endregion

        #region < METODOS ESTATICOS >
        /// <summary>
        /// Web Utility Function For Exporting Data Set to Specified Format. Se necesitan marcas de delimitacion
        /// para custom, cvs, tsv. 
        /// </summary>
        /// <param name="dsResults">Result Data Set</param>
        /// <param name="enExport">Export Enum Values</param>
        /// <param name="strColDelim">Column Delimiter value</param>
        /// <param name="strRowDelim">Row Delimiter value</param>
        /// <param name="strFileName">Name output file</param>
        /// <param name="nameDataSet">Name DataSet put xml</param>
        public static void ExportDataSet(DataSet dsResults, ExportFormat enExport, string strColDelim, string strRowDelim, string strFileName, string nameDataSet)
        {
            DataGrid dgExport = new DataGrid();
            dgExport.AllowPaging = false;
            dgExport.DataSource = dsResults;
            dsResults.DataSetName = nameDataSet;
            dgExport.DataMember = dsResults.Tables[0].TableName;
            dgExport.DataBind();
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            System.Web.HttpContext.Current.Response.Charset = "";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName);
            switch (enExport.ToString().ToLower())
            {
                case "xls":
                    {
                        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                        dgExport.RenderControl(oHtmlTextWriter);
                        System.Web.HttpContext.Current.Response.Write(oStringWriter.ToString());
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
                case "custom":
                    {
                        string strText;
                        System.Web.HttpContext.Current.Response.ContentType = "text/txt";
                        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                        dgExport.RenderControl(oHtmlTextWriter);
                        strText = oStringWriter.ToString();
                        strText = ParseToDelim(strText, strRowDelim, strColDelim);
                        System.Web.HttpContext.Current.Response.Write(strText);
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
                case "csv":
                    {
                        string strText;
                        strRowDelim = System.Environment.NewLine;
                        strColDelim = ",";
                        System.Web.HttpContext.Current.Response.ContentType = "text/txt";
                        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                        dgExport.RenderControl(oHtmlTextWriter);
                        strText = oStringWriter.ToString();
                        strText = ParseToDelim(strText, strRowDelim, strColDelim);
                        System.Web.HttpContext.Current.Response.Write(strText);
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
                case "tsv":
                    {
                        string strText;
                        strRowDelim = System.Environment.NewLine;
                        strColDelim = "\t";
                        System.Web.HttpContext.Current.Response.ContentType = "text/txt";
                        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                        dgExport.RenderControl(oHtmlTextWriter);
                        strText = oStringWriter.ToString();
                        strText = ParseToDelim(strText, strRowDelim, strColDelim);
                        System.Web.HttpContext.Current.Response.Write(strText);
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
                case "xml":
                    {
                        //(") ANSII
                        char comillas = (char)34;
                        //out type
                        System.Web.HttpContext.Current.Response.ContentType = "text/xml";
                        //Write of header
                        System.Web.HttpContext.Current.Response.Write("<?xml version=" + comillas + "1.0" + comillas +
                            " encoding=" + comillas + "UTF-8" + comillas + " standalone=" + comillas + "yes"
                            + comillas + "?>");
                        //Retorno de carro
                        System.Web.HttpContext.Current.Response.Write("\r\n");
                        //
                        String str;
                        ///
                        StringBuilder strXML = new StringBuilder("");
                        ///Se copia en el StringBuilder el squema xml y los datos. 
                        dsResults.WriteXml(new System.IO.StringWriter(strXML), XmlWriteMode.IgnoreSchema);
                        ///se optiene del StringBuilder la cadena
                        str = strXML.ToString();
                        ///Se Scrbe en la salida standart la cana
                        System.Web.HttpContext.Current.Response.Write(str);
                        ///Se obliga a la salida standar a scribir lo del buffer.
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
                case "html":
                    {
                        System.Web.HttpContext.Current.Response.ContentType = "text/html";
                        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                        dgExport.RenderControl(oHtmlTextWriter);
                        System.Web.HttpContext.Current.Response.Write(oStringWriter.ToString());
                        ///Se obliga a la salida standar a scribir lo del buffer.
                        System.Web.HttpContext.Current.Response.End();
                        break;
                    }
            }
        }
        /// <summary>
        ///Permite reemplazar los marcadores de fila o culumna de html por cualquier cadena nueva.<br />
        /// Export To a Delim Format.
        /// </summary>
        /// <param name="strText">Texto HTML</param>
        /// <param name="strRowDelim">Cadena que se cambiara por la maraca de fila</param>
        /// <param name="strColDelim">Cadena que se cambiara por la maraca de columna</param>
        /// <returns></returns>
        public static string ParseToDelim(string strText, string strRowDelim, string strColDelim)
        {
            Regex objReg = new Regex(@"(>\s+<)", RegexOptions.IgnoreCase);
            strText = objReg.Replace(strText, "><");
            strText = strText.Replace(System.Environment.NewLine, "");
            strText = strText.Replace("</td></tr><tr><td>", strRowDelim);
            strText = strText.Replace("</td><td>", strColDelim);
            objReg = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            strText = objReg.Replace(strText, "");
            strText = System.Web.HttpUtility.HtmlDecode(strText);
            return strText;
        }
        #endregion
    }
}