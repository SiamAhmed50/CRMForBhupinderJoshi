﻿namespace CRM.API.ViewModels
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> MenuIds { get; set; }
    }

}
