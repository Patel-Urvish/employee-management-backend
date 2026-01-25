using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    public int DepartmentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string DepartmentName { get; set; } = null!;

    public bool? IsActive { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Designation> Designations { get; set; } = new List<Designation>();
}
