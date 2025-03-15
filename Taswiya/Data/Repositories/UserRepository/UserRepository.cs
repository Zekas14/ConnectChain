using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Authentication;
using ConnectChain.ViewModel.Authentication.ForgetPassword;
using ConnectChain.ViewModel.Authentication.ResetPassword;
using ConnectChain.ViewModel.Authentication.SignIn;

namespace ConnectChain.Data.Repositories.UserRepository
{
    public class UserRepository(UserManager<User> userManager,IMemoryCache cache,IMailServices mailServices) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMemoryCache _cache = cache;
        private readonly IMailServices _mailServices= mailServices;
        #region Register
        public async Task<RequestResult<bool>> Register(UserRegisterRequestViewModel viewModel,Func<string, string> generateUrl)
        {
            var user = new User
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                UserName = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber,
                Address = viewModel.Address,
                Country= viewModel.Country,
            };
            
            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
              //  await userManager.AddToRoleAsync(user,"user");
                var requestResult = await SendConfirmationEmail(user.Id, generateUrl);
                if(!requestResult.isSuccess)
                {
                    return RequestResult<bool>.Failure(requestResult.errorCode, requestResult.message);
                }
                return RequestResult<bool>.Success(requestResult.data,requestResult.message);
            }
            return RequestResult<bool>.Failure(ErrorCode.BadRequest,string.Join(", ",result.Errors.Select(e => e.Description)));

        }
        #endregion

        #region Confirm Email
        public async Task<RequestResult<bool>> ConfirmEmail(string userId)
        {
            if (userId == null )
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Invalid email confirmation request.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found.");
            }
            user.EmailConfirmed= true;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RequestResult<bool>.Success(true, "Email confirmed successfully. You can now log in.");
            }
            return RequestResult<bool>.Failure(ErrorCode.InvalidInput, $"{string.Join(", ", result.Errors)}");

        }
        #endregion

        #region  Send Confirmation Email
        public async  Task<RequestResult<bool>> SendConfirmationEmail(string email,Func<string,string> generateUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Found");
            }
            var callBackUrl = generateUrl( user.Id);
            var emailBody = $"<h1>Dear {user.UserName}! Welcome To ConnectChain.</h1><p>Please <a href='{callBackUrl}'>Click Here</a> To Confirm Your Email.</p>";
            await _mailServices.SendEmailAsync(user.Email, "Email Confirmation", emailBody);
            return RequestResult<bool>.Success(true, "Email Confirmation sent , Please Verify your Email");
        }
        #endregion

        #region Forget Password
        public async Task<RequestResult<bool>> ForgetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found.");
            }
            var otp = GenerateOtp();
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var requestResult = await SendOtp(new SendOtpRequestViewModel { Email = email, OTP = otp, ResetToken = resetToken });
            return requestResult;
        }
        #endregion
        
        #region Send Otp
        private static string GenerateOtp()
        {
            return new Random().Next(1000, 10000).ToString();
        }
        public async Task<RequestResult<bool>> SendOtp(SendOtpRequestViewModel viewModel)
        {
            _cache.Set($"otp", viewModel.OTP, TimeSpan.FromMinutes(15));
            _cache.Set($"resetToken", viewModel.ResetToken, TimeSpan.FromMinutes(10));
            await mailServices.SendEmailAsync(viewModel.Email, "Otp Verification", $" Your Verification OTP Code is : {viewModel.OTP} ");
            return RequestResult<bool>.Success(true, "Otp Sent Successfully");
        }
        #endregion
        
        #region Verify OTP
        public async Task<RequestResult<bool>> VerifyOtp(VerifyRequestOtpViewModel viewModel)
        {
            string validateToken = null;
            _cache.Set($"validateToken", validateToken, TimeSpan.FromMinutes(15));
            var user = await userManager.FindByEmailAsync(viewModel.Email);
            if (user is not null)
            {
                if (_cache.TryGetValue("otp", out string validOtp))
                {
                    if (viewModel.OTP== validOtp)
                    {
                        validateToken = _cache.Get<string>("resetToken")!;
                        _cache.Set($"validateToken", validateToken, TimeSpan.FromMinutes(15));
                        return RequestResult<bool>.Success(true, "OTP Verified Successfully");
                    }
                    return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Invalid OTP");
                }
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "OTP  Timed Out or Not Found ");
            }
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Found");
        }
        #endregion
        
        #region ResetPassword
        public async Task<RequestResult<bool>> ResetPassword(ResetPasswordRequestViewModel viewModel)
        {
            var user = await userManager.FindByEmailAsync(viewModel.Email);
            if (user is not null)
            {
                if (!_cache.TryGetValue("validateToken", out string validateToken) || validateToken == null)
                {
                    return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Otp Not Verified ");
                }
                var resetToken = _cache.Get<string>("resetToken");
                if (resetToken == validateToken)
                {
                    var result = await userManager.ResetPasswordAsync(user, resetToken, viewModel.Password);
                    if (result.Succeeded)
                    {
                        return RequestResult<bool>.Success(true, "Password Reset Successfully");
                    }

                }
            }
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "User Not Found");
        }
        #endregion
        
        #region Sign In
        public async Task<RequestResult<UserSignInResponseViewModel>> SignIn(UserSignInRequestViewModel viewModel)
        {
            var currentUser = await userManager.FindByEmailAsync(viewModel.Email);
            if (currentUser is null)
            {
                return RequestResult<UserSignInResponseViewModel>.Failure(ErrorCode.NotFound, "User Not Found");
            }
            if (!currentUser.EmailConfirmed)
            {
                return RequestResult<UserSignInResponseViewModel>.Failure(ErrorCode.InvalidInput, "Email not confirmed. Please check your email inbox to verify your email address.");
            }
            bool found = await userManager.CheckPasswordAsync(currentUser, viewModel.Password);
            if (!found)
            {
                return RequestResult<UserSignInResponseViewModel>.Failure(ErrorCode.InvalidInput, "Invalid Email or Password");
            }
            var token = await userManager.CreateTokenAsync(currentUser);
            string tokenData = new JwtSecurityTokenHandler().WriteToken(token);
            UserSignInResponseViewModel user = new()
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,

                Email = currentUser.Email,
                Address = currentUser.Address,
                Token = tokenData,
                Phone = currentUser.PhoneNumber,
            };
            return RequestResult<UserSignInResponseViewModel>.Success(user, "User Logged In Successfully");
        }
        #endregion
        
        #region Repository Functions

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void SaveInclude(User entity, params string[] properties)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void HardDelete(User entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAllWithDeleted()
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public User GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAysnc()
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetByPage(PaginationHelper paginationParams)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAllWithIncludes(Func<IQueryable<User>, IQueryable<User>> includeExpression)
        {
            throw new NotImplementedException();
        }

        public User GetByIDWithIncludes(int id, Func<IQueryable<User>, IQueryable<User>> includeExpression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
