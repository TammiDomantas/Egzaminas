using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.Identity.Client;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Diagnostics;
namespace Egzaminas
{

    public class Atkodavimas
    {
        public int threadcount; // giju skaicius
        public string hashed;
        public string salt;
        private static readonly char[] Simboliai = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public bool rastas = false; // ar slaptazodis rastas?
        private static readonly object lockObj = new object(); // uzrakinimas
        private static Stopwatch stopwatch = new Stopwatch(); // laiko matavimui
        public Atkodavimas() // constructor, paema uzkuoduota reiksme
        {
            string file = @"C:\Users\Intel\source\repos\Egzaminas\hash.txt";
            this.hashed = File.ReadAllText(file);
            string sfile = @"C:\Users\Intel\source\repos\Egzaminas\salt.txt";
            this.salt = File.ReadAllText(sfile);
        }
        public void Main() // sukuria gijas pagal giju skaiciu
        {
            Thread[] threads = new Thread[threadcount];
            stopwatch.Start();
            for (int i = 0; i < threadcount; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(BruteForce));
                threads[i].Start(i);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            stopwatch.Stop();
        }
        public void BruteForce(object obj)
        {
            int threadIndex = (int)obj;
            for(int ilgis = 1; ilgis <= 4; ilgis++)
            {
                int KombinacijuSkaicius = (int)Math.Pow(Simboliai.Length, ilgis);
                for(int i = threadIndex; i < KombinacijuSkaicius; i += threadcount)
                {
                    string bandymas = Generavimas(i, ilgis);
                    if(Tikrinimas(bandymas))
                    {
                        lock (lockObj)
                        {
                            if (!rastas)
                            {
                            rastas = true;
                            stopwatch.Stop();
                            MessageBox.Show("Rastas slaptazodis: "+ bandymas);
                            MessageBox.Show("Uztrukes laikas: "+ stopwatch.Elapsed);
                            Environment.Exit(0);
                            }
                        }
                    }
                }
            }
            
        }
        public static string Generavimas(int index, int ilgis)
        {
            char[] password = new char[ilgis];
            for(int i = 0; i < ilgis; i++)
            {
                password[i] = Simboliai[index % Simboliai.Length];
                index /= Simboliai.Length;
            }
            return new string(password);
        }
        public bool Tikrinimas(string password)
        {
            SHA256 hash = SHA256.Create();
            var passwordBytes = Encoding.Default.GetBytes(password + this.salt);
            var hashedPasswordBytes = hash.ComputeHash(passwordBytes);
            string hashedPassword = Convert.ToHexString(hashedPasswordBytes);
            return hashedPassword.Equals(this.hashed, StringComparison.OrdinalIgnoreCase);
        }
    }
}
