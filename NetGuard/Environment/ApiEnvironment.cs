namespace NetGuard.Environment
{
	public class ApiEnvironment
	{
		private const RunEnvironment activeEnvironment = RunEnvironment.Local;

		public enum RunEnvironment { Production, Development, Local }
		public static bool IsDevelopment()
		{
			return activeEnvironment == RunEnvironment.Development;
		}

		public static bool IsProduction()
		{
			return activeEnvironment == RunEnvironment.Production;
		}

		public static bool IsLocal()
		{
			return activeEnvironment == RunEnvironment.Local;
		}
	}
}
