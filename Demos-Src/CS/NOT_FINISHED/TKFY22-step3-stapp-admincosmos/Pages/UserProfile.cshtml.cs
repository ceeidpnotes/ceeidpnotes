using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using NuGet.Protocol;
using System.Security.Claims;
using System.Security.Principal;

namespace TKFY22_step3_stapp_admincosmos.Pages
{
    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { Constants.ScopeUserRead })]
    public class UserProfileModel : PageModel
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        byte[] _photoByte;
        public User Me;
        public IUserAppRoleAssignmentsCollectionPage MeAppRolesAsigments;
        public IUserMemberOfCollectionWithReferencesPage MeMemberOf;
        public IEnumerable<Claim> NetClaims;
        public ClaimsIdentity NetIdentity;

        public UserProfileModel(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
            _photoByte = null;
            Me = null;
            MeAppRolesAsigments = null;
            MeMemberOf= null;
            NetClaims = null;
            NetIdentity = null;
        }
        public async Task OnGet()
        {
            GraphServiceClient graphClient = getGraphServiceClient(new[] { Constants.ScopeUserRead });

            Me = await graphClient.Me.Request().GetAsync();
            MeAppRolesAsigments = await graphClient.Me.AppRoleAssignments.Request().GetAsync();
            MeMemberOf = await graphClient.Me.MemberOf.Request().GetAsync();
            NetClaims = User.Claims;
            NetIdentity = (ClaimsIdentity)User.Identity;
            try
            {
                // Get user photo
                var photoStream = await graphClient.Me.Photo.Content.Request().GetAsync();
                _photoByte = ((MemoryStream)photoStream).ToArray();
                //ViewData["Photo"] = Convert.ToBase64String(photoByte);
            }
            catch (System.Exception)
            {
                _photoByte = null;
            }
        }

        private GraphServiceClient getGraphServiceClient(string[] scopes)
        {
            return GraphServiceClientFactory.GetAuthenticatedGraphClient(async () =>
            {
                string result = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                return result;
            }, "https://graph.microsoft.com/v1.0");
        }
    }
}
