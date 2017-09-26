using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RBAC.Models;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace RBAC.Controllers
{
    public class RoleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: Role
        public ActionResult Index()
        {
            return View(db.Roles);
        }
        public ActionResult Edit(int id)
        {
            var role = db.Roles.FirstOrDefault(r => r.Id == id);

            return View(role);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save(Role role)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Roles.AddOrUpdate(role);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
        public ActionResult Delete(int id)
        {
            //实例化一个要删除的module，必须设置主键字段ID
            Role role = new Role { Id = id };
            //把这个我们创建出来的实体，伪装成数据库读出来的实体
            db.Roles.Attach(role);
            //删除实体
            db.Roles.Remove(role);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
    }
}