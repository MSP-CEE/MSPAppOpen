using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using MSPApp.DB;
using MSPApp.Infrastructure;
using MSPApp.Services;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Constants = MSPApp.Infrastructure.Constants;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace MSPApp.Controllers
{
    public class NewSubmissionController : BaseController
    {
        private List<string> KnownMSPs => CurrentSession.GetFromJSONTo<List<string>>(Constants.MSPsKey);
        public NewSubmissionController(ITokenAcquisition tokenAcquisition, IOptions<WebOptions> webOptionValue) : base(tokenAcquisition, webOptionValue)
        {
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<ActionResult> Index(int acctivityID = 3)
        {
            Submission currentSubmission = await GetSubmission(acctivityID);
            ViewData[Constants.ActivityKey] = currentSubmission.Activity.Name;

            if (KnownMSPs == null)
            {
                using MSPAppContext dbLink = new MSPAppContext();
                List<string> mspData = dbLink.UserData.Select(x => x.Mspmail).ToList();

                CurrentSession.AddToJSONFrom<List<string>>(Constants.MSPsKey, mspData);
            }

            return View(currentSubmission);
        }

        private async Task<Submission> GetSubmission(int activityID = 3)
        {
            Submission currentSubmission =
                CurrentSession.GetFromJSONTo<Submission>(Constants.SubmissionKey);

            if (currentSubmission == null)
            {
                (User graphUser, _) = await GetGraphUserData();
                UserData currentUser = await graphUser.ToDBObject();

                using MSPAppContext dbLink = new MSPAppContext();
                ActivityType currentActivity = dbLink.ActivityType.FirstOrDefault(
                    x => x.Id == activityID);
                List<ActivityDetails> details = dbLink.ActivityDetails.Where(
                    x => x.ActivityId == activityID).ToList();

                int submissionID = dbLink.Submission.Count();
                int startingID = dbLink.SubmissionDetail.Count();

                currentSubmission = new Submission
                {
                    Id = submissionID,
                    UserId = currentUser.Id,
                    ActivityId = activityID,
                    AnythingElse = "",
                    SubmissionDetail = details.Select(
                        x => new SubmissionDetail
                        {
                            SubmissionId = submissionID,
                            ActivityDetailId = x.Id,
                            Id = startingID++,
                            Value = ""
                        }).ToList(),
                    Details = details.Select(x => "").ToList()
                };

                dbLink.Submission.Add(currentSubmission);
                await dbLink.SaveChangesAsync();

                currentSubmission = dbLink.Submission.FirstOrDefault(x => x.Id == submissionID);
                CurrentSession.AddToJSONFrom(Constants.SubmissionKey, currentSubmission);
            }

            return currentSubmission;
        }

        private void RemoveSubmission()
        {
            CurrentSession.Remove(Constants.SubmissionKey);
            CurrentSession.Remove(Constants.MSPsKey);
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        [HttpPost]
        public async Task<ActionResult> Validate(Submission newData)
        {
            using MSPAppContext dbLink = new MSPAppContext();
            int submissionID = newData.Id;
            (User graphUser, _) = await GetGraphUserData();
            UserData currentUser = await graphUser.ToDBObject();
            Submission currentSubmission = dbLink.Submission.FirstOrDefault(x => x.Id == submissionID);
            currentSubmission.SubmissionDetail = dbLink.SubmissionDetail.Where(
                x => x.SubmissionId == submissionID).ToList();

            for (int i = 0; i < currentSubmission.SubmissionDetail.Count; i++)
            {
                currentSubmission.SubmissionDetail.ElementAt(i).Value = newData.Details[i];
            }
            currentSubmission.AnythingElse = newData.AnythingElse;
            await dbLink.SaveChangesAsync();

            Dictionary<string, UserData> knownUsers = dbLink.UserData.ToDictionary(
                key => key.Mspmail.Split("@studentpartner.com").FirstOrDefault().ToLower(),
                value => value);
            int startIndex = dbLink.SubmissionAssociation.Count();

            if (!string.IsNullOrEmpty(newData.MSPsThatHelped))
            {
                foreach (string userName in newData.MSPsThatHelped.Split(";"))
                {
                    string formatted = userName.Trim().ToLower();

                    if (knownUsers.ContainsKey(formatted))
                    {
                        int userID = knownUsers[formatted].Id;
                        if (userID == currentUser.Id) continue;

                        dbLink.SubmissionAssociation.Add(new SubmissionAssociation
                        {
                            Id = startIndex++,
                            SubmissionId = submissionID,
                            UserId = userID
                        });
                    }
                }
            }
            await dbLink.SaveChangesAsync();

            RemoveSubmission();

            return RedirectToAction("Index", "Home");
        }

        [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
        public async Task<ActionResult> Cancel()
        {
            Submission currentSubmission = await GetSubmission();
            int submissionID = currentSubmission.Id;
            using MSPAppContext dbLink = new MSPAppContext();

            dbLink.SubmissionDetail.RemoveRange(dbLink.SubmissionDetail.Where(x => x.SubmissionId == submissionID));
            dbLink.Submission.Remove(dbLink.Submission.FirstOrDefault(x => x.Id == submissionID));
            await dbLink.SaveChangesAsync();

            RemoveSubmission();

            return RedirectToAction("Index", "Home");
        }
    }
}