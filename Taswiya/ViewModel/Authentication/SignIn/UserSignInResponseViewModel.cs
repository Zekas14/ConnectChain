﻿namespace ConnectChain.ViewModel.Authentication.SignIn
{
    public class UserSignInResponseViewModel
    {
        public string? FirstName{ get; set; }
        public string? LastName{ get; set; }
        public string? Email { get; set; }
        public string? Address{ get; set; }
        public string? Token { get; set; }
        public string? Phone { get; internal set; }
    }
}
