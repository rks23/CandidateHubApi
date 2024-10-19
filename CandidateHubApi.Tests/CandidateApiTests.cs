using System.Net;

namespace CandidateHubApi.Tests
{
    public class CandidateApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public CandidateApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async void Get_By_Id_Test()
        {
            //arrange
            int id = 3;

            //act
            var result = await _client.GetAsync($"/api/candidate/{id}");

            //assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}