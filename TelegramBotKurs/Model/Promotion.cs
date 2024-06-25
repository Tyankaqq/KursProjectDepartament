using System;
using System.Collections.Generic;

namespace KursProjectDepartament.Model;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public int? EmployeeId { get; set; }

    public string? OldPosition { get; set; }

    public string? NewPosition { get; set; }

    public string? PromotionDate { get; set; }

    public string? Reason { get; set; }

    public virtual Employee? Employee { get; set; }
}
