using Microsoft.Extensions.Configuration;
using NLog;
using RaftLab.Assessment.Interfaces;
using RaftLab.Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RaftLab.Assessment.Services
{
  public class ExternalUserService : IExternalUserService
  {
    #region attributes

    private readonly IHttpClientFactoryHelper<UserResponse> _httpUserResponseHelper;
    private readonly IHttpClientFactoryHelper<PaginatedUserResponse> _httpPaginatedUserResponse;
    private readonly IConfiguration _config;
    private static Logger _log = LogManager.GetCurrentClassLogger();

    private readonly string configUserId = "UserId";
    private readonly string configUserApi = "ReqresApi:Endpoints:ApiUserEndpoint";
    private readonly string configAllUsersApi = "ReqresApi:Endpoints:ApiAllUsersEndpoint";

    #endregion

    #region constructor

    public ExternalUserService(IHttpClientFactoryHelper<UserResponse> httpUserResponseHelper, IHttpClientFactoryHelper<PaginatedUserResponse> httpPaginatedUserResponse, IConfiguration config)
    {
      _httpUserResponseHelper = httpUserResponseHelper;
      _httpPaginatedUserResponse = httpPaginatedUserResponse;
      _config = config;
    }

    #endregion

    #region public methods

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<User?> GetUserByIdAsync(int? id)
    {
      try
      {
         // Fetch user Id from appsettings file
         if (id == null)
         {
            id = _config.GetValue<int>(configUserId);
         }
         
         string endPoint = $"{_config.GetValue<string>(configUserApi)}{id}";

         UserResponse? user = await _httpUserResponseHelper.SendGetAsync(endPoint);

         var getUserByIdlog = user == null ? $"Something went wrong while calling the API {endPoint} or did not find any user with the id {id}" :
                                             $"User details from API : {JsonSerializer.Serialize(user)}";

         _log.Info(getUserByIdlog);

         return user?.Data;
      }
      catch (Exception ex)
      {
        _log.Error(ex);
        throw;
      }
      
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<List<User>>?> GetAllUsersAsync()
    {

      try
      {
         string? endPoint = _config.GetValue<string>(configAllUsersApi);
         int totalPages = 0;
         int i = 1;

         List<List<User>> allUsers = new List<List<User>>();

         do
         {
             PaginatedUserResponse? usersList = await _httpPaginatedUserResponse.SendGetAsync($"{endPoint}{i}");
             if (usersList != null)
             {

                 //Prevent updating the value for multiple times
                 if (i == 1)
                 {
                     totalPages = usersList.TotalPages;
                 }
                 // To list out all the users from all the pages
                 allUsers.Add(usersList.Data);
                 i++;

             }
             else
             {
                 _log.Info($"Something went wrong while calling API or Empty response recieved from the endpoint {endPoint}{i}.");
             }

         } while (i <= totalPages);


         _log.Info($"List of all users from the API : {JsonSerializer.Serialize(allUsers)}");

         return allUsers;
      }
      catch ( Exception ex )
      {
        _log.Error(ex);
        throw;
      }

    }

    #endregion

  }
}
