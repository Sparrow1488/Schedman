using Schedman.Entities;
using Schedman.Example.Next.Services;
using Schedman.Tools.IO;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Schedman.Exceptions;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Schedman.Example.Next
{
    internal class Program
    {
        public static IConfiguration Configuration { get; set; }
        
        private static async Task Main()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("schedman.settings.json").Build();
            
            using var rsa = ImportRsaWithConfigKeys();
            var encryptedFile = await File.ReadAllBytesAsync(Configuration["Access:FilePath"]);
            var decryptedFile = Encoding.UTF8.GetString(rsa.Decrypt(encryptedFile, RSAEncryptionPadding.Pkcs1));

            var accessFileLines = decryptedFile.Split("\n");

            var access = new AccessPermission(accessFileLines[0], accessFileLines[1]);
            var manager = new VkManager();

            await manager.AuthorizateAsync(access);

            IService executableService = new DownloadVideosService(manager);
            executableService.Configuration = Configuration;

            try
            {
                await executableService.StartAsync();
            }
            catch (SchedmanException ex)
            {
                Console.WriteLine("SchedmanException: " + ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void CreateEncryptedAccessFile(
            string login, string password, string savePathDirectory)
        {
            using var rsa = ImportRsaWithConfigKeys();
            var encryptedFile = rsa.Encrypt(Encoding.UTF8.GetBytes($"{login}\n{password}"), RSAEncryptionPadding.Pkcs1);

            File.WriteAllBytes(Path.Combine(savePathDirectory, "vkAccess"), encryptedFile);
        }

        private static RSA ImportRsaWithConfigKeys()
        {
            var rsa = RSA.Create();
            var securityKeysSection = Configuration.GetRequiredSection("Security:Keys");
            rsa.ImportRSAPublicKey(Convert.FromBase64String(securityKeysSection["RsaPublicKey"]), out var publicSize);
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(securityKeysSection["RsaPrivateKey"]), out var privateSize);
            return rsa;
        }
    }
}
