using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RBAC.Models;
using RBAC.ViewModels;

namespace RBAC.Controllers
{
    public class RoleModuleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: RoleModule
        public ActionResult Index()
        {
            var result = db.Roles.Include(r => r.Modules);
            return View(result);
        }
        public ActionResult Create()
        {
            //所有角色的下拉列表项
            ViewBag.RoleOptions = from r in db.Roles
                                  select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            //所有模块下拉列表
            ViewBag.ModuleOptions = from r in db.Modules
                                  select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            return View();
        }
        public ActionResult Edit(RoleModuleViewModel rolemodule)
        {
           //所有模块下拉列表
            ViewBag.ModuleOptions = from r in db.Modules
                                    select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            //角色名称
            rolemodule.RoleName = db.Roles.FirstOrDefault(r => r.Id == rolemodule.RoleId).Name;
            //模块名称
            rolemodule.ModuleName = db.Modules.FirstOrDefault(r => r.Id == rolemodule.ModuleId).Name;
            return View(rolemodule);
        }
        public ActionResult Delete(RoleModuleViewModel rolemodule)
        {
            var role = db.Roles.FirstOrDefault(r => r.Id == rolemodule.RoleId);
            var module = new Module { Id = rolemodule.ModuleId };

            //伪装成从数据库读取出来一样
            db.Modules.Attach(module);
            role.Modules.Remove(module);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
        public ActionResult Insert(RoleModuleViewModel rolemodule)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            var role = db.Roles.FirstOrDefault(r => r.Id == rolemodule.RoleId);
            var module = new Module { Id = rolemodule.ModuleId };

            //伪装成从数据库读取出来一样
            db.Modules.Attach(module);
            //这一步是关联的关键，把modul添加到role的moduls
            role.Modules.Add(module);
            if (db.SaveChanges()==0)
            {
                return Json(new { code = 400 }); 
            }
            return Json(new { code = 200 });
        }
        public ActionResult Update(RoleModuleViewModel roleModule)
        {
            if (roleModule.ModuleId==roleModule.UpdateModuleId)
            {
                return Json(new { code = 400, msg = "更新模块不能和原模块相同" });
            }
            var role = db.Roles.FirstOrDefault(r => r.Id == roleModule.RoleId);
            var module = new Module { Id = roleModule.ModuleId };
            var updateModule = new Module { Id = roleModule.UpdateModuleId };
            db.Modules.Attach(module);
            db.Modules.Attach(updateModule);
            role.Modules.Remove(module);
            role.Modules.Add(updateModule);
            if (db.SaveChanges()==0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}