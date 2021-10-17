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
        public Task<IActionResult> Index(int page=1)
        {
            
        }
    }
}
