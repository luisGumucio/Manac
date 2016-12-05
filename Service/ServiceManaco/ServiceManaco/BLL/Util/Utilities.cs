using System;
using System.Data;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bata.Aquarella.BLL.Control;
using System.Web;
using System.Linq;

namespace Bata.Aquarella.BLL.Util
{
    public class Utilities
    {
        public static void logout(HttpSessionState session, HttpResponse response)
        {
            string url = FormsAuthentication.LoginUrl;
            session.Clear();
            session.Abandon();
            FormsAuthentication.SignOut();
            response.Redirect(url, true);
        }

        public static void permitsNavegartion(HttpContext _pageRequest)
        {
            bool PageInList = false;

            if (_pageRequest.Session["_MENU"] != null)
            {
                try
                {
                    string currentPage = System.IO.Path.GetFileName(HttpContext.Current.Request.Url.AbsolutePath);

                    string Current_urlUser = _pageRequest.Request.Url.AbsolutePath;

                    List<ApplicationFunctions> _applicationsUser = new List<ApplicationFunctions>();
                    _applicationsUser = (List<ApplicationFunctions>)_pageRequest.Session["_MENU"];



                    var res = _applicationsUser.Where(x => x._url.ToUpper().Contains(currentPage.ToUpper())).ToList();

                    if (res != null && res.Count > 0)
                        PageInList = true;

                    if (!PageInList)
                        logout(_pageRequest.Session, _pageRequest.Response);
                }
                catch
                {
                    logout(_pageRequest.Session, _pageRequest.Response);
                }
                

                /*foreach (var _app in _applicationsUser)
                {
                    if (_app._url != "")
                        if (_app._url.ToUpper().Contains(Current_urlUser.ToUpper()))
                            PageInList = true;
                }*/
            }
        }


