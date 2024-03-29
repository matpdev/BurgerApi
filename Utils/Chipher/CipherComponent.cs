using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly IConfigurationRoot config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    public static string Encrypt(string clearText)
    {
        string EncryptionKey = config["JwtSettings:Key"]!;
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb =
                new(
                    EncryptionKey,
                    new byte[]
                    {
                        0x49,
                        0x76,
                        0x61,
                        0x6e,
                        0x20,
                        0x4d,
                        0x65,
                        0x64,
                        0x76,
                        0x65,
                        0x64,
                        0x65,
                        0x76
                    }
                );
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new())
            {
                using (
                    CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                )
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public static string Decrypt(string cipherText)
    {
        string EncryptionKey = config["JwtSettings:Key"]!;
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb =
                new(
                    EncryptionKey,
                    new byte[]
                    {
                        0x49,
                        0x76,
                        0x61,
                        0x6e,
                        0x20,
                        0x4d,
                        0x65,
                        0x64,
                        0x76,
                        0x65,
                        0x64,
                        0x65,
                        0x76
                    }
                );
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new())
            {
                using (
                    CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                )
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    public static bool ValidateCipher(string password, string comparePassword)
    {
        string newPass = Decrypt(comparePassword);
        if (password == newPass)
            return true;
        return false;
    }
}
