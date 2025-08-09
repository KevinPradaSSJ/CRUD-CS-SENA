
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace CRUD_SC_SENA.Models
{
    // Esta clase representa un tipo de producto en la tienda
    public class TipoProducto
    {
        //CLASE PARA EL MODELO DE TIPO PRODUCTO

        [Key]
        [Display(Name = "#")]
        public int IdTipo { get; set; }     // ID único del tipo de producto (coincide con id_tipo en BD)

        [Required(ErrorMessage = "El nombre del tipo de producto es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\-\.]+$", ErrorMessage = "El nombre solo puede contener letras, espacios, guiones y puntos")]
        [Display(Name = "Nombre del tipo de producto")]
        public string Nombre { get; set; }   // Nombre del tipo de producto

        // Propiedad de navegación para la relación con Producto (uno a muchos)
        public virtual ICollection<Producto> Productos { get; set; }

        // Constructor para inicializar la colección
        public TipoProducto()
        {
            Productos = new List<Producto>();
        }

        // Método de validación personalizada
        public bool EsValido(out List<string> errores)
        {
            errores = new List<string>();

            // Validar que el nombre no esté vacío después de limpiar espacios
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                errores.Add("El nombre del tipo de producto no puede estar vacío");
            }
            else
            {
                // Limpiar espacios extra
                Nombre = Nombre.Trim();
                
                // Validar longitud después de limpiar
                if (Nombre.Length < 2)
                {
                    errores.Add("El nombre debe tener al menos 2 caracteres");
                }
                else if (Nombre.Length > 100)
                {
                    errores.Add("El nombre no puede tener más de 100 caracteres");
                }

                // Validar que no contenga solo espacios
                if (Nombre.All(c => char.IsWhiteSpace(c)))
                {
                    errores.Add("El nombre no puede contener solo espacios");
                }

                // Validar caracteres especiales
                if (!Regex.IsMatch(Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\-\.]+$"))
                {
                    errores.Add("El nombre solo puede contener letras, espacios, guiones y puntos");
                }

                // Validar que no empiece o termine con espacios, guiones o puntos
                if (Nombre.StartsWith(" ") || Nombre.EndsWith(" ") || 
                    Nombre.StartsWith("-") || Nombre.EndsWith("-") ||
                    Nombre.StartsWith(".") || Nombre.EndsWith("."))
                {
                    errores.Add("El nombre no puede empezar o terminar con espacios, guiones o puntos");
                }

                // Validar que no tenga espacios dobles
                if (Nombre.Contains("  "))
                {
                    errores.Add("El nombre no puede contener espacios dobles");
                }
            }

            return errores.Count == 0;
        }

        // Método para limpiar y normalizar el nombre
        public void NormalizarNombre()
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                // Eliminar espacios al inicio y final
                Nombre = Nombre.Trim();
                
                // Reemplazar múltiples espacios con uno solo
                Nombre = Regex.Replace(Nombre, @"\s+", " ");
                
                // Capitalizar primera letra de cada palabra
                Nombre = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Nombre.ToLower());
            }
        }

        // Override del método ToString para mejor representación
        public override string ToString()
        {
            return $"{IdTipo} - {Nombre}";
        }

        // Método para validar antes de guardar
        public bool PuedeGuardarse(out string mensaje)
        {
            List<string> errores;
            bool esValido = EsValido(out errores);
            
            if (!esValido)
            {
                mensaje = string.Join(", ", errores);
                return false;
            }
            
            mensaje = "Tipo de producto válido";
            return true;
        }
    }
}
