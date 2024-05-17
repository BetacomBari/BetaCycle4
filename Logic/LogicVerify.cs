using System.Globalization;
using System.Text.RegularExpressions;

namespace BetaCycle4.Logic
{
    public static class LogicVerify
    {
        // VERIFY EMAIL
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        //

        // Funzione per verificare se una stringa ha più di tot caratteri
        public static bool VerifyLength(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (input.Length > maxLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Funzione per verificare se una password è valida
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Verifica che la password abbia almeno 8 caratteri
            if (password.Length < 8)
            {
                return false;
            }

            // Verifica che la password contenga almeno un numero
            if (!Regex.IsMatch(password, @"\d"))
            {
                return false;
            }

            // Verifica che la password contenga almeno una lettera maiuscola
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return false;
            }

            // Verifica che la password contenga almeno un carattere speciale
            if (!Regex.IsMatch(password, @"[!@#$%&?{}|<>]"))
            {
                return false;
            }

            return true;
        }

    }
}
