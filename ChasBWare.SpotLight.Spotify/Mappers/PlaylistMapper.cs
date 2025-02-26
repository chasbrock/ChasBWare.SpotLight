using System.Text;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;

namespace ChasBWare.SpotLight.Mappings.Mappers;

public static class PlaylistMapper
{
    public static Playlist? CopyToPlaylist(this FullPlaylist source)
    {
        if (source.Id == null)
        {
            return null;
        }

        return new Playlist
        {
            Id = source.Id,
            Description = source.Description!.Trim() ?? string.Empty,
            Name = source.Name!.Trim() ?? string.Empty,
            Owner = source.Owner!.DisplayName ?? string.Empty,
            PlaylistType = PlaylistType.Playlist,
            Uri = source.Uri ?? string.Empty,
            Image = source.Images.GetMediumImage(),
            LastAccessed = DateTime.Now
        };
    }

    public static Playlist? CopyToPlaylist(this SavedAlbum source)
    {
        if (source.Album == null)
        {
            return null;
        }

        return new Playlist
        {
            Id = source.Album.Id,
            Description = source.Album.Name!.Trim(),
            Name = source.Album.Name!.Trim(),
            Owner = source.Album.Artists.PackOwner(),
            PlaylistType = PlaylistType.Album,
            Uri = source.Album.Uri,
            ReleaseDate = source.Album.ReleaseDate.ConvertReleaseDate(),
            Image = source.Album.Images.GetMediumImage(),
            LastAccessed = DateTime.Now
        };
    }

    public static Playlist CopyToPlaylist(this FullAlbum source)
    {

        return new Playlist
        {
            Id = source.Id,
            Description = source.Name!.Trim(),
            Name = source.Name!.Trim(),
            Owner = source.Artists.PackOwner(),
            PlaylistType = PlaylistType.Album,
            Uri = source.Uri,
            ReleaseDate = source.ReleaseDate.ConvertReleaseDate(),
            Image = source.Images.GetMediumImage(),
            LastAccessed = DateTime.Now
        };
    }

    public static Playlist CopyToPlaylist(this SimpleAlbum source)
    {
        return new Playlist
        {
            Id = source.Id,
            Description = string.Empty,
            Name = source.Name!.Trim() ?? string.Empty,
            Owner = source.Artists.PackOwner(),
            PlaylistType = PlaylistType.Album,
            Uri = source.Uri ?? string.Empty,
            ReleaseDate = source.ReleaseDate.ConvertReleaseDate(),
            Image = source.Images.GetMediumImage(),
            LastAccessed = DateTime.Now
        };
    }

 

}
