using RemTestSys.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemTestSys.Extensions
{
    public static class AnswerExtensions
    {
        public async static Task ToDb(this Answer answer, AppDbContext dbContext)
        {
            if (answer.GetType() == typeof(TextAnswer))
                dbContext.TextAnswers.Add((TextAnswer)answer);

            await dbContext.SaveChangesAsync();
        }
    }
}
