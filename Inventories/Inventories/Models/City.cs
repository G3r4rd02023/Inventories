using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(30, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Ciudad")]
        [Index("City_Name_Index",2, IsUnique = true)]
        public string Name { get; set; }

        [Index("City_Name_Index",1, IsUnique = true)]
        [Display(Name = "Departamento")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}