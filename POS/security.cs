using System;
using System.Text;
using System.Security.Cryptography;

namespace POS.Security
{
    public class Hasher : IHasher
    {
        public string fullName { get; set; }
        public string password { get; set; }

        // default constructor
        public Hasher()
        { }

        // constructor with parameters
        public Hasher(string fullName, string password)
        {
            this.fullName = fullName;
            this.password = password;
        }

        // perform the hashing
        public string computeHash()
        {
            // create a SHA256
            using (SHA256 hash = SHA256.Create())
            {
                
                // include fullName as salt
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(fullName + password));

                // convert byte array to string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString());
                }

                // return the salted hash
                return builder.ToString(); 
            }
        }
    }

    public interface IHasher
    {
        string fullName { get; set; }
        string password { get; set; }

        string computeHash();
    }
}