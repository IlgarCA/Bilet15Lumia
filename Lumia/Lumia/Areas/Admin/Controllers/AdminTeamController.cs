using Lumia.DAL;
using Lumia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Lumia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminTeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if(!ModelState.IsValid) { return View(); }
            
            
            bool isExsisFullName = await _context.Employees.AnyAsync(c => c.FullName.ToLower().Trim() == employee.FullName.ToLower().Trim());
            bool isExsisProfessian = await _context.Employees.AnyAsync(c => c.Professian.ToLower().Trim() == employee.Professian.ToLower().Trim());
            bool isExsisBio = await _context.Employees.AnyAsync(c => c.Bio.ToLower().Trim() == employee.Bio.ToLower().Trim());
            bool isExsisTwitter = await _context.Employees.AnyAsync(c => c.Twitter.ToLower().Trim() == employee.Twitter.ToLower().Trim());
            bool isExsisFacebook = await _context.Employees.AnyAsync(c => c.Facebook.ToLower().Trim() == employee.Facebook.ToLower().Trim());
            bool isExsisInstagram = await _context.Employees.AnyAsync(c => c.Instagram.ToLower().Trim() == employee.Instagram.ToLower().Trim());
            bool isExsisLinkedin = await _context.Employees.AnyAsync(c => c.Linkedin.ToLower().Trim() == employee.Linkedin.ToLower().Trim());

            if (isExsisFullName) { ModelState.AddModelError("FullName", "already exsist"); return View(); }
            if (isExsisProfessian) { ModelState.AddModelError("Profession", "already exsist"); return View(); }
            if (isExsisBio) { ModelState.AddModelError("Bio", "already exsist"); return View(); }
            if (isExsisTwitter) { ModelState.AddModelError("Twitter", "already exsist"); return View(); }
            if (isExsisFacebook) { ModelState.AddModelError("Facebook", "already exsist"); return View(); }
            if (isExsisInstagram) { ModelState.AddModelError("Instagram", "already exsist"); return View(); }
            if (isExsisLinkedin) { ModelState.AddModelError("Linkedin", "already exsist"); return View(); }

            if(employee.FormFile is null) { ModelState.AddModelError("FormFile", "already exsist"); return View(); }
            if (!employee.FormFile.ContentType.Contains("image")){ ModelState.AddModelError("FormFile", "already exsist"); return View(); }
            if (employee.FormFile.Length / 1024 / 1024 >= 5) { ModelState.AddModelError("FormFile", "already exsist"); return View(); }


            string FileName = Guid.NewGuid() + employee.FormFile.FileName;
            string FolderName = "/assets/img/team/";
            string FullPath = _env.WebRootPath + FolderName + FileName;
            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                employee.FormFile.CopyTo(fileStream);
            }
            employee.Img = FileName;
            
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Employee? employee = _context.Employees.Find(id);
            if (employee == null) { return NotFound(); }

            return View(employee);

        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Employee employee)
        {
            Employee? editEmployee = _context.Employees.Find(employee.Id);
            if (editEmployee == null) { return View(); }
            if (!ModelState.IsValid) { return View(); }

            editEmployee.FullName = employee.FullName;
            editEmployee.Professian = employee.Professian;
            editEmployee.Bio = employee.Bio;
            editEmployee.Twitter = employee.Twitter;
            editEmployee.Facebook = employee.Facebook;
            editEmployee.Instagram = employee.Instagram;
            editEmployee.Linkedin = employee.Linkedin;
            editEmployee.FormFile = employee.FormFile;


            if (employee.FormFile is null)
            {
                ModelState.AddModelError("FormFile", "Image can not be null");
                return View();
            }
            if (!employee.FormFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("FormFile", "enter image");
                return View();
            }
            if (employee.FormFile.Length / 1024 / 1024 >= 5)
            {
                ModelState.AddModelError("FormFile", "dont input image big 5MB");
                return View();
            }

            string FileName = Guid.NewGuid() + employee.FormFile.FileName;
            string FolderName = ("/assets/img/team/");
            string FullPath = _env.WebRootPath + FolderName + FileName;
            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                employee.FormFile.CopyTo(fileStream);
            }
            employee.Img = FileName;


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Employee? employee = _context.Employees.Find(id);
            if (employee == null) { return View(); }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
