using System;
using System.Text;
using System.Security.Cryptography;

namespace Mixed.Models
{
	public class Passwordhasher
	{
        public static string hashPass(string password)
        {
            var sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }
    }
}

