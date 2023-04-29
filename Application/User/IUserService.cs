using Application.User.Models.Request;
using Application.User.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User
{
    public interface IUserService
    {
        Task<List<UserResponseModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserResponseModel> GetAsync(CancellationToken cancellationToken, string userId);
        Task<List<UserResponseModel>> GetNormalUsers(CancellationToken cancellationToken);
        Task GiveModerator(string userId, CancellationToken cancellationToken);
        Task<string> LoginAsync(UserLoginModel model, CancellationToken cancellationToken);
        Task RegisterAsync(UserRegisterModel model, CancellationToken cancellationToken);
    }
}
