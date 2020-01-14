//using System;
//using System.IO;
//using Microsoft.AspNetCore.Authentication;

//namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Options
//{
//    public class SpnegoAuthenticationOptions
//    {
//        public string PrincipalPassword { get; set; }

//        public override void Validate()
//        {

//            if (PrincipalPassword == null && KeytabFile == null)
//            {
//                throw new InvalidOperationException("Must set either principal password or keytab");
//            }

//            if (PrincipalPassword == null && !File.Exists(KeytabFile))
//            {
//                throw new InvalidOperationException($"Keytab not found at {KeytabFile}");
//            }
//        }
//    }
//}