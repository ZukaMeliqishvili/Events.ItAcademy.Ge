using Application.CustomExceptions;
using Application.Localization;
using Application.User.Models.Request;
using Application.User.Models.Response;
using Domain.User;
using Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;

namespace Application.User
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<List<UserResponseModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<ApplicationUser> users = await _unitOfWork.User.GetAllAsync(cancellationToken);
            return users.Adapt<List<UserResponseModel>>();
        }

        public async Task<UserResponseModel> GetAsync(CancellationToken cancellationToken,string userId)
        {
            ApplicationUser user = await this._unitOfWork.User.GetByIdAsync(cancellationToken, userId);
            if(user is null)
            {
                throw new UserNotFoundException(ErrorMessages.NoUserFound);
            }
            return user.Adapt<UserResponseModel>();
        }

        public async Task GiveModerator(string userId, CancellationToken cancellationToken)
        {
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(ErrorMessages.NoUserFound);
            var roles =await _userManager.GetRolesAsync(user);
            if(roles.Count>0)
            {
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }    
            await _userManager.AddToRoleAsync(user, "Moderator");
        }

        public async Task<string> LoginAsync(UserLoginModel model, CancellationToken cancellationToken)
        {
            SignInResult result = await this._signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (!result.Succeeded)
                throw new InvalidUserCredentialsException(ErrorMessages.InvalidLogin);
            IdentityUser user = await this._userManager.FindByNameAsync(model.UserName);
            string id = user.Id;
            return id;
        }

        public async Task RegisterAsync(UserRegisterModel model, CancellationToken cancellationToken)
        {
            bool isUnique = await _unitOfWork.User.Exists(cancellationToken,x=>x.UserName == model.UserName||x.Email==model.Email);
            if (isUnique)
                throw new InvalidUserCredentialsException(ErrorMessages.AlreadyUsed);
            var  applicationUser = new ApplicationUser();
            applicationUser.UserName = model.UserName;
            applicationUser.Email = model.Email;
            var  result = await this._userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
            {
                throw new InvalidUserCredentialsException(ErrorMessages.RegisterProblem);
            }
            else
            {
              await this._userManager.AddToRoleAsync(applicationUser, "User");
            }
        }
        public async Task<List<UserResponseModel>> GetNormalUsers(CancellationToken cancellationToken)
        {
           var users= await _userManager.GetUsersInRoleAsync("User");
            return users.Adapt<List<UserResponseModel>>();
        }
    }
}
