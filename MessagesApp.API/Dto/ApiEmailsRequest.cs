namespace MessagesApp.API.Dto
{
    public class ApiEmailsRequest
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }       
    }
}
