using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models;

[Table("Designation")]
public partial class Designation
{
    [Key]
    public int DesignationId { get; set; }

    public int DepartmentId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DesignationName { get; set; }

    [ForeignKey("DepartmentId")]
    [InverseProperty("Designations")]
    public virtual Department Department { get; set; } = null!;

    [InverseProperty("Designation")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
