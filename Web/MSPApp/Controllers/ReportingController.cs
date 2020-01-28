using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MSPApp.DB;
using MSPApp.Infrastructure;
using MSPApp.Models;
using MSPApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constants = MSPApp.Infrastructure.Constants;

namespace MSPApp.Controllers
{
    public class ReportingController : BaseController
    {
        private readonly int[] CommonIDs = new int[] { 1, 2, 3, 4, 5 };
        public ReportingController(ITokenAcquisition tokenAcquisition, IOptions<WebOptions> webOptionValue) : base(tokenAcquisition, webOptionValue)
        {
        }

        // GET: Home
        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<ActionResult> Index()
        {
            (User graphUser, _) = await GetGraphUserData();
            UserData currentUser = await graphUser.ToDBObject();
            ViewData[Constants.UserKey] = currentUser;

            using MSPAppContext dbLink = new MSPAppContext();
            List<ActivityType> submissionTypes = dbLink.ActivityType.ToList();
            ViewData[Constants.ActivityKey] = submissionTypes;

            IEnumerable<int> myDirectSubmission = dbLink.Submission.Where(
                x => x.UserId == currentUser.Id).Select(x => x.Id);
            IEnumerable<int> myIndirectSubmissions = dbLink.SubmissionAssociation.Where(
                x => x.UserId == currentUser.Id).Select(x => x.Id);
            List<int> mySubmissions = myDirectSubmission.Union(myIndirectSubmissions).ToList();

            List<int> sameUniversitySubmissions = dbLink.Submission.Where(
                x => CommonIDs.Contains(x.ActivityId) &&
                (!mySubmissions.Contains(x.Id)) &&
                x.User.UniversityId == currentUser.UniversityId).Select(x => x.Id).ToList();

            if (sameUniversitySubmissions.Any())
            {
                Dictionary<int, string> previousSubmissions = dbLink.SubmissionDetail.Where(
                    x => sameUniversitySubmissions.Contains(x.SubmissionId) &&
                    x.ActivityDetail.DataType == "TITLE")
                    .ToDictionary(key => key.SubmissionId, value => value.Value);

                ViewData[Constants.PreviousActivitiesKey] = previousSubmissions;
            }
            else
            {
                ViewData[Constants.PreviousActivitiesKey] = null;
            }

            return View(new ReportingDecisionModel());
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        [HttpPost]
        public async Task<ActionResult> ExistingActivity(ReportingDecisionModel decissionTaken)
        {
            int ID = Convert.ToInt32(decissionTaken.Value);
            using MSPAppContext dbLink = new MSPAppContext();
            (User graphUser, _) = await GetGraphUserData();
            UserData currentUser = await graphUser.ToDBObject();

            dbLink.SubmissionAssociation.Add(new SubmissionAssociation
            {
                Id = dbLink.SubmissionAssociation.Count(),
                SubmissionId = ID,
                UserId = currentUser.Id
            });
            await dbLink.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult NewActivity(ReportingDecisionModel decissionTaken)
        {
            int ID = Convert.ToInt32(decissionTaken.Value);

            return RedirectToAction("Index", "NewSubmission", new { @acctivityID = ID });
        }
    }
}
