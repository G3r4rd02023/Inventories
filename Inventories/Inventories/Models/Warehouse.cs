using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }

        [Display(Name = "Compañia")]
        [Range(1,double.MaxValue, ErrorMessage ="Debe seleccionar {0}" )]
        [Index("Warehouse_CompanyID_Name_Index", 1, IsUnique = true)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50,ErrorMessage ="El Campo {0} debe tener un maximo de {1} caracteres")]
        [Display(Name = "Bodega")]
        [Index("Warehouse_CompanyID_Name_Index",2,IsUnique = true)]
        public string Name { get; set; }
            
        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "El Campo {0} debe tener un maximo de {1} caracteres")]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(50, ErrorMessage = "El Campo {0} debe tener un maximo de {1} caracteres")]
        [Display(Name = "Dirección")]
        public string Address{ get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentID { get; set; }

        [Display(Name = "Ciudad")]
        public int CityID { get; set; }

        public virtual Department Department { get; set; }
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }


    }
}