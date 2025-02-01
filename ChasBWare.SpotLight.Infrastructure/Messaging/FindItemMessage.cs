namespace ChasBWare.SpotLight.Infrastructure.Messaging
{
    public class FindItemMessageArg(string itemType, string id)
    {
        public string ItemType { get; } = itemType;
        public string Id { get; } = id;
    }

    public class FindItemMessage(string itemType, string id) 
               : Message<FindItemMessageArg>(new FindItemMessageArg(itemType, id))
    {
    }
}
