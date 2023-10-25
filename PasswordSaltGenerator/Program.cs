using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordSaltGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int passwordLength = 255;

            Console.Write("Enter your password to hash and secure: ");
            string password = Console.ReadLine();


            Console.Clear();
            Console.WriteLine($"Your password is: {password}");
            Console.WriteLine();
            string securePasswordSalt = GenerateSecureSalt(passwordLength);
            Console.WriteLine($"Generated secure salt: {securePasswordSalt}"); 
            Console.WriteLine();

            password = password + securePasswordSalt;
            string hashedPassword = HashPasswordSHA256(password);

            //Console.WriteLine($"Original Password with salt: {password}"); password with salt
            //Console.WriteLine();
            Console.WriteLine($"Hashed Password (with salt): {hashedPassword}");
            Console.ReadLine();
        }

        static string GenerateSecureSalt(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rngCsp.GetBytes(randomBytes);

                char[] randomChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    randomChars[i] = validChars[randomBytes[i] % validChars.Length];
                }

                return new string(randomChars);
            }
        }
        static string HashPasswordSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string. This conversion is just so i can display it later. Usually leave in byte[]
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2")); // "x2" formats each byte as a two-digit hexadecimal number
                }
                return builder.ToString();
            }
        }
    }
}