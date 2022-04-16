using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Schedman.Entities
{
    public class AccessPermission
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public AccessPermission(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public AccessPermission(string fromFilePath)
        {
            var fileLines = File.ReadLines(fromFilePath).ToArray();
            Login = fileLines[0]?.Trim() ?? string.Empty;
            Password = fileLines[1]?.Trim() ?? string.Empty;
        }
    }
}
