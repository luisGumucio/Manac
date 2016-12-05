using Bata.Aquarella.BLL;
using ServiceManaco.Authentication;
using ServiceManaco.Models;
using ServiceManaco.SecurityToken;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceManaco.Controllers
{
    [AuthorizeRest]
    public class LoginController : Controller
    {
        /// <summary>
        /// login for access to app
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult session(User user)
        {
            Users current = new Users();
            current = loadUser(user.UserName);
            SecurityManager.GenerateToken(current._usv_name, current._usv_password,
                Configuration.ip, Configuration.agent, Configuration.time);
            return Json(current, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Carga de usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private Users loadUser(string userName)
        {
            DataTable dtUser = Users.getAndLoadAnyUserByUserName(userName).Tables[0];

            if (dtUser == null || dtUser.Rows.Count <= 0)
            {
                return null;
            }

            string idUserEmployee = dtUser.Rows[0]["emn_employee_id"].ToString();
            bool isEmployee = false;
            string idUserCustomer = dtUser.Rows[0]["con_coordinator_id"].ToString();
            bool isCustomer = false;
            string wareId, wareDesc, areaId, areaDesc, regionId;
            DataRow dr = dtUser.Rows[0];

            // 1. Verificar si es empleado
            if (string.IsNullOrEmpty(idUserEmployee))
                // 2. Verificar si es cliente
                if (string.IsNullOrEmpty(idUserCustomer))
                    return null;
                else
                {
                    isCustomer = true;
                    wareId = dr["cov_warehouseid"].ToString();
                    wareDesc = dr["wav_descriptionc"].ToString();
                    areaDesc = dr["arv_descriptionc"].ToString();
                    areaId = dr["bdv_area_id"].ToString();
                    regionId = dr["dpv_region"].ToString();
                }
            else
            {
                isEmployee = true;
                wareId = dr["dpv_warehouse"].ToString();
                wareDesc = dr["wav_description"].ToString();
                areaDesc = dr["arv_descriptione"].ToString();
                areaId = dr["dpv_area"].ToString();
                regionId = dr["dpv_region"].ToString();
            }

            Users u = new Users
            {
                _usn_userid = Convert.ToDecimal(dr["usn_userid"]),
                _usv_co = dr["usv_co"].ToString(),
                _usv_employee = isEmployee,
                _usv_customer = isCustomer,
                _usv_answer = dr["usv_answer"].ToString(),
                _usv_question = dr["usv_question"].ToString(),
                _usv_status = dr["usv_status"].ToString(),
                _usv_username = dr["usv_username"].ToString(),
                _usv_name = dr["name"].ToString(),
                _usd_creation = System.DateTime.Parse(dr["usd_creation"].ToString()),
                _usv_warehouse = wareId,
                _usv_warehouse_name = wareDesc,
                _usv_area = areaId,
                _usv_area_name = areaDesc,
                _usv_region = regionId,
                _usv_password = dr["usv_password"].ToString(),
                _attempts = int.Parse(dr["usn_failedattemptcount"].ToString()),
                _storages = dr["cov_storages"].ToString()
            };

            return u;
        }

        public string validate()
        {
            return null;
        }
    }
}