using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Models;
using System.Data.Entity.Migrations;

namespace RBAC.Controllers
{
    public class UserController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: User
        public ActionResult Index()
        {
            return View(db.Users);
        }
        public ActionResult Edit(int id)
        {
            var user = db.Users.FirstOrDefault(r => r.Id == id);
            return View(user);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Save(User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = 400 });
            }
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
        public ActionResult Delete(int id)
        {
            //实例化一个要删除的module，必须设置主键字段ID
            User user = new User { Id = id };
            //把这个我们创建出来的实体，伪装成数据库读出来的实体
            db.Users.Attach(user);
            //删除实体
            db.Users.Remove(user);
            db.SaveChanges();
            return Json(new { code = 200 });
        }
    }
}