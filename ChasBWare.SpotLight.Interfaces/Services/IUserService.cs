using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Services
{
    public interface IUserService
    {
        User? CurrentUser { get; }

        /// <summary>
        /// we need access to current user to provide country code.
        /// if it is in db then use that otherwise get from api
        /// </summary>
        /// <returns></returns>
        Task<User> LoadCurrentUser();

    }
}
