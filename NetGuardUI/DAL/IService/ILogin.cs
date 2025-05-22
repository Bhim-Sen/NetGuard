using NetGuardUI.Data;

namespace NetGuardUI.Interface
{
	public interface ILogin
	{
 		Task<Result> GetUserToken(UserDto User);
		Task<Response> LogInMethod(UserLoginDto userDto);

	}
}
