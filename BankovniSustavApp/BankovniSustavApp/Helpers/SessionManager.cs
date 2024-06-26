using BankovniSustavApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankovniSustavApp.Helpers
{
    public static class SessionManager
    {
        private static IUserAccountService _userAccountService;

        // This property holds the currently logged-in user's ID.
        public static int CurrentKorisnikId { get; private set; }

        // Inject the IUserAccountService dependency.
        public static void Initialize(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        // A method to set the current user's ID.
        public static async Task SetCurrentKorisnikIdByEmailAsync(string email)
        {
            if (_userAccountService == null)
            {
                throw new InvalidOperationException("SessionManager must be initialized with an instance of IUserAccountService.");
            }

            CurrentKorisnikId = await _userAccountService.GetKorisnikIdByEmailAsync(email);
        }

        // Optionally, a method to clear the session.
        public static void ClearSession()
        {
            CurrentKorisnikId = 0;
        }
    }

}
