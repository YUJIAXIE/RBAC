using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Models;
using RBAC.Filters;

namespace RBAC.Controllers
{
    [CustomAuthorizationArrtibute(Type=AuthorizationType.None)]
    public class LoginController : Controller
    {
        RbacDb db = new RbacDb();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(User loginUser)
        {
            if (!ModelState.IsValid)
            {
                //模型绑定验证失败
                return Json(new { code = 400 });
            }
            //查找用户及密码
            var user = db.Users.Include("Roles").FirstOrDefault(u => u.UserName == loginUser.UserName && u.PassWord == loginUser.PassWord);
            //如果没找到，就返回json404
            if (user == null) return Json(new { code = 404 });
            //存入Session，作为身份认证的标识
            Session["user"] = user;
            //当前用户所有角色列表
            var roles = user.Roles.ToList();
            Session["roles"] = roles;
            //获取当前这个用户所有角色的所有模块,并转换为字典类型，Key是角色的Id，value是角色的所有模块
            var roleModules = user.Roles.ToDictionary(r => r.Id, r => r.Modules) ;
            //存入session，以便于之后复用，不再查数据库
            Session["roleModules"] = roleModules;
            //设定登录时的默认角色
            Session["role"] = roles[0];
            return Json(new { code = 200 });
        }
    }
}