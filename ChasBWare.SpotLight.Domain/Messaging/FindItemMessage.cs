using ChasBWare.SpotLight.Domain.Enums;

namespace ChasBWare.SpotLight.Domain.Messaging
{
    public class FindItemMessage(PageType pageType, string id)
               : Message()
    {
        public PageType PageType { get; } = pageType;
        public string Id { get; } = id;
    }
}
