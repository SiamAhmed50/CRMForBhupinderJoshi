using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.UI.ViewModel;
using System.Linq;
using CRM.Data.Entities;
using CRM.Web.Pages;

public class UsersModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<MenuModel> AllMenus { get; set; }
    public List<Client> AllClients { get; set; }    // <-- new
    [BindProperty]
    public RegisterUserDto UserModel { get; set; }

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

                // fetch clients…
                var clientsResponse = await client.GetAsync("/api/Clients");
                AllClients = JsonConvert.DeserializeObject<List<Client>>(await clientsResponse.Content.ReadAsStringAsync());

            }
        }
        catch
        {
            // Log error if needed
        }
    }

    public async Task<IActionResult> OnGetListAsync()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        // 1) Fetch users
        var response = await client.GetAsync("/api/Account/users");
        if (!response.IsSuccessStatusCode)
            return new JsonResult(new { data = new List<UserListDto>() });

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserListDto>>(content);

        // 2) Fetch all menus
        var menuResponse = await client.GetAsync("/api/Menu/menus");
        var allMenus = new List<MenuModel>();
        if (menuResponse.IsSuccessStatusCode)
        {
            var menuJson = await menuResponse.Content.ReadAsStringAsync();
            allMenus = JsonConvert.DeserializeObject<List<MenuModel>>(menuJson);
        }

        // 3) Fetch all clients
        var clientResponse = await client.GetAsync("/api/Clients");
        var allClients = new List<ClientModel>();
        if (clientResponse.IsSuccessStatusCode)
        {
            var clientJson = await clientResponse.Content.ReadAsStringAsync();
            allClients = JsonConvert.DeserializeObject<List<ClientModel>>(clientJson);
        }

        // 4) Map names
        foreach (var user in users)
        {
            user.MenuNames = user.MenuIds != null
                ? allMenus
                    .Where(m => user.MenuIds.Contains(m.Id))
                    .Select(m => m.Name)
                    .ToList()
                : new List<string>();

            user.ClientNames = user.ClientIds != null
                ? allClients
                    .Where(c => user.ClientIds.Contains(c.Id))
                    .Select(c => c.Name)
                    .ToList()
                : new List<string>();
        }

        // 5) Return both menu & client names in the payload
        return new JsonResult(new { data = users });
    }


    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> OnPostRegisterAsync()
    {
        var form = await Request.ReadFormAsync();

        var userModel = new RegisterUserDto
        {
            UserName = form["Name"],
            Email = form["Email"],
            Password = form["Password"],
            ConfirmPassword = form["ConfirmPassword"],
            MenuIds = form["MenuIds"].Select(int.Parse).ToList(),
            ClientIds = form["ClientIds"].Select(int.Parse).ToList()
        };

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("/api/Account/register", userModel);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new JsonResult(new { success = false, message = error }) { StatusCode = 400 };
        }

        return new JsonResult(new { success = true });
    }
    public async Task<IActionResult> OnPostDeleteAsync([FromForm] string email)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.DeleteAsync($"/api/Account/delete/{email}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new JsonResult(new { success = false, message = error });
        }

        return new JsonResult(new { success = true });
    }
}
