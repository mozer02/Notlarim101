using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Notlarim101.Common;
using Notlarim101.Entity;
using Notlarim101.WebApp.Models;

namespace Notlarim101.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (CurrentSession.User !=null)
            {
                NotlarimUser user= CurrentSession.User as NotlarimUser;
                return user.Username;
            }

            return "system";
        }
    }
}