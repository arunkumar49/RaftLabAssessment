using RaftLab.Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Interfaces
{
  public interface IExternalUserService
  {
    Task<User?> GetUserByIdAsync();

    Task<List<List<User>>?> GetAllUsersAsync();
  }
}
