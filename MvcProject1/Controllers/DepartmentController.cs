using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject1.BLL.UnitOfWork;
using MvcProject1.DAL.Models;
using MvcProject1.PL.ViewModels;
using System.Runtime.Intrinsics.Arm;

namespace MvcProject1.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            unitOfWork = UnitOfWork;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            IEnumerable<Department> departments = unitOfWork.DepartmentRepository.Value.GetAll();
            IEnumerable<DepartmentViewModel> departmentViewModel = mapper
                .Map<IEnumerable<Department>,
                IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentViewModel);
        }

        public IActionResult Create()
        {
            return View();


        }
        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {


                var dep = unitOfWork.DepartmentRepository.Value.Get(id.Value);
                if (dep != null)
                {
                    DepartmentViewModel departmentViewModel = mapper
                .Map<Department,
                DepartmentViewModel>(dep);
                    return View(departmentViewModel);
                }
                return NotFound();
            }
            return BadRequest();

        }
        public IActionResult Edit(int? id)
        {
            if (id.HasValue)
            {


                var dep = unitOfWork.DepartmentRepository.Value.Get(id.Value);
                if (dep != null)
                {
                    DepartmentViewModel departmentViewModel = mapper
                .Map<Department,
                DepartmentViewModel>(dep);
                    return View(departmentViewModel);
                }
                return NotFound();
            }
            return BadRequest();

        }
        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {


                var dep = unitOfWork.DepartmentRepository.Value.Get(id.Value);
                if (dep != null)
                {
                    DepartmentViewModel departmentViewModel = mapper.Map<Department,
                        DepartmentViewModel>(dep);
                    return View(departmentViewModel);
                }
                return NotFound();
            }
            return BadRequest();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel newDep)
        {
            if (ModelState.IsValid)
            {
                Department department = mapper.Map<
                DepartmentViewModel, Department>(newDep);
                unitOfWork.DepartmentRepository.Value.Add(department);
                unitOfWork.saveChange();
                return RedirectToAction("Index");
            }
            return View(newDep);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentViewModel Dep)
        {
            if (ModelState.IsValid)
            {
                Department department = mapper.Map<
                DepartmentViewModel, Department>(Dep);
                unitOfWork.DepartmentRepository.Value.Update(department);

                unitOfWork.saveChange();

                return RedirectToAction("Index");
            }
            return View(Dep);


        }

        public IActionResult DeleteAction(int id)
        {

            unitOfWork.DepartmentRepository.Value.Delete(id);

            unitOfWork.saveChange();

            return RedirectToAction("Index");




        }
    }
}
