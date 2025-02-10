
namespace ChasBWare.SpotLight.Infrastructure.Repositories
{
    internal partial class RepositoryHelper
    {
        /// <summary>
        /// get non saved, artist
        /// param: UserId 
        /// </summary>
        internal const string GetRecentArtists =
@"select a.Id, a.Name, a.Image, ri.LastAccessed 
    from Artist a
    join RecentItem ri on a.Id = ri.ItemId
   where ri.Userid = ?";

        /// <summary>
        /// find all tracks for playlist that vcan be deleted safely
        /// param: "PlaylistId"
        /// </summary>
        internal const string GetDeleteableTracks =
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
        /// delete all info for recent artists
        /// </summary>
        internal static string[] DeleteAllRecentArtists =[
@"delete from playlist  
 where id in (
	select pl.id 	
  	  from ArtistPlaylist apl
      join Playlist pl on pl.Id = apl.PlaylistId
      left join RecentItem rPl on rPl.ItemId = pl.Id 
     where apl.ArtistId in (
	    select ri.ItemId
	     from RecentItem ri
         join Artist a on a.Id = ri.ItemId)
    and IsSaved is null);",
@"delete from ArtistPlaylist;",
@"delete from Artist;",
@"delete from track 
  where id in 
	(select t.Id
       from Track t	
		left join PlaylistTrack plt on plt.TrackId = t.id
  where plt.id is  null);"];

        /// <summary>
        /// find all playlists that can be deleted safely
        /// for lis
        /// param: UserId 
        /// </summary>
        internal const string GetDeleteableAllArtistPlaylists =
@"select pl.Id
  from ArtistPlaylist apl
  join Playlist pl on pl.Id = apl.PlaylistId
  left join RecentItem rPl on rPl.ItemId = pl.Id 
 where apl.ArtistId in (
	select ri.ItemId
	  from RecentItem ri
      join Artist a on a.Id = ri.ItemId)
  and IsSaved is null 
  and UserId = ?";

        /// <summary>
        /// get all playlist for artist 
        /// param: Artistid 
        /// </summary>
        internal const string GetArtistAlbums =
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
        internal const string GetPlaylists =
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
        internal const string CheckIfPlaylistBelongsToAnArtist =
@"select a.Id 
    from ArtistPlaylist apl
    join Artist a on apl.ArtistId = a.Id
    join Playlist pl on apl.PlaylistId = pl.id
   where pl.Id = ?";

        /// <summary>
        /// select all tracks for playlist
        /// param: PlaylistId
        /// </summary>
        internal const string GetPlaylistTracks =
@"select t.*
    from Track t
    join PlaylistTrack plt on plt.TrackId = t.Id
   where plt.PlaylistId = ?
   order by plt.TrackOrder";

        /// <summary>
        /// select all hated items regarless of type
        /// param: UserId
        /// </summary>
        internal const string GetHatedItems =
@"select ItemId 
    from HatedItem
   where UserId = ?";

        /// <summary>
        /// selects all artist that oner is linked to
        /// but are not linked to others
        /// param: userId
        /// </summary>
        internal const string GetDeleteableUserArtists =
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
