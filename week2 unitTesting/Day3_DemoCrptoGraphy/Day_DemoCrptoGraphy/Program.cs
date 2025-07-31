//need to include following packages
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

class Program
{
    static void Main()
    {
        //A menu that shows options toget started 
        while (true)
        {
            Console.WriteLine("\nSecure Message App - Genreral features");
            Console.WriteLine("1. Encrypt Message");
            Console.WriteLine("2. Decrypt Message");
            Console.WriteLine("3. Exit ");
            Console.WriteLine(" Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine(" Enter a Message to encrypt: ");
                    string? message = Console.ReadLine();
                    Console.WriteLine(" Enter a 16 Digit key ");
                    string? KeyEncrypt = Console.ReadLine();
                    string encrypted = Encrypt(message, KeyEncrypt);
                    Console.WriteLine($"Encrypted text: {encrypted}");
                    break;

                case "2":
                    Console.WriteLine(" Enter the Encrypted message: ");//cipher key
                    string? encryptText = Console.ReadLine();
                    Console.WriteLine(" Enter 16 Digit Key");
                    string? keyDecrypt = Console.ReadLine();
                    string decrypted = Decrypt(encryptText, keyDecrypt);
                    Console.WriteLine($"Decrypted : {decrypted}");
                    break;

                case "3":
                    return;


                default:
                    Console.WriteLine(" Invalid Choice. Try again.");
                    break;
            }

        }
    }

    static string Encrypt(string PlainText, string key)
    {
        //Here we can use any algorithm
        //Step 1: Creating an AES Object 
        //Step 2: Setting up the encryption Key
        //Step 3: Genrate and Store IV(Iniialization Vecotor)
        //Step 4: Create an Encryptor 
        //Step 5: Creating a memory stream and Store IV
        //Step 6: Writte plain text through cryptoStream
        //Step 7: Converting Encrypted Bytes to Base64 string 

        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);//Encoding all char in a sequence of Bytes 
        aes.GenerateIV();//performing encrypion

        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor();//Creating encryptor
        using var ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);// storing IV in output
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {

            sw.Write(PlainText);
        }

        return Convert.ToBase64String(ms.ToArray());// after this conversion  string can be stored, printed and transmitted  
    }


    static string Decrypt(string cipherText, string key)
    {
        //using the encrypted text to make it readable again 
        //Step 1: Conver the encrypted Base 64 test to Bytes
        var fullBytes = Convert.FromBase64String(cipherText);
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        //Step 2: Create AES object and Set the key
        byte[] iv = new byte[16];
        Array.Copy(fullBytes, 0, iv, 0, 16);
        aes.IV = iv;
        //Step 3: Extract and Set initlization Vecor(IV)
        using var decryptor = aes.CreateDecryptor();

        //Step 4: Create a Decryptor object
        using var ms = new MemoryStream(fullBytes, 16, fullBytes.Length - 16);
        //Step 5: Create a Memory Stream for the Encrypted Data  
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        //Step 6: Wrap cryptoStream and read the Decryped Data
        return sr.ReadToEnd();
        //Step 7: Return the Decrypted plain text
    }

}


