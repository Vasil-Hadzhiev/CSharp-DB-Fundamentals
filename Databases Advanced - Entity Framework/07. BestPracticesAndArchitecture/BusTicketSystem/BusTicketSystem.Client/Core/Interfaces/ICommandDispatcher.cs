namespace BusTicketSystem.Client.Core.Interfaces
{
    public interface ICommandDispatcher
    {
        string DispatchCommand(string[] commandParameters);
    }
}