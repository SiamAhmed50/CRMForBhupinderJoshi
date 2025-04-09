// MenuService.cs
using System.Security.Claims;
using CRM.Data.Entities;
using CRM.Service.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

public class MenuService : IMenuService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserMenuRepository _userMenuRepo;

    public MenuService(IHttpContextAccessor httpContextAccessor, IUserMenuRepository userMenuRepo)
    {
        _httpContextAccessor = httpContextAccessor;
        _userMenuRepo = userMenuRepo;
    }

    public async Task<List<Menus>> GetMenusForCurrentUserAsync(string userId)
    {
        var userMenus = await _userMenuRepo.GetMenusByUserIdAsync(userId);
        return userMenus.Select(m => new Menus
        {
            Id = m.Id,
            Name = m.Name,
            Url = m.Url
        }).ToList();
    }


    public async Task<List<Menus>> GetAllMenusAsync()
    {
        var userMenus = await _userMenuRepo.GetMenusAsync();
        return userMenus.Select(m => new Menus
        {
            Id = m.Id,
            Name = m.Name,
            Url = m.Url
        }).ToList();
    }


}
