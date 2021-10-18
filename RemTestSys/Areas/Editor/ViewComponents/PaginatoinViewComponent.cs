namespace RemTestSys.Areas.Editor.ViewComponents
{
public class PaginationViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int countOfElements, int currentPage, int elementsPerPage)
    {
        return View();
    }
}
}
