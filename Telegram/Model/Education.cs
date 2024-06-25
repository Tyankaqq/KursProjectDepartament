using System;
using System.Collections.Generic;

namespace KursProjectDepartament.Model;

public partial class Education
{
    public int EducationId { get; set; }

    public int? EmployeeId { get; set; }

    public string Institution { get; set; } = null!;

    public string? Degree { get; set; }

    public string? FieldOfStudy { get; set; }

    public string? GraduationYear { get; set; }

    public virtual Employee? Employee { get; set; }
}
