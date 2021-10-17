namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Roles="Editor")]
    public class ResultsController : Controller
    {
        public ResultsController(AppDbContext dbContext)
        {
            this.dbContext=dbContext??throw new ArgumentNullException(nameof(dbContext));
        }
        private readonly AppDbContext dbContext;
        [HttpGet]
        public async Task<IActionResult> Index(int page=1)
        {
            int listLength=30;
            int count = await dbContext.ResultsOfTesting.CountAsync();
            if(page<1)page=1;
            List<ResultViewModel> resultList=await dbContext.ResultsOfTesting
                                                            .Skip((page-1)*30)
                                                            .Take(30)
                                                            .OrderByDescending(r=>r.PassedAt)
                                                            .ToListAsync();
            return View(resultList);
        }
    }
}
