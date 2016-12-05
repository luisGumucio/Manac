﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bata.Aquarella.BLL.Util
{
    public class BarCodes
    {
        public BarCodes()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeBar"></param>
        /// <returns></returns>
        public static String[] getInfoFromTheBarCode(String codeBar)
        {
            String[] infoArray = new String[2];
            ///

            /// Determinar el numero de digitos enviados en la cadena
            /// 
            int numDigitsInCodeBar = codeBar.Length;

            /// Determinado el numero de digitos en el codigo de barras enviarlo a la
            /// funcion que lo podra evaluar
            /// 
            if (numDigitsInCodeBar == 14)
            {
                ///
                String refArticle = codeBar.ToString().Substring(0, 8);

                ///
                String size = codeBar.Substring(8, 2);

                /// Ref Articulo
                /// 
                infoArray[0] = refArticle;

                /// Talla
                /// 
                infoArray[1] = size;

                ///
                return infoArray;
            }
            else if (numDigitsInCodeBar == 10)
            {
                ///
                ///
                String refArticle = codeBar.ToString().Substring(0, 8);

                ///
                String size = codeBar.Substring(8, 2);

                /// Ref Articulo
                /// 
                infoArray[0] = refArticle;

                /// Talla
                /// 
                infoArray[1] = size;

                ///
                return infoArray;
            }
            /// para cuando sea un accesorio talla de un caracter
            else if (numDigitsInCodeBar == 9)
            {
                ///
                ///
                String refArticle = codeBar.ToString().Substring(0, 8);

                ///
                String size = codeBar.Substring(8, 1);

                /// Ref Articulo
                /// 
                infoArray[0] = refArticle;

                /// Talla
                /// 
                infoArray[1] = size;

                ///
                return infoArray;
            }
            /// Ean13
            /// Digit 1-> Company
            /// Digit 2-9-> Article ref
            /// Digit 10-> Cte
            /// Digit 11-12-> Position in plane
            /// Digit 13-> Static of verification
            else if (numDigitsInCodeBar == 13)
            {
                ///
                String refArticle = codeBar.ToString().Substring(1, 8);

                ///
                String posPlaneColumn = (Convert.ToDecimal(codeBar.Substring(10, 2))).ToString();

                /// Ref Articulo
                /// 
                infoArray[0] = refArticle;

                /// Columna en el plano
                /// 
                infoArray[1] = posPlaneColumn;

                ///
                return infoArray;
            }

            ///
            return null;
        }
    }
}