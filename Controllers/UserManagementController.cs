using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task4.Converters;
using Task4.Services.Interfaces;

namespace Task4.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagement _userManagement;

        public UserManagementController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers(string? userEmail)
        {
            if (await _userManagement.IsUserBlocked(userEmail))
                return RedirectToAction("Login", "Account");

            return View(await _userManagement.GetUsers());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> HandleUserManageActions(string action, List<string> selectedUserEmails, string userEmail)
        {
            await _userManagement.HandleUserManageActions(action.ToUserManageActions(), selectedUserEmails);

            return RedirectToAction("GetUsers", new { userEmail = userEmail });
        }
    }
}
