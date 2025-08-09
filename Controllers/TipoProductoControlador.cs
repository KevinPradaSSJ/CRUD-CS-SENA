using CRUD_SC_SENA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CRUD_SC_SENA.Controllers
{
    public class TipoProductoControlador
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
                    using (var command = new MySqlCommand(query, connection))
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

        public bool EliminarTipoProducto(int id)
        {
            try
            {
                string query = "DELETE FROM tipo_producto WHERE id_tipo = @id_tipo";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_tipo", id);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Tipo de producto eliminado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo eliminar el tipo de producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al eliminar el tipo de producto: {ex.Message}");
                return false;
            }
        }

        public List<TipoProducto> LeerTiposProducto()
        {
            List<TipoProducto> listaTiposProducto = new List<TipoProducto>();

            try
            {
                string query = "SELECT id_tipo, nombre FROM tipo_producto";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TipoProducto tipoProducto = new TipoProducto
                                {
                                    IdTipo = reader.GetInt32("id_tipo"),
                                    Nombre = reader.GetString("nombre")
                                };

                                listaTiposProducto.Add(tipoProducto);
                            }
                        }
                    }
                }

                Console.WriteLine(" Tipos de producto cargados correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al leer los tipos de producto: {ex.Message}");
            }

            return listaTiposProducto;
        }

        public TipoProducto ObtenerTipoProductoPorId(int id)
        {
            TipoProducto tipoProducto = null;

            try
            {
                string query = "SELECT id_tipo, nombre FROM tipo_producto WHERE id_tipo = @id_tipo";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_tipo", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tipoProducto = new TipoProducto
                                {
                                    IdTipo = reader.GetInt32("id_tipo"),
                                    Nombre = reader.GetString("nombre")
                                };
                            }
                        }
                    }
                }

                Console.WriteLine(tipoProducto != null ? " Tipo de producto encontrado correctamente" : " Tipo de producto no encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al obtener el tipo de producto: {ex.Message}");
            }

            return tipoProducto;
        }
    }
}
