using CandidateHubApi.Context;
using CandidateHubApi.Tests.Models;
using Newtonsoft.Json;
using System.Net;

namespace CandidateHubApi.Tests
{
    public class CandidateApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string grpPrefix = "/api/candidate";
        public CandidateApiTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void Add_Candidate_Required_Fields_BadRequest()
        {
            //arrange
            var dt = new Candidate();

            //act
            var content = new StringContent(JsonConvert.SerializeObject(dt), null, "application/json");
            var result = await _client.PostAsync($"{grpPrefix}", content);
            var json = result.Content.ReadAsStringAsync();

            //assert

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Add_Candidate_Email_Exists_Fields_BadRequest()
        {
            //arrange
            var dt = new Candidate
            {
                FirstName = "Rahul",
                LastName = "Singh",
                Email = "candidate1@example.com",
                Bio = "This is my bio"
            };

            //act
            var content = new StringContent(JsonConvert.SerializeObject(dt), null, "application/json");
            var result = await _client.PostAsync($"{grpPrefix}", content);
            var json = result.Content.ReadAsStringAsync();

            //assert

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Add_Candidate_Non_Required_Fileds_Validation_BadRequest()
        {
            //arrange
            var dt = new Candidate
            {
                FirstName = "Rahul",
                LastName = "Singh",
                Email = "candidate21@example.com",
                Bio = "This is my bio",
                GithubURL = "",
                LinkedInURL = "",
                PhoneNo = "34234",
                TimeInterval = "asdasd"
            };

            //act
            var content = new StringContent(JsonConvert.SerializeObject(dt), null, "application/json");
            var result = await _client.PostAsync($"{grpPrefix}", content);
            var json = await result.Content.ReadAsStringAsync();
            var validations = JsonConvert.DeserializeObject<IEnumerable<ValidationResultDTO>>(json);

            //assert

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            //4 invalidate data validations
            Assert.Equal(validations?.Count(), 4);
        }

        [Fact]
        public async void Add_Candidate_Ok()
        {
            //arrange
            var dt = new Candidate
            {
                FirstName = "Rahul",
                LastName = "Singh",
                Email = "candidate21@example.com",
                Bio = "This is my bio",
                GithubURL = "https://www.google.com",
                LinkedInURL = "https://www.google.com",
                PhoneNo = "123-456-7896",
                TimeInterval = "15:45-18:45"
            };

            //act
            var content = new StringContent(JsonConvert.SerializeObject(dt), null, "application/json");
            var result = await _client.PostAsync($"{grpPrefix}", content);
            var json = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Candidate>(json);

            //assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal(response.CandidateId, 21);
        }


        [Fact]
        public async void Get_By_Id_Test_Ok()
        {
            //arrange
            int id = 3;

            //act
            var result = await _client.GetAsync($"{grpPrefix}/{id}");

            //assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void Get_By_Id_Test_Not_Found()
        {
            //arrange
            int id = 999999999;

            //act
            var result = await _client.GetAsync($"{grpPrefix}/{id}");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async void Get_List_Of_Candidates_Ok()
        {
            //arrange
            int pageNo = 1, pageSize = 50, totalRecords = 20;

            //act
            var result = await _client.GetAsync($"{grpPrefix}?pagenumber={pageNo}&pagesize={pageSize}");
            var json = await result.Content.ReadAsStringAsync();
            var candidates = JsonConvert.DeserializeObject<IEnumerable<Candidate>>(json);

            //assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(candidates);
            Assert.Equal(totalRecords, candidates.Count());
        }
    }
}