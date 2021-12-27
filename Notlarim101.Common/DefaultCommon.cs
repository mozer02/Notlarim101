using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Common
{
    public class DefaultCommon : ICommon
    {
        //UI katmani ilk calistiginda bu class i new leyecek.
        public string GetCurrentUsername()
        {
            return "system";
        }
    }
}
