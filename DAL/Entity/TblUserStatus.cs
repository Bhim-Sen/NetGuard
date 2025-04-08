using System;
using System.Collections.Generic;

namespace DAL.Entity;

public partial class TblUserStatus
{
    public Guid Id { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? IsOnline { get; set; }

    public string? Ipaddress { get; set; }

    public string? Location { get; set; }

    public short? BatteryLevel { get; set; }

    public string? DeviceName { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }
}
