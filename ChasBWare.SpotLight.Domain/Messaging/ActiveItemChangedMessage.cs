using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Messaging;


public class ActiveItemChangedMessage(PageType pageType, object? model)
           : Message
{
    public PageType PageType { get; } = pageType;
    public object? Model { get; } = model;
}

