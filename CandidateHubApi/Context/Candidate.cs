using CandidateHubApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CandidateHubApi.Context
{

    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(12)]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in the format 123-456-7891.")]
        public string PhoneNo { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [TimeInterval]
        public string? TimeInterval { get; set; }
        [Url]
        public string? LinkedInURL { get; set; }
        [Url]
        public string? GithubURL { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}
