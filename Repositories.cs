using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyDiscordBot
{
    public class Repositories
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("html_url")]
        public string Url { get; private set; }

        public static async Task<List<Repositories>> GetRepositories(string url)
        {
            string responseBody;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                using var response = client.GetAsync("https://api.github.com/users/gymnasy55/repos?sort=updated").Result;
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            var kek = JsonConvert.DeserializeObject<List<Repositories>>(responseBody);
            return kek;
        }
    }
}