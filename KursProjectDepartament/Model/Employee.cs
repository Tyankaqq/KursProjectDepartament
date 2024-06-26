using System;
using System.Collections.Generic;

namespace KursProjectDepartament.Model;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public partial class Employee : Baseclass
{
    [Key]
    public int EmployeeId { get; set; }

    public int children;

    public int Children
    {
        get { return children; }
        set
        {
            children = value;
            OnPropetyChanged(nameof(children));
        }
    }

    private string firstName = null!;
    public string FirstName
    {
        get { return firstName; }
        set
        {
            firstName = value;
            OnPropetyChanged(nameof(FirstName));
        }
    }

    private string lastName = null!;
    public string LastName
    {
        get { return lastName; }
        set
        {
            lastName = value;
            OnPropetyChanged(nameof(LastName));
        }
    }

    private string? middleName;
    public string? MiddleName
    {
        get { return middleName; }
        set
        {
            middleName = value;
            OnPropetyChanged(nameof(MiddleName));
        }
    }

    private string birthDate = null!;
    public string BirthDate
    {
        get { return birthDate; }
        set
        {
            birthDate = value;
            OnPropetyChanged(nameof(BirthDate));
        }
    }

    private string? gender;
    public string? Gender
    {
        get { return gender; }
        set
        {
            gender = value;
            OnPropetyChanged(nameof(Gender));
        }
    }

    private string? maritalStatus;
    public string? MaritalStatus
    {
        get { return maritalStatus; }
        set
        {
            maritalStatus = value;
            OnPropetyChanged(nameof(MaritalStatus));
        }
    }

    private string? phoneNumber;
    public string? PhoneNumber
    {
        get { return phoneNumber; }
        set
        {
            phoneNumber = value;
            OnPropetyChanged(nameof(PhoneNumber));
        }
    }

    private string? email;
    public string? Email
    {
        get { return email; }
        set
        {
            email = value;
            OnPropetyChanged(nameof(Email));
        }
    }

    private string? address;
    public string? Address
    {
        get { return address; }
        set
        {
            address = value;
            OnPropetyChanged(nameof(Address));
        }
    }

    private string hireDate = null!;
    public string HireDate
    {
        get { return hireDate; }
        set
        {
            hireDate = value;
            OnPropetyChanged(nameof(HireDate));
        }
    }

    private string? position;
    public string? Position
    {
        get { return position; }
        set
        {
            position = value;
            OnPropetyChanged(nameof(Position));
        }
    }

    private int? departmentId;
    public int? DepartmentId
    {
        get { return departmentId; }
        set
        {
            departmentId = value;
            OnPropetyChanged(nameof(DepartmentId));
        }
    }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<SickLeaf> SickLeaves { get; set; } = new List<SickLeaf>();

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();

    public virtual ICollection<WorkHistory> WorkHistories { get; set; } = new List<WorkHistory>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}