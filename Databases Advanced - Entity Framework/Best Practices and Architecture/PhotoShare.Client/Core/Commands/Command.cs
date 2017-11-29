namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;

    public abstract class Command
    {
        public abstract string Execute(string[] data, PhotoShareContext context);
    }
}
