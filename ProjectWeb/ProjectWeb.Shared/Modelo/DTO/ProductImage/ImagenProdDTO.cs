using ProjectWeb.Shared.Modelo.DTO.Producto;
using System.ComponentModel.DataAnnotations;

namespace ProjectWeb.Shared.Modelo.DTO.ProductImage
{
    public class ImagenProdDTO
    {
        //[Key]
        public int Id_imagen { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Name_imagen { get; set; } = null!;

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(100, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Descripcion_imagen { get; set; } = null!;

        [Display(Name = "Imagen")]
        public byte[]? Foto_Producto { get; set; }

        [Display(Name = "Tipo Imagen")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string? Tipo_Imagen { get; set; } 


        [Display(Name = "Estado Imagen")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_Imagen { get; set; } = string.Empty;

        public ProductoDTO? Productos { get; set; } = null!;

        public int Id_producto { get; set; }
    }
}
