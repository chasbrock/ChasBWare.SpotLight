using ChasBWare.SpotLight.Domain.DbContext;

using SQLite;

namespace ChasBWare.SpotLight.DependencyInjection
{
    public class DefaultDbSettings : IDbSettings
    {
        public string Filename { get => "SpotLight.db3"; }

        public SQLiteOpenFlags Flags
        {
            get => SQLiteOpenFlags.ReadWrite |
                   SQLiteOpenFlags.Create |
                   SQLiteOpenFlags.SharedCache;
        }
        public string FullPath { get => Path.Combine(FileSystem.AppDataDirectory, Filename); }
    }
}
