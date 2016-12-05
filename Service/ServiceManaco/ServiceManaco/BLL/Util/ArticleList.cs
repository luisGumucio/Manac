using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace Bata.Aquarella.BLL.Util
{
    public class ArticleList
    {
        #region < VARIABLES >
        static public int _COLSIZE = 0;
        static public int _COLQTY = 1;
        static public int _COLEXIST_QTY = 2;
        static public String[] _COLS_LIST_SIZES = { "ASV_SIZE_DISPLAY", "qty", "QTY_EXISTENCE" };
        private String _idArticle;
        private DataTable _listSizes;
        /// <summary>
        /// Es falso por defecto
        /// </summary>
        private Boolean _ok;
        #endregion

        #region < CONTRUCTORES >
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idArticle"></param>
        /// <param name="ListSizes"></param>
        public ArticleList(String idArticle, DataTable ListSizes)
        {
            ///
            _idArticle = idArticle;
            ///
            _listSizes = ListSizes;
            ///
            _ok = false;
        }
        #endregion

        #region < PROPIEDADES >
        /// <summary>
        /// Identificador del articulo.
        /// </summary>
        public string IdArticle
        {
            get { return _idArticle; }
            set { _idArticle = value; }
        }
        /// <summary>
        /// tabla con los detalles del articulo.
        /// </summary>
        public DataTable ListSizes
        {
            get { return _listSizes; }
            set { _listSizes = value; }
        }
        /// <summary>
        /// Campo de estado para la manipulacion en una lista.
        /// </summary>
        public Boolean OkElement
        {
            get { return _ok; }
            set { _ok = value; }
        }
        /// <summary>
        /// contructor vacio, coloca el iedntificador de articulo en vacio la lista en nueva y <br />
        /// el campo de verificacion en false.
        /// </summary>
        public ArticleList()
        {
            ///
            _idArticle = "";
            ///
            _listSizes = new DataTable();
            ///
            _ok = false;
        }
        #endregion

        #region<METODOS PUBLICOS>

        /// <summary>
        /// Cuanta el numero de pares de contenidos en la lsiata de detalles del objeto.
        /// </summary>
        /// <returns></returns>
        public Decimal countPares()
        {
            ///
            Decimal totalPares = 0;
            ///
            foreach (DataRow dli in ListSizes.Rows)
            {
                //totalPares = totalPares + Convert.ToDecimal(((TextBox)dli.FindControl("txtQty")).Text);
                totalPares = totalPares + Convert.ToDecimal(dli[_COLS_LIST_SIZES[_COLQTY]]);
            }
            ///
            return totalPares;
        }
        /// <summary>
        /// retornma el precio publico total de todos los detalles del articulo.
        /// </summary>
        /// <param name="public_price_Unit"></param>
        /// <returns></returns>
        public Decimal public_Price_Total(Decimal public_price_Unit)
        {
            ///
            return countPares() * public_price_Unit;
        }
        /// <summary>
        /// retornma el costo total de todos los detalles del articulo.
        /// </summary>
        /// <param name="cost_Unit"></param>
        /// <returns></returns>
        public Decimal cost_Total(Decimal cost_Unit)
        {
            ///
            return countPares() * cost_Unit;
        }

        #endregion

        #region<METODOS PRIVADOS>
        #endregion

        #region<METODOS ESTATICOS>

        /// <summary>
        /// saca de un arreglo el Articlelist especificado.
        /// </summary>
        /// <param name="idArticle"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        static public ArticleList getArtcleList(String idArticle, ArrayList articleListArray)
        {
            ///
            ArticleList al;
            ///
            for (int i = 0; i < articleListArray.Count; i++)
            {
                ///
                al = (ArticleList)articleListArray[i];
                ///
                if (al.IdArticle.Equals(idArticle))
                {
                    ///
                    return al;
                }
            }
            ///
            return null;
        }
        /// <summary>
        /// Elimina uno de los articulos con lista de una ArrayList.
        /// </summary>
        /// <param name="idArticle"></param>
        /// <param name="articleList"></param>
        /// <returns></returns>
        static public bool removeArticleOfList(String idArticle, ArrayList articleList)
        {
            ///
            int i = 0;
            ///
            ArticleList al;
            ///
            while (i < articleList.Count)
            {
                ///
                al = (ArticleList)articleList[i];
                ///
                if (al.IdArticle == idArticle)
                {
                    ///
                    articleList.RemoveAt(i);
                    ///
                    return true;
                }
                ///
                i++;
            }
            ///
            return false;
        }
        /// <summary>
        /// Verifica la existyencia de un nombre de ariculo en un ArrayList de ArticlesList.
        /// </summary>
        /// <param name="idArticle"></param>
        /// <param name="articleList"></param>
        /// <returns></returns>
        static public bool containId(String idArticle, ArrayList articleList)
        {
            ///
            ArticleList al;
            ///
            for (int i = 0; i < articleList.Count; i++)
            {
                ///
                al = (ArticleList)articleList[i];
                ///
                if (al.IdArticle.Equals(idArticle))
                {
                    ///
                    return true;
                }
            }
            ///
            return false;
        }
        /// <summary>
        /// Solo Retorna el detalle del articulo en forma de ArticleList LA tabla interna tendra los campos
        /// ASV_SIZE_DISPLAY, qty, QTY_EXISTENCEdentro de sus DataList y Solo funciona si DataList controles
        /// internos lblTalla, txtQty, hdQtyExistenteArt" dedondde se sacarn estos tres campos.
        /// </summary>
        /// <param name="article"></param>
        /// <param name="datalist"></param>
        /// <returns></returns>
        static public ArticleList convetToArticleList(string article, DataList datalist)
        {
            ///
            DataTable dt = new DataTable();
            dt.Columns.Add(_COLS_LIST_SIZES[_COLSIZE]);
            dt.Columns.Add(_COLS_LIST_SIZES[_COLQTY]);
            dt.Columns.Add(_COLS_LIST_SIZES[_COLEXIST_QTY], typeof(int));
            DataRow dr;
            //
            DataListItem item = datalist.Items[0];
            //
            foreach (DataListItem dli in datalist.Items)
            {
                //
                dr = dt.NewRow();
                //
                dr[_COLS_LIST_SIZES[_COLSIZE]] = ((Label)(dli.FindControl("lblTalla"))).Text;
                //
                dr[_COLS_LIST_SIZES[_COLQTY]] = ((TextBox)(dli.FindControl("txtQty"))).Text;
                //
                string str = ((HiddenField)(dli.FindControl("hdQtyExistenteArt"))).Value;
                //
                if (!string.IsNullOrEmpty(str))
                {
                    dr[_COLS_LIST_SIZES[_COLEXIST_QTY]] = Convert.ToInt32(str);
                }
                else
                {
                    dr[_COLS_LIST_SIZES[_COLEXIST_QTY]] = 0;
                }
                //
                dt.Rows.Add(dr);
            }
            ///
            return new ArticleList(article, dt);
        }
        #endregion
    }
}