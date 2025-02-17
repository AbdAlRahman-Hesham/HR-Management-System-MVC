using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcProject1.BLL.UnitOfWork;
using MvcProject1.DAL.Models;
using MvcProject1.PL.ViewModels;
using MvcProject1.PL.FileManagers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
namespace MvcProject1.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork UnitOfWork, IMapper mapper)
        {

            unitOfWork = UnitOfWork;
            this.mapper = mapper;
        }

        #region Index
        public IActionResult Index(string searchInp)
        {
            if (!searchInp.IsNullOrEmpty())
            {
                IEnumerable<Employee> Emps =
                    unitOfWork.EmployeeRepository.Value
                    .Find(e =>e.FirstName.ToLower().Contains(searchInp.ToLower())
                                || e.LastName.ToLower().Contains(searchInp.ToLower())
                    );

                IEnumerable<EmployeeViewModel> EmpViewModel
                    = mapper.Map<IEnumerable<Employee>,
                    IEnumerable<EmployeeViewModel>>(Emps);
                return View(EmpViewModel);
            }
            IEnumerable<Employee> Employees = unitOfWork.EmployeeRepository.Value.GetAll();
            IEnumerable<EmployeeViewModel> EmployeeViewModel
                = mapper.Map<IEnumerable<Employee>,
                IEnumerable<EmployeeViewModel>>(Employees);
            return View(EmployeeViewModel);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewData["Department"] = unitOfWork.DepartmentRepository.Value.GetAll();

            return View();


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel newEmp)
        {
            ViewData["Department"] = unitOfWork.DepartmentRepository.Value.GetAll();
            if (ModelState.IsValid)
            {
                if (newEmp.Image != null)
                {

                    newEmp.ImgUrl = FileManager.
                        UploadFile(newEmp.Image, newEmp.Id);
                }
                Employee employeeModel =
                        mapper.Map<EmployeeViewModel, Employee>(newEmp);

                unitOfWork.EmployeeRepository.Value.Add(employeeModel);
                unitOfWork.saveChange();

                return RedirectToAction("Index");
            }
            return View(newEmp);


        }
        #endregion

        #region Details
        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {


                var Emp = unitOfWork.EmployeeRepository.Value.Get(id.Value);
                if (Emp != null)
                {
                    EmployeeViewModel employeeViewModel =
                        mapper.Map<Employee, EmployeeViewModel>(Emp);

                    return View(employeeViewModel);
                }
                return NotFound();
            }
            return BadRequest();

        }
        #endregion

        #region Edit
        public IActionResult Edit(int? id)
        {

            if (id.HasValue)
            {

                ViewData["Department"] = unitOfWork.DepartmentRepository.Value.GetAll();

                var Emp = unitOfWork.EmployeeRepository.Value.Get(id.Value);
                if (Emp != null)
                {
                    EmployeeViewModel employeeViewModel =
                        mapper.Map<Employee, EmployeeViewModel>(Emp);

                    return View(employeeViewModel);
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel Emp)
        {
            ViewData["Department"] = unitOfWork.DepartmentRepository.Value.GetAll();
            if (ModelState.IsValid)
            {
                if (Emp.Image != null)
                {
                    if (Emp.ImgUrl != null)
                    {
                        FileManager.DeleteFile(Emp.ImgUrl);
                    }
                    Emp.ImgUrl = FileManager.
                        UploadFile(Emp.Image, Emp.Id);
                }
                Employee employeeModel =
                       mapper.Map<EmployeeViewModel, Employee>(Emp);
                unitOfWork.EmployeeRepository.Value.Update(employeeModel);

                unitOfWork.saveChange();
                return RedirectToAction("Index");
            }
            return View(Emp);


        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {


                var Emp = unitOfWork.EmployeeRepository.Value.Get(id.Value);
                if (Emp != null)
                {

                    EmployeeViewModel employeeViewModel =
                        mapper.Map<Employee, EmployeeViewModel>(Emp);

                    return View(employeeViewModel);
                }
                return NotFound();
            }
            return BadRequest();


        }



        public IActionResult DeleteAction(int id)
        {
            var Emp = unitOfWork.EmployeeRepository.Value.Get(id);
            if (Emp != null)
            {
                if (Emp.ImgUrl != null)
                {
                    FileManager.DeleteFile(Emp.ImgUrl);
                }

                EmployeeViewModel employeeViewModel =
                    mapper.Map<Employee, EmployeeViewModel>(Emp);

                unitOfWork.EmployeeRepository.Value.Delete(id);
            }


            unitOfWork.saveChange();

            return RedirectToAction("Index");




        }

        #endregion

    }
}
