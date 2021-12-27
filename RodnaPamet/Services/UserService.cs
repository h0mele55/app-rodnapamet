using Newtonsoft.Json;
using RodnaPamet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RodnaPamet.Services
{
    public class UserService
    {
        private RestService rest;
        public List<User> SubscribersList;

        public string Language { get; set; }

        public UserService(string key1, string key2)
        {
            rest = new RestService(Constants.UsersUrl);
            SetCredentials(key1, key2);
            SubscribersList = new List<User>();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(path, "rodna");
            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter form = new BinaryFormatter();
                        List<User> res = (List<User>) form.Deserialize(fs);
                        SubscribersList = res;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public async Task<bool> AuthorizeUserAsync(string mail, string pass)
        {
            RestService arest = new RestService(Constants.AuthUrl);
            User item = new User();
            item.EMail = mail;
            item.Password = pass;
            bool res = await arest.UpdateItemAsync(item);
            return res;
        }

        public async Task<bool> ConfirmEMailAsync(string mail, string code)
        {
            RestService arest = new RestService(Constants.ConfirmUrl);
            User item = new User();
            item.EMail = mail;
            item.VerificationCode = code;
            bool res = await arest.UpdateItemAsync(item);
            return res;
        }

        public async Task<bool> CheckUserAsync(string mail)
        {
            RestService arest = new RestService(Constants.CheckUrl);
            User item = new User();
            item.EMail = mail;
            bool res = await arest.AddItemAsync(item);
            SubscribersList.Clear();
            if (res)
            { 
                SubscribersList.Add(new List<User>((User[])JsonConvert.DeserializeObject<User[]>(arest.LastItems.ToString()))[0]);
            }
            return res;
        }

        public bool Persist()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(path, "rodna");
            try
            {
                BinaryFormatter form = new BinaryFormatter();
                using (var fs = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    form.Serialize(fs, SubscribersList);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> SetCredentials(string c1, string c2)
        {
            return rest.SetCredentials(c1, c2);
        }

        public async Task<bool> AddItemAsync(User item)
        {
            if (await rest.AddItemAsync(item))
            {
                SubscribersList = new List<User>(JsonConvert.DeserializeObject<User[]>(rest.LastItems.ToString()));
                return true;
            }
            else if(rest.LastItems != null)
            {
                try
                {
                    SubscribersList = new List<User>(JsonConvert.DeserializeObject<User[]>(rest.LastItems.ToString()));
                }
                catch (Exception ex)
                { 
                
                }
            }
            return false;
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            return rest.DeleteItemAsync(id);
        }

        public async Task<User> GetItemAsync(string id)
        {
            KeyValuePair<string, object>[] filter = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("Id", id) };

            try
            {
                dynamic t = await rest.InternalGetItemsAsync(filter, true);
                User[] ts = JsonConvert.DeserializeObject<User[]>(t.ToString());
                SubscribersList = new List<User>(ts);
                if (ts.Length > 0)
                    return await Task.FromResult(ts[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                App.HasCommunicationError = true;
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetItemsAsync(KeyValuePair<string, object>[] filter, bool forceRefresh = false)
        {
            User[] ts;
            try
            {
                dynamic t = await rest.InternalGetItemsAsync(filter, forceRefresh);
                ts = JsonConvert.DeserializeObject<User[]>(t.ToString());
            }
            catch (Exception ex)
            {
                ts = new User[0];
                App.HasCommunicationError = true;
            }
            SubscribersList = new List<User>(ts);
            return await Task.FromResult(ts);
        }

        public Task<dynamic> InternalGetItemsAsync(KeyValuePair<string, object>[] item, bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(User item)
        {
            try
            {
                if (await rest.UpdateItemAsync(item))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                App.HasCommunicationError = true;
                return false;
            }
            return false;
        }

        public string GetLastMessage()
        {
            return rest.GetLastMessage();
        }
    }
}
