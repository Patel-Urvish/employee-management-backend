using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Dtos;

public class DesignationDto
{
    public int DesignationId { get; set; }
    public int DepartmentId { get; set; }
    public string? DesignationName { get; set; }
}
