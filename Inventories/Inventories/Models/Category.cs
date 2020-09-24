using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(30, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Descripcion")]
        [Index("Category_CompanyID_Name_Index", 2, IsUnique = true)]
        public string Descripcion { get; set; }

        [Index("Category_CompanyID_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Compañia")]
        public int CompanyID { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}