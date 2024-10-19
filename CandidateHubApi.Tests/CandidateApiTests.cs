using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace CandidateHubApi.Tests
{
    public class CandidateApiTests
    {
        [Fact]
        public async void Get_By_Id_Test()
        {
            //arrange
            int id = 3;

            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            //act
            var result = await client.GetAsync($"/api/candidate/{id}");

            //assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}