using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly ApplicationDbContext context;
    public DepartmentController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [Route("GetAll")]
    [HttpGet]
    public IActionResult GetAllDepartment()
    {
        var deptList = context.Departments.ToList();

        return Ok(deptList);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddDepartment(DepartmentDto departmentDto)
    {

        var dept = context.Departments.Any(d => d.DepartmentName.ToLower() == departmentDto.DepartmentName.ToLower());
        if(dept)
        {
            return BadRequest("DpartmntName must be unique");
        }
        var department = new Department();
        department.DepartmentName = departmentDto.DepartmentName;
        department.IsActive = departmentDto.IsActive;


        context.Departments.Add(department);
        await context.SaveChangesAsync();
        return Ok(new { message = "Department Added Successfully" });
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdateDepartment(DepartmentDto departmentDto)
    {
        var existingDept = context.Departments.Find(departmentDto.DepartmentId);
        if (existingDept == null)
        {
            return NotFound("Department Not Found.");
        }
        existingDept.DepartmentName = departmentDto.DepartmentName;
        existingDept.IsActive = departmentDto.IsActive;

        await context.SaveChangesAsync();
        return Ok(new { message = "Department Updated Successfully" });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await context.Departments
            .Where(e => e.DepartmentId == id)
            .Select(e => new DepartmentDto
            {
                DepartmentId = e.DepartmentId,
                DepartmentName = e.DepartmentName,
                IsActive = e.IsActive,
                
            })
            .FirstOrDefaultAsync();

        if (department == null)
        {
            return NotFound("Department not found.");
        }
        return Ok(new { data = department });
    }

    [HttpDelete("Delete/{departmentId}")]
    public async Task<IActionResult> DeleteDepartment(int departmentId)
    {
        var existingDept = context.Departments.Find(departmentId);
        if (existingDept == null)
        {
            return NotFound("Department Not Found.");
        }

        context.Remove(existingDept);
        await context.SaveChangesAsync();
        return Ok(new { message = "Department Deleted Successfully" });
    }
}
