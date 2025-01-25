
namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    public partial class RepositoryHelper
    {
        /// <summary>
        /// get non saved, artist
        /// param: UserId 
        /// </summary>
        public const string GetRecentArtists =
@"select a.Id, a.Name, a.Image, ri.LastAccessed 
    from Artist a
    join RecentItem ri on a.Id = ri.ItemId
   where ri.Userid = ?";

        /// <summary>
        /// find all tracks for playlist that vcan be deleted safely
        /// param: "PlaylistId"
        /// </summary>
        public const string GetDeleteableTracks =
@"select TrackId 
    from PlaylistTrack
   where PlaylistId = ?
     and TrackId not in 
	     (select TrackId
            from PlaylistTrack  
           where TrackId in 
		         (select TrackId
    		     	from PlaylistTrack
			  	   where PlaylistId = ?)
          group by trackId
         having count(*) > 1)";

        /// <summary>
        /// find all playlists that can be deleted safely
        /// param: ArtistId 
        /// param: UserId 
        /// </summary>
        public const string GetDeleteableArtistPlaylists =
@"select PlaylistId 
    from ArtistPlaylist apl
    join RecentItem ri on ri.ItemId = apl.PlaylistId
   where ArtistId = ?
     and UserId = ?
	 and IsSaved = false";

        /// <summary>
        /// get all playlist for artist 
        /// param: Artistid 
        /// </summary>
        public const string GetArtistAlbums =
@"select pl.*, ri.LastAccessed
    from Playlist pl 
    join ArtistPlaylist apl on pl.Id = apl.PlaylistId
    left join RecentItem ri on ri.ItemId = pl.Id
   where apl.ArtistId = ?";

        /// <summary>
        /// get all playlist
        /// param: UserId 
        /// param: PlaylistType 
        /// param: IsSaved 
        /// </summary>
        public const string GetPlaylists =
   @"select pl.*, ri.LastAccessed
    from Playlist pl
    join RecentItem ri on ri.ItemId = pl.Id
   where ri.UserId = ?
     and pl.PlaylistType = ?
     and ri.IsSaved = ?";

        /// <summary>
        /// find id of first artist thatowns this album
        /// param: PlaylistId
        /// </summary>
        public const string CheckIfPlaylistBelongsToAnArtist =
@"select a.Id 
    from ArtistPlaylist apl
    join Artist a on apl.ArtistId = a.Id
    join Playlist pl on apl.PlaylistId = pl.id
   where pl.Id = ?";

        /// <summary>
        /// select all tracks for playlist
        /// param: PlaylistId
        /// </summary>
        public const string GetPlaylistTracks =
@"select t.*
    from Track t
    join PlaylistTrack plt on plt.TrackId = t.Id
   where plt.PlaylistId = ?
   order by plt.TrackOrder";

        /// <summary>
        /// select all hated items regarless of type
        /// param: UserId
        /// </summary>
        public const string GetHatedItems =
@"select ItemId 
    from HatedItem
   where UserId = ?";

        /// <summary>
        /// selects all artist that oner is linked to
        /// but are not linked to others
        /// param: userId
        /// </summary>
        public const string GetDeleteableUserArtists =
@"select ItemId, count(*)
  from RecentItem
  where ItemId in 
	(select ItemId
       from RecentItem ri
       join Artist a on a.Id = ri.ItemId
      where ri.UserId =?)
group by ItemId
having count(*) = 1";


    }
}
