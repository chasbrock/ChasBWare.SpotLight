using ChasBWare.SpotLight.Definitions.Enums;

namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class FindItemMessageArg(PageType pageType, string id)
    {
        public PageType PageType { get; } = pageType;
        public string Id { get; } = id;
    }

    public class FindItemMessage(PageType pageType, string id) 
               : Message<FindItemMessageArg>(new FindItemMessageArg(pageType, id))
    {
    }
}
