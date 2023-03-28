using Microsoft.AspNetCore.Identity;
using BarcodeAPI.Entities;

namespace BarcodeAPI.Entities;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public int RoleId { get; set; } = 3;

    public virtual Role Role { get; set; }
}