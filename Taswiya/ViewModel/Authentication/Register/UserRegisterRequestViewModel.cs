using ConnectChain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConnectChain.ViewModel.Authentication
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserRegisterRequestViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string? Name { get; set;  }
        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$",ErrorMessage = "Invalid Phone Number")]
        public string? PhoneNumber { get;set; }
        [Required]
        [EmailAddress]
        
        public string? Email { get; set; }
        [JsonIgnore]
        public string? FcmToken { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MinLength(3)]
        public string? BusinessType { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MinLength(3)]
        public string? Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password Not Matched")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [AllowedValues(new object[] { Role.Customer, Role.Supplier },ErrorMessage =" Role Should be Supplier Or Customer")]
        public Role Role { get; set; }

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
