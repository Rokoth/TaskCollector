namespace TaskCollector.Contract.Model
{
    public interface IIdentity
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}