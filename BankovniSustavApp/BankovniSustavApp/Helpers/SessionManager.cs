using BankovniSustavApp.Services;
using System;
using System.Threading.Tasks;

namespace BankovniSustavApp.Helpers
{
    public static class SessionManager
    {
        private static IUserAccountService _userAccountService;

        public static int CurrentKorisnikId { get; private set; }
        public static string CurrentUserEmail { get; private set; }

        public static void Initialize(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public static async Task SetCurrentKorisnikIdByEmailAsync(string email)
        {
            if (_userAccountService == null)
            {
                throw new InvalidOperationException("SessionManager must be initialized with an instance of IUserAccountService.");
            }

            CurrentKorisnikId = await _userAccountService.GetKorisnikIdByEmailAsync(email);
            CurrentUserEmail = email;
        }

        public static void ClearSession()
        {
            CurrentKorisnikId = 0;
            CurrentUserEmail = null;
        }
    }
}
