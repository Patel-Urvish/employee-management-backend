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
    public IActionResult GetAllDesignation()
    {
        var degList = context.Designations.ToList();

        return Ok(degList);
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

        var data = await query.ToListAsync();
        return Ok(data);
    }

    [HttpPost("Add")]
    public IActionResult AddDesignation(DesignationDto designationDto)
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
        context.SaveChangesAsync();
        return Ok("Designation Added Successfully");
    }

    [HttpPut("Update")]
    public IActionResult UpdateDesignation(DesignationDto designationDto)
    {
        var existingDeg = context.Designations.Find(designationDto.DesignationId);
        if (existingDeg == null)
        {
            return NotFound("Designation Not Found.");
        }
        var designation = new Designation();
        designation.DesignationName = designationDto.DesignationName;
        designation.DepartmentId = designationDto.DepartmentId;

        context.SaveChangesAsync();
        return Ok("Designation Updated Successfully");
    }

    [HttpDelete("Delete/{designationId}")]
    public IActionResult DeleteDesignation(int designationId)
    {
        var existingDeg = context.Designations.Find(designationId);
        if (existingDeg == null)
        {
            return NotFound("Designation Not Found.");
        }

        context.Remove(existingDeg);
        context.SaveChangesAsync();
        return Ok("Designation Deleted Successfully");
    }
}
