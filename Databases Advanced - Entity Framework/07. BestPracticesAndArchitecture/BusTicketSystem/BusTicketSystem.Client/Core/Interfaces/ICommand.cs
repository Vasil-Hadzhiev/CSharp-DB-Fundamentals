namespace BusTicketSystem.Client.Core.Interfaces
{
    public interface ICommand
    {
        string Execute(string[] args);
    }
}