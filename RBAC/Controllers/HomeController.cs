using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Filters;
using RBAC.Models;

namespace RBAC.Controllers
{
    [CustomAuthorizationArrtibute(Type=AuthorizationType.Identity)]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 头部的分部视图Action
        /// </summary>
        /// <returns></returns>
        public ActionResult Header()
        {
            var user = Session["user"] as User;
            var role = Session["role"] as Role;

            var roles = Session["roles"] as List<Role>;
            //声明一个下拉列表项的集合
            var rolesSelect = new List<SelectListItem>();
            foreach (var item in roles)
            {
                rolesSelect.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString(), Selected = item.Id == role.Id });
            }
            ViewBag.RoleSelect = rolesSelect;
            return PartialView(user);
        }
        /// <summary>
        /// 导航栏的分部视图Action
        /// </summary>
        /// <returns></returns>
        public ActionResult Nav(int roleId=0)
        {
            //从Session拿到用户的所有角色模块
            var roleModules = Session["roleModules"] as Dictionary<int, ICollection<Module>>;
            //从Session拿到用户的所有角色模块
            var roles = Session["roles"] as List<Role>;
            ICollection<Module> modules;
            if (roleModules.ContainsKey(roleId))
            {
                //如果参数的角色ID存在
                modules = roleModules[roleId];
                //设定当前角色为rolesId参数指定的角色
                Session["role"] = roles.FirstOrDefault(r => r.Id == roleId);
            }
            else
            {
                //如果参数的角色ID不存在
                //从session拿到登录设定角色
                var role = Session["role"] as Role;
                //从当前角色拿到所有模块
                modules = role.Modules;
            }
            
            
            //modules作为强类型，传入nav的分部视图
            return PartialView(modules);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}