
using AussieCake.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace AussieCake.Context
{
    /// <summary>
    /// Class to simplify connections to a SQLite database
    /// </summary>
    public class SqLiteCommands
    {
        private static readonly string ConnectionString;

        /// <summary>
        /// Checks if the database exists and creates the connnection to the database
        /// </summary>
        /// <param name="DatabasePath">Database Path</param>
        static SqLiteCommands()
        {
            // Checks if the database exists
            if (!File.Exists(CakePaths.Database))
            {
                Debug.Fail("quack");
                SQLiteConnection.CreateFile(CakePaths.Database);
            }

            ConnectionString = "Data Source=" + CakePaths.Database + "; Version=3";
        }

        /// <summary>
        /// Creates a database
        /// </summary>
        /// <param name="query">SQL statement</param>
        protected static void CreateDb(string query)
        {
            SendQuery(query);
        }

        /// <summary>
        /// Sends a SQL query that doesn't return any value
        /// It can also be used to create a table, but it's recomended to use CreateDb instead
        /// </summary>
        /// <param name="query">SQL statement</param>
        protected static bool SendQuery(string query)
        {
            query = query.Replace("'',", "NULL,");

            try
            {
                using (SQLiteConnection Connection = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand SqlCmd = new SQLiteCommand())
                    {
                        SqlCmd.Connection = Connection;
                        SqlCmd.CommandText = query;
                        Connection.Open();
                        SqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                return Errors.ThrowErrorMsg(ErrorType.SQLite, e.Message);
            }

            return true;
        }

        /// <summary>
        /// Gets the first result from the query
        /// </summary>
        /// <param name="query">SQL statement</param>
        /// <returns>Return the value inside a System.Object and must be converted. If no result, returns null</returns>
        protected static SQLiteDataReader GetFromQuery(string query)
        {
            SQLiteDataReader result = null;
            using (SQLiteConnection Connection = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand SqlCmd = new SQLiteCommand())
                {
                    SqlCmd.Connection = Connection;
                    SqlCmd.CommandText = query;
                    Connection.Open();
                    using (SQLiteDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.NextResult())
                        {
                            result = Reader;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets table values inside a dataset
        /// </summary>
        /// <param name="query">SQL statement</param>
        /// <param name="tableName">Name of the table</param>
        /// <returns>All the returned values into a dataset</returns>
        protected static DataSet GetTable(string db_name)
        {
            string query = "SELECT * FROM " + db_name;
            DataSet data = new DataSet();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    adapter.Fill(data);
                }
            }


            return data;
        }

        protected static DataSet GetTable(Model type)
        {
            return GetTable(type.ToDesc());
        }

        /// <summary>
        /// Get Last updated to insert in logic list
        /// </summary>
        protected static DataSet GetLast(string db_name)
        {
            string query = "SELECT * FROM " + db_name + " WHERE ID = (SELECT MAX(ID) FROM " + db_name + ")";
            DataSet data = new DataSet();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    adapter.Fill(data);
                }
            }


            return data;
        }

        protected static List<string> GetFromDB(string fromTable, string fromColumn, List<string> whichContains)
        {
            List<string> ImportedFiles = new List<string>();
            using (SQLiteConnection connect = new SQLiteConnection(ConnectionString))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"select " + fromColumn + " from " + fromTable + " where ";

                    for(int i = 0; i < whichContains.Count; i++)
                    {                        if (i != 0)
                            fmd.CommandText += " and ";

                        fmd.CommandText += "Text like '%" + whichContains[i] + "%' ";
                    }
                    
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        ImportedFiles.Add(Convert.ToString(r[fromColumn]));
                    }
                    //Console.WriteLine(fmd.CommandText);
                }
            }
            return ImportedFiles;
        }

    }
}