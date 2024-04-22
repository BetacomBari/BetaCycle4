using BetaCycle4.Models;
using Microsoft.Data.SqlClient;
using SqlManager.BLogic;

namespace BetaCycle4.Logic
{
    public static class ErrorManager
    {
        /// <summary>
        /// Metodo unico per la registrazione degli errori all'interno della tabella apposita, riprendendone
        /// il messaggio di errore, il codice e dove sia avvenuto l'errore
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="location"></param>
        public static void RegisterError(string message, short code, string location)
        {
            //Apro la connessione al DB che contiene la tabella degli errori -S
            DbUtility errormanager = new DbUtility("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            errormanager.ErrorWriter(message, code, location);

        }


    }
}
