﻿namespace ChasBWare.SpotLight.Infrastructure.Repositories;

internal partial class RepositoryHelper
{
    /// <summary>
    /// delete all tracks that are not connected to
    /// a playlist / album
    /// </summary>
    internal const string DeleteOrphanTracks =
@"delete from track
   where id in 
        (select t.Id
           from Track t
            left join PlaylistTrack plt on plt.TrackId = t.id
           where plt.id is null)";

    internal const string DeleteOrphanPlaylistTracks =
@"delete from PlaylistTrack
   where playlistId in (  
     select playlistId
       from PlaylistTrack plt
        left join Playlist pl on plt.PlaylistId = pl.id
	   where pl.id is null)";

    /// <summary>
    /// delete all playlists    
    /// </summary>
    internal const string DeleteOrphanPlaylists =
 @"delete from playlist
    where id in (
       select pl.id 
         from playlist pl
         left join OwnedPlaylist opl on pl.id= opl.PlaylistId
         left join LibraryItem li on pl.id = li.id
         left join SearchItem si on pl.Id = si.Itemid
        where li.id is null
          and si.id is null
          and opl.id is null)";

    /// <summary>
    /// get all playlist in library
    /// param: PlaylistType 
    /// </summary>
    internal const string DeleteLibraryItem =
@"delete from LibraryItem where Id = ?";

    /// <summary>
    /// delete list of ids from library
    /// </summary>
    internal static string[] DeleteLibraryItems = [
@"delete from LibraryItem where Id in (?)",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];


    /// <summary>
    /// find playlist for track,
    /// also check prior track to see if we can find an
    /// exact match for playlist
    /// param: trackId
    /// param: priorTrack
    /// param: trackId
    /// </summary>
    internal const string GetPlaylistForTrack =
@"select PlaylistId
    from (
        select 1 priority, tnow.PlaylistId PlaylistId
          from PlaylistTrack tnow 
          join PlaylistTrack tprior 
            on tnow.TrackOrder = tprior.trackorder+1
	       and tnow.PlaylistId = tprior.PlaylistId
         where tnow.TrackId = ?
          and tprior.TrackId = ?
        union
        select 2 priority, playlistid 
          from PlaylistTrack
         where TrackId = ? )
   order by priority";

    /// <summary>
    /// get all playlist in library
    /// param: PlaylistType 
    /// </summary>
    internal const string GetLibraryItemIds =
@"select pl.Id
    from Playlist pl
    join LibraryItem li on li.Id = pl.Id";

    /// <summary>
    /// get all playlist in library
    /// param: PlaylistType 
    /// </summary>
    internal const string GetLibraryItems =
@"select pl.*
    from Playlist pl
    join LibraryItem li on li.Id = pl.Id
   where pl.PlaylistType = ?";

    /// <summary>
    /// get all artists in searchitems
    /// </summary>
    internal const string GetSearchArtists =
@"select a.*
    from Artist a
    join SearchItem si on si.ItemId = a.Id";

    /// <summary>
    /// get all playlist in search
    /// param: PlaylistType 
    /// </summary>
    internal const string GetSearchPlaylists =
@"select pl.*
    from Playlist pl
    join SearchItem si on si.ItemId = pl.Id
   where pl.PlaylistType = ?";

    /// <summary>
    /// get all users in searchitems
    /// </summary>
    internal const string GetSearchUsers =
@"select u.*
    from User u
    join SearchItem si on si.ItemId = u.Id";

    /// <summary>
    /// collection of scripts to remove a single
    /// playlist from search history
    /// </summary>
    internal static string[] DeleteRecentPlaylist = [
@"delete from SearchItem where ItemId = ?",
@"delete from playlist
   where id not in (select id from LibraryItem )
     and id = ?",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];

    /// <summary>
    /// delete all info for recent artists
    /// </summary>
    internal static string[] DeleteAllRecentArtists = [
@"delete from OwnedPlaylist where OwnerId in (select id from Artist)",
@"delete from Artist",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];

    /// <summary>
    /// delete all info for recent artist
    /// param: ArtistId
    /// </summary>
    internal static string[] DeleteRecentArtist = [
@"delete from OwnedPlaylist where OwnerId = ?",
@"delete from Artist where id = ?",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];


    /// <summary>
    /// delete all info for recent users
    /// </summary>
    internal static string[] DeleteAllRecentUsers = [
@"delete from OwnedPlaylist where OwnerId in (select id from User)",
@"delete from User",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];

    /// <summary>
    /// delete all info for recent user
    /// param: UserId
    /// </summary>
    internal static string[] DeleteRecentUser = [
@"delete from OwnedPlaylist where OwnerId = ?",
@"delete from User where id = ?",
DeleteOrphanPlaylists,
DeleteOrphanPlaylistTracks,
DeleteOrphanTracks];

    /// <summary>
    /// collection of scripts to remove all
    /// playlists from search history
    /// </summary>
    internal static string[] DeleteAllRecentPlaylists = [
@"delete from SearchItem
    where ItemId in (
      select pl.id
        from Playlist pl
        join SearchItem si on si.ItemId = pl.Id
       where plPlaylistType = ?)",
 DeleteOrphanPlaylists,
 DeleteOrphanPlaylistTracks,
 DeleteOrphanTracks];

    /// <summary>
    /// get all playlist for artist 
    /// param: Artistid 
    /// </summary>
    internal const string GetArtistAlbums =
@"select pl.*
    from Playlist pl 
    join OwnedPlaylist opl on pl.Id = opl.PlaylistId
    left join SearchItem si on si.ItemId = pl.Id
   where opl.OwnerId = ?";

    /// <summary>
    /// get all playlist for artist 
    /// param: Artistid 
    /// </summary>
    internal const string GetUserAlbums =
@"select pl.*
    from Playlist pl 
    join OwnedPlaylist opl on pl.Id = opl.PlaylistId
    left join SearchItem si on si.ItemId = pl.Id
   where opl.OwnerId = ?";

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

}
