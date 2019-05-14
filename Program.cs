using System;
using System.Collections.Generic;

using Mono.Options;

namespace foldercompare
{
    class Program
    {
        static void Main(string[] args)
        {
            var showHelp = false;
            var source = string.Empty;
            var destination = string.Empty;

            var options = new OptionSet { 
                { "s|source=", "the source folder.", n => source=n},
                { "d|destination=", "the source folder.", n => destination=n},
                { "h|help", "show this message and exit", h => showHelp = h != null },
            };

            List<string> extra;
            try {
                // parse the command line
                extra = options.Parse(args);

                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
                    throw new OptionException("Source and Destination are required.", "source");
            } catch (OptionException e) {
                // output some error message
                Console.Write("foldercompare: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `foldercompare --help' for more information.");
                return;
            }

            if (showHelp)
            {
                ShowHelp(options);
                return;
            }

            var iterator = new FolderIterator(source, destination);
            iterator.Compare();
        }

        static void ShowHelp(OptionSet p)
        {
            System.Console.WriteLine("Usage: foldercompare [OPTIONS]");
            System.Console.WriteLine();
            System.Console.WriteLine("Options:");
            p.WriteOptionDescriptions(System.Console.Out);
        }
    }
}
