using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventories.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }

       [Required]
        public int WarehouseID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public decimal Stock { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }

    }
}