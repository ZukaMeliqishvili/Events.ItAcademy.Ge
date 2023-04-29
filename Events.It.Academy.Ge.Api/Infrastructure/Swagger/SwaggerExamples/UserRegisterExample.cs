using Application.User.Models.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Events.It.Academy.Ge.Api.Infrastructure.Swagger.SwaggerExamples
{
    public class UserRegisterExample : IExamplesProvider<UserRegisterModel>
    {
        public UserRegisterModel GetExamples()
        {
            return new UserRegisterModel
            {
                UserName = "Terminat2",
                Email = "machine@gmail.com",
                Password = "Password1!"
            };
        }
    }
}
