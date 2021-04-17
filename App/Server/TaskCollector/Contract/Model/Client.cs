namespace TaskCollector.Contract.Model
{
    public class Client : Entity
    {
        public string MapRules { get; set; }
    }

    public class ClientCreator
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string MapRules { get; set; }
    }

}
