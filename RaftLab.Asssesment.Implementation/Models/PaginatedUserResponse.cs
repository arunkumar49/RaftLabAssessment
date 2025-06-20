using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Models
{
  public class PaginatedUserResponse
  {
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("data")]
    public List<User> Data { get; set; }

    [JsonPropertyName("support")]
    public Support Support { get; set; }

  }
}
