using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using CRM.UI.ViewModel;

public class UsersModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<MenuModel> AllMenus { get; set; }

    public UsersModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        AllMenus = new List<MenuModel>();

        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/Menu/menus");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                AllMenus = JsonConvert.DeserializeObject<List<MenuModel>>(json);
            }
        }
        catch
        {
            // Handle or log error
        }
    }

    public async Task<IActionResult> OnGetListAsync()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetAsync("/api/Account/users");

        if (!response.IsSuccessStatusCode)
            return new JsonResult(new { data = new List<UserListDto>() });

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserListDto>>(content);

        // Return wrapped for DataTables
        return new JsonResult(new { data = users });
    }

    public async Task<IActionResult> OnPostRegisterAsync([FromBody] RegisterUserDto model)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("/api/Account/register", model);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        return new JsonResult(true);
    }
}
