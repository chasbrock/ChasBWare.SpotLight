namespace ChasBWare.SpotLight.Definitions.Repositories;

public interface IAppSettingsRepository
{
    Task Delete(string name);
    Task<string?> Load(string name);
    Task<T?> Load<T>(string name);
    Task Save(string name, string value);
    Task Save<T>(string name, T value, bool deferUpdate = false);
}