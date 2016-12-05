using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.OleDb;
using System.Xml;
using System.Collections;

using Bata.Aquarella.BLL;
using Bata.Aquarella.BLL.Maestros;
using Bata.Aquarella.BLL.Planning;
using Bata.Aquarella.BLL.Logistica;

namespace Bata.Aquarella.BLL.Util
{
    public class Import
    {
        #region <VARIABLES>
        /// <summary>
        /// 
        /// </summary>
        public static string directoryTempODV = "~/uploads/temp/";
        /// <summary>
        /// 
        /// </summary>
        public static string[] namesColumnsExcelODV = { "ARTICULO", "ODV" };
        /// <summary>
        /// 
        /// </summary>
        public static string directoryTempPUBLICS_PRICES = "~/uploads/temp/";
        /// <summary>
        /// 
        /// </summary>
        public static string[] namesColumnsExcelPUBLICS_PRICES = { "ARTICULO", "PRECIO_PUBLICO" };
        /// <summary>
        /// 
        /// </summary>        
        public static string directoryTempArticlesBySizes = "~/uploads/temp/";
        /// <summary>
        /// 
        /// </summary>
        public static string[] namesColumnsExcelArticlesBySizes = { "ARTICULO", "TALLA", "CANTIDAD" };
        /// <summary>
        /// 
        /// </summary>
        static string worksheet = "Hoja1";
        #endregion

        #region <CONSTRUCTORES>

        #endregion

        #region <PROPIEDADES>

        #endregion

        #region < METODOS PUBLICOS >
        #endregion

        #region < METODOS PRIVADOS >
        #endregion

        #region <METODOS ESTATICOS>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Absolud filenam</param>
        /// <param name="nameHojasRead"></param>
        /// <param name="columns"></param>
        /// <param name="limitedvertcal"></param>
        /// <returns></returns>
        //static public System.Data.DataTable importExcel(String fileName, object[] nameHojasRead, object[] columns, int limitedvertcal)
        //{
        //    _Application xlApp;
        //    _Workbook xlLibro;
        //    _Worksheet xlHoja1;
        //    Sheets xlHojas;
        //    string fileName = "c:\\users\\manisol\\desktop\\cosa.xls";
        //    xlApp = new Application();
        //    //xlApp.Visible = true;
        //    xlApp.Visible = false;
        //    xlLibro = xlApp.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //    xlHojas = xlLibro.Sheets;
        //    xlHoja1 = (_Worksheet)xlHojas["Hoja1"];
        //    string val1 = (string)xlHoja1.get_Range("A1", Missing.Value).Text;

