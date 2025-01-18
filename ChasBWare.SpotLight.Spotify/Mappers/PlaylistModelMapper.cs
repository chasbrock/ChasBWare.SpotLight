using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;


namespace ChasBWare.SpotLight.Mappings.Mappers
{

    public static class PlaylistModelMapper
    {
        public static Playlist? CopyToPlaylist(this FullPlaylist source)
        {
            if (source.Id == null)
                return null;
            return new Playlist
            {
                Id = source.Id,
                Description = source.Description ?? string.Empty,
                Name = source.Name ?? string.Empty,
                Owner = source.Owner?.DisplayName ?? string.Empty,
                PlaylistType = PlaylistType.Playlist,
                Uri = source.Uri ?? string.Empty,
                Image = source.Images.GetMediumImage()
            };
        }

        public static Playlist? CopyToPlaylist(this SavedAlbum source)
        {
            if (source.Album == null)
                return null;

            var owners = source.Album.Artists?.Select(a => a.Name);
            return new Playlist
            {
                Id = source.Album.Id,
                Description = source.Album.Name,
                Name = source.Album.Name,
                Owner = owners != null ? string.Join(',', owners) : string.Empty,
                PlaylistType = PlaylistType.Album,
                Uri = source.Album.Uri,
                ReleaseDate = source.Album.ReleaseDate.ConvertReleaseDate(),
                Image = source.Album.Images.GetMediumImage()
            };
        }

        public static Playlist CopyToPlaylist(this SimpleAlbum source)
        {
            var owners = source.Artists?.Select(a => a.Name);
            return new Playlist
            {
                Id = source.Id,
                Description = string.Empty,
                Name = source.Name ?? string.Empty,
                Owner = owners != null ? string.Join(',', owners) : string.Empty,
                PlaylistType = PlaylistType.Album,
                Uri = source.Uri ?? string.Empty,
                ReleaseDate = source.ReleaseDate.ConvertReleaseDate(),
                Image = source.Images.GetMediumImage()
            };
        }
    }
}
