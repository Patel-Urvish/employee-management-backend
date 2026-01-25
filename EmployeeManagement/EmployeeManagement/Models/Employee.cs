using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    [Column("EmployeeId")]
    public int EmployeeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Contact { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string State { get; set; } = null!;

    [StringLength(6)]
    [Unicode(false)]
    public string PinCode { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Address { get; set; }

    public int DesignationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    [ForeignKey("DesignationId")]
    [InverseProperty("Employees")]
    public virtual Designation Designation { get; set; } = null!;
}
