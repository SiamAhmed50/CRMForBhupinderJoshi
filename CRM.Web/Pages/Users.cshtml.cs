using CRM.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Polly;

namespace CRM.UI.Pages
{
    public class UsersModel : PageModel
    {
       

        private readonly UserManager<ApplicationUser> _userManager;

        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public UserModel UserModel { get; set; }
        public List<UserModel> Users { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var users = await _userManager.Users.ToListAsync(); // Fetch users first
            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"; 
                userModels.Add(new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = role
                });
            }

            Users = userModels;
            return Page();
        }

        public async Task<JsonResult> OnPostList()
        {

            var users = await _userManager.Users.ToListAsync(); 
            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User"; 
                userModels.Add(new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = role
                });
            }

            return new JsonResult(userModels);
        }

        public async Task<IActionResult> OnPost()
        {
            
            var user = new ApplicationUser
            {
                NormalizedUserName=UserModel.FullName,
                UserName = UserModel.UserName,
                Email = UserModel.Email
            };

            var result = await _userManager.CreateAsync(user, UserModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserModel.Role);
                return RedirectToPage();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description); // Add model errors if creation fails
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return new JsonResult(new { success = true });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, errors = result.Errors.Select(e => e.Description) });
                    }
                }
                return new JsonResult(new { success = false, message = "User not found." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> OnGetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new JsonResult(new { success = false, message = "Invalid user ID" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "User not found" });
            }

            return new JsonResult(new { success = true, data = user });
        }


        public async Task<IActionResult> OnPostUpdate()
        {
            var existingUser = await _userManager.FindByIdAsync(UserModel.Id);
            if (existingUser == null)
            {
                return new JsonResult(new { success = false, message = "User not found." });
            }

            existingUser.UserName = UserModel.UserName;
            existingUser.Email = UserModel.Email;


            if (!string.IsNullOrEmpty(UserModel.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, UserModel.Password);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description); // Add errors if password update fails
                    }
                    return new JsonResult(new { success = false, message = "Failed to update password." });
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            if (currentRoles.FirstOrDefault() != UserModel.Role)
            {
                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                await _userManager.AddToRoleAsync(existingUser, UserModel.Role);
            }


            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToPage();
            }

            return RedirectToPage();
        }

    }


    public class UserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public List<string> AvailableRoles { get; set; }
    }
}



//public async Task<IActionResult> OnPostDelete(string id)
//{

//    var user = await _userManager.FindByIdAsync(id);
//    if (user != null)
//    {
//        await _userManager.DeleteAsync(user);
//    }
//    return new JsonResult(new { success = true });
//}



// $('#userTable tbody').on('click', 'span.delete-user', function () {

//     var id = $(this).data('id');
//     Swal.fire({
//         title: "Are you sure?",
//         text: "You won't be able to revert this!",
//         icon: "warning",
//         showCancelButton: true,
//         confirmButtonColor: "#3085d6",
//         cancelButtonColor: "#d33",
//         confirmButtonText: "Yes, delete it!"
//     }).then((result) => {
//         if (result.isConfirmed) {
//             $.ajax({
//                 url: `/Users?handler=Delete&id=${id}`, 
//                 type: 'POST',
//                 beforeSend: function (xhr) {
//                     xhr.setRequestHeader('XSRF-TOKEN', $('input:hidden[name="__RequestVerificationToken"]').val());
//                 },
//                 success: function (response) {
//                     table.ajax.reload();
//                     Swal.fire({
//                         title: 'Deleted!',
//                         text: 'The user has been deleted.',
//                         icon: 'success'
//                     });
//                 },
//                 error: function (error) {
//                     Swal.fire({
//                         title: 'Error!',
//                         text: 'An error occurred while deleting the client.',
//                         icon: 'error'
//                     });
//                 }
//             });
//         }
//     });
// });