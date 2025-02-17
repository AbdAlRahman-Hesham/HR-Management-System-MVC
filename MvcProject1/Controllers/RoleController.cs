using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MvcProject1.BLL.UnitOfWork;
using MvcProject1.DAL.Models;
using MvcProject1.PL.FileManagers;
using MvcProject1.PL.ViewModels;

namespace MvcProject1.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {

        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManger;

        public RoleController(IMapper mapper, RoleManager<IdentityRole> roleManger)
        {

            this._mapper = mapper;
            this._roleManger = roleManger;
        }

        #region Index
        public IActionResult Index(string searchInp)
        {
            if (!searchInp.IsNullOrEmpty())
            {
                IEnumerable<IdentityRole> users = _roleManger.Roles
                    .Where(r => r.Name.Contains(searchInp)).ToList();

                IEnumerable<RoleViewModel> roleViewModel
                    = _mapper.Map<IEnumerable<IdentityRole>,
                    IEnumerable<RoleViewModel>>(users);
                return View(roleViewModel);
            }
            IEnumerable<IdentityRole> Users = _roleManger.Roles.ToList();
            IEnumerable<RoleViewModel> RoleViewModel
                = _mapper.Map<IEnumerable<IdentityRole>,
                IEnumerable<RoleViewModel>>(Users);
            return View(RoleViewModel);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {

            return View();


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel newRole)
        {
            if (ModelState.IsValid)
            {
                
                var role = _mapper.Map<RoleViewModel, IdentityRole>(newRole);
                await _roleManger.CreateAsync(role);

                return RedirectToAction("Index");
            }
            return View(newRole);


        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string? id)
        {
            if (!id.IsNullOrEmpty())
            {


                var role = await _roleManger.FindByIdAsync(id);
                if (role != null)
                {
                    RoleViewModel RoleViewModel =
                        _mapper.Map<IdentityRole, RoleViewModel>(role);

                    return View(RoleViewModel);
                }
                return NotFound();
            }
            return BadRequest();

        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {

            if (!id.IsNullOrEmpty())
            {
                var role = await _roleManger.FindByIdAsync(id);
                if (role != null)
                {
                    RoleViewModel RoleViewModel =
                        _mapper.Map<IdentityRole, RoleViewModel>(role);

                    return View(RoleViewModel);
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _roleManger.FindByIdAsync(role.Id.ToString());

                if (existingUser == null)
                {
                    return NotFound();
                }

                _mapper.Map(role, existingUser);

                // Update the role
                var result = await _roleManger.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(role);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (!id.IsNullOrEmpty())
            {


                var role = await _roleManger.FindByIdAsync(id);
                if (role != null)
                {

                    RoleViewModel RoleViewModel =
                        _mapper.Map<IdentityRole, RoleViewModel>(role);

                    return View(RoleViewModel);
                }
                return NotFound();
            }
            return BadRequest();


        }



        public async Task<IActionResult> DeleteAction(string id)
        {
            var role = await _roleManger.FindByIdAsync(id);
            if (role != null)
            {


                await _roleManger.DeleteAsync(role);
            }

            return RedirectToAction("Index");

        }

        #endregion

    }
}
