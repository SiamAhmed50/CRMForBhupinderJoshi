using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
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
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetAsync("/api/Menu");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            AllMenus = JsonConvert.DeserializeObject<List<MenuModel>>(json);
        }
        else
        {
            AllMenus = new List<MenuModel>(); // Make sure it's not null
        }
    }


    public async Task<IActionResult> OnPostRegisterAsync([FromBody] RegisterUserDto model)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("/api/User/Register", model);

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        return new JsonResult(true);
    }

    public async Task<IActionResult> OnPostListAsync()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.GetAsync("/api/User");

        if (!response.IsSuccessStatusCode)
            return new JsonResult(new List<UserListDto>());

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserListDto>>(content);
        return new JsonResult(users);
    }
}
