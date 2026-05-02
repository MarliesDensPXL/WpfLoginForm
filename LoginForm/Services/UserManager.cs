using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LoginForm.Models;

namespace LoginForm.Services
{
    public class UserManager
    {
        Dictionary<string, string> users = new Dictionary<string, string>();

        //private string _username;
        //private string _passwordHash;
        
        public bool Register(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                return false;
            }
                        
            string passwordHash = HashPassword(password);
            users.Add(username, passwordHash);
            return true;
            
        }

        public bool TryLogin(Registration credentials)
        {
            if (!users.ContainsKey(credentials.Username))
            {
                return false;
            }

            string hash = HashPassword(credentials.Password);

            return users[credentials.Username] == hash;
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
