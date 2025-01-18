using ChasBWare.SpotLight.Domain.Entities;

namespace ChasBWare.SpotLight.Definitions.Repositories
{
    public interface IUserRepository
    {
        User CurrentUser { get; set; } 
    }
}