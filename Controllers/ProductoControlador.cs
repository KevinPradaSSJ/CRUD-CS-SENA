
using CRUD_SC_SENA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CRUD_SC_SENA.Controllers
{
    public class ProductoControlador
    {

        
        public bool CrearProducto(Producto producto)
        {
            try
            {
                string query = @"INSERT INTO productos (nombre, precio, tipo_id, stock) 
                               VALUES (@nombre, @precio, @tipo_id, @stock)";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@precio", producto.Precio);
                        command.Parameters.AddWithValue("@tipo_id", producto.TipoId);
                        command.Parameters.AddWithValue("@stock", producto.Stock);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Producto creado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo crear el producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al crear el producto: {ex.Message}");
                return false;
            }
        }

        public bool ActualizarProducto(Producto producto)
        {
            try
            {
                string updateQuery = @"UPDATE productos 
                                     SET nombre = @nombre, precio = @precio, 
                                         tipo_id = @tipo_id, stock = @stock 
                                     WHERE id_producto = @id_producto";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", producto.Nombre);
                        command.Parameters.AddWithValue("@precio", producto.Precio);
                        command.Parameters.AddWithValue("@tipo_id", producto.TipoId);
                        command.Parameters.AddWithValue("@stock", producto.Stock);
                        command.Parameters.AddWithValue("@id_producto", producto.IdProducto);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Producto actualizado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo actualizar el producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al actualizar el producto: {ex.Message}");
                return false;
            }
        }

        public bool EliminarProducto(int id)
        {
            try
            {
                string query = "DELETE FROM productos WHERE id_producto = @id";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int filasAfectadas = command.ExecuteNonQuery();
                        
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine(" Producto eliminado exitosamente");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine(" No se pudo eliminar el producto");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al eliminar el producto: {ex.Message}");
                return false;
            }
        }

         public List<Producto> LeerProductos()
        {
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                string query = "SELECT id_producto, nombre, precio, tipo_id, stock FROM productos";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Producto producto = new Producto
                                {
                                    IdProducto = reader.GetInt32("id_producto"),
                                    Nombre = reader.GetString("nombre"),
                                    Precio = reader.GetDecimal("precio"),
                                    TipoId = reader.GetInt32("tipo_id"),
                                    Stock = reader.GetInt32("stock")
                                };

                                listaProductos.Add(producto);
                            }
                        }
                    }
                }

                Console.WriteLine(" Productos cargados correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al leer los productos: {ex.Message}");
            }

            return listaProductos;
        }


        public Producto ObtenerProductoPorId(int id)
        {
            Producto producto = null;

            try
            {
                string query = "SELECT id_producto, nombre, precio, tipo_id, stock FROM productos WHERE id_producto = @id";

                using (var connection = DatabaseConnection.GetConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto
                                {
                                    IdProducto = reader.GetInt32("id_producto"),
                                    Nombre = reader.GetString("nombre"),
                                    Precio = reader.GetDecimal("precio"),
                                    TipoId = reader.GetInt32("tipo_id"),
                                    Stock = reader.GetInt32("stock")
                                };
                            }
                        }
                    }
                }

                Console.WriteLine(producto != null ? " Producto encontrado correctamente" : " Producto no encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al obtener el producto: {ex.Message}");
            }

            return producto;
        }

    }
}
