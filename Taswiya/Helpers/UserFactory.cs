using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Authentication;
namespace ConnectChain.Helpers
{
    public static class UserFactory
    {
        public static User CreateUser(UserRegisterRequestViewModel viewModel)
        {
            return viewModel.Role switch
            {
                Role.Supplier => new Supplier
                {
                    Name= viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = viewModel.Address,
                    BusinessType = viewModel.BusinessType
                },
                Role.Customer => new Customer
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = viewModel.Address,
                    BusinessType = viewModel.BusinessType
                },
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel.Role), "Unsupported Role")
            };
        }
    }

}
