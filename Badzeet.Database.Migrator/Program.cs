using DbUp;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Badzeet.Book.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonFileName = GetJsonFileName();

            if (jsonFileName == null)
            {
                Console.WriteLine("No config, no migration. Bye");
                Console.ReadLine();
                return;
            }

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile(jsonFileName)
                .Build();

            var connectionString = configuration.GetConnectionString("badzeetDb");

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .JournalToSqlTable("dbo", "schema_versions")
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }

        private static string GetJsonFileName()
        {
            Console.WriteLine("Which config you want to use?");
            Console.WriteLine("[Enter] for Development");
            Console.WriteLine("[p]     for Production");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Development - there you go");
                return $"appsettings.Development.json";
            }
            if (input.ToLowerInvariant() == "p")
            {
                Console.WriteLine("Production - there you go");
                return $"appsettings.Production.json";
            }
            return null;
        }
    }
}