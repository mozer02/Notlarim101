using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notlarim101.DataAccessLayer;

namespace Notlarim101.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static NotlarimContext db; //= new NotlarimContext();
        private static object _lockSync = new object();

        public RepositoryBase()
        {
            CreateContext();
        }
        public static void CreateContext()
        {
            if (db == null)
            {
                lock (_lockSync)
                {
                    if (db == null)
                    {
                        db = new NotlarimContext();
                    }
                }
            }
        }
    }
}
