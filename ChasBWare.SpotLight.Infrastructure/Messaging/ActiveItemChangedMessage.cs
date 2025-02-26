using ChasBWare.SpotLight.Definitions.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class ActiveItemChangedMessageArg(PageType pageType, object? model)
    {
        public PageType PageType { get; } = pageType;
        public object? Model { get; set; } = model;

    }

    public class ActiveItemChangedMessage(PageType pageType, object? model)
               : Message<ActiveItemChangedMessageArg>(new ActiveItemChangedMessageArg(pageType, model))
    {
    }
}

