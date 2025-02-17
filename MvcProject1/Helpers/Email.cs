namespace MvcProject1.PL.Helpers
{
    public class Email
    {
        public string To { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }
    }
    public class EmailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
