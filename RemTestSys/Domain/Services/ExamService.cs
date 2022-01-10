using RemTestSys.Domain.Interfaces;

namespace RemTestSys.Domain.Services
{
    public class ExamService : IExamService
    {
        public ExamService(AppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext dbContext;

        public async Task<IEnumerable<ExamInfo>> GetAvailableExamsForAsync(int studentId)
        {
            var tests = await dbContext.AccessesToTestForAll
                                       .Include(a=>a.Test.MapParts)
                                       .Select(at => at.Test)
                                       .ToListAsync();
            tests.AddRange(await dbContext.AccessesToTestForGroup
                                          .Where(a => a.GroupId == student.GroupId)
                                          .Include(a=>a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());
            tests.AddRange(await dbContext.AccessesToTestForStudent
                                          .Where(a => a.StudentId == student.Id)
                                          .Include(a=>a.Test.MapParts)
                                          .Select(a => a.Test)
                                          .ToArrayAsync());

            var vmList = new List<TestInfoViewModel>();
            foreach (var tst in tests)
            {
                string lastMark = "-";
                ResultOfTesting lastRes = await dbContext.ResultsOfTesting
                                                         .Where(r => r.Student.Id == student.Id && r.Test.Id == tst.Id)
                                                         .OrderByDescending(r => r.PassedAt)
                                                         .FirstOrDefaultAsync();
                if (lastRes != null)
                {
                    lastMark = lastRes.Mark.ToString();
                }
                var tstInfo = new TestInfoViewModel
                {
                    TestId = tst.Id,
                    TestName = tst.Name,
                    TestDescription = tst.Description,
                    CountOfQuestions = tst.QuestionsCount,
                    Duration = tst.Duration,
                    LastMark = lastMark
                };
                vmList.Add(tstInfo);
        }
    }
}