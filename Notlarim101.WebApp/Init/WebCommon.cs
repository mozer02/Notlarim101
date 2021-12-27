using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Notlarim101.Common;
using Notlarim101.Entity;

namespace Notlarim101.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"]!=null)
            {
                NotlarimUser user=HttpContext.Current.Session["login"] as NotlarimUser;
                return user.Username;
            }

            return "system";
        }
    }
}