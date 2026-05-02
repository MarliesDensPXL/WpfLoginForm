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
        private string _username;
        private string _passwordHash;
        
        public bool Register(string username, string password)
        {
            _username = username;
            _passwordHash = HashPassword(password);
            return true;
            
        }

        public bool TryLogin(Registration credentials)
        {
            if (!credentials.Username.Equals(_username, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string hash = HashPassword(credentials.Password);

            if (!hash.Equals(_passwordHash))
            {
                return false;
            }

            return true;
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
