namespace RemTestSys.Areas.Editor.Controllers
{
    [Area("Editor")]
    [Authorize(Role="Editor")]
    public class ResultsController : Controller
    {
        public ResultsController()
        {
            
        }
        public Task<IActionResult> Index()
        {
            
        }
    }
}
