using GoogleKeep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoogleKeep.Tests
{
    public class NotesAPIIntegraationTest
    {
        private static HttpClient _client;
        static NotesAPIIntegraationTest()
        {
            var host = new TestServer(new WebHostBuilder()
            .UseEnvironment("Testing")
            .UseStartup<Startup>()
            );

            _client = host.CreateClient();

        }

        [Fact]
        public async Task Post()
        {
            var note = new Note()
            {
                ID = 3,
                Title = "Note 3"
            };
            string jsonNote = JsonConvert.SerializeObject(note);
            var stringContent = new StringContent(jsonNote, UnicodeEncoding.UTF8, "application/json");
            var postRequest = await _client.PostAsync("api/Notes", stringContent);
            postRequest.EnsureSuccessStatusCode();
            var responseString = await postRequest.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }

        [Fact]
        public async Task Get()
        {
            var getRequest = await _client.GetAsync("api/Notes");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }

        [Fact]
        public async Task Put()
        {
            var note = new Note()
            {
                ID = 3,
                Title = "Note name Changed"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var putRequest = await _client.PutAsync("api/Notes/3", stringContent);
            putRequest.EnsureSuccessStatusCode();
            var getRequest = await _client.GetAsync("api/Notes?title=Note name Changed");
            var responseString = await getRequest.Content.ReadAsStringAsync();
            Console.WriteLine("This is Put Response \n" + responseString);
        }

        [Fact]
        public async Task Delete()
        {
            var deleteRequest = await _client.DeleteAsync("api/Notes/3");
            deleteRequest.EnsureSuccessStatusCode();
            var responseString = await deleteRequest.Content.ReadAsStringAsync();
            Console.WriteLine("This is Delete Response \n" + responseString);
        }




    }
}
