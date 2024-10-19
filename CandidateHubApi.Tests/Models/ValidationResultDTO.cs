namespace CandidateHubApi.Tests.Models
{
    internal class ValidationResultDTO
    {
        public IEnumerable<string> MemberNames { get; set; }
        public string ErrorMessage { get; set; }
    }
}
