using System;
using System.Collections.Generic;

namespace KursProjectDepartament.Model;

public partial class WorkHistory
{
    public int WorkHistoryId { get; set; }

    public int? EmployeeId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Position { get; set; }

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public string? ReasonForLeaving { get; set; }

    public virtual Employee? Employee { get; set; }
}
