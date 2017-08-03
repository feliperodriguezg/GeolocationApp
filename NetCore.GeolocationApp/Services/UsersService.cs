using NetCore.CacheManager;
using NetCore.GeolocationApp.Models;
using NetCore.GeolocationApp.Repositories;
using NetCore.GeolocationApp.Repositories.Interfaces;
using NetCore.GeolocationApp.WebApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.GeolocationApp.Services
{
    public class UsersService
    {
        private IUsersRepository _repository;

        public UsersService(IDataCacheManager cacheManager)
        {
            _repository = new UserMemoryRepository(cacheManager);
        }

        public ServiceResponse Authentication(AuthenticationRequest request)
        {
            var response = new ServiceResponse();
            try
            {
                bool result = _repository.Authenticate(request.Username, request.Password);
                if (!result)
                    response.Status = Enums.ResponseStatusTypes.AuthenticationError;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse GetUserAppInformation(UserAppInformationRequest request)
        {
            var response = new ServiceResponse();
            try
            {
                if (String.IsNullOrEmpty(request.Username))
                    response.Status = Enums.ResponseStatusTypes.UsernameRequired;
                else
                    response.Data = _repository.GetUserData(request.Username);
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse RegisterNewUser(RegisterNewUserRequest request)
        {
            var response = new ServiceResponse();
            try
            {
                if (String.IsNullOrEmpty(request.Username))
                    response.Status = Enums.ResponseStatusTypes.UsernameRequired;
                else if (String.IsNullOrEmpty(request.Password))
                    response.Status = Enums.ResponseStatusTypes.PasswordRequired;
                else
                {
                    bool result = _repository.RegisterNewUser(request.Password, new UserData
                    {
                        Email = request.Email,
                        Name = request.Name,
                        Username = request.Username
                    });
                    if (!result)
                        response.Status = Enums.ResponseStatusTypes.RegisterNewUserError;
                    else
                        response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse UpdateUserAppInformation(UpdateUserAppInformationRequest request)
        {
            var response = new ServiceResponse();
            try
            {
                if (String.IsNullOrEmpty(request.Username))
                    response.Status = Enums.ResponseStatusTypes.UsernameRequired;
                else
                {
                    bool updated = _repository.UpdateUserInformation((string)request.Username, new UserData
                    {
                        Email = request.Email,
                        Name = request.Name,
                        Username = request.Username
                    });
                    if (!updated)
                        response.Status = Enums.ResponseStatusTypes.UpdateUserInformationError;
                }
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
