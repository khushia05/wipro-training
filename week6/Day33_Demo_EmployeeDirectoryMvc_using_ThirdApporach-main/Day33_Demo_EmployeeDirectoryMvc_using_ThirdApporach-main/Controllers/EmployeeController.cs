using EmployeeDirectoryMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EmployeeDirectoryMvc.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDirectoryDbContext _context;

        public EmployeeController(EmployeeDirectoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }
    }
}