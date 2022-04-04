using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace XLab.Common.Securitys
{
    public class Sha256Utils
    {
        public static string HashString(string input)
        {
            byte[] clearBytes =Encoding.UTF8.GetBytes(input);
            using (SHA256 sha256 = new SHA256Managed())
            {
                sha256.ComputeHash(clearBytes);
                byte[] hashedBytes = sha256.Hash;
                sha256.Clear();
                string output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return output;
            }
        }
    }
}
