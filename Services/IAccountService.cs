using BarcodeAPI.Models;

namespace BarcodeAPI.Services
{
    public interface IAccountService
    {
        TokenResponseModel GenerateJWT(LoginUserDto dto);

        void AddNewUser(AddUserBodyRequest body);
    }
}