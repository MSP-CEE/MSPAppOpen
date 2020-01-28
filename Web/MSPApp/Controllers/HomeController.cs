using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using MSPApp.Models;
using MSPApp.Services;
using Microsoft.Graph;
using Constants = MSPApp.Infrastructure.Constants;
using MSPApp.Infrastructure;

namespace MSPApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ITokenAcquisition tokenAcquisition, IOptions<WebOptions> webOptionValue) : base(tokenAcquisition, webOptionValue)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<IActionResult> Profile()
        {
            (User graphUser, string imageData) = await GetGraphUserData();

            ViewData["Me"] = graphUser.GetDataFromGraphUser();
            ViewData["photo"] = imageData;

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult ShowSubmissions()
        {
            return View();
        }
    }
}