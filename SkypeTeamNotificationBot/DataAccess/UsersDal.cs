﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using MongoDB.Bson;
using MongoDB.Driver;
using Remotion.Linq.Clauses;
using SkypeTeamNotificationBot.DataModels;

namespace SkypeTeamNotificationBot.DataAccess
{
    public class UsersDal
    {
        private MongoClient _client;
        private IMongoDatabase _db;
        private IMongoCollection<UserModel> _colection;

        public UsersDal(string conectionString)
        {
            _client = new MongoClient(conectionString);
            _db = _client.GetDatabase("DevelopexBotDb");
            _colection = _db.GetCollection<UserModel>("Users");
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            try
            {
                return await _colection.Find(new BsonDocumentFilterDefinition<DataModels.UserModel>(new BsonDocument()))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                //todo: add logs and some handlers in caller methods
                throw;
            }
        }

        public async Task<UserModel> AddNewUserAsync(UserModel user)
        {
            try
            {
                var users = await _colection.CountAsync(x => x.Role == Role.Admin);
                if (users == 0)
                {
                    user.Role = Role.Admin;
                }
                await _colection.InsertOneAsync(user);
                return user;
            }
            catch(Exception ex)
            {
                //todo: add logs and some handlers in caller methods
                throw;
            }
        }

        public async Task<UserModel> GetUserByNameAsync(string name)
        {
            try
            {
                return await _colection.Find(x => x.Name == name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                //todo: add logs and some handlers in caller methods
                throw;
            }
        }


    }
}