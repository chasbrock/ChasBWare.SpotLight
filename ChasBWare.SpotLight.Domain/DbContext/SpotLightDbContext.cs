﻿using ChasBWare.SpotLight.Domain.Entities;
using SQLite;

namespace ChasBWare.SpotLight.Domain.DbContext;

public partial class SpotLightDbContext(IDbSettings _dbSettings) : IDbContext
{
    private SQLiteAsyncConnection? _database;
    async Task<SQLiteAsyncConnection> Init()
    {
        if (_database is not null)
            return _database;

        _database = new SQLiteAsyncConnection(_dbSettings.FullPath, _dbSettings.Flags, true);
        await _database.CreateTableAsync<AppSetting>();
        await _database.CreateTableAsync<Artist>();
        await _database.CreateTableAsync<OwnedPlaylist>();
        await _database.CreateTableAsync<HatedItem>();
        await _database.CreateTableAsync<LibraryItem>();
        await _database.CreateTableAsync<Playlist>();
        await _database.CreateTableAsync<PlaylistTrack>();
        await _database.CreateTableAsync<SearchItem>();
        await _database.CreateTableAsync<Track>();
        await _database.CreateTableAsync<User>();

        return _database;
    }

    public async Task<SQLiteAsyncConnection> GetConnection()
    {
        if (_database != null)
        {
            return _database;
        }
        return await Init();
    }

    public async Task EnsureDbExists()
    {
        await Init();
    }
}
