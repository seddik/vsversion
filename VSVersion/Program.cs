using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace VSVersion
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Count() != 1)
            {
                Console.WriteLine("!!! Error args count !!!");
                return -1;
            }
            var items = args[0].Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (items.Count() != 2)
            {
                Console.WriteLine("!!! Bad formed argument !!!");
                return -1;
            }

            var fullpath = Path.Combine(items[0], @"Properties\AssemblyInfo.cs");

            if (!File.Exists(fullpath))
            {
                Console.WriteLine("!!! Not .NET project !!!");
                return -1;
            }
            Console.WriteLine(fullpath);
            var props = File.ReadAllText(fullpath).Split(@"AssemblyVersion(""");
            if (props.Count() != 2)
            {
                Console.WriteLine("!!! Properties file invalid format !!!");
                return -1;
            }

            var newtext = props[0];
            newtext += @"AssemblyVersion(""" + DateTime.Now.ToString(items[1]) + @""")]";

            newtext += string.Join(Environment.NewLine, props[1]
    .Split(Environment.NewLine.ToCharArray())
    .Skip(1)
    .ToArray());

            File.WriteAllText(fullpath, newtext);

            Console.WriteLine();
            return 0;
        }
    }
}
