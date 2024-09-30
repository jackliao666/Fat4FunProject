using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace FAT4FUN.BackEnd.Site.Models
{
    public class CustomPrincipal : IPrincipal
    {
        //private string[] _roles;
        private string[] _functions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="roles"></param>能操作的功能 例如:"Book,User,Order"
        public CustomPrincipal(IIdentity identity, string[] functions)
        {
            //將本使用者可以操作的功能分割成陣列,並且轉小寫,去除左右空白
            //_roles = roles.Split(',')
            //    .Select(x => x.Trim().ToLower())
            //    .ToArray();

            //this.Identity = identity;

            _functions = functions;

            this.Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            // 檢查是否角色在這個使用者的可用角色列表中
            //return _roles.Contains(role.ToLower());
            /*System.Diagnostics.Debug.WriteLine($"Checking role: {role} against user roles: {string.Join(",", _functions)}")*/;

            System.Diagnostics.Debug.WriteLine($"Checking if user has role: {role}. User roles are: {string.Join(", ", _functions)}");

            return _functions.Contains(role.ToLower());
        }


        /// <summary>
        /// Debug用
        /// </summary>
        /// <returns></returns>
        //public string[] GetRoles()
        //{
        //    System.Diagnostics.Debug.WriteLine($"User Roles: {string.Join(",", _functions)}");
        //    return _functions;
        //}
    }
}