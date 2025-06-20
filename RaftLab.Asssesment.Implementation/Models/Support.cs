using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Models
{
  public class Support
  {
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
  }

}
