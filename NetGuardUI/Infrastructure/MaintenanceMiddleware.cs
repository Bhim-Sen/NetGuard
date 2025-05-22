namespace NetGuardUI.Infrastructure
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly bool _isInMaintenanceMode;
        private readonly IWebHostEnvironment _env;
        public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment env)
        {
            _next = next;
            _isInMaintenanceMode = configuration.GetValue<bool>("MaintenanceMode");
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (_isInMaintenanceMode)
            {
                var maintenanceFilePath = Path.Combine(_env.WebRootPath, "SiteMaintainance.html");

                if (File.Exists(maintenanceFilePath))
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(maintenanceFilePath);
                }
                else
                {
                    await context.Response.WriteAsync("Maintenance page not found.");
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
