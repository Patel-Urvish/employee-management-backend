using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DesignationController : ControllerBase
{
    private readonly ApplicationDbContext context;
    public DesignationController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [Route("GetAll")]
    [HttpGet]
    public async Task<IActionResult> GetAllDesignation()
    {
        var degList = await context.Designations.Include(d => d.Department)
            .Select(d => new
            {
                d.DesignationId,
                d.DesignationName,
                d.DepartmentId,
                Department = new
                {
                    departmentId = d.Department.DepartmentId,
                    departmentName = d.Department.DepartmentName
                }

            }).ToListAsync();

        return Ok(degList);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDesignationById(int id)
    {
        var designation = await context.Designations
            .Where(e => e.DesignationId == id)
            .Select(e => new DesignationDto
            {
                DepartmentId = e.DepartmentId,
                DesignationId = e.DesignationId,
                DesignationName = e.DesignationName,

            })
            .FirstOrDefaultAsync();

        if (designation == null)
        {
            return NotFound("Designation not found.");
        }
        return Ok(new { data = designation });
    }

    [Route("filter")]
    [HttpGet]
    public async Task<IActionResult> Filter(int? departmentId, string? search)
    {
        var query = context.Designations.AsQueryable();
        if(departmentId.HasValue)
        {
            query = query.Where(d => d.DepartmentId == departmentId);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(d => d.DesignationName.ToLower() == search.ToLower());
        }

        var data = await query.Select(d => new
        {
            d.DesignationId,
            d.DesignationName,
            d.DepartmentId,
            Department = new
            {
                departmentId = d.Department.DepartmentId,
                departmentName = d.Department.DepartmentName
            }
        }).ToListAsync();

        return Ok(data);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddDesignation(DesignationDto designationDto)
    {

        var deg = context.Designations.Any(d => d.DesignationName.ToLower() == designationDto.DesignationName.ToLower());
        if (deg)
        {
            return BadRequest("DesignationName must be unique");
        }
        var designation = new Designation();
        designation.DesignationName = designationDto.DesignationName;
        designation.DepartmentId = designationDto.DepartmentId;


        context.Designations.Add(designation);
        await context.SaveChangesAsync();
        return Ok(new { message = "Designation Added Successfully" });
    }

    [HttpPut("Update")]
    public IActionResult UpdateDesignation(DesignationDto designationDto)
    {
        var existingDeg = context.Designations.Find(designationDto.DesignationId);
        if (existingDeg == null)
        {
            return NotFound("Designation Not Found.");
        }

        existingDeg.DesignationName = designationDto.DesignationName;
        existingDeg.DepartmentId = designationDto.DepartmentId;

        context.SaveChangesAsync();
        return Ok(new { message = "Designation Updated Successfully" });
    }

    [HttpDelete("Delete/{designationId}")]
    public async Task<IActionResult> DeleteDesignation(int designationId)
    {
        var existingDeg = context.Designations.Find(designationId);
        if (existingDeg == null)
        {
            return NotFound("Designation Not Found.");
        }

        context.Remove(existingDeg);
        await context.SaveChangesAsync();
        return Ok(new { message = "Designation Deleted Successfully" });
    }
}
