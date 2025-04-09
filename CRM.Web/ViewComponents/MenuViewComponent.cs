using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using CRM.Data.Entities;

public class MenuViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MenuViewComponent(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
        if (string.IsNullOrEmpty(token))
            return View(new List<Menus>());

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:44300/"); // API base URL
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("api/Menu/usermenus");

        if (!response.IsSuccessStatusCode)
            return View(new List<Menus>());

        var content = await response.Content.ReadAsStringAsync();
        var menus = JsonSerializer.Deserialize<List<Menus>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(menus ?? new List<Menus>());
    }
}
