using System;
using System.Collections.Generic;

namespace BlazorCrudApp.Server.Data;

public partial class Personal
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public DateTime? DateDeleted { get; set; }
}
