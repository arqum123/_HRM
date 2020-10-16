using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HRM.WebAPI
{
    public class WebRoleProvider : RoleProvider
    {
        private string[] result;

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IUserTypeService objUserTypeService = IoC.Resolve<IUserTypeService>("UserTypeService");
            User User = objUserService.GetAllUser().Where(x => x.FirstName == username).FirstOrDefault();
            UserType userType = objUserTypeService.GetAllUserType().Where(x => x.Id == User.UserTypeId).FirstOrDefault();
            string UserTypeName = Convert.ToString(userType.Name);
            result = new string[1] {UserTypeName };
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}