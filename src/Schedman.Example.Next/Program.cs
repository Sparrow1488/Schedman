using Schedman.Entities;
using Schedman.Example.Next.Services;
using Schedman.Tools.IO;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Services;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Schedman.Example.Next
{
    internal class Program
    {
        private static async Task Main()
        {
            using var rsa = ImportRSAWithConfigKeys();
            var encryptedFile = await File.ReadAllBytesAsync(ConfigurationManager.AppSettings["accessFile"]);
            var decryptedFile = Encoding.UTF8.GetString(rsa.Decrypt(encryptedFile, RSAEncryptionPadding.Pkcs1));

            var accessFileLines = decryptedFile.Split("\n");

            var access = new AccessPermission(accessFileLines[0], accessFileLines[1]);
            var manager = new VkManager();

            await manager.AuthorizateAsync(access);

            IService executableService = new DownloadVideosService(manager);
            await executableService.StartAsync();
        }

        private static void CreateEncryptedAccessFile(
            string login, string password, string savePathDirectory)
        {
            using var rsa = ImportRSAWithConfigKeys();
            var encryptedFile = rsa.Encrypt(Encoding.UTF8.GetBytes($"{login}\n{password}"), RSAEncryptionPadding.Pkcs1);

            File.WriteAllBytes(Path.Combine(savePathDirectory, "vkAccess"), encryptedFile);
        }

        private static RSA ImportRSAWithConfigKeys()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(ConfigurationManager.AppSettings["rsaPublicKey"]), out int publicSize);
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(ConfigurationManager.AppSettings["rsaPrivateKey"]), out int privateSize);
            return rsa;
        }
    }
}
