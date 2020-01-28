using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MSPApp.Infrastructure;
using MSPApp.Services;
using System.Threading.Tasks;
using Constants = MSPApp.Infrastructure.Constants;

namespace MSPApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class BaseController : Controller
    {
        protected readonly ITokenAcquisition tokenAcquisition;
        protected readonly WebOptions webOptions;
        protected ISession CurrentSession => this.HttpContext.Session;

        public BaseController(ITokenAcquisition tokenAcquisition,
                              IOptions<WebOptions> webOptionValue)
        {
            this.tokenAcquisition = tokenAcquisition;
            this.webOptions = webOptionValue.Value;
        }

        protected GraphServiceClient GetGraphServiceClient(string[] scopes)
        {
            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(scopes);
                return result;
            }, webOptions.GraphApiUrl);
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        protected async Task<(User, string)> GetGraphUserData()
        {
            User graphUser = CurrentSession.GetFromJSONTo<User>(Constants.UserKey);
            string imageData;

            if (graphUser == null)
            {
                GraphServiceClient graphClient = GetGraphServiceClient(new[] { Constants.ScopeUserRead });
                (graphUser, imageData) = await graphClient.GetCurrentUserFromGraph();

                CurrentSession.AddToJSONFrom<User>(Constants.UserKey, graphUser);

                if (imageData != null)
                    CurrentSession.SetString(Constants.PictureKey, imageData);
            }
            else
            {
                imageData = CurrentSession.GetString(Constants.PictureKey);
            }

            return (graphUser, imageData);
        }
    }
}
