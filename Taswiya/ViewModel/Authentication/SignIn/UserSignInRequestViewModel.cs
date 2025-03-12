﻿namespace ConnectChain.ViewModel.Authentication.SignIn
{
    public class UserSignInRequestViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    /* public class RegisterUserRequestViewModelValidator : AbstractValidator<UserRegisterRequestViewModel>
     {
         public RegisterUserRequestViewModelValidator()
         {
             RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required.")
                 .EmailAddress().WithMessage("Please provide a valid email address.")
                 .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
                 .WithMessage("Please enter a correctly formatted email address.");

             RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("Password is required.")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                 .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                 .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                 .Matches(@"\d").WithMessage("Password must contain at least one number.")
                 .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).")
                 .Must(email => !email.Contains("gamil.com")).WithMessage("Did you mean 'gmail.com'? Please check your email.");

             RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("Name is required.");
             RuleFor(x => x.LastName)
                 .NotEmpty().WithMessage("Name is required.");

             RuleFor(x => x.PhoneNumber)
                 .NotEmpty().WithMessage("Phone number is required.")
                 .Matches(@"^\+?\d{10,15}$").WithMessage("Please provide a valid phone number.");

         }
     }*/
}