        //    xlLibro.Close(false, Missing.Value, Missing.Value);
        //    xlApp.Quit();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <param name="nameSheet"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        static public DataTable ImportODV(string company, string path, string nameFile, string nameSheet, String document)
        {
            ArrayList array = new ArrayList();
            //
            DataTable aux = new DataTable();
            //
            Boolean valid;
            //
            //String date;
            //
            Decimal odv;
            //try
            //{
            DataTable dt;
            if (nameFile.Substring(nameFile.Length - 3).Equals("xls"))
            {

                dt = importExcel(path, nameFile, nameSheet, 0);
            }
            else
            {
                dt = importXML(path, nameFile);
            }
            //}catch {}
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    valid = true;
                    //
                    String article = dr[namesColumnsExcelODV[0]].ToString();
                    //
                    if (valid && !Article.existArticle(company, article))
                    {
                        valid = false;
                    }
                    //
                    if (valid && Odv.Existe(article, company, document))
                    {
                        valid = false;
                    }
                    //
                    try
                    {
                        odv = Convert.ToDecimal(dr[namesColumnsExcelODV[1]].ToString());
                    }
                    catch { valid = false; }
                    //
                    if (SessionDataTable.existRowODV(SessionDataTable.getNameTableOdV(), article))
                    {
                        valid = false;
                    }
                    //
                    if (!valid)
                    {
                        array.Add(dr[namesColumnsExcelODV[0]].ToString());
                    }
                }
                //
                //Boolean toLeave=false;
                //
                int i = 0;
                //
                string[] list = (string[])array.ToArray(typeof(string));
                //
                foreach (string str in list)
                {
                    //
                    while (i < dt.Rows.Count/*!toLeave*/)
                    {
                        if (str == (dt.Rows[i])[namesColumnsExcelODV[0]].ToString())
                        {
                            dt.Rows.Remove(dt.Rows[i]);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    //
                    i = 0;
                }
                //
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <param name="nameSheet"></param>
        /// <param name="circular"></param>
        /// <returns></returns>
        static public DataTable ImportPUBLICS_PRICES(string company, string path, string nameFile, string nameSheet, String circular)
        {
            ArrayList array = new ArrayList();
            //
            DataTable aux = new DataTable();
            //
            Boolean valid;
            //
            //String date;
            //
            Decimal price;
            //try
            //{
            DataTable dt;
            if (nameFile.Substring(nameFile.Length - 3).Equals("xls"))
            {

                dt = importExcel(path, nameFile, nameSheet, 1);
            }
            else
            {
                dt = importXML(path, nameFile);
            }
            //}catch {}
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    valid = true;
                    //
                    String article = dr[namesColumnsExcelPUBLICS_PRICES[0]].ToString();
                    //
                    if (valid && !Article.existArticle(company, article))
                    {
                        valid = false;
                    }
                    //
                    if (valid && Prices.Existe(article, circular, company))
                    {
                        valid = false;
                    }
                    //
                    try
                    {
                        price = Convert.ToDecimal(dr[namesColumnsExcelPUBLICS_PRICES[1]].ToString());
                    }
                    catch { valid = false; }
                    //
                    if (SessionDataTable.existRowODV(SessionDataTable.getNameTablePrices(), article))
                    {
                        valid = false;
                    }
                    //
                    if (!valid)
                    {
                        array.Add(dr[namesColumnsExcelPUBLICS_PRICES[0]].ToString());
                    }
                }
                //
                //Boolean toLeave=false;
                //
                int i = 0;
                //
                string[] list = (string[])array.ToArray(typeof(string));
                //
                foreach (string str in list)
                {
                    //
                    while (i < dt.Rows.Count/*!toLeave*/)
                    {
                        if (str == (dt.Rows[i])[namesColumnsExcelPUBLICS_PRICES[0]].ToString())
                        {
                            dt.Rows.Remove(dt.Rows[i]);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    //
                    i = 0;
                }
                //
                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Retorna un DataTable con los datos de un .xls (Archivo de Excel) donde la primera fila 
        /// tiene los nombres de las columnas y el resto de filas es la informacion.
        /// Recuerde para importar con este metodo debe tener instalado en el servidor web
        /// el Microsoft.Jet.OLEDB.4.0, que es el motor get de office.
        /// </summary>
        /// <param name="path">Absoluth path</param>
        /// <param name="name">file name + .xls extend</param>
        /// <returns>Datatable data file xls</returns>
        public static DataTable importExcel(string path, string nameFile, string nameSheet, int table)
        {
            //
            string columnKey;
            //
            if (table == 0)
                columnKey = namesColumnsExcelODV[0];
            else
                columnKey = namesColumnsExcelPUBLICS_PRICES[0];
            //"Provider=Microsoft.Jet.OLEDB.4.0;" +
            string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; " +
            "Data Source=" + path + nameFile + ";" + "Extended Properties=Excel 8.0;";
            //Seleccion de la hoja            
            string sqlExcel = "Select * From [" + nameSheet + "$] order by " + columnKey;
            //Instanciacion de un SataSet.
            DataSet DS = new DataSet();
            //
            OleDbConnection oledbConn = new OleDbConnection(sConnectionString);
            //
            oledbConn.Open();
            //Creamos un comad para ejecutar la sentencia SELECT.
            OleDbCommand oledbCmd = new OleDbCommand(sqlExcel, oledbConn);
            //Creamos un dataAdapter para leer los datos y asocialor al DataSet.
            OleDbDataAdapter da = new OleDbDataAdapter(oledbCmd);
            //
            da.Fill(DS);
            //retornamos Datatable con los datos del .xls 
            oledbConn.Close();
            //
            return DS.Tables[0];
        }
        /// <summary>
        /// Importa de una archivo excel grabado con el tipo Hoja de calculo XML 2003 a un DataTable. 
        /// (Simplificar con Xpath) 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <param name="nameSheet"></param>
        /// <returns></returns>
        public static DataTable importXML(string path, string nameFile, string nameSheet)
        {
            //
            Boolean isFirst = true;
            //
            DataTable dt = new System.Data.DataTable();
            // Defines an XmlTextReader objet to read the RSS feed from an URL
            XmlTextReader reader = new XmlTextReader(path + nameFile);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            // Creates an XPathNavigator to select the nodes we want to get
            XmlDocument doc = new XmlDocument();
            //
            XmlNodeList nodes, nodes2, nodes3, nodes4, nodes5;
            //
            doc.Load(reader);
            //
            //nodes = doc.SelectNodes("Workbook/Worksheet/Table/Row");
            //
            nodes = doc.ChildNodes;
            //
            foreach (XmlNode node in nodes)
            {
                //
                if (node.LocalName == "Workbook")
                {
                    //
                    nodes2 = node.ChildNodes;
                    foreach (XmlNode node2 in nodes2)
                    {
                        //
                        if (node2.LocalName == "Worksheet")
                        {
                            //
                            nodes3 = node2.ChildNodes;
                            foreach (XmlNode node3 in nodes3)
                            {
                                //
                                if (node3.LocalName == "Table")
                                {
                                    //
                                    nodes4 = node3.ChildNodes;
                                    foreach (XmlNode node4 in nodes4)
                                    {
                                        //
                                        if (node4.LocalName == "Row")
                                        {
                                            //
                                            string cad;
                                            //
                                            nodes5 = node4.ChildNodes;
                                            //
                                            if (isFirst)
                                            {
                                                //
                                                foreach (XmlNode node5 in nodes5)
                                                {
                                                    //
                                                    if (node5.LocalName == "Cell")
                                                    {
                                                        //
                                                        cad = node5.InnerText;//node5.SelectSingleNode("/Data").InnerText;
                                                        //Creation of the names of the columns 
                                                        dt.Columns.Add(cad);
                                                    }
                                                }
                                                //
                                                isFirst = false;
                                            }
                                            else
                                            {
                                                //
                                                int i = 0;
                                                //
                                                DataRow dr = dt.NewRow();
                                                //
                                                foreach (XmlNode node6 in nodes5)
                                                {
                                                    //
                                                    if (node6.LocalName == "Cell")
                                                    {
                                                        //aqui recorro cada celda y hago lo qeu deba para un dataset
                                                        cad = node6.InnerText;//node5.SelectSingleNode("/Data").InnerText;
                                                        //
                                                        dr[i] = cad;
                                                        //
                                                        i++;
                                                    }
                                                }
                                                //
                                                dt.Rows.Add(dr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //
            return dt;
        }
        /// <summary>
        /// Importa de una archivo XML bien formado a un DataTabla.
        /// </summary>
        /// <param name="path">absolut path directory content file</param>
        /// <param name="nameFile">Name file.xml</param>
        /// <returns></returns>
        public static DataTable importXML(string path, string nameFile)
        {
            //
            DataSet ds = new DataSet();
            //
            ds.ReadXml(path + nameFile);
            //
            return ds.Tables[0];
        }
        /// <summary>
        /// Importa de un archivo cvs separado por comas a un DataTable.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static DataTable importCVS(string path, string nameFile)
        {
            return null;//new Exception("metodo no implementado");
        }
        /// <summary>
        /// Solo lectura--Retorna el nombre de la hoja que se esta momando en la lectura del archivo xls. 
        /// </summary>
        /// <returns></returns>
        public static string getWorkSheet()
        {
            return worksheet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int linesXML(string path, string fileName)
        {
            //
            return importXML(path, fileName).Rows.Count;
        }


        /// <summary>
        /// Ojo no debe poder recibir articulo repetidos. la mejor form es creando la array list de 
        /// articles lisde una..
        /// </summary>
        /// <param name="_COMPANY"></param>
        /// <param name="path"></param>
        /// <param name="xls"></param>
        /// <param name="storage_id"></param>
        /// <param name="warehouse_id"></param>
        /// <returns></returns>
        public static ArrayList ImportArticleSizeQty(string company, string path, string nameFile, string storage_id, string warehouse_id)
        {
            ///Lista de objetos ArticleList. Se usa para hacer las Verificiones y es retornado par su uso enla interfaz.
            ArrayList array = new ArrayList();
            ///Se usa para verificar si es valido a cada paso el archivo leido.
            Boolean valid;
            ///Es el id del articulo leido del XML.
            String article;
            ///Talla del articulo de la linea XML
            String size = "";
            ///Cantidad del articulo de la linea XML
            int qty = 0;
            ///Tabla de las lineas del XML
            DataTable dt;
            //try
            //{                        
            ///Convierte el XML en DataTable (La ruta es relativa y debe estar en el servor)
            dt = importXML(path, nameFile);
            //}catch { dt = null; }
            ///validadcion de carga del DataTable.
            if (dt != null)
            {
                ///Recorrido del DataTable son todos lo detalle con idArticle, size & qty.
                foreach (DataRow dr in dt.Rows)
                {
                    ///
                    valid = true;
                    try
                    {
                        ///Obtencion del articulo
                        article = dr[namesColumnsExcelArticlesBySizes[0]].ToString();
                    }
                    catch { dt = null; array = null; article = "0"; valid = false; break; }
                    ///validadcion de existencia del articulo
                    if (valid && !Article.existArticle(company, article))
                    {
                        valid = false;
                    }
                    else
                    {
                        try
                        {
                            ///Obtencion de la talla del Articulo
                            size = (String)dr[namesColumnsExcelArticlesBySizes[1]];
                            ///Obtencion de la cantidad del Artticulo.    
                            qty = Convert.ToInt16((String)dr[namesColumnsExcelArticlesBySizes[2]]);
                        }
                        catch { valid = false; }
                        ///Se obtiene el detalle del articulo para la validaciones de talla y cantidad.
                        DataTable dtDetalle =  STOCKTANSFERSTORAGE.GetArtSizesQtyFromDetTransferStorage(company, article, storage_id).Tables[0];
                        ///
                        if (valid)
                        {
                            ///
                            string sizedetalle;
                            ///
                            int qty_existence_detalle;
                            ///
                            valid = false;
                            ///Ciclo para averiguar si hay la contidades del articulo son validas
                            ///si hay menos de las solicitadas, coloca las disponible en el momento
                            ///si no hay, no coloca nada y descarta la fila.
                            foreach (DataRow drDetalle in dtDetalle.Rows)
                            {
                                ///
                                sizedetalle = (String)drDetalle["ASV_SIZE_DISPLAY"];
                                ///
                                qty_existence_detalle = Convert.ToInt16(drDetalle["QTY_EXISTENCE"]);
                                ///solo valida que ambos sean mayores qeu sero en la 
                                ///apliacion coloca la cantidad qeu este en qty si lo pedido es mayor.
                                if (size.Equals(sizedetalle) && (qty >= 0 && qty_existence_detalle >= 0))
                                {
                                    ///
                                    valid = true;
                                    break;
                                }
                            }
                            ///
                            if (valid)
                            {
                                ///
                                ArticleList al;
                                ///Hace que solo se tenga en cuanta los primeros valores de las tallas qeu se metan
                                if (!ArticleList.containId(article, array))
                                {
                                    ///
                                    al = new ArticleList();
                                    al.IdArticle = article;
                                    ///Creación de un Squema vacio.
                                    DataTable dtDetalleVacio = dtDetalle.Copy();
                                    foreach (DataRow DR in dtDetalleVacio.Rows)
                                    {
                                        DR["qty"] = 0;
                                    }
                                    ///
                                    valid = false;
                                    ///Valida la existencia de la talla.
                                    ///Como logor qeu solo me qeuden los buenos.
                                    ////
                                    foreach (DataRow drArt in dtDetalleVacio.Rows)
                                    {
                                        ///
                                        String sd = (String)drArt["ASV_SIZE_DISPLAY"];
                                        ///
                                        qty_existence_detalle = Convert.ToInt16(drArt["QTY_EXISTENCE"]);
                                        //
                                        if (sd.Equals(size))
                                        {
                                            drArt["qty"] = qty_existence_detalle < qty ? qty_existence_detalle : qty;
                                            //drArt["QTY_EXISTENCE"] = qty;
                                            valid = true;
                                            ///El elemento se deja en un estado valido para 
                                            ///ser leido en la aplicacion y para ser verificado
                                            ///antes del envio.
                                            al.OkElement = true;
                                            ///
                                            al.ListSizes = dtDetalleVacio;
                                            break;
                                        }
                                    }
                                    ///Agrega a la lista
                                    if (valid)
                                    {
                                        array.Add(al);
                                    }
                                }
                                else
                                {
                                    ///
                                    al = ArticleList.getArtcleList(article, array);
                                    ///
                                    valid = false;
                                    ///Valida la existencia de la talla.
                                    ///Como logor qeu solo me qeuden los buenos.
                                    ////
                                    foreach (DataRow drArt2 in al.ListSizes.Rows)
                                    {
                                        ///
                                        String sd = (String)drArt2["ASV_SIZE_DISPLAY"];
                                        ///
                                        qty_existence_detalle = Convert.ToInt16(drArt2["QTY_EXISTENCE"]);
                                        ///
                                        ///int qtyLocal = ()drArt["qty"];
                                        if (sd.Equals(size))
                                        {
                                            ///
                                            int qtyLocal = 0;
                                            //try
                                            //{
                                            qtyLocal = Convert.ToInt16(drArt2["qty"]);
                                            //}
                                            //catch
                                            //{
                                            //    qtyLocal = -1;
                                            //}
                                            ///
                                            if (qtyLocal == 0)
                                            {
                                                drArt2["qty"] = qty_existence_detalle < qty ? qty_existence_detalle : qty;
                                                //drArt2["QTY_EXISTENCE"] = qty;
                                                ///El elemento se deja en un estado valido para 
                                                ///ser leido en la aplicacion y para ser verificado
                                                ///antes del envio.
                                                al.OkElement = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                ///
                ///Boolean toLeave=false;
                ///
                int i = 0;
                //
                //string[] list = (string[])array.ToArray(typeof(string));
                ///Eliminacion a furza bruta de los elementos no validos
                while (i < array.Count/*!toLeave*/)
                {
                    ArticleList artList = (ArticleList)array[i];
                    if (!artList.OkElement)
                    {
                        ///Remueve de la lista los articulos que no Fueron habilitados
                        ArticleList.removeArticleOfList(artList.IdArticle, array);
                        //dt.Rows.Remove(dt.Rows[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                ///Si aun qeu dadn lineas de detalle se retornan
                ///en la vista se debe armar lo que se necesita aqui solo se depura lo innencesario
                if (dt.Rows.Count > 0)
                {
                    return array;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        public static ArrayList ImportArticleSizeQtyFromExcel(string company, string path, string nameFile, string storage_id, string warehouse_id)
        {
            ///Lista de objetos ArticleList. Se usa para hacer las Verificiones y es retornado par su uso enla interfaz.
            ArrayList array = new ArrayList();
            ///Se usa para verificar si es valido a cada paso el archivo leido.
            Boolean valid;
            ///Es el id del articulo leido del XML.
            String article;
            ///Talla del articulo de la linea XML
            String size = "";
            ///Cantidad del articulo de la linea XML
            int qty = 0;
            ///Tabla de las lineas del XML
            DataTable dt;           
            dt = importExcel(path,nameFile,"Hoja1",0);
       
            if (dt != null)
            {
                ///Recorrido del DataTable son todos lo detalle con idArticle, size & qty.
                foreach (DataRow dr in dt.Rows)
                {
                    ///
                    valid = true;
                    try
                    {
                        ///Obtencion del articulo
                        article = dr[namesColumnsExcelArticlesBySizes[0]].ToString();
                    }
                    catch { dt = null; array = null; article = "0"; valid = false; break; }
                    ///validadcion de existencia del articulo
                    if (valid && !Article.existArticle(company, article))
                    {
                        valid = false;
                    }
                    else
                    {
                        try
                        {
                            ///Obtencion de la talla del Articulo
                            size = dr[namesColumnsExcelArticlesBySizes[1]].ToString();
                            ///Obtencion de la cantidad del Artticulo.    
                            qty = Convert.ToInt16(dr[namesColumnsExcelArticlesBySizes[2]].ToString());
                        }
                        catch { valid = false; }
                        ///Se obtiene el detalle del articulo para la validaciones de talla y cantidad.
                        DataTable dtDetalle = STOCKTANSFERSTORAGE.GetArtSizesQtyFromDetTransferStorage(company, article, storage_id).Tables[0];
                        ///
                        if (valid)
                        {
                            ///
                            string sizedetalle;
                            ///
                            int qty_existence_detalle;
                            ///
                            valid = false;
                            ///Ciclo para averiguar si hay la contidades del articulo son validas
                            ///si hay menos de las solicitadas, coloca las disponible en el momento
                            ///si no hay, no coloca nada y descarta la fila.
                            foreach (DataRow drDetalle in dtDetalle.Rows)
                            {
                                ///
                                sizedetalle = (String)drDetalle["ASV_SIZE_DISPLAY"];
                                ///
                                qty_existence_detalle = Convert.ToInt16(drDetalle["QTY_EXISTENCE"]);
                                ///solo valida que ambos sean mayores qeu sero en la 
                                ///apliacion coloca la cantidad qeu este en qty si lo pedido es mayor.
                                if (size.Equals(sizedetalle) && (qty >= 0 && qty_existence_detalle >= 0))
                                {
                                    ///
                                    valid = true;
                                    break;
                                }
                            }
                            ///
                            if (valid)
                            {
                                ///
                                ArticleList al;
                                ///Hace que solo se tenga en cuanta los primeros valores de las tallas qeu se metan
                                if (!ArticleList.containId(article, array))
                                {
                                    ///
                                    al = new ArticleList();
                                    al.IdArticle = article;
                                    ///Creación de un Squema vacio.
                                    DataTable dtDetalleVacio = dtDetalle.Copy();
                                    foreach (DataRow DR in dtDetalleVacio.Rows)
                                    {
                                        DR["qty"] = 0;
                                    }
                                    ///
                                    valid = false;
                                    ///Valida la existencia de la talla.
                                    ///Como logor qeu solo me qeuden los buenos.
                                    ////
                                    foreach (DataRow drArt in dtDetalleVacio.Rows)
                                    {
                                        ///
                                        String sd = (String)drArt["ASV_SIZE_DISPLAY"];
                                        ///
                                        qty_existence_detalle = Convert.ToInt16(drArt["QTY_EXISTENCE"]);
                                        //
                                        if (sd.Equals(size))
                                        {
                                            drArt["qty"] = qty_existence_detalle < qty ? qty_existence_detalle : qty;
                                            //drArt["QTY_EXISTENCE"] = qty;
                                            valid = true;
                                            ///El elemento se deja en un estado valido para 
                                            ///ser leido en la aplicacion y para ser verificado
                                            ///antes del envio.
                                            al.OkElement = true;
                                            ///
                                            al.ListSizes = dtDetalleVacio;
                                            break;
                                        }
                                    }
                                    ///Agrega a la lista
                                    if (valid)
                                    {
                                        array.Add(al);
                                    }
                                }
                                else
                                {
                                    ///
                                    al = ArticleList.getArtcleList(article, array);
                                    ///
                                    valid = false;
                                    ///Valida la existencia de la talla.
                                    ///Como logor qeu solo me qeuden los buenos.
                                    ////
                                    foreach (DataRow drArt2 in al.ListSizes.Rows)
                                    {
                                        ///
                                        String sd = (String)drArt2["ASV_SIZE_DISPLAY"];
                                        ///
                                        qty_existence_detalle = Convert.ToInt16(drArt2["QTY_EXISTENCE"]);
                                        ///
                                        ///int qtyLocal = ()drArt["qty"];
                                        if (sd.Equals(size))
                                        {
                                            ///
                                            int qtyLocal = 0;
                                            //try
                                            //{
                                            qtyLocal = Convert.ToInt16(drArt2["qty"]);
                                            //}
                                            //catch
                                            //{
                                            //    qtyLocal = -1;
                                            //}
                                            ///
                                            if (qtyLocal == 0)
                                            {
                                                drArt2["qty"] = qty_existence_detalle < qty ? qty_existence_detalle : qty;
                                                //drArt2["QTY_EXISTENCE"] = qty;
                                                ///El elemento se deja en un estado valido para 
                                                ///ser leido en la aplicacion y para ser verificado
                                                ///antes del envio.
                                                al.OkElement = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                ///
                ///Boolean toLeave=false;
                ///
                int i = 0;
                //
                //string[] list = (string[])array.ToArray(typeof(string));
                ///Eliminacion a furza bruta de los elementos no validos
                while (i < array.Count/*!toLeave*/)
                {
                    ArticleList artList = (ArticleList)array[i];
                    if (!artList.OkElement)
                    {
                        ///Remueve de la lista los articulos que no Fueron habilitados
                        ArticleList.removeArticleOfList(artList.IdArticle, array);
                        //dt.Rows.Remove(dt.Rows[i]);
                    }
                    else
                    {
                        i++;
                    }
                }
                ///Si aun qeu dadn lineas de detalle se retornan
                ///en la vista se debe armar lo que se necesita aqui solo se depura lo innencesario
                if (dt.Rows.Count > 0)
                {
                    return array;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}