using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Filters;
using RBAC.Models;
using System.Data.Entity.Migrations;

namespace RBAC.Controllers
{
    public class ModuleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: Module
        public ActionResult Index()
        {
            return View(db.Modules);
        }
        public ActionResult Edit(int id)
        {
            var module = db.Modules.FirstOrDefault(r => r.Id == id);

            return View(module);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save(Module module)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Modules.AddOrUpdate(module);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
        public ActionResult Delete(int id)
        {
            //实例化一个要删除的module，必须设置主键字段ID
            Module module = new Module { Id = id };
            //把这个我们创建出来的实体，伪装成数据库读出来的实体
            db.Modules.Attach(module);
            //删除实体
            db.Modules.Remove(module);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
    }
}