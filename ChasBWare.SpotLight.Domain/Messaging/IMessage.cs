namespace ChasBWare.SpotLight.Domain.Messaging;

public interface IMessage
{
    bool Completed { get; set; }
}