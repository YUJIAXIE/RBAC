using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RBAC.ViewModels
{
    public class UserRoleViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
        public int UpdateRoleId { get; set; }

        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}