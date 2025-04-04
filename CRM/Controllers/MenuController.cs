using CRM.API.ViewModel;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using CRM.Service.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserMenuRepository _userMenuRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMenuService _menuService;

  

    public MenuController(
        IUnitOfWork unitOfWork,
        IUserMenuRepository userMenuRepo,
        UserManager<ApplicationUser> userManager,
        IMenuService menuService) // Inject here
    {
        _unitOfWork = unitOfWork;
        _userMenuRepo = userMenuRepo;
        _userManager = userManager;
        _menuService = menuService;
    }



   
    [HttpGet("usermenus")]
    public async Task<IActionResult> GetUserMenus()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var menus = await _menuService.GetMenusForCurrentUserAsync(userId);
        return Ok(menus);
    }

}
