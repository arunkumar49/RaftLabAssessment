using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Interfaces
{
  public interface IHttpClientFactoryHelper<T>
  {
    Task<T?> SendGetAsync(string childUrl);
  }
}
