using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingService.Models;

namespace VendingService
{
    public class RoleManager
    {
        public enum eRole
        {
            Unknown = 0,
            Administrator = 1,
            Customer = 2,
            Executive = 3,
            Serviceman = 4
        }

        public UserItem User { get; }
        public eRole RoleName { get; }

        public RoleManager(UserItem user)
        {
            User = user;

            if (user != null)
            {
                RoleName = (eRole)user.RoleId;
            }
            else
            {
                RoleName = eRole.Unknown;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                return RoleName == eRole.Administrator;
            }
        }

        public bool IsCustomer
        {
            get
            {
                return RoleName == eRole.Customer;
            }
        }

        public bool IsExecutive
        {
            get
            {
                return RoleName == eRole.Executive;
            }
        }

        public bool IsServiceman
        {
            get
            {
                return RoleName == eRole.Serviceman;
            }
        }

        public bool IsUnknown
        {
            get
            {
                return RoleName == eRole.Unknown;
            }
        }
    }
}
