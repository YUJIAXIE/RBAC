using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Models;
using RBAC.Filters;

namespace RBAC.Controllers
{
    [CustomAuthorizationArrtibute(Type =AuthorizationType.None)]
    public class RegController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: Reg
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reg(User regUser)
        {
            //验证数据模型
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            try
            {
                var role = db.Roles.FirstOrDefault(r => r.Id == 3);
                regUser.Roles.Add(role);
                db.Users.Add(regUser);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}