namespace TKFY22_step3_stapp_admincosmos
{
    /// <summary>
    /// Contains a list of all the Azure AD app roles this app depends on and works with.
    /// </summary>
    public static class AppRole
    {
        /// <summary>
        /// User readers can read basic profiles of all users in the directory.
        /// </summary>
        public const string UserReaders = "UserReaders";

        /// <summary>
        /// Directory viewers can view objects in the whole directory.
        /// </summary>
        public const string DirectoryViewers = "DirectoryViewers";
    }

    /// <summary>
    /// Wrapper class the contain all the authorization policies available in this application.
    /// </summary>
    public static class AuthorizationPolicies
    {
        public const string AssignmentToUserReaderRoleRequired = "AssignmentToUserReaderRoleRequired";
        public const string AssignmentToDirectoryViewerRoleRequired = "AssignmentToDirectoryViewerRoleRequired";
    }

    public static class Constants
    {
        public const string ScopeUserRead = "User.Read";
        public const string ScopeUserReadAll = "User.ReadBasic.All";
        public const string BearerAuthorizationScheme = "Bearer";
        public const string UserConsentDeclinedErrorMessage = "User declined to consent to access the app";
        public const string UserConsentDeclinedError = "AADSTS65004";
    }

    public static class GraphScopes
    {
        public const string UserRead = "User.Read";
        public const string UserReadBasicAll = "User.ReadBasic.All";
        public const string DirectoryReadAll = "Directory.Read.All";
    }

    public class WebOptions
    {
        public string GraphApiUrl { get; set; }
    }
}
