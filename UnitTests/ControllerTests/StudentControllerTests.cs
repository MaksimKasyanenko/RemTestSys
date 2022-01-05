using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;
using RemTestSys.Domain;
using RemTestSys.Controllers;

namespace UnitTests.ControllerTests{
    public class StudentControllerTests : TestClassWithDb{
        public StudentControllerTests() : base(new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Filename=Test.db").Options){}

        [Fact]
        public async Task ReturnsRedirectFromAllMethods_WhenAuthorizationHasNotBeenDone(){
            SeedDb(c=>{});
            using(AppDbContext context = new AppDbContext(ContextOptions)){
                StudentController  controller = new StudentController(context, new SessionBuilder(context));
                controller.ControllerContext = new ControllerContext();
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
                List<IActionResult> actionResults = new List<IActionResult>();

                actionResults.Add(await controller.AvailableTests());
                actionResults.Add(await controller.Results());
                actionResults.Add(await controller.Testing(0));
                actionResults.Add(await controller.ResultOfTesting(0));

                actionResults.ForEach(ar => Assert.IsType<RedirectToActionResult>(ar));
            }
        }

        public class AvailableTestsActionMethodTests{
            
        }
    }
}