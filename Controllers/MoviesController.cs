using System.Globalization;
using System.IO.Compression;
using System.Text;
using dotnet_profiling_demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace dotnet_profiling_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private static List<Movie>? _cachedMovies;

    // GET
    [HttpGet]
    public ActionResult<List<Movie>> Get()
    {
        StringValues q;
        var query = "";
        if (Request.Query.TryGetValue("q", out q) && q.Count > 0)
        {
            query = q[0];
        }

        var movies = GetMovies();
        
        SortByDescReleaseDate(movies);

        if (query is not null && !query.Equals(""))
        {
            query = query.ToUpper();
            movies = movies.FindAll(movie => movie.Title is not null && movie.Title.ToUpper().Contains(query));
        }

        return movies;
    }

    private static void SortByDescReleaseDate(List<Movie> movies)
    {
        movies.Sort((movie1, movie2) =>
        {
            if (!DateTime.TryParseExact(movie1.ReleaseDate, "yyyy-MM-dd", null,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal, out DateTime dateTime1))
            {
                dateTime1 = DateTime.MinValue;
            }

            if (!DateTime.TryParseExact(movie2.ReleaseDate, "yyyy-MM-dd", null,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal, out DateTime dateTime2))
            {
                dateTime2 = DateTime.MinValue;
            }

            return -1 * dateTime1.CompareTo(dateTime2);
        });
    }

    private static List<Movie> GetMovies()
    {
        if (_cachedMovies is not null)
        {
            return _cachedMovies;
        }

        return _cachedMovies = LoadMovies();
    }

    private static List<Movie> LoadMovies()
    {
        if (_cachedMovies is not null)
        {
            return _cachedMovies;
        }

        using (var fileStream = new FileStream("./movies5000.json.gz", FileMode.Open, FileAccess.Read))
        {
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                using (var streamReader = new StreamReader(gzStream, Encoding.Default))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();
                        var movies = (List<Movie>?)serializer.Deserialize(jsonTextReader, typeof(List<Movie>));

                        return _cachedMovies = movies ?? new List<Movie>();
                    }
                }
            }
        }
    }
}