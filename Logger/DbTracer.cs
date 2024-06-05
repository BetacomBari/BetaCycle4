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
         _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
*/
        #endregion

        #region Istanze della classe

        SqlConnection sqlCnn = new();
        SqlCommand sqlCmd = new();
        private bool ConnectionCheck = false;

        #endregion

        #region Costruttore
        public DbTracer()
        {
            try
            {
                sqlCnn.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            }
            catch (Exception ex) //Qualsiasi errore che avviene durante la comunicazione col db, lo scrivo su file
            {
                PrintError(ex.Source, ex.Message, ex.StackTrace);
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
                checkDbOpen();

                sqlCmd.CommandText = "INSERT INTO LogError (ErrorMessage, ErrorCode, ErrorDate, ErrorLocation) VALUES (@message, @code, @date, @location)";
                sqlCmd.Parameters.AddWithValue("@message", message);
                sqlCmd.Parameters.AddWithValue("@code", code);
                sqlCmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
                sqlCmd.Parameters.AddWithValue("@location", location);

                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                PrintError(ex.Source, ex.Message, ex.StackTrace);
            }

            checkDbClose();
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

        #region Check DB Open e Close
        internal void checkDbOpen()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Closed)
            {
                sqlCnn.Open();
            }
        }

        internal void checkDbClose()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Open)
            {
                sqlCnn.Close();
            }
            sqlCmd.Parameters.Clear();
        }
        #endregion
    }
}
