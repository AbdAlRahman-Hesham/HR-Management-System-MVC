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
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManger;

        public UserController(IMapper mapper,UserManager<AppUser> userManger)
        {

            this._mapper = mapper;
            this._userManger = userManger;
        }

        #region Index
        public IActionResult Index(string searchInp)
        {
            if (!searchInp.IsNullOrEmpty())
            {
                IEnumerable<AppUser> users =  _userManger.Users
                    .Where(u=>u.UserName.Contains(searchInp)).ToList();

                IEnumerable<UserViewModel> userViewModel
                    = _mapper.Map<IEnumerable<AppUser>,
                    IEnumerable<UserViewModel>>(users);
                return View(userViewModel);
            }
            IEnumerable<AppUser> Users = _userManger.Users.ToList();
            IEnumerable<UserViewModel> UserViewModel
                = _mapper.Map<IEnumerable<AppUser>,
                IEnumerable<UserViewModel>>(Users);
            return View(UserViewModel);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(string? id)
        {
            if (!id.IsNullOrEmpty())
            {


                var user = await _userManger.FindByIdAsync(id);
                if (user != null)
                {
                    UserViewModel UserViewModel =
                        _mapper.Map<AppUser, UserViewModel>(user);
                    UserViewModel.UserRolesNames = await _userManger
                        .GetRolesAsync(user);

                    return View(UserViewModel);
                }
                return NotFound();
            }
            return BadRequest();

        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _userManger.FindByIdAsync(id);
                if (user != null)
                {
                    var userViewModel = _mapper.Map<AppUser, UserViewModel>(user);
                    userViewModel.UserRolesNames = await _userManger.GetRolesAsync(user);
                    return View(userViewModel);
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManger.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                _mapper.Map(user, existingUser);
                var result = await _userManger.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    // Update roles
                    var currentRoles = await _userManger.GetRolesAsync(existingUser);
                    var rolesToAdd = user.SelectedRoles.Except(currentRoles).ToList();
                    var rolesToRemove = currentRoles.Except(user.SelectedRoles).ToList();

                    foreach (var role in rolesToAdd)
                    {
                        await _userManger.AddToRoleAsync(existingUser, role);
                    }

                    foreach (var role in rolesToRemove)
                    {
                        await _userManger.RemoveFromRoleAsync(existingUser, role);
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(user);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (!id.IsNullOrEmpty())
            {


                var user = await _userManger.FindByIdAsync(id);
                if (user != null)
                {

                    UserViewModel UserViewModel =
                        _mapper.Map<AppUser, UserViewModel>(user);

                    return View(UserViewModel);
                }
                return NotFound();
            }
            return BadRequest();


        }



        public async Task<IActionResult> DeleteAction(string id)
        {
            var user = await _userManger.FindByIdAsync(id);
            if (user != null)
            {
           

                await _userManger.DeleteAsync(user);
            }

            return RedirectToAction("Index");

        }

        #endregion

    }
}
