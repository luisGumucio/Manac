using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Maestros
{
    public class SubCategory: SubCategoryBase
    {

         #region < VARIABLES >
        #endregion

        #region < CONSTRUCTORES >

        public SubCategory() : base() { }
		
		#endregion

        #region < PROPIEDADES >
        #endregion

        #region < METODOS PUBLICOS >
        #endregion

        #region < METODOS PRIVADOS >
        #endregion

        #region < METODOS ESTATICOS >

        /// <summary>
        /// Permite la actualizacion de la subcategoria especificada. Es un metodo especializado para <br />
        /// los update de lso objectdatasourse.
        /// </summary>
        /// <param name="SCV_CO"></param>
        /// <param name="SCV_SUBCAT_ID"></param>
        /// <param name="SCV_DESCRIPTION"></param>
        /// <returns></returns>
        static public bool actualizarSC(string SCV_CO, string SCV_SUBCAT_ID, string SCV_DESCRIPTION)
        {
            SubCategory sc = new SubCategory();
            ///
            sc.SCV_CO = SCV_CO;
            sc.SCV_SUBCAT_ID = SCV_SUBCAT_ID.ToUpper().Trim();
            sc.SCV_DESCRIPTION = SCV_DESCRIPTION.ToUpper().Trim();
            ///
            sc.IsNew = false;
            ///Verificaion de guardado
            if (sc.Save())
                return true;
            else
                return false;
        }

        #endregion
    }
}