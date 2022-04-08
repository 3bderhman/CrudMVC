using AutoMapper;
using Crud.BL.Helper;
using Crud.BL.Interface;
using Crud.BL.Model;
using Crud.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crud.PL.Controllers
{
    [Authorize(Roles = "SuberAdmin, Admin, HR, User")]
    public class EmployeeController : Controller
    {
        private readonly IGenaricDERep<Employee> employee;
        private readonly IGenaricDERep<Department> department;
        private readonly IMapper mapper;

        public EmployeeController(IGenaricDERep<Employee> employee, IGenaricDERep<Department> department, IMapper mapper)
        {
            this.employee = employee;
            this.department = department;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var data = await employee.GetAllAsync(x => x.IsActive == true && x.IsUpdated == false);
            var result = mapper.Map<IEnumerable<EmployeeVM>>(data);
            return View(result);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await employee.GetByIdAsync(x => x.IsActive == true && x.IsUpdated == false && x.Id == id);
            var result = mapper.Map<EmployeeVM>(data);
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            var dataDepartemnt = await department.GetAllAsync();
            ViewBag.DepartmentList = new SelectList(dataDepartemnt, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM obj)
        {
            var dataDepartemnt = await department.GetAllAsync();
            ViewBag.DepartmentList = new SelectList(dataDepartemnt, "Id", "Name");
            try
            {
                if (ModelState.IsValid)
                {
                    obj.ImgName = Files.UploadFile(obj.Img, "Imgs");
                    obj.FileName = Files.UploadFile(obj.File, "Docs");
                    var data = mapper.Map<Employee>(obj);
                    await employee.Create(data);
                    return RedirectToAction("Index");
                }
                ViewData["Message"] = "Validation Error";
                return View(obj);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(obj);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data = await employee.GetByIdAsync(x => x.IsActive == true && x.IsUpdated == false && x.Id == id);
            var result = mapper.Map<EmployeeVM>(data);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeVM obj)
        {
            Files.RemoveFile("Imgs", obj.ImgName);
            Files.RemoveFile("Docs", obj.FileName);
            var dataDepartemnt = await department.GetAllAsync();
            ViewBag.DepartmentList = new SelectList(dataDepartemnt, "Id", "Name");

            try
            {
                await employee.Delete(obj.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(obj);
            }
        }
        public async Task<IActionResult> Update(int id)
        {
            var dataDepartemnt = await department.GetAllAsync();
            ViewBag.DepartmentList = new SelectList(dataDepartemnt, "Id", "Name", await department.GetByIdAsync(x => x.Id == id));
            var data = await employee.GetByIdAsync(x => x.IsActive == true && x.IsUpdated == false && x.Id == id);
            var result = mapper.Map<EmployeeVM>(data);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeVM obj)
        {
            var dataDepartemnt = await department.GetAllAsync();
            ViewBag.DepartmentList = new SelectList(dataDepartemnt, "Id", "Name");
            try
            {
                if (ModelState.IsValid)
                {
                    obj.ImgName = Files.UploadFile(obj.Img, "Imgs");
                    obj.FileName = Files.UploadFile(obj.File, "Docs");
                    var data = mapper.Map<Employee>(obj);
                    await employee.Update(data);
                    return RedirectToAction("Index");
                }
                ViewData["Message"] = "Validation Error";
                return View(obj);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(obj);
            }
        }
    }
}
