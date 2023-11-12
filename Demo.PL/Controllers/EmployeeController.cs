using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IActionResult Index(string SearchValue = "" , int SearchByDepartment = 0)
        {
            List<Employee> employees;
            List<EmployeeViewModel> employeesViewModel;
            ViewBag.Departments = unitOfWork.DepartmentRepository.GetAll();
            if(string.IsNullOrEmpty(SearchValue) && SearchByDepartment == 0)
            {
                employees = unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                if (SearchByDepartment == 0)
                    employees = unitOfWork.EmployeeRepository.SearchByName(SearchValue);
                else
                    employees = unitOfWork.EmployeeRepository.GetEmployeesByDepatmentId(SearchByDepartment);
            }
            employeesViewModel = mapper.Map<List<EmployeeViewModel>>(employees);
            return View(employeesViewModel);
        }
        public IActionResult Create()
        {
            List<Department> departments = unitOfWork.DepartmentRepository.GetAll();
            ViewBag.departments = mapper.Map<List<DepartmentViewModel>>(departments);
            return View(new EmployeeViewModel());
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employee = mapper.Map<Employee>(employeeViewModel);
                employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                unitOfWork.EmployeeRepository.Add(employee);
                return RedirectToAction("Index");
            }
            return View(employeeViewModel);
        }
        public IActionResult Update(int id)
        {
            if (id == null)
                return NotFound();
            var employee = unitOfWork.EmployeeRepository.GetById(id);
            TempData["ImageUrl"] = employee.ImageUrl;
            if (employee is null)
                return NotFound();
            List<Department> departments = unitOfWork.DepartmentRepository.GetAll();
            ViewBag.departments = mapper.Map<List<DepartmentViewModel>>(departments);
            var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
            return View(employeeViewModel);
        }
        [HttpPost]
        public IActionResult Update(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                DocumentSettings.DeleteFile((string)TempData["ImageUrl"], "Images");
                var employee = mapper.Map<Employee>(employeeViewModel);
                employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                unitOfWork.EmployeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View(employeeViewModel);
        } 
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();
            var employee = unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null)
                return NotFound();
            ViewBag.DeptName = unitOfWork.DepartmentRepository.GetById(employee.DepartmentId).Name;
            var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
            return View(employeeViewModel);
        }
        public IActionResult Delete(int id)
        {
            if (id == null)
                return NotFound();
            var employee = unitOfWork.EmployeeRepository.GetById(id);
            if (employee is null)
                return NotFound();
            try
            {
                DocumentSettings.DeleteFile(employee.ImageUrl, "Images");
                unitOfWork.EmployeeRepository.Delete(employee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
