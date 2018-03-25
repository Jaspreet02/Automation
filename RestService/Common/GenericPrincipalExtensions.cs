using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MobileService.Common
{
    public static class GenericPrincipalExtensions
    {
        public static List<string> Users(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
               return claimsIdentity.FindFirst("UserList").Value.Split(';').ToList();
            }
            else
                return null;
        }

        public static List<int> Clients(this IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                var claim = claimsIdentity.FindFirst("ClientList");                
                    if (!string.IsNullOrEmpty(claim.Value))
                        return claim.Value.Split(';').Select(Int32.Parse).ToList();
                    else
                     return new List<int>();
            }
            else
                return new List<int>();
        }

        public static List<int> Applications(this IPrincipal user,int? clientId)
        {
            List<int> result = new List<int>();
            if (user.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == "ApplicationList" && !string.IsNullOrEmpty(claim.Value))
                    {

                        var dataDictionary = claim.Value.Split(';').Select(p => p.Trim().Split(':')).ToDictionary(p => Convert.ToInt32(p[0]), p => p[1].Split(',').Select(Int32.Parse).ToList());
                        
                        if (clientId.HasValue)
                        {
                            foreach (var item in dataDictionary)
                            {
                                if (clientId.HasValue && item.Key == clientId.Value)
                                {
                                    result.AddRange(item.Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in dataDictionary)
                            {
                                result.AddRange(item.Value);
                            }
                        }
                    }
                }
                return result;
            }
            else
                return null;
        }

        public static string Encrypt(string password)
        {          
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
        }        
    }
}