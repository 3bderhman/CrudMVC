using AutoMapper;
using Crud.BL.Interface;
using Crud.BL.Model;
using Crud.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crud.PL.Controllers
{
    [Authorize(Roles = "SuberAdmin, Admin, HR, User")]
    public class DepartmentController : Controller
    {
        private readonly IGenaricDERep<Department> department;
        private readonly IMapper mapper;

        public DepartmentController(IGenaricDERep<Department> department, IMapper mapper)
        {
            this.department = department;
            this.mapper = mapper;
        }
        public  async Task<IActionResult> Index()
        {
            var data = await department.GetAllAsync();
            var result = mapper.Map<IEnumerable<DepartmentVM>>(data);
            return View(result);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await department.GetByIdAsync(x => x.Id == id);
            var result = mapper.Map<DepartmentVM>(data);
            return View(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(obj);
                    await department.Create(data);
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
            var data = await department.GetByIdAsync(x => x.Id == id);
            var result = mapper.Map<DepartmentVM>(data);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentVM obj)
        {
            try
            {
                await department.Delete(obj.Id);
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
            var data = await department.GetByIdAsync(x => x.Id == id);
            var result = mapper.Map<DepartmentVM>(data);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(DepartmentVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = mapper.Map<Department>(obj);
                    await department.Update(data);
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
