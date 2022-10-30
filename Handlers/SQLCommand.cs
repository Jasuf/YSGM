﻿namespace YSGM.Handlers
{
    public class SQLCommand : BaseCommand
    {
        public string Execute(string[] args)
        {
            var a = SQLManager.Instance.Execute(args[0], args.Skip(1).Aggregate((x, y) => x + " " + y));
            return a.InnerXml;
        }
    }
}
