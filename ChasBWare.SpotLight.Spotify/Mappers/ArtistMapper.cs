using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Domain.Enums;
using SpotifyAPI.Web;


namespace ChasBWare.SpotLight.Mappings.Mappers;

public static class ArtistMapper
{
    public static Artist CopyToArtist(this FullArtist source)
    {
        return new Artist {
            Id = source.Id ?? string.Empty,
            Name = source.Name ?? string.Empty,
            Image = (source?.Images).GetMediumImage() };
    }

    public static Playlist CopyToTopTracksPlaylist(this Artist artist)
    {
        var playlist = new Playlist
        {
            Id = $"TT{artist.Id}",
            Description = $"{artist.Name} Top Tracks",
            Image = artist.Image,
            LastAccessed = DateTime.Now,
            Name = $"{artist.Name} Top Tracks",
            Owner = artist.PackOwner(),
            PlaylistType = PlaylistType.TopTracks,
            ReleaseDate = DateTime.Today,
            Uri = null
        };
        return playlist;
    }
}
