using SQLite;

namespace ChasBWare.SpotLight.Domain.DbContext;

public interface IDbSettings
{
    string Filename { get; }
    SQLiteOpenFlags Flags { get; }
    string FullPath { get; }
}
