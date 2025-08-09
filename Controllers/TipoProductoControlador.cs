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

        public bool VerificarTipoProductoEnUso(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM productos WHERE tipo_id = @tipo_id";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tipo_id", id);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        
                        Console.WriteLine($" Productos encontrados con este tipo: {count}");
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al verificar uso del tipo de producto: {ex.Message}");
                return false;
            }
        }

        public bool ExisteTipoProducto(string nombre)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM tipo_producto WHERE nombre = @nombre";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        
                        Console.WriteLine($" Tipos de producto encontrados con ese nombre: {count}");
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al verificar existencia del tipo de producto: {ex.Message}");
                return false;
            }
        }

        public bool ExisteTipoProductoParaActualizar(string nombre, int idActual)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM tipo_producto WHERE nombre = @nombre AND id_tipo != @id_tipo";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@id_tipo", idActual);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        
                        Console.WriteLine($" Otros tipos con ese nombre: {count}");
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al verificar existencia para actualización: {ex.Message}");
                return false;
            }
        }

        public int ContarProductosPorTipo(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM productos WHERE tipo_id = @tipo_id";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tipo_id", id);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        
                        Console.WriteLine($" Productos contados para este tipo: {count}");
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al contar productos por tipo: {ex.Message}");
                return 0;
            }
        }

        public List<TipoProducto> BuscarTiposPorNombre(string nombre)
        {
            List<TipoProducto> listaTiposProducto = new List<TipoProducto>();

            try
            {
                string query = "SELECT id_tipo, nombre FROM tipo_producto WHERE nombre LIKE @nombre";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", $"%{nombre}%");

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

                Console.WriteLine($" Tipos de producto encontrados en búsqueda: {listaTiposProducto.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al buscar tipos de producto: {ex.Message}");
            }

            return listaTiposProducto;
        }

        public List<TipoProducto> ObtenerTiposConProductos()
        {
            List<TipoProducto> listaTiposProducto = new List<TipoProducto>();

            try
            {
                string query = @"SELECT DISTINCT tp.id_tipo, tp.nombre 
                               FROM tipo_producto tp 
                               INNER JOIN productos p ON tp.id_tipo = p.tipo_id";

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

                Console.WriteLine($" Tipos de producto con productos asociados: {listaTiposProducto.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al obtener tipos con productos: {ex.Message}");
            }

            return listaTiposProducto;
        }

        public List<TipoProducto> ObtenerTiposSinProductos()
        {
            List<TipoProducto> listaTiposProducto = new List<TipoProducto>();

            try
            {
                string query = @"SELECT tp.id_tipo, tp.nombre 
                               FROM tipo_producto tp 
                               LEFT JOIN productos p ON tp.id_tipo = p.tipo_id 
                               WHERE p.tipo_id IS NULL";

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

                Console.WriteLine($" Tipos de producto sin productos asociados: {listaTiposProducto.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al obtener tipos sin productos: {ex.Message}");
            }

            return listaTiposProducto;
        }
    }
}
