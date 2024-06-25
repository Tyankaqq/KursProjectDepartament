using System;
using System.Collections.Generic;

namespace KursProjectDepartament.Model;

public partial class Vacation
{
    public int VacationId { get; set; }

    public int? EmployeeId { get; set; }

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string? Type { get; set; }

    public virtual Employee? Employee { get; set; }
}
