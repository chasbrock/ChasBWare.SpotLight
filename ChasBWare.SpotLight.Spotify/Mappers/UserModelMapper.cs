using ChasBWare.SpotLight.Domain.Entities;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers;

public static class UserModelMapper
{
    public static User CopyToUser(this PrivateUser source)
    {
        return new User
        {
            Id = source.Id,
            Country = source.Country,
            Name = source.DisplayName,
            Image = source.Images.GetMediumImage(),
            Uri = source.Uri
        };
    }

    public static User CopyToUser(this PublicUser source)
    {
        return new User
        {
            Id = source.Id,
            Name = source.DisplayName,
            Image = source.Images.GetMediumImage(),
            Uri = source.Uri
        };
    }
}
