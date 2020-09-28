using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(256, ErrorMessage = "El registro {0} debe tener  {1} caracteres de longitud")]
        [Display(Name = "Email")]
        
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El registro {0} debe tener  {1} caracteres de longitud")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El registro {0} debe tener  {1} caracteres de longitud")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El registro {0} debe tener  {1} caracteres de longitud")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El registro {0} debe tener  {1} caracteres de longitud")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debes seleccionar {0}")]
        [Display(Name = "Departmento")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debes Seleccionar {0}")]
        [Display(Name = "Ciudad")]
        public int CityId { get; set; }

        [Display(Name = "Cliente")]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<CompanyCustomers> CompanyCustomers { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}