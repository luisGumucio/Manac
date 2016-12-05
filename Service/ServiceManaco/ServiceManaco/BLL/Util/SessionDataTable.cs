using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Bata.Aquarella.BLL;
using Bata.Aquarella.BLL.Maestros;

namespace Bata.Aquarella.BLL.Util
{
    public class SessionDataTable
    {
        #region <VARIABLES>
        /// <summary>
        /// Columna que contiene el estado de la fila, permite saber si la fila fue modoficada, llego nueva desde la
        /// base de datos o no se hizo nada con ella.
        /// </summary>
        public static String columnNameState = "STATE";
        /// <summary>
        /// Lista de posibles estados de las filas de la tabla none = sin cambio, amended = modificado,
        /// new = filas nuevas.
        /// </summary>
        static String[] rowStates = { "NONE", "AMENDED", "NEW" };
        /// <summary>
        ///Indice de la cadena que representa el valor sin cambio en las filas 
        /// </summary>
        public static int NONE_ROW = 0;
        /// <summary>
        /// Indice de la cadena que representa el valor modificado en las filas 
        /// </summary>
        public static int AMENDED_ROW = 1;
        /// <summary>
        /// Indice de la cadena que representa el valor nuevo en las filas 
        /// </summary>
        public static int NEW_ROW = 2;
        /// <summary>
        /// Nombre de la tabla donde se guaradará la informacion temporal/ de ODV
        /// </summary>
        static String nameTablaODV = "ODVTABLETEMP";
        /// <summary>
        /// Nombre de las columnas calculadas o alimentadas de una tabla para mostrar.
        /// </summary>
        public static String[] columnsNameODVCalc = { "OLD_ODV", "DIFERENT", "MARGEN" };
        /// <summary>
        /// 
        /// </summary>
        public static int _OLD_ODV = 0;
        /// <summary>
        /// 
        /// </summary>
        public static int _DIFFERENT_ODV = 1;
        /// <summary>
        /// 
        /// </summary>
        public static int _MARGEN_ODV = 2;
        //Colocar estaticas con las columnas de la tabla odvtemp 
        /// <summary>
        /// Nombre de la tabla donde se guaradará la informacion temporal/ de PRICES
        /// </summary>
        static String nameTablePrices = "PRICETABLETEMP";
        /// <summary>
        /// Nombre de las columnas calculadas o alimentadas de uana tabla para mostrar.
        /// </summary>
        public static String[] columnsNamePricesCalc = { "OLD_PRICE", "DIFERENT", "COST", "MARGEN", };
        /// <summary>
        /// 
        /// </summary>
        public static int _OLD_PRICE = 0;
        /// <summary>
        /// 
        /// </summary>
        public static int _DIFERENT_PRICE = 1;
        /// <summary>
        /// 
        /// </summary>
        public static int _COST = 2;
        /// <summary>
        /// 
        /// </summary>
        public static int _MARGEN_PRICE = 3;
        #endregion

