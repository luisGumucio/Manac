using System;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Bata.Aquarella.BLL.Maestros
{
    public class Category : CategoryBase
    {
          #region < VARIABLES >
        #endregion

        #region < CONSTRUCTORES >

        public Category() : base() { }
		
		#endregion
		        
        #region < PROPIEDADES >
        #endregion

        #region < METODOS PUBLICOS >
        #endregion

        #region < METODOS PRIVADOS >
        #endregion

        #region < METODOS ESTATICOS >
        
        /// <summary>
        /// Permite actualizar las Categorias
        /// </summary>
        /// <param name="CAV_CO"></param>
        /// <param name="CAV_CAT_ID"></param>
        /// <param name="CAV_DESCRIPTION"></param>
        /// <returns>true update exitosa, false caso contrario.</returns>
        static public bool actualizarC(string CAV_CO, string CAV_CAT_ID, string CAV_DESCRIPTION)
        {

            Category c = new Category();

            c.CAV_CO = CAV_CO;
            c.CAV_CAT_ID = CAV_CAT_ID;
            c.CAV_DESCRIPTION = CAV_DESCRIPTION.ToUpper().Trim();

            c.IsNew = false;

            if (c.Save())
                return true;
            else
                return false;
        }

        #endregion
    }
}