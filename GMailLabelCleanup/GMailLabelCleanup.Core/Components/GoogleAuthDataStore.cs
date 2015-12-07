using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Google.Apis.Util.Store;
using GMailLabelCleanup.Data;
using System.Data.Entity;
using Newtonsoft.Json;
using GMailLabelCleanup.Data.Models.Identity;

namespace GMailLabelCleanup.Core.Components
{
    public class GoogleAuthDataStore : IDataStore
    {

        private readonly string _userId;

        public GoogleAuthDataStore(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException("userId");

            _userId = userId;
        }

        #region IDataStore Members

        public async System.Threading.Tasks.Task ClearAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var items = await context.GoogleAuthData
                    .Where(o => o.UserId == _userId)
                    .ToArrayAsync()
                    .ConfigureAwait(false);

                foreach (var item in items)
                {
                    context.GoogleAuthData.Remove(item);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync<T>(string key)
        {
            key = GenerateTypeKey<T>(key);
            using (var context = new ApplicationDbContext())
            {
                var item = await context.GoogleAuthData
                    .FirstOrDefaultAsync(o => o.UserId == _userId && o.Key == key);

                if (item != null)
                {
                    context.GoogleAuthData.Remove(item);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        public async System.Threading.Tasks.Task<T> GetAsync<T>(string key)
        {
            key = GenerateTypeKey<T>(key);
            using (var context = new ApplicationDbContext())
            {
                var item = await context.GoogleAuthData
                    .FirstOrDefaultAsync(o => o.UserId == _userId && o.Key == key)
                    .ConfigureAwait(false);

                return item != null ? JsonConvert.DeserializeObject<T>(item.Value) : default(T);
            }
        }

        public async System.Threading.Tasks.Task StoreAsync<T>(string key, T value)
        {
            key = GenerateTypeKey<T>(key);
            var json = JsonConvert.SerializeObject(value);
            using (var context = new ApplicationDbContext())
            {
                var item = await context.GoogleAuthData
                    .FirstOrDefaultAsync(o => o.UserId == _userId && o.Key == key)
                    .ConfigureAwait(false);
                if (item != null)
                {
                    item.Value = json;
                }
                else
                {
                    item = new GoogleAuthData
                    {
                        UserId = _userId,
                        Key = key,
                        Value = json,
                        Type = typeof(T).FullName,
                        DateCreatedUtc = DateTime.UtcNow                        
                    };
                    context.GoogleAuthData.Add(item);
                }
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private string GenerateTypeKey<T>(string key)
        {
            //Key should be the email address / User.UserName.
            return string.Format("{0}:{1}", key, typeof(T).FullName);
        }

        #endregion
    }
}