using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Egzaminas
{
    internal class Uzkodavimas
    {
        public string Slapt;
        public string Hash;
        public string HashPassword(string Slapt)
        {
            string sfile = @"C:\Users\Intel\source\repos\Egzaminas\salt.txt";
            string salt = File.ReadAllText(sfile);
            SHA256 hash = SHA256.Create();
            var SlaptBytais = Encoding.Default.GetBytes(this.Slapt + salt);
            var hashedSlapt = hash.ComputeHash(SlaptBytais);
            return Convert.ToHexString(hashedSlapt);
        }
        public void Isvedimas()
        {
            string text = this.Hash;
            string file = @"C:\Users\Intel\source\repos\Egzaminas\hash.txt";
            File.WriteAllText(file, text);
        }
    }
}
