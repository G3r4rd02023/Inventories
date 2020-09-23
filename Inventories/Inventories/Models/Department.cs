using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(30, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Departamento")]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

    }
}