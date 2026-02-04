using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Dtos;

public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = null!;
    public string Contact { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string PinCode { get; set; } = null!;
    public string? Address { get; set; }
    public string? Role { get; set; }
    public int DesignationId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
