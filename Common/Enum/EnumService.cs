using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enum
{

	// Do not change the order of this enum, as it is used for role-based access control.
	public enum UserRole
	{
		User,
		Admin,
		SuperAdmin,
		Vendor,
		Manager
	}

}
