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
            {
                dbContext.TextAnswers.Add((TextAnswer)answer);
                await dbContext.SaveChangesAsync();
                return;
            }else if (answer.GetType() == typeof(OneOfFourVariantsAnswer))
            {
                dbContext.OneVariantAnswers.Add((OneOfFourVariantsAnswer)answer);
                await dbContext.SaveChangesAsync();
                return;
            }
            throw new InvalidOperationException($"Suitable action for {answer.GetType().FullName} type is not found");
        }
    }
}
