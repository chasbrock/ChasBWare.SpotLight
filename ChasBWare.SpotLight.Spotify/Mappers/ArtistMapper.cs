using ChasBWare.SpotLight.Domain.Entities;
using SpotifyAPI.Web;


namespace ChasBWare.SpotLight.Mappings.Mappers
{
    public static class ArtistMapper
    {
        public static void CopyToArtist(this FullArtist source, Artist target)
        {
            target.Id = source.Id ?? string.Empty;
            target.Name = source.Name ?? string.Empty;
            target.Image = (source?.Images).GetMediumImage();
        }
    }
}
