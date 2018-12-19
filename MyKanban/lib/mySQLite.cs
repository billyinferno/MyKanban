using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace MyKanban.lib
{
    /// <summary>
    /// mySQLite Library
    /// This library will perform all the basic function that needed for the SQLite database.
    /// Such as performing a connection, executing query, and read record.
    /// 
    /// This is to separate the task that need to be performed everytime by the model, so it can
    /// be done using single library without needed to type the same code for each model and
    /// data access.
    /// </summary>
    class mySQLite
    {
        private static SQLiteConnection conn = null;
        private static SQLiteCommand cmd = null;
        public static bool isConnected;

        /// <summary>
        /// Connect to the SQLite database, based on the "Default" connection string.
        /// </summary>
        public static void Connect()
        {
            // try to connect to the database if the isConnected is false
            if (!isConnected)
            {
                conn = new SQLiteConnection(LoadConnectionString());
                try
                {
                    conn.Open();
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    isConnected = false;
                    throw new Exception("Error:\n" + ex.Message + ".\nWhen trying to connect to database.");
                }
            }
        }

        /// <summary>
        /// Close current database connection.
        /// </summary>
        public static void Close()
        {
            if (conn != null && isConnected)
            {
                conn.Close();
                conn = null;
            }
        }

        /// <summary>
        /// Execute an SQL query and put it into SQLite Command.
        /// </summary>
        /// <param name="SQL"></param>
        public static void Command(string SQL)
        {
            if (isConnected)
            {
                if (cmd != null)
                {
#if DEBUG
                    Console.WriteLine("mySQLite: Previous command exit, dispose previous command.");
#endif
                    cmd.Dispose();
                    cmd = null;
                }

                cmd = new SQLiteCommand(SQL, conn);
            }
            else
            {
                throw new Exception("No connection available to execute the command.");
            }
        }

        /// <summary>
        /// Execute SQLite Command to read all the records.
        /// </summary>
        /// <returns></returns>
        public static SQLiteDataReader CommandExecuteReader()
        {
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Get the Configuration "Default" connection string.
        /// </summary>
        /// <param name="connID"></param>
        /// <returns></returns>
        private static string LoadConnectionString(string connID = "Default")
        {
            return ConfigurationManager.ConnectionStrings[connID].ConnectionString;
        }
    }
}
