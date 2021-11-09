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
        static SomeVariantsAnswer[] someVariantsAnswers;
        static SequenceAnswer[] sequenceAnswers;
        static ConnectedPairsAnswer[] connectedPairsAnswers;
        static AccessToTestForAll[] accessToTestForAlls;
        static AccessToTestForGroup[] accessToTestForGroups;
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Enter connection string>>");
            string connectionString = Console.ReadLine();
            DbContextOptionsBuilder<AppDbContext> optsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optsBuilder.UseSqlServer(connectionString);
            using(AppDbContext context = new AppDbContext(optsBuilder.Options))
            {
                tests = await context.Tests.ToArrayAsync();
                questions = await context.Questions.ToArrayAsync();
                groups = await context.Groups.ToArrayAsync();
                textAnswers = await context.TextAnswers.ToArrayAsync();
                oneOfFourVariantsAnswers = await context.OneVariantAnswers.ToArrayAsync();
                someVariantsAnswers = await context.SomeVariantsAnswers.ToArrayAsync();
                sequenceAnswers = await context.SequenceAnswers.ToArrayAsync();
                connectedPairsAnswers = await context.ConnectedPairsAnswers.ToArrayAsync();
                accessToTestForAlls = await context.AccessesToTestForAll.ToArrayAsync();
                accessToTestForGroups = await context.AccessesToTestForGroup.ToArrayAsync();
            }

            object[][] arrs = new object[10][];
            arrs[0] = tests;
            arrs[1] = groups;
            arrs[2] = textAnswers;
            arrs[3] = oneOfFourVariantsAnswers;
            arrs[4] = someVariantsAnswers;
            arrs[5] = sequenceAnswers;
            arrs[6] = connectedPairsAnswers;
            arrs[7] = accessToTestForAlls;
            arrs[8] = accessToTestForGroups;
            arrs[9] = questions;

            JsonSerializerOptions opts = new JsonSerializerOptions {
                WriteIndented=true
            };

            string folderPath = @"d:\\dbJson\";
            for(int i = 0; i < 10; i++)
            {
                File.WriteAllText(folderPath + arrs[i].GetType().Name, JsonSerializer.Serialize(arrs[i], opts));
            }
            Console.WriteLine("Done");
        }
    }
}
