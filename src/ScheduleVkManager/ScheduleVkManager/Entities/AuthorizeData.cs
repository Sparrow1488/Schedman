using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace ScheduleVkManager.Entities
{
    public class AuthorizeData : IValidatableObject
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthorizeData(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public AuthorizeData(string fromFilePath)
        {
            var fileLines = File.ReadLines(fromFilePath).ToArray();
            Login = fileLines[0]?.Trim() ?? string.Empty;
            Password = fileLines[1]?.Trim() ?? string.Empty;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(Login))
                results.Add(new ValidationResult("Login cannot be null or empty"));
            if (string.IsNullOrWhiteSpace(Password))
                results.Add(new ValidationResult("Password cannot be null or empty"));
            return results;
        }
    }
}
