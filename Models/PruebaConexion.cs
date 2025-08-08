using System;

namespace TiendaCS
{
    class PruebaConexion
    {
        static void Main(string[] args)
        {
            try
            {
                // Intentar obtener la conexión
                var connection = DatabaseConnection.GetConnection();
                
                Console.WriteLine("CONEXIÓN EXITOSA");
                
                // Cerrar conexión
                DatabaseConnection.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CONEXIÓN FALLIDA");
            }
        }
    }
}
