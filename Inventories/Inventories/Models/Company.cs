using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(30, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Compañia")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(20, ErrorMessage =
            "El Campo {0} puede contener {1} y minimo {2} caracteres",
            MinimumLength = 3)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]       
        [DataType(DataType.MultilineText)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Logo")]
        public byte[] Logo { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentID { get; set; }

        [Display(Name = "Ciudad")]
        public int CityID { get; set; }

        public virtual Department Department { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}