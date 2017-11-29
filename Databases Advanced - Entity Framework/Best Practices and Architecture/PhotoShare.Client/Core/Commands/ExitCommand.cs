namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using System;

    public class ExitCommand : Command
    {
        public override string Execute(string[] data, PhotoShareContext context)
        {
            Environment.Exit(0);
            return "Good Bye!";
        }
    }
}
