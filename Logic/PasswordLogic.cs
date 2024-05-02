using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BetaCycle4.Logic
{
    public static class PasswordLogic
    {
        //DA STRING PASSWORD A PASSWORD HASH CON SALT
        public static KeyValuePair<string, string> GetPasswordHashAndSalt(string password)
        {

            byte[] byteSalt = new byte[6];
            string encryptedResult = string.Empty;
            string encryptedSalt = string.Empty;

            RandomNumberGenerator.Fill(byteSalt);

            encryptedResult = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: byteSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 32
            ));

            encryptedSalt = Convert.ToBase64String(byteSalt);

            return new KeyValuePair<string, string>(encryptedResult, encryptedSalt);
        }


        //VERIFICA SE LA INPUT PASSWORD è UGUALE A PASSWORD HASH CON IL proprio SALT
        public static bool Encrypted(string encryptedResult, string encryptedSalt, string inputPassword)
        {
            byte[] decryptSalt = Convert.FromBase64String(encryptedSalt);

            string decryptedResult = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: inputPassword,
                    salt: decryptSalt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 1000,
                    numBytesRequested: 32));

            return decryptedResult.Equals(encryptedResult);
        }
    }
}

