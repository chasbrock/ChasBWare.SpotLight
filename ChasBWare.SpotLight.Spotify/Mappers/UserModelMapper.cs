using ChasBWare.SpotLight.Domain.Entities;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers
{
    public static class UserModelMapper
    {

        public static User CopyPrivateUser(this PrivateUser source)
        {
            return new User
            {
                Id = source.Id,
                Country = source.Country,
                Name = source.DisplayName,
                Uri = source.Uri
            };
        }
    }
}
