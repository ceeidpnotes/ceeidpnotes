https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2

Before - install (as admin)
Install-Module -Name MSOnline - will be handy
Install-Module -Name AzureAD - mandatory



**Important** - update libraries / framework (for .NET 6.0). Otherwise - we will have a strange error in Entity Framework In Mem database - like "Sequence contains more than one matching element" or "The type initializer for 'Microsoft.EntityFrameworkCore.Query.QueryableMethods' threw an exception"

