using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Models
{
  public class UserResponse
  {

    [JsonPropertyName("data")]
    public User Data { get; set; }

    [JsonPropertyName("support")]
    public Support Support { get; set; }

  }
}
