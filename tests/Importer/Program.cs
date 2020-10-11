using System;

namespace POC.Storage.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = args.Length > 0 ? args[0] : "help";

            if (type == "help")
            {
                Help();
            }
            else if (type == "create")
            {
                new DocumentTests().ImportAync().Wait();
            }
            else if (type == "create1000")
            {
                while (true)
                {
                    new DocumentTests().ImportAync(1000).Wait();
                }
            }
            else if (type == "update")
            {
                while (true)
                {
                    new DocumentTests().UpdateAsync().Wait();
                }
            }

            Wait();
        }


        static void Wait()
        {
            Console.WriteLine($"FINISHED. Press ENTER to exit.");
            Console.ReadLine();
        }

        static void Help()
        {
            Console.WriteLine("Argument list:");
            Console.WriteLine();
            Console.WriteLine("help       : shows possible arugments");
            Console.WriteLine("create     : creates some documents");
            Console.WriteLine("create1000 : creates 1000 documents");
            Console.WriteLine("update     : updates 10 000 documents");
            Console.WriteLine();
        }

        static string ToFormattedTime(long ticks)
        {
#pragma warning disable CA1305 // Specify IFormatProvider
            return new TimeSpan(ticks).ToString("hh\\:mm\\:ss");
#pragma warning restore CA1305 // Specify IFormatProvider
        }

    }
}
