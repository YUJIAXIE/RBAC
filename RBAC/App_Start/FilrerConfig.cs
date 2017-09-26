using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RBAC.Filters;
namespace RBAC
{
    public class FilrerConfig
    {
        //创建全局过滤器
        public static void RegisterGlobaFilter(GlobalFilterCollection filters)
        {
            filters.Add(new CustomAuthorizationArrtibute());
        }

    }
}