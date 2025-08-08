using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace TiendaCS
{
    public class DatabaseConnection
    {
        private static string connectionString = "Server=localhost;Database=tienda_cs;Uid=root;Pwd=;Port=3306;CharSet=utf8mb4;";
        private static MySqlConnection connection;

        /// <summary>
        /// Obtiene la conexión a la base de datos MySQL
        /// </summary>
        /// <returns>MySqlConnection configurada para Laragon</returns>
        public static MySqlConnection GetConnection()
        {
            try
            {
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    Console.WriteLine("Conexión a MySQL establecida correctamente.");
                }
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error al conectar con MySQL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Cierra la conexión a la base de datos
        /// </summary>
        public static void CloseConnection()
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    Console.WriteLine("Conexión cerrada correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL y retorna un DataTable
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>DataTable con los resultados</returns>
        public static DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
                throw;
            }

            return dataTable;
        }

        /// <summary>
        /// Ejecuta una consulta SQL que no retorna datos (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="query">Consulta SQL a ejecutar</param>
        /// <returns>Número de filas afectadas</returns>
        public static int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;
            
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
                throw;
            }

            return rowsAffected;
        }

        /// <summary>
        /// Verifica si la conexión está activa
        /// </summary>
        /// <returns>True si la conexión está abierta, False en caso contrario</returns>
        public static bool IsConnectionActive()
        {
            return connection != null && connection.State == ConnectionState.Open;
        }
    }
}
