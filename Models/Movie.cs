using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace dotnet_profiling_demo.Models;

public class Movie
{
    [JsonPropertyName("title")]
    [JsonProperty("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("vote_average")]
    [JsonProperty("vote_average")]
    public double VoteAverage { get; set; }
    
    [JsonPropertyName("release_date")]
    [JsonProperty("release_date")]
    public string? ReleaseDate { get; set; }
}