using Models.Tienda_CS;
using System;
using System.Collections.Generic;
using System.Data;
using Tienda_CS;

namespace Controllers.Tienda_CS
{
    internal class TipoProductoControlador
    {
        //CLASE PARA EL CONTROLADOR DE TIPO PRODUCTO
        public bool CrearTipoProducto(TipoProducto tipoProducto)
        {
            try
            {
                string query = @"INSERT INTO tipo_producto (nombre) 
                               VALUES (@nombre)";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", tipoProducto.Nombre);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Tipo de producto creado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo crear el tipo de producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al crear el tipo de producto: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarTipoProducto(TipoProducto tipoProducto)
        {
            try
            {
                string updateQuery = @"UPDATE tipo_producto 
                                     SET nombre = @nombre 
                                     WHERE id_tipo = @id_tipo";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", tipoProducto.Nombre);
                        command.Parameters.AddWithValue("@id_tipo", tipoProducto.IdTipo);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Tipo de producto actualizado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo actualizar el tipo de producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al actualizar el tipo de producto: {ex.Message}");
                return false;
            }
        }
    }
}
