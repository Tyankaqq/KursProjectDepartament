using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursProjectDepartament.Model
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }

        public string OrderType { get; set; } = null!;
        public string OrderDate { get; set; } = null!;
        public string? OrderDetails { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
