namespace Employees.App.Core.Commands
{
    public interface ICommand
    {
        string Execute(params string[] data);
    }
}