namespace NetGuardUI.Infrastructure
{
    public static class ApiURL
    {


        public static string? BaseUrl { get; private set; }
        public static string? ImageBaseUrl { get; private set; }


        //--------------------------------------------//


        public static void Configure(IConfiguration configuration)
        {
            BaseUrl = configuration["ApiSettings:BaseUrl"];
            ImageBaseUrl = configuration["ApiSettings:ImageBaseUrl"];

			// Auth Section

			Login = BaseUrl + "Customer/Login";

		}

		public static string? Login { get; private set; }


	}


   
}
