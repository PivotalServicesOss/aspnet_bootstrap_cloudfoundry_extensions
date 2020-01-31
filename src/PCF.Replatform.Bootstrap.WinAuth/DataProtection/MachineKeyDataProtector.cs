using Microsoft.Extensions.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using System;
using System.Web.Security;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.DataProtection
{
    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly IConfiguration configuration;

        public MachineKeyDataProtector(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public byte[] Protect(byte[] unsecuredData)
        {
            return MachineKey.Protect(unsecuredData, configuration[AuthConstants.DATA_PROTECTION_KEY_NM]);
        }

        public byte[] UnProtect(byte[] securedData)
        {
            return MachineKey.Protect(securedData, configuration[AuthConstants.DATA_PROTECTION_KEY_NM]);
        }
    }
}
