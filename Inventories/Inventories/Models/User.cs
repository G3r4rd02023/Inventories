
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Inventories.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(256, ErrorMessage =
           "El Campo {0} puede contener {1} y minimo {2} caracteres",
           MinimumLength = 3)]
        [Display(Name = "Email")]
        [Index("User_UserName_Index",  IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50, ErrorMessage =
           "El Campo {0} puede contener {1} y minimo {2} caracteres",
           MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Usuario")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(50, ErrorMessage =
           "El Campo {0} puede contener {1} y minimo {2} caracteres",
           MinimumLength = 3)]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(20, ErrorMessage =
           "El Campo {0} puede contener {1} y minimo {2} caracteres",
           MinimumLength = 3)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        public byte[] Foto { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentID { get; set; }

        [Display(Name = "Ciudad")]
        public int CityID { get; set; }

        [Display(Name = "Compañia")]
        public int CompanyID { get; set; }

        public virtual Department Department { get; set; }
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }


    }
}