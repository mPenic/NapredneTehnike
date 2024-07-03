using System;

namespace BankingLibrary
{
    public class Utility
    {
        public string FormatCurrency(decimal amount)
        {
            return string.Format("{0:0.00} EUR", amount);
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}