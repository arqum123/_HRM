﻿using System;
using System.Collections.Generic;
using HRM.Core;
using HRM.Core.IService;
using HRM.WebAPI.Extensions;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.User;
using HRM.WebAPI.ActionFilters;


namespace HRM.WebAPI.Api
{
    public class UserController : ApiControllerBase
    {
        IUserService userService= IoC.Resolve<IUserService>("UserService");

         #region GET
         
        [CacheOutput(60, 60, false, true)]
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> transfer = new DataTransfer<List<GetOutput>>();
            try
            {
                return userService.GetAll();
            }
            catch (Exception ex)
            {
                transfer.IsSuccess = false;
                transfer.Errors = new string[1];
                transfer.Errors[0] = ex.Message;
                return transfer;
            }
        }         
        [CacheOutput(60, 60, false, true)]
        public DataTransfer<GetOutput> Get(string id)
        {
            DataTransfer<GetOutput> transfer = new DataTransfer<GetOutput>();
            try
            {
                return userService.Get(id);
            }
            catch (Exception ex)
            {
                transfer.IsSuccess = false;
                transfer.Errors = new string[1];
                transfer.Errors[0] = ex.Message;
                return transfer;
            }
        }

        #endregion

        #region POST

        public DataTransfer<PostOutput> Post(PostInput Input)
        {
            DataTransfer<PostOutput> transfer = new DataTransfer<PostOutput>();
            try
            {
                return userService.Insert(Input);
            }
            catch (Exception ex)
            {
                transfer.IsSuccess = false;
                transfer.Errors = new string[1];
                transfer.Errors[0] = ex.Message;
                return transfer;
            }

        }

        #endregion

        #region PUT
        
        public DataTransfer<PutOutput> Put(PutInput Input)
        {
            DataTransfer<PutOutput> transfer = new DataTransfer<PutOutput>();
            try
            {
                return userService.Update(Input);
            }
            catch (Exception ex)
            {
                transfer.IsSuccess = false;
                transfer.Errors = new string[1];
                transfer.Errors[0] = ex.Message;
                return transfer;
            }
        }

        #endregion

        #region DELETE

        public DataTransfer<string> Delete(string id)
        {
            DataTransfer<string> transfer = new DataTransfer<string>();
            try
            {
                return userService.Delete(id);
            }
            catch (Exception ex)
            {
                transfer.IsSuccess = false;
                transfer.Errors = new string[1];
                transfer.Errors[0] = ex.Message;
                return transfer;
            }
        }

        #endregion
    }
}