        #region <METODOS COMUNES>
        /// <summary>
        /// Guarda una tabla con un esquema definido o contenido en la session.
        /// </summary>
        /// <param name="nameTable">Nombre de la tabla</param>
        /// <param name="table">Schema o tablas con datos</param>
        /// <returns>true si crea la tabla, false si no pude o si ya existe.</returns>
        public static Boolean saveTable(String nameTable, DataTable table)
        {
            if (HttpContext.Current.Session[nameTable] == null)
            {
                HttpContext.Current.Session[nameTable] = table;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Busca en la session la tabla con este nombre y la retorna.
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        static public DataTable getTable(String nameTable)
        {
            DataTable dt;
            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                return dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Agrega una fila a la tabla con este nombre.
        /// </summary>
        /// <param name="nameTable">Tabla en la session</param>
        /// <param name="row">Nueva fila con el esquema de la tabla con este nombre</param>
        /// <returns></returns>
        static public Boolean addRow(String nameTable, DataRow row)
        {
            DataTable dt;
            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                dt.Rows.Add(row);
                HttpContext.Current.Session[nameTable] = dt;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Aegrega el row especificado a la tabla especificada en la posicion especificada.
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="row"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        static public Boolean addRow(String nameTable, DataRow row, int pos)
        {
            DataTable dt;
            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                dt.Rows.InsertAt(row, pos);
                HttpContext.Current.Session[nameTable] = dt;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// elimina un row de la tabla especificada en la posicion especificada.
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        static public Boolean deleteRow(String nameTable, int pos)
        {
            DataTable dt;
            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                dt.Rows.RemoveAt(pos);
                HttpContext.Current.Session[nameTable] = dt;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Verifica ala existencia del row especificado en la tabla de la session. 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        static public Boolean existRow(String nameTable, object[] keys)
        {
            DataTable dt;

            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];

                return dt.Rows.Contains(keys);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Table needs the definition of a PK
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public Boolean existRow(String nameTable, object key)
        {
            DataTable dt;

            if (HttpContext.Current.Session[nameTable] != null)
            {
                dt = (DataTable)HttpContext.Current.Session[nameTable];

                return dt.Rows.Contains(key);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Remove table in session state.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Null if you do not find the table o table no delete, True if not</returns>
        static public Boolean deleteTable(string nameTable)
        {
            if (HttpContext.Current.Session[nameTable] != null)
            {
                HttpContext.Current.Session.Remove(nameTable);
                if (HttpContext.Current.Session[nameTable] == null)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Retorna el nombre de la columna de estados qeu tienen todas las tablas manejadas en la session.
        /// </summary>
        /// <returns></returns>
        public static string getNameColState()
        {
            return columnNameState;
        }
        /// <summary>
        /// Retorna los valores qeu representan los estados de la columna de estados.
        /// </summary>
        /// <returns>Cadena que representa un estado, o vacio si el estdo buscado no existe</returns>
        public static string getStatesRow(int state)
        {
            switch (state)
            {
                case 0:
                    return rowStates[NONE_ROW];
                case 1:
                    return rowStates[AMENDED_ROW];
                case 2:
                    return rowStates[NEW_ROW];
            }
            return "";
        }

        # endregion

        #region <METODOS ODV>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        static public DataRow getDataRowSchemeODV()
        {
            if (HttpContext.Current.Session[nameTablaODV] == null)
            {
                //Tabla vacia  |campo1|campo2|campo3|...|campoN|
                DataTable dt = Odv.getSchemeTableODV();
                //
                dt.Columns.Add(columnsNameODVCalc[_OLD_ODV]);
                dt.Columns[columnsNameODVCalc[_OLD_ODV]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnsNameODVCalc[_DIFFERENT_ODV]);
                dt.Columns[columnsNameODVCalc[_DIFFERENT_ODV]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnNameState);
                //Margen
                dt.Columns.Add(columnsNameODVCalc[_MARGEN_ODV]);
                dt.Columns[columnsNameODVCalc[_MARGEN_ODV]].DataType = typeof(System.Decimal);
                //
                saveTable(getNameTableOdV(), dt);
                //
                return dt.NewRow();
            }
            else
            {
                //return Next row  datatable ODV
                return ((DataTable)HttpContext.Current.Session[nameTablaODV]).NewRow();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public string getNameTableOdV()
        {
            return nameTablaODV;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public Boolean deleteRowODV(String nameTable, string key)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(key))
                    {
                        //
                        dr.Delete();
                        return true;
                    }
                }
            }
            //
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public Boolean upDateRowODV(String nameTable, string key, decimal value)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(key))
                    {
                        try
                        {
                            //
                            dr[dt.Columns[4]] = value;

                            dr[dt.Columns[6]] = Convert.ToDecimal(dr[dt.Columns[5]]) - Convert.ToDecimal(dr[dt.Columns[4]]);

                            dr[dt.Columns[7]] = getStatesRow(AMENDED_ROW);

                            dr[dt.Columns[8]] = Utilities.margenCalc(Prices.getLastPricePublic(dr[dt.Columns[0]].ToString(), key), Convert.ToDecimal(value));
                        }
                        catch { return false; }
                        return true;
                    }
                }
            }
            //
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        static public Boolean existRowODV(String nameTable, string article)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(article))
                    {
                        return true;
                    }
                }
            }
            //
            return false;
        }
        # endregion

        #region <METODOS PRICES>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string getNameTablePrices()
        {
            return nameTablePrices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        static public DataRow getDataRowSchemePrices()
        {
            //
            if (HttpContext.Current.Session[nameTablePrices] == null)
            {
                //Tabla vacia  |campo1|campo2|campo3|...|campoN|
                DataTable dt = Prices.getSchemeTablePrices();
                //
                dt.Columns.Add(columnsNamePricesCalc[_OLD_PRICE]);
                dt.Columns[columnsNamePricesCalc[_OLD_PRICE]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnsNamePricesCalc[_DIFERENT_PRICE]);
                dt.Columns[columnsNamePricesCalc[_DIFERENT_PRICE]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnsNamePricesCalc[_COST]);
                dt.Columns[columnsNamePricesCalc[_COST]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnsNamePricesCalc[_MARGEN_PRICE]);
                dt.Columns[columnsNamePricesCalc[_MARGEN_PRICE]].DataType = typeof(System.Decimal);
                //
                dt.Columns.Add(columnNameState);
                //
                saveTable(getNameTablePrices(), dt);
                //
                return dt.NewRow();
            }
            else
            {
                //return Next row  datatable ODV
                return ((DataTable)HttpContext.Current.Session[nameTablePrices]).NewRow();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        static public Boolean existRowPrice(String nameTable, string article)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(article))
                    {
                        return true;
                    }
                }
            }
            //
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        static public Boolean deleteRowPrice(String nameTable, string key)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(key))
                    {
                        //
                        dr.Delete();
                        return true;
                    }
                }
            }
            //
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public Boolean upDateRowPUBLICS_PRICES(String nameTable, string key, Decimal value)
        {
            //
            DataTable dt;
            //
            if (HttpContext.Current.Session[nameTable] != null)
            {
                //
                dt = (DataTable)HttpContext.Current.Session[nameTable];
                //
                foreach (DataRow dr in dt.Rows)
                {
                    //
                    if (dr[dt.Columns[1]].ToString().Equals(key))
                    {
                        try
                        {
                            //
                            dr[dt.Columns[2]] = value;
                            //
                            dr[dt.Columns[6]] = Convert.ToDecimal(dr[dt.Columns[2]]) - Convert.ToDecimal(dr[dt.Columns[5]]);
                            //
                            dr[dt.Columns[8]] = Utilities.margenCalc(Convert.ToDecimal(dr[dt.Columns[2]]), Convert.ToDecimal(dr[dt.Columns[7]]));
                            //
                            dr[dt.Columns[9]] = getStatesRow(AMENDED_ROW);
                        }
                        catch { return false; }
                        return true;
                    }
                }
            }
            //
            return false;
        }
        #endregion

    }
}