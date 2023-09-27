using System;
using System.Collections.Generic;

namespace API_CraftyOrnaments.Models;

public partial class Role
{
    public byte RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<AccountProfile> AccountProfiles { get; set; } = new List<AccountProfile>();
}
