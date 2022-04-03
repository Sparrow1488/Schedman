namespace Schedman.Entities
{
    public class AccessPermission
    {
        public AccessPermission(string login, string password)
        {
            _login = login;
            _password = password;
        }

        private readonly string _login;
        private readonly string _password;

        internal string Login => _login;
        internal string Password => _password;
    }
}
