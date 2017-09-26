using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RBAC.Models;


namespace RBAC.Filters
{
    /// <summary>
    /// 授权过滤器认证类型枚举
    /// None 无限制，不认证，比如登录，注册
    /// Identity 仅身份认证，比如首页，导航，头部
    /// Authorize 授权身份
    /// </summary>
    public enum AuthorizationType { None,Identity,Authorize}
    public class CustomAuthorizationArrtibute:ActionFilterAttribute,IAuthorizationFilter
    {
        /// <summary>
        /// 授权过滤器认证类型属性，默认是授权认证
        /// </summary>
        public AuthorizationType Type = AuthorizationType.Authorize;
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //无限制
            if (Type == AuthorizationType.None) return;
            //2.身份认证
            if (filterContext.HttpContext.Session["user"]==null)
            {
                RedirectToLogin(filterContext);
                return;
            }
            if (Type == AuthorizationType.Identity) return;
            //3.授权认证，对比控制器
            //3.1获取用户请求的控制器名称
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //3.2获取当前角色的所有模块，然后进行控制器名称对比
            var role = filterContext.HttpContext.Session["role"] as Role;

            //bool flag = false;
            //foreach (var item in role.Modules)
            //{
            //    if (item.Controller==controller)
            //    {
            //        flag = true;
            //        break;
            //    }
            //}
            //if (flag=false)
            //{
            //    RedirectToLogin(filterContext);
            //}
            var module = role.Modules.FirstOrDefault(m => m.Controller == controller);
            //3.3如果没有找到控制器，就返回
            if (module==null)
            {
                RedirectToLogin(filterContext);
            }
            //if (role.Modules.All(m => m.Controller != controller))
            //{
            //    RedirectToLogin(filterContext);
            //}
        }
        /// <summary>
        /// 重定向到登陆页
        /// </summary>
        /// <param name="filterContext"></param>
        public void RedirectToLogin(AuthorizationContext filterContext)
        {
            //实例化一个UrlHelper对象
            var url = new UrlHelper(filterContext.RequestContext);
            //设置返回结果为重定向到登陆页
            filterContext.Result = new RedirectResult(url.Action("Index", "Login"));
        }
    }
}