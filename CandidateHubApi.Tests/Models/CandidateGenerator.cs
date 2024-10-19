using CandidateHubApi.Context;

namespace CandidateHubApi.Tests.Models
{
    public class CandidateGenerator
    {
        public static List<Candidate> GetCandidates()
        {
            var candidates = new List<Candidate>();
            for (int i = 1; i <= 20; i++)
            {
                candidates.Add(new Candidate
                {
                    CandidateId = i,
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    Email = $"candidate{i}@example.com",
                    GithubURL = string.Empty,
                    LinkedInURL = string.Empty,
                    Bio = $"This is bio for candidate {i}",
                    PhoneNo = $"123-456-789{i}"
                });
            }
            return candidates;
        }
    }
}
