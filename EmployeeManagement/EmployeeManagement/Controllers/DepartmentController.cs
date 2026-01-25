using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult AddDepartment(DepartmentDto departmentDto)
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
        context.SaveChangesAsync();
        return Ok("Department Added  Successfully");
    }

    [HttpPut("Update")]
    public IActionResult UpdateDepartment(DepartmentDto departmentDto)
    {
        var existingDept = context.Departments.Find(departmentDto.DepartmentId);
        if (existingDept == null)
        {
            return NotFound("Department Not Found.");
        }
        var department = new Department();
        department.DepartmentName = departmentDto.DepartmentName;
        department.IsActive = departmentDto.IsActive;

        context.SaveChangesAsync();
        return Ok("Department Updated Successfully");
    }

    [HttpDelete("Delete/{departmentId}")]
    public IActionResult DeleteDepartment(int departmentId)
    {
        var existingDept = context.Departments.Find(departmentId);
        if (existingDept == null)
        {
            return NotFound("Department Not Found.");
        }

        context.Remove(existingDept);
        context.SaveChangesAsync();
        return Ok("Department Deleted Successfully");
    }
}
