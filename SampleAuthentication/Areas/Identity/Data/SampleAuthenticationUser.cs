using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SampleAuthentication.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SampleAuthenticationUser class
public class SampleAuthenticationUser : IdentityUser
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PreferredName { get; set; }
}

