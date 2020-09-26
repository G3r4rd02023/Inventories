using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }      

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Index("Product_CompanyID_Descripcion_Index", 1, IsUnique = true)]
        [Index("Product_CompanyID_Codigo_Index", 1, IsUnique = true)]
        [Display(Name = "Compañia")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Producto")]
        [Index("Product_CompanyID_Descripcion_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(15, ErrorMessage =
           "El Campo {0} puede contener {1} y minimo {2} caracteres",
           MinimumLength = 3)]
        [Display(Name = "Codigo")]
        [Index("Product_CompanyID_Codigo_Index", 2, IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido!")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe elegir un {0}")]
        [Display(Name = "Categoria")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido!")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe elegir un {0}")]
        [Display(Name = "Impuesto")]
        public int TaxID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido!")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe elegir un {0} entre {1} y {2}")]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Stock { get { return Inventories.Sum(i => i.Stock); } } 

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public virtual Company Company { get; set; }
        public virtual Category Category { get; set; }
        public virtual Tax Tax { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderDetailTmp> OrderDetailTmps { get; set; }
    }
}