using Microsoft.AspNetCore.Authentication;

namespace NetGuardUI.Data.Extension
{
    public  class ErrorHandlingMiddleware
    {
        public static async Task HandleRemoteFailure(RemoteFailureContext context)
        {
            // If the user cancels the sign-in process, redirect them to the home page
            if (context.Failure.Message.Contains("Access was denied "))
            {
                context.Response.Redirect("/");
                context.HandleResponse();
            }
             
        }
    }
}
