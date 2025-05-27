using System;
using System.Collections.Generic;

namespace DAL.Entity;

public partial class TblUser
{
    public Guid Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Gmail { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; }
}
