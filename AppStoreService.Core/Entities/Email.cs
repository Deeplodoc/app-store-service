namespace AppStoreService.Core.Entities
{
    public class Email
    {
        public object Id { get; set; }
        public string Title { get; set; }
        public string AddressTo { get; set; }
        public string AddressFrom { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}