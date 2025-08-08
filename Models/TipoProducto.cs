
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Models.Tienda_CS
{
    // Esta clase representa un tipo de producto en la tienda
    public class TipoProducto
    {
        //CLASE PARA EL MODELO DE TIPO PRODUCTO

        [Key]
        [Display(Name = "#")]
        public int IdTipo { get; set; }     // ID único del tipo de producto (coincide con id_tipo en BD)

        [Required(ErrorMessage = "El nombre del tipo de producto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre del tipo de producto")]
        public string Nombre { get; set; }   // Nombre del tipo de producto

        // Propiedad de navegación para la relación con Producto (uno a muchos)
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
