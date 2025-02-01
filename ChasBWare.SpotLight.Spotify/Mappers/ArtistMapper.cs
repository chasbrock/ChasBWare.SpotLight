using ChasBWare.SpotLight.Domain.Entities;
using SpotifyAPI.Web;


namespace ChasBWare.SpotLight.Mappings.Mappers
{
    public static class ArtistMapper
    {
        public static Artist CopyToArtist(this FullArtist source)
        {
            return new Artist {
                Id = source.Id ?? string.Empty,
                Name = source.Name ?? string.Empty,
                Image = (source?.Images).GetMediumImage() };
        }
    }
}
