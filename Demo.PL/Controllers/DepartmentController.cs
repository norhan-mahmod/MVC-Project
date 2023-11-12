using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var departments = unitOfWork.DepartmentRepository.GetAll();
            var departmentsViewModel = mapper.Map<List<DepartmentViewModel>>(departments);
            return View(departmentsViewModel);
        }
        public IActionResult Create()
        {
            return View(new DepartmentViewModel());
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                // manual mapping
                //var department = new Department()
                //{
                //    Code = departmentViewModel.Code,
                //    Name = departmentViewModel.Name
                //};
                // Auto Mapping (using AutoMapper package)
                // [map according to names and datatypes of properties]
                var department = mapper.Map<Department>(departmentViewModel);
                unitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction("Index");
            }
            return View(departmentViewModel);
        }
        public IActionResult Update(int id)
        {
            if (id == null)
                return NotFound();
            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
                return NotFound();
            var departmentViewModel = mapper.Map<DepartmentViewModel>(department);
            return View(departmentViewModel);
        }
        [HttpPost]
        public IActionResult Update(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var department = mapper.Map<Department>(departmentViewModel);
                unitOfWork.DepartmentRepository.Update(department);
                return RedirectToAction("Index");
            }
            return View(departmentViewModel);
        }
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();
            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
                return NotFound();
            var departmentViewModel = mapper.Map<DepartmentViewModel>(department);
            return View(departmentViewModel);
        }
        public IActionResult Delete(int id)
        {
            if (id == null)
                return NotFound();
            var department = unitOfWork.DepartmentRepository.GetById(id);
            if (department is null)
                return NotFound();
            try
            {
                unitOfWork.DepartmentRepository.Delete(department);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
