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
        [MaxLength(20)]
        public string PhoneNo { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Key]
        public string Email { get; set; }
        [Url]
        public string LinkedInURL { get; set; }
        [Url]
        public string GithubURL { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}
