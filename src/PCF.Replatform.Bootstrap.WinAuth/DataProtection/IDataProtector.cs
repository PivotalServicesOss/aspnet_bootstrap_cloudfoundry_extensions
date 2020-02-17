using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.DataProtection
{
    public interface IDataProtector
    {
        byte[] Protect(byte[] unsecuredData);
        byte[] UnProtect(byte[] securedData);
    }
}
