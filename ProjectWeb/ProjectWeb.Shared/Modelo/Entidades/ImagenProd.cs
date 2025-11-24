using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Shared.Modelo.Entidades
{
    public class ImagenProd
    {
        [Key]
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

        public DateTime Date_reg { get; set; } = DateTime.Now;


        [Display(Name = "Estado Imagen")]
        [Required(ErrorMessage = "El Campo {0} es Obligatorio!")]
        [MaxLength(10, ErrorMessage = "El Campo {0} no puede mas de {1} Caracteres")]
        public string Estado_Imagen { get; set; } = string.Empty;

        public Producto Productos { get; set; } = null!;

        public int Id_producto { get; set; }

    }
}
