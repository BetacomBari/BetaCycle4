using Microsoft.Data.SqlClient;

namespace BetaCycle4.Logger
{
    public class DbTracer
    {
        #region COMANDI DA METTERE AD OGNI CATCH
        /* N.B. Questi comandi NON funzionano se non INIETTATI come da esempio:
         *  private readonly DbTracer _dbTracer;
         *        public LoginController(DbTracer dbTracer)
                     {
                          _dbTracer = dbTracer;
                      }

       FINE ESEMPIO

        COMANDO:
         _dbtracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
*/
        #endregion

        #region Istanze della classe

        SqlConnection sqlCnn = new();
        SqlConnection credentialsCnn = new();
        SqlCommand sqlCmd = new();
        private bool ConnectionCheck = false;

        #endregion

        #region Costruttore
        public DbTracer()
        {
            try
            {
                sqlCnn.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                credentialsCnn.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            }
            catch (Exception e) //Qualsiasi errore che avviene durante la comunicazione col db, lo scrivo su file
            {
                PrintError(e.Source, e.Message, e.StackTrace);
            }
        }
        #endregion

        #region InsertError che scrive in LOGERROR

        /// <summary>
        /// Metodo unico richiamato dalla classe statica per la scrittura degli errori nella tabella apposita.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="location"></param>
        public void InsertError(string message, int code, string location)
        {
            try
            {
                checkDbLtOpen();

                sqlCmd.CommandText = "INSERT INTO LogError (ErrorMessage, ErrorCode, ErrorDate, ErrorLocation) VALUES (@message, @code, @date, @location)";
                sqlCmd.Parameters.AddWithValue("@message", message);
                sqlCmd.Parameters.AddWithValue("@code", code);
                sqlCmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
                sqlCmd.Parameters.AddWithValue("@location", location);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                PrintError(e.Source, e.Message, e.StackTrace);
            }

            checkDbLtClose();
        }

        #endregion

        #region PrintError che scrive su file gli errori che avvengono principalmente con la connessione al db
        /// <summary>
        /// Metodo per la stampa su file TXT degli errori, principalmente relativo agli errori legati
        /// alla connessione al DB
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="location"></param>
        public void PrintError(string source, string message, string location)
        {
            string txtpath = ".\\errors.txt";
            string errorprinted = "Data: " + DateTime.Now.ToString() + " Provenienza: " + source + " Messaggio: " + message;
            using (StreamWriter s = File.AppendText(txtpath))
            {
                s.WriteLine(errorprinted);
                s.WriteLine("Situato in: " + location);
                s.WriteLine(new string('_', 100));
            }
        }
        #endregion

        #region LogTrace che registra l'ultimo login in LogTrace (CustomerCredentials)

        /// <summary>
        /// Funziona Unica che traccia e inserisce l'ultimo Login effettuato nella tabella LogTrace, del db Customer Credentials
        /// </summary>
        /// <param name="dataChecker"></param>
        public void LogTrace(KeyValuePair<string, string> dataChecker)
        {
            int credentialsId = 0;
            try
            {
                checkDbCredentialsOpen();
                sqlCmd.CommandText = "SELECT Id FROM Credentials WHERE PasswordHash LIKE @password";
                sqlCmd.Parameters.AddWithValue("@password", dataChecker.Key);
                credentialsId = sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "INSERT INTO LogTrace (LastLogin, CredentialsId) VALUES (@date, @id)";
                sqlCmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
                sqlCmd.Parameters.AddWithValue("@id", credentialsId);
            }
            catch (Exception e)
            {
                PrintError(e.Source, e.Message, e.StackTrace);
            }
            finally
            {
                checkDbCredentialsClose();
            }
        }


        #endregion

        #region Check DB Open e Close per entrambi i DB
        internal void checkDbLtOpen()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Closed)
            {
                sqlCnn.Open();
            }
        }

        internal void checkDbLtClose()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Open)
            {
                sqlCnn.Close();
            }
            sqlCmd.Parameters.Clear();
        }
        internal void checkDbCredentialsOpen()
        {
            if (credentialsCnn.State == System.Data.ConnectionState.Closed)
            {
                credentialsCnn.Open();
            }
        }

        internal void checkDbCredentialsClose()
        {
            if (credentialsCnn.State == System.Data.ConnectionState.Open)
            {
                credentialsCnn.Close();
            }
            sqlCmd.Parameters.Clear();
        }
        #endregion
    }
}