        /// <summary>
        /// Obtener un datatable en una posicion de un dataset
        /// </summary>
        /// <param name="dtObj"></param>
        /// <param name="posTable"></param>
        /// <returns></returns>
        public static DataTable getTableFromDataset(object dtObj, int posTable)
        {
            try
            {
                DataSet ds = (DataSet)dtObj;
                return ds.Tables[posTable];
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Calcula el margen en porcentaje entre precio y costo.
        /// Para obtener el porcentaje multiplicar por 100 (*100).
        /// </summary>
        /// <param name="price"></param>
        /// <param name="cost"></param>
        /// <returns>numero decimal menor o igual que uno que representa el porcentaje.</returns>
        public static decimal margenCalc(decimal price, decimal cost)
        {
            if (price < cost)
            {
                //No multiplicado por 100.
                return Decimal.Round(((cost - price) / cost) * (-1), 4);
            }
            else if (price > cost)
            {
                return Decimal.Round(((price - cost) / price), 4);
            }
            else
            {
                return Decimal.Round(0, 2);
            }
        }

        /// <summary>
        /// Realizar un filtro linq sobre un objeto dataset
        /// </summary>
        /// <param name="dtObj">Objeto dataset</param>
        /// <param name="posTable">indice de tabla sobre la cual realizar el linq</param>
        /// <param name="f1">campo 1 de comparacion</param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        /// <param name="fieldValue1">valor 1 de comparacion frente al campo 1</param>
        /// <param name="fieldValue2"></param>
        /// <param name="fieldValue3"></param>
        /// <param name="fieldOrder">campo de ordenacion</param>
        /// <returns></returns>
        public static DataTable getFilterObject(object dtObj, int posTable, string f1, string f2, string f3,
            string fieldValue1, string fieldValue2, string fieldValue3, string fieldOrder)
        {
            try
            {
                DataTable ds = (DataTable)dtObj;
                var result = (from x in ds.AsEnumerable()
                              where x.Field<string>(f1).ToUpper().Contains((!string.IsNullOrEmpty(fieldValue1) ? fieldValue1.ToUpper() : string.Empty)) ||
                                  x.Field<string>(f2).ToUpper().Contains((!string.IsNullOrEmpty(fieldValue2) ? fieldValue2.ToUpper() : string.Empty)) ||
                                  (!string.IsNullOrEmpty(Convert.ToString(x[f3])) ? Convert.ToString(x[f3]) : string.Empty).Contains(fieldValue3)
                              //orderby x.Field<string>(fieldOrder)
                              select x).CopyToDataTable();
                return result;
            }
            catch
            {
                return new DataTable();
            }
        }

        public static DataTable getFilterObject(object dtObj, int posTable, string f1, string fieldValue1, string fieldOrder)
        {
            try
            {
                DataTable ds = (DataTable)dtObj;
                var result = (from x in ds.AsEnumerable()
                              where x.Field<string>(f1).ToUpper().Contains((!string.IsNullOrEmpty(fieldValue1) ? fieldValue1.ToUpper() : string.Empty))
                              //orderby x.Field<string>(fieldOrder.ToString())
                              select x).CopyToDataTable();
                return result;
            }
            catch
            {
                return new DataTable();
            }
        }


        #region < Funciones de ayudas para paginación y ordenacion de grillas >

        /// <summary>
        /// Paginación y ordenación de la fuente de datos
        /// </summary>
        public static void pageAndSort(GridView gv,ref SortDirection GridViewSortDirection, string sortField, DataTable dt, int iPage)
        {
            if (dt != null)
            {
                if (!string.IsNullOrEmpty(sortField))
                {
                    int actualPage = gv.PageIndex;

                    // Si es paginacion, permanecer con la misma direccion de ordenación
                    if (actualPage != iPage)
                    {
                        // Esta asc ordenarlo desc
                        if (GridViewSortDirection.Equals(SortDirection.Ascending))
                            gv.DataSource = dt.AsEnumerable().OrderBy(x => x.Field<object>(sortField)).CopyToDataTable();
                        else
                            gv.DataSource = dt.AsEnumerable().OrderByDescending(x => x.Field<object>(sortField)).CopyToDataTable();
                    }
                    else
                    {
                        // Esta asc ordenarlo desc
                        if (GridViewSortDirection.Equals(SortDirection.Ascending))
                        {
                            gv.DataSource = dt.AsEnumerable().OrderByDescending(x => x.Field<object>(sortField)).CopyToDataTable();
                            GridViewSortDirection = SortDirection.Descending;
                        }
                        else
                        {
                            gv.DataSource = dt.AsEnumerable().OrderBy(x => x.Field<object>(sortField)).CopyToDataTable();
                            GridViewSortDirection = SortDirection.Ascending;
                        }
                    }
                }
                else
                    gv.DataSource = dt;

                gv.PageIndex = iPage;
                gv.DataBind();
            }
        }
        #endregion

        #region < Enviar Correo Liquidacion WEBSERVICE>
        /// <summary>
        /// Async SendMailLiquidation
        /// </summary>
        public static void sendMailLiquidation(string _co, string _noLiq)
        {
            Task.Run(async () =>
            {
                // Inicio de una tarea que se completara despues del tiempo de vencimiento especificado (mil)
                await Task.Delay(1);

                //MailMessage [0] - destinatario 
                //MailMessage [1] - asunto 
                //MailMessage [2] - Encabezado
                //MailMessage [3] - Cuerpo del mensaje con la liquidacion
                List<string> mailMessage = BLL.Liquidation_Dtl.getMailLiquidation(_co,_noLiq);
                /*
                ServiceUtil.UtilClient clientUtil = new ServiceUtil.UtilClient();
                bool sended = clientUtil.sendMailMessageEmail1(mailMessage[0], mailMessage[1], mailMessage[2], mailMessage[3]);
                */
            });
        }
        #endregion

        #region < Enviar Correo Confirmacion de pago. WEBSERVICE >
        /// <summary>
        /// Async SenMailVerifPayment // envia un correo al verificar un pago. 
        /// </summary>
        public static void sendMailVerifPayment(string email, string cuenta, string banco, string fecha, string valor, string descripStatus, string newStatus) {

            Task.Run(async () => {
                await Task.Delay(1);

                string destinatario = email;
                string asunto = descripStatus + " Aquarella" ;
                string header = "<strong>" + descripStatus + "</strong><br/><br/>" ;
                string body = "<strong>Detalles de la transaccion:</strong><br/><br/>";
                body += @"<table style='border: 0px; font-size: 12px; font-family: Calibri; margin: 10px;'>
                            <tr>
                                <th style='border-bottom: 1px solid #202020; padding: 0.3em; background-color: #484848; color: white;'>Fecha</th>
                                <th style='border-bottom: 1px solid #202020; padding: 0.3em; background-color: #484848; color: white;'>Banco</th>
                                <th style='border-bottom: 1px solid  #202020; padding: 0.3em; background-color: #484848; color: white;'>Cuenta</th>
                                <th style='border-bottom: 1px solid  #202020; padding: 0.3em; background-color: #580000; color: white;'>Valor</th>
                            </tr>
                            <tr>
                                <td style='border: 1px solid silver; padding: 0.2em; border-bottom-color: maroon; align: center;'>"+fecha+@"</td>
                                <td style='border: 1px solid gray; padding: 0.2em; border-bottom-color: maroon;'>"+banco+@"</td>
                                <td style='border: 1px solid silver; padding: 0.2em; border-bottom-color: maroon;'>"+cuenta+@"</td>
                                <td style='border: 1px solid silver; padding: 0.2em; border-bottom-color: maroon;'>"+valor+@"</td>
                            </tr>
                        </table>";
                /*
                ServiceUtil.UtilClient clientUtil = new ServiceUtil.UtilClient();
                bool sended = clientUtil.sendMailMessageEmail1(destinatario, asunto, header, body);
                */
            
            });
        }
        #endregion

        #region < Enviar Correo Al realizar Cruce de pago y liquidacion WEBSERVICE >
        /// <summary>
        /// Async SendMailClear // envia un correo al realizar un cruce de un pago con una liquidacion. 
        /// </summary>
        public static void sendMailClear(string _co,string ListLiq) {
            Task.Run(async () =>
            {
                await Task.Delay(1);

                string _noLiq = "";

                // Obtiene la primera liquidacion si hay una lista separadas por comas de varias liquidaciones
                if (ListLiq.Contains(",")){
                    string[] noLiq = ListLiq.Split(',');
                    _noLiq = noLiq[0];
                }
                else
                    _noLiq = ListLiq;

                // Obtiene la informacion de la liquidacion
                // En ella esta la informacion del destinatario para el mensaje
                List<string> mailMessage = BLL.Liquidation_Dtl.getMailLiquidation(_co, _noLiq);
                

                string destinatario = mailMessage[0];
                string asunto = "Se ha realizado su cruce financiero" + " Aquarella";
                string header = "<strong>Cruce financiero:</strong><br/><br/>";
                string body = "<strong>Se Ha realizado su cruce financiero exitosamente del siguiente pedido ("+ListLiq+")</strong><br/>" +
                              "<strong>Esos pedidos pasaran a su empacado y posteriormente a su facturación </strong><br/>";

                /*
                ServiceUtil.UtilClient clientUtil = new ServiceUtil.UtilClient();
                bool sended = clientUtil.sendMailMessageEmail1(destinatario, asunto, header, body);
                 */

            });
        }
        #endregion

        public static void sendMailPasswordRecovery(string _destinatario, string _asunto, string _header, string _body)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1);

                /*
                ServiceUtil.UtilClient clientUtil = new ServiceUtil.UtilClient();
                bool sended = clientUtil.sendMailMessageEmail1(_destinatario, _asunto, _header, _body);
                */
            });
        }

        #region < Envio de correos >

        public static void sendMessage(string destinatario, string asunto, string cuerpo)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("mail@manisol.com.co");
            message.To.Add(destinatario);
            message.Subject = asunto;
            message.Body = cuerpo;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Host = "email1.bata.com";
                smtpClient.EnableSsl = false;
                smtpClient.Send(message);
            }
            catch 
            {   
                
            }
        }

        public static void sendInstitutionalMessage(string destinatario, string asunto, string header, string body, string pathTemplate)
        {
            string HTMLTemplateMail = TemplateHTML(pathTemplate);

            HTMLTemplateMail = BindTemplateEmail(HTMLTemplateMail, header, body);

            MailMessage message = new MailMessage();
            message.From = new MailAddress("mail@manisol.com.co");
            message.To.Add(destinatario);
            message.Subject = asunto;
            message.Body = HTMLTemplateMail;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Host = "email1.bata.com";
                smtpClient.EnableSsl = false;
                smtpClient.Send(message);
            }
            catch
            {
            }
        }
        

        /// <summary>Encarga de cargar el templatehtml en un string
        /// </summary>
        /// <returns>String con el codigo html del mensaje</returns>
        public static string TemplateHTML(string path)
        {
            string HTML = null;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
                    {
                        while (reader.Peek() >= 0)
                        {
                            HTML += reader.ReadLine();
                        }
                    }
                }

            }
            catch
            {
                //msnMessage.LoadMessage(error.Message, UserControl.ucMessage.MessageType.Error);
            }

            return HTML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HTMLTemplate"></param>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string BindTemplateEmail(string HTMLTemplate, string header, string body)
        {
            try
            {
                HTMLTemplate = HTMLTemplate.Replace(@"#_DATE", DateTime.Now.ToString("dddd d, MMMM yyyy, HH:mm:ss"));
                HTMLTemplate = HTMLTemplate.Replace(@"#_HEADER_MAIL", header);
                HTMLTemplate = HTMLTemplate.Replace(@"#_BODY_MAIL", body);
            }
            catch (Exception)
            {
                return HTMLTemplate;
            }
            return HTMLTemplate;
        }

        #endregion

    }
}