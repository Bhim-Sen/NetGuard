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
		// The order of these roles is important for role-based access control.
		User,
		Admin,
		SuperAdmin,
		Vendor,
		Manager
		// Add more roles as needed, but maintain the order.
	}

}
