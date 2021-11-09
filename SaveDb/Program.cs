using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using RemTestSys.Domain;
using RemTestSys.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SaveDb
{
    class Program
    {
        static Test[] tests;
        static Question[] questions;
        static Group[] groups;
        static TextAnswer[] textAnswers;
        static OneOfFourVariantsAnswer[] oneOfFourVariantsAnswers;
        static AccessToTestForGroup[] accessToTestForGroups;
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Enter connection string>>");
            string connectionString = Console.ReadLine();
            Console.WriteLine("Enter action>>");
            string act = Console.ReadLine();
            switch (act)
            {
                case "save": await SaveDb(connectionString); break;
                case "loadToDb": await LoadToDb(connectionString); break;
            }
        }

        private static async Task LoadToDb(string connectionString)
        {
            Group[] groups = JsonSerializer.Deserialize<Group[]>(await File.ReadAllTextAsync(@"d:\\dbJson\Group[]"));
            Test[] tests = JsonSerializer.Deserialize<Test[]>(await File.ReadAllTextAsync(@"d:\\dbJson\Test[]"));
            Question[] questions = JsonSerializer.Deserialize<Question[]>(await File.ReadAllTextAsync(@"d:\\dbJson\Question[]"));
            TextAnswer[] textAnswers = JsonSerializer.Deserialize<TextAnswer[]>(await File.ReadAllTextAsync(@"d:\\dbJson\TextAnswer[]"));
            OneOfFourVariantsAnswer[] oneOfFourVariants = JsonSerializer.Deserialize<OneOfFourVariantsAnswer[]>(await File.ReadAllTextAsync(@"d:\\dbJson\OneOfFourVariantsAnswer[]"));
            AccessToTestForGroup[] accessToTestForGroups = JsonSerializer.Deserialize<AccessToTestForGroup[]>(await File.ReadAllTextAsync(@"d:\\dbJson\AccessToTestForGroup[]"));

            DbContextOptionsBuilder<AppDbContext> optsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optsBuilder.UseSqlServer(connectionString);
            using (AppDbContext context = new AppDbContext(optsBuilder.Options))
            {

            }
        }

        private static async Task SaveDb(string connectionString)
        {
            DbContextOptionsBuilder<AppDbContext> optsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optsBuilder.UseSqlServer(connectionString);
            using (AppDbContext context = new AppDbContext(optsBuilder.Options))
            {
                tests = await context.Tests.ToArrayAsync();
                questions = await context.Questions.ToArrayAsync();
                groups = await context.Groups.ToArrayAsync();
                textAnswers = await context.TextAnswers.ToArrayAsync();
                oneOfFourVariantsAnswers = await context.OneVariantAnswers.ToArrayAsync();
                accessToTestForGroups = await context.AccessesToTestForGroup.ToArrayAsync();
            }

            object[][] arrs = new object[6][];
            arrs[0] = tests;
            arrs[1] = groups;
            arrs[2] = textAnswers;
            arrs[3] = oneOfFourVariantsAnswers;
            arrs[4] = accessToTestForGroups;
            arrs[5] = questions;

            JsonSerializerOptions opts = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string folderPath = @"d:\\dbJson\";
            for (int i = 0; i < 6; i++)
            {
                File.WriteAllText(folderPath + arrs[i].GetType().Name, JsonSerializer.Serialize(arrs[i], opts));
            }
            Console.WriteLine("Saving done");
        }
    }
}
