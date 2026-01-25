using EmployeeManagement.Dtos;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly ApplicationDbContext context;
    public EmployeeController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [Route("GetAll")]
    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        var empList = context.Employees.ToList();

        return Ok(empList);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await context.Employees
            .Where(e => e.EmployeeId == id)
            .Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                Contact = e.Contact,
                Email = e.Email,
                City = e.City,
                State = e.State,
                PinCode = e.PinCode,
                Address = e.Address,
                DesignationId = e.DesignationId,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate
            })
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            return NotFound("Employee not found.");
        }

        return Ok(employee);
    }


    //[Route("filter")]
    //[HttpGet]
    //public async Task<IActionResult> Filter(int? employeeId, string? search)
    //{
    //    var query = context.Employees.AsQueryable();
    //    if (employeeId.HasValue)
    //    {
    //        query = query.Where(d => d.EmployeeId == employeeId);
    //    }

    //    if (!string.IsNullOrEmpty(search))
    //    {
    //        query = query.Where(d => d.Name.ToLower() == search.ToLower());
    //    }

    //    var data = await query.ToListAsync();
    //    return Ok(data);
    //}

    [HttpPost("add")]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check duplicate Contact or Email
        bool exists = context.Employees.Any(e =>
            e.Contact.ToLower() == employeeDto.Contact.ToLower() ||
            e.Email.ToLower() == employeeDto.Email.ToLower()
        );

        if (exists)
        {
            return BadRequest("Contact or Email already exists.");
        }

        // Map DTO → Entity
        var employee = new Employee();

        employee.Name = employeeDto.Name;
        employee.Contact = employeeDto.Contact;
        employee.Email = employeeDto.Email;
        employee.City = employeeDto.City;
        employee.State = employeeDto.State;
        employee.PinCode = employeeDto.PinCode;
        employee.Address = employeeDto.Address;
        employee.DesignationId = employeeDto.DesignationId;
        employee.CreatedDate = DateTime.Now;
        employee.UpdatedDate = DateTime.Now;
        

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return Ok("Employee added successfully.");
    }


    [HttpPut("Update")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
    {
        var existingEmp = context.Employees
            .FirstOrDefault(e => e.EmployeeId == employeeDto.EmployeeId);

        if (existingEmp == null)
        {
            return NotFound("Employee not found.");
        }

        // Optional: check duplicate Contact / Email for other employees
        bool exists = context.Employees.Any(e =>
            (e.Contact == employeeDto.Contact || e.Email == employeeDto.Email)
            && e.EmployeeId != employeeDto.EmployeeId
        );

        if (exists)
        {
            return BadRequest("Contact or Email already exists.");
        }

        // Update fields
        existingEmp.Name = employeeDto.Name;
        existingEmp.Contact = employeeDto.Contact;
        existingEmp.Email = employeeDto.Email;
        existingEmp.City = employeeDto.City;
        existingEmp.State = employeeDto.State;
        existingEmp.PinCode = employeeDto.PinCode;
        existingEmp.Address = employeeDto.Address;
        existingEmp.DesignationId = employeeDto.DesignationId;
        existingEmp.UpdatedDate = DateTime.Now;

        await context.SaveChangesAsync();

        return Ok("Employee updated successfully.");
    }


    [HttpDelete("Delete/{employeeId}")]
    public IActionResult DeleteEmployee(int employeeId)
    {
        var emp = context.Employees.Find(employeeId);
        if (emp == null)
        {
            return NotFound("Employee Not Found.");
        }

        context.Remove(emp);
        context.SaveChangesAsync();
        return Ok("Employee Deleted Successfully");
    }

    [HttpGet("list")]
    public IActionResult GetEmployees([FromQuery] PaginationDto pagination)
    {
        var query = context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            string search = pagination.Search.ToLower();

            query = query.Where(e =>
                e.Name.ToLower().Contains(search) ||
                e.Email.ToLower().Contains(search) ||
                e.Contact.ToLower().Contains(search) ||
                e.City.ToLower().Contains(search)
            );
        }

        // Sorting
        switch (pagination.SortBy.ToLower())
        {
            case "name":
                query = pagination.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(e => e.Name)
                    : query.OrderBy(e => e.Name);
                break;

            case "createddate":
                query = pagination.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(e => e.CreatedDate)
                    : query.OrderBy(e => e.CreatedDate);
                break;

            default:
                query = query.OrderBy(e => e.EmployeeId);
                break;
        }

        int totalRecords = query.Count();

        var data = query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                Contact = e.Contact,
                Email = e.Email,
                City = e.City,
                State = e.State,
                PinCode = e.PinCode,
                Address = e.Address,
                DesignationId = e.DesignationId,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate
            })
            .ToList();

        return Ok(new
        {
            TotalRecords = totalRecords,
            pagination.PageNumber,
            pagination.PageSize,
            Data = data
        });
    }


}
