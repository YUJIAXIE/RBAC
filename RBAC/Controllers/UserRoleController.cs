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
    public class UserRoleController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: UserRole
        public ActionResult Index()
        {
            var result = db.Users.Include(r => r.Roles);
            return View(result);
        }
        public ActionResult Create()
        {
            //所有角色的下拉列表项
            ViewBag.UserOptions = from r in db.Users
                                  select new SelectListItem { Text = r.UserName, Value = r.Id.ToString() };
            //所有模块下拉列表
            ViewBag.RoleOptions = from r in db.Roles
                                    select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            return View();
        }
        public ActionResult Edit(UserRoleViewModel userRole)
        {
            //所有模块下拉列表
            ViewBag.RoleOptions = from r in db.Roles
                                    select new SelectListItem { Text = r.Name, Value = r.Id.ToString() };
            //用户名称
            userRole.UserName = db.Users.FirstOrDefault(r => r.Id == userRole.UserId).UserName;
            //角色名称
            userRole.RoleName = db.Roles.FirstOrDefault(r => r.Id == userRole.RoleId).Name;
            return View(userRole);
        }
        public ActionResult Delete(UserRoleViewModel userRole)
        {
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);
            var role = new Role { Id = userRole.RoleId };

            //伪装成从数据库读取出来一样
            db.Roles.Attach(role);
            user.Roles.Remove(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
        public ActionResult Insert(UserRoleViewModel userRole)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);
            var role = new Role { Id = userRole.RoleId };

            //伪装成从数据库读取出来一样
            db.Roles.Attach(role);
            //这一步是关联的关键，把modul添加到role的moduls
            user.Roles.Add(role);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
        public ActionResult Update(UserRoleViewModel userRole)
        {
            if (userRole.RoleId == userRole.UpdateRoleId)
            {
                return Json(new { code = 400, msg = "更新模块不能和原模块相同" });
            }
            var user = db.Users.FirstOrDefault(r => r.Id == userRole.UserId);
            var role = new Role { Id = userRole.RoleId };
            var updateRole = new Role { Id = userRole.UpdateRoleId };
            db.Roles.Attach(role);
            db.Roles.Attach(updateRole);
            user.Roles.Remove(role);
            user.Roles.Add(updateRole);
            if (db.SaveChanges() == 0)
            {
                return Json(new { code = 400 });
            }
            return Json(new { code = 200 });
        }
    }
}