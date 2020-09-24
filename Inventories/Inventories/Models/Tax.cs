using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Tax
    {
        [Key]
        public int TaxID { get; set; }

        [Required(ErrorMessage ="El campo es requerido") ]
        [StringLength(30, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Impuesto")]
        [Index("Tax_CompanyID_Descripcion_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido!")] 
        [DisplayFormat(DataFormatString ="{0:P2}",ApplyFormatInEditMode = false)]
        [Display(Name = "Tasa")] 
        [Range(0.00, 1.00, ErrorMessage = "Debe elegir un {0} entre {1} y {2}")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Index("Tax_CompanyID_Descripcion_Index", 1, IsUnique = true)]
        [Display(Name = "Compañia")]
        public int CompanyID { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}