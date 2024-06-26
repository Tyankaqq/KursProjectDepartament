using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace KursProjectDepartament.Model;

public partial class HumanDepartmentDbContext : DbContext
{
    public HumanDepartmentDbContext()
    {
    }

    public HumanDepartmentDbContext(DbContextOptions<HumanDepartmentDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<SickLeaf> SickLeaves { get; set; }

    public virtual DbSet<Vacation> Vacations { get; set; }

    public virtual DbSet<WorkHistory> WorkHistories { get; set; }

    public Employee GetEmployeeById(int? employeeId)
    {
        return Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        
    }

    // Список сотрудников без высшего образования
    public List<Education> GetEmployeesWithoutHigherEducation()
    {
        var educationWithoutHigherDegree = Educations
        .Include(e => e.Employee)
        .Where(ed => ed.Degree != "Высшее")
        .ToList();

        return educationWithoutHigherDegree;
    }

    // Список сотрудников, образование которых не соответствует должности
    public List<Employee> GetEducationsWithMismatchedFieldOfStudy()
    {
        using (var dbContext = new HumanDepartmentDbContext())
        {
            var employees = new List<Employee>();
            var educationsWithMismatchedFieldOfStudy = dbContext.Educations
                .Include(ed => ed.Employee) 
                .Where(ed => ed.FieldOfStudy != ed.Employee.Position)
                .ToList();

            foreach (var employee in educationsWithMismatchedFieldOfStudy)
            {
                employees.Add(GetEmployeeById(employee.EmployeeId));
            }

            return employees;
        }
    }





        // Список всех детей сотрудников (в зависимости от семейного положения)
        public List<Employee> GetAllChildren()
    {
        var allChildren = Employees
            .Where(e => e.Children > 0)
            .ToList();

        return allChildren;
    }

    // Список сотрудников, не проживающих в заданном городе
    public List<Employee> GetEmployeesNotLivingInCity(string Adress)
    {
        var employeesNotLivingInCity = Employees.Where(e => e.Address != Adress).ToList();
        return employeesNotLivingInCity;
    }

    // Список сотрудников отдела
    public List<Employee> GetDepartmentEmployees(int departmentId)
    {
        var departmentEmployees = Employees.Where(e => e.DepartmentId == departmentId).ToList();
        return departmentEmployees;
    }

    // Список сотрудников на больничном или в отпуске в заданный период
    public List<Employee> GetEmployeesOnLeave(DateTime startDate, DateTime endDate)
    {
        var allSickLeaves = SickLeaves.ToList(); // Загрузка всех записей из таблицы SickLeaves с включением связанного сотрудника

        var employeesOnSickLeave = new List<Employee>();

        foreach (var sickLeave in allSickLeaves)
        {
            DateTime sickLeaveStartDate = DateTime.Parse(sickLeave.StartDate);
            DateTime sickLeaveEndDate = DateTime.Parse(sickLeave.EndDate);

            if (sickLeaveStartDate.Date <= endDate && sickLeaveEndDate.Date >= startDate)
            {
                employeesOnSickLeave.Add(GetEmployeeById(sickLeave.EmployeeId));
            }
        }

        return employeesOnSickLeave.Distinct().ToList(); // Удаление дубликатов сотрудников
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\Пользователь\\DataGripProjects\\Human Resources Department\\HumanDepartmentDB.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName).HasColumnName("department_name");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.ToTable("Education");

            entity.Property(e => e.EducationId).HasColumnName("education_id");
            entity.Property(e => e.Degree).HasColumnName("degree");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FieldOfStudy).HasColumnName("field_of_study");
            entity.Property(e => e.GraduationYear).HasColumnName("graduation_year");
            entity.Property(e => e.Institution).HasColumnName("institution");

            entity.HasOne(d => d.Employee).WithMany(p => p.Educations).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MaritalStatus).HasColumnName("marital_status");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasForeignKey(d => d.DepartmentId);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.NewPosition).HasColumnName("new_position");
            entity.Property(e => e.OldPosition).HasColumnName("old_position");
            entity.Property(e => e.PromotionDate).HasColumnName("promotion_date");
            entity.Property(e => e.Reason).HasColumnName("reason");

            entity.HasOne(d => d.Employee).WithMany(p => p.Promotions).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<SickLeaf>(entity =>
        {
            entity.HasKey(e => e.SickLeaveId);

            entity.Property(e => e.SickLeaveId).HasColumnName("sick_leave_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.SickLeaves).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.Property(e => e.VacationId).HasColumnName("vacation_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Employee).WithMany(p => p.Vacations).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<WorkHistory>(entity =>
        {
            entity.ToTable("WorkHistory");

            entity.Property(e => e.WorkHistoryId).HasColumnName("work_history_id");
            entity.Property(e => e.CompanyName).HasColumnName("company_name");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.ReasonForLeaving).HasColumnName("reason_for_leaving");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkHistories).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.OrderType).HasColumnName("order_type");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.OrderDetails).HasColumnName("order_details");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders).HasForeignKey(d => d.EmployeeId);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}



