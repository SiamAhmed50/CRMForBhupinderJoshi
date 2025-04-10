using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using CRM.UI.ViewModel;

public class UsersModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public List<MenuModel> AllMenus { get; set; }
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

        // Step 1: Get users
        var response = await client.GetAsync("/api/Account/users");
        if (!response.IsSuccessStatusCode)
            return new JsonResult(new { data = new List<UserListDto>() });

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserListDto>>(content);

        // Step 2: Get all menus
        var menuResponse = await client.GetAsync("/api/Menu/menus");
        List<MenuModel> allMenus = new();
        if (menuResponse.IsSuccessStatusCode)
        {
            var menuJson = await menuResponse.Content.ReadAsStringAsync();
            allMenus = JsonConvert.DeserializeObject<List<MenuModel>>(menuJson);
        }

        // Step 3: Map MenuIds to MenuNames
        foreach (var user in users)
        {
            user.MenuNames = user.MenuIds != null
                ? allMenus
                    .Where(m => user.MenuIds.Contains(m.Id))
                    .Select(m => m.Name)
                    .ToList()
                : new List<string>();
        }

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
            MenuIds = form["MenuIds"].Select(int.Parse).ToList()
        };

    

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("https://localhost:44300/api/Account/register", userModel);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new JsonResult(new { success = false, message = error }) { StatusCode = 400 };
        }

        return new JsonResult(new { success = true });
    }
































    //[IgnoreAntiforgeryToken] // Important: no form token with raw JSON
    //public async Task<IActionResult> OnPostRegisterAsync([FromBody] RegisterUserDto userModel)
    //{
    //    // Your logic here


    //    var client = _httpClientFactory.CreateClient("ApiClient");
    //    userModel.UserName = userModel.Email; // Temporary mapping if 'Name' is used in the form

    //    var response = await client.PostAsJsonAsync("https://localhost:44300/api/Account/register", userModel);



    //    if (!response.IsSuccessStatusCode)
    //        return RedirectToPage(); // optionally add TempData["Error"]

    //    TempData["Success"] = "User registered successfully.";
    //    return RedirectToPage(); // reload page and show alert
    //}


}
