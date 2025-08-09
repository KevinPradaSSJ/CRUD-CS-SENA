
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace CRUD_SC_SENA.Models
{
    //Esta clase representa un producto en la tienda
    public class Producto
    {
        //CLASE PARA EL MODELO DE PRODUCTO

        [Key]
        [Display(Name = "#")]
        public int IdProducto { get; set; }     // ID único del producto (coincide con id_producto en BD)

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s\-\.\,\(\)]+$", ErrorMessage = "El nombre solo puede contener letras, números, espacios y algunos símbolos básicos")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }   // Nombre del producto

        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre $0.01 y $999,999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe tener máximo 2 decimales")]
        [Display(Name = "Precio del producto")]
        public decimal Precio { get; set; }  // Precio del producto

        [Required(ErrorMessage = "Seleccionar un tipo de producto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de producto válido")]
        [Display(Name = "Tipo de producto")]
        public int TipoId { get; set; } // ID del tipo de producto (coincide con tipo_id en BD)

        [Required(ErrorMessage = "Ingresar la cantidad en stock es obligatorio")]
        [Range(0, 999999, ErrorMessage = "El stock debe estar entre 0 y 999,999 unidades")]
        [Display(Name = "Stock disponible")]
        public int Stock { get; set; }  // Cantidad disponible del producto en stock

        // Propiedad de navegación para la relación con TipoProducto
        [Display(Name = "Tipo de producto")]
        public virtual TipoProducto TipoProducto { get; set; }

        // Propiedades calculadas adicionales
        [Display(Name = "Valor total en inventario")]
        public decimal ValorTotalInventario => Precio * Stock;

        [Display(Name = "Estado del stock")]
        public string EstadoStock
        {
            get
            {
                if (Stock == 0) return "Sin stock";
                if (Stock <= 5) return "Stock bajo";
                if (Stock <= 20) return "Stock normal";
                return "Stock alto";
            }
        }

        [Display(Name = "Disponible")]
        public bool EstaDisponible => Stock > 0;

        // Constructor
        public Producto()
        {
            // Valores por defecto
            Precio = 0.01m;
            Stock = 0;
        }

        // Método de validación personalizada completa
        public bool EsValido(out List<string> errores)
        {
            errores = new List<string>();

            // Validar nombre
            ValidarNombre(errores);

            // Validar precio
            ValidarPrecio(errores);

            // Validar stock
            ValidarStock(errores);

            // Validar tipo de producto
            ValidarTipoProducto(errores);

            return errores.Count == 0;
        }

        private void ValidarNombre(List<string> errores)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                errores.Add("El nombre del producto no puede estar vacío");
                return;
            }

            // Limpiar espacios extra
            Nombre = Nombre.Trim();

            // Validar longitud
            if (Nombre.Length < 2)
            {
                errores.Add("El nombre debe tener al menos 2 caracteres");
            }
            else if (Nombre.Length > 150)
            {
                errores.Add("El nombre no puede tener más de 150 caracteres");
            }

            // Validar que no contenga solo espacios o números
            if (Nombre.All(c => char.IsWhiteSpace(c) || char.IsDigit(c)))
            {
                errores.Add("El nombre debe contener al menos una letra");
            }

            // Validar caracteres permitidos
            if (!Regex.IsMatch(Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s\-\.\,\(\)]+$"))
            {
                errores.Add("El nombre contiene caracteres no permitidos");
            }

            // Validar formato
            if (Nombre.StartsWith(" ") || Nombre.EndsWith(" "))
            {
                errores.Add("El nombre no puede empezar o terminar con espacios");
            }

            if (Nombre.Contains("  "))
            {
                errores.Add("El nombre no puede contener espacios dobles");
            }
        }

        private void ValidarPrecio(List<string> errores)
        {
            if (Precio <= 0)
            {
                errores.Add("El precio debe ser mayor a cero");
            }
            else if (Precio > 999999.99m)
            {
                errores.Add("El precio no puede exceder $999,999.99");
            }

            // Validar que no tenga más de 2 decimales
            decimal precioRedondeado = Math.Round(Precio, 2);
            if (Precio != precioRedondeado)
            {
                errores.Add("El precio no puede tener más de 2 decimales");
            }

            // Validar precios sospechosos
            if (Precio < 0.01m)
            {
                errores.Add("El precio mínimo es $0.01");
            }
        }

        private void ValidarStock(List<string> errores)
        {
            if (Stock < 0)
            {
                errores.Add("El stock no puede ser negativo");
            }
            else if (Stock > 999999)
            {
                errores.Add("El stock no puede exceder 999,999 unidades");
            }
        }

        private void ValidarTipoProducto(List<string> errores)
        {
            if (TipoId <= 0)
            {
                errores.Add("Debe seleccionar un tipo de producto válido");
            }
        }

        // Método para normalizar los datos
        public void NormalizarDatos()
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                // Limpiar y normalizar nombre
                Nombre = Nombre.Trim();
                Nombre = Regex.Replace(Nombre, @"\s+", " ");
                Nombre = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Nombre.ToLower());
            }

            // Redondear precio a 2 decimales
            Precio = Math.Round(Precio, 2);
        }

        // Método para validar reglas de negocio
        public bool ValidarReglasNegocio(out List<string> advertencias)
        {
            advertencias = new List<string>();

            // Advertir sobre precios muy altos
            if (Precio > 100000m)
            {
                advertencias.Add("Precio muy alto, verifique que sea correcto");
            }

            // Advertir sobre stock muy alto
            if (Stock > 10000)
            {
                advertencias.Add("Stock muy alto, verifique la cantidad");
            }

            // Advertir sobre productos sin stock
            if (Stock == 0)
            {
                advertencias.Add("Producto sin stock disponible");
            }

            // Advertir sobre precios muy bajos
            if (Precio < 1.00m)
            {
                advertencias.Add("Precio muy bajo, verifique que sea correcto");
            }

            return advertencias.Count == 0;
        }

        // Método principal de validación antes de guardar
        public bool PuedeGuardarse(out string mensaje, out List<string> advertencias)
        {
            List<string> errores;
            bool esValido = EsValido(out errores);
            
            // Obtener advertencias
            ValidarReglasNegocio(out advertencias);

            if (!esValido)
            {
                mensaje = "Errores de validación: " + string.Join(", ", errores);
                return false;
            }

            if (advertencias.Count > 0)
            {
                mensaje = "Producto válido con advertencias: " + string.Join(", ", advertencias);
            }
            else
            {
                mensaje = "Producto válido";
            }
            
            return true;
        }

        // Override del método ToString
        public override string ToString()
        {
            return $"{IdProducto} - {Nombre} (${Precio:F2}) - Stock: {Stock}";
        }

        // Método para obtener información resumida
        public string ObtenerResumen()
        {
            return $"{Nombre} - ${Precio:F2} - {EstadoStock} ({Stock} unidades)";
        }
    }
}
