
using Models.Tienda_CS;
using System;
using System.Collections.Generic;
using System.Data;
using Tienda_CS;


namespace Controllers.Tienda_CS
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
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
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
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
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
                    using (var command = new MySql.Data.MySqlClient.MySqlCommand(query, connection))
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
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexionBD"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    producto = new Producto
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Stock = Convert.ToInt32(reader["Stock"])
                    };
                }

                reader.Close();
            }

            return producto;
        }

    }
}
