using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RodnaPamet.Models;

namespace RodnaPamet.Services
{
    public class RestService : IDataStore<RestItem>
    {
        HttpClient _client;
        private string lastError = "";
        private string apiEndPoint = "";

        dynamic items;
        Newtonsoft.Json.Linq.JContainer lastItems;
        List<RestItem> typedItems = new List<RestItem>();
        private string errorHandlingUrl;

        public RestService(string _apiEndPoint)
        {
            _client = new HttpClient();

            items = new List<RestItem>();
            apiEndPoint = _apiEndPoint;
        }

        public async Task<bool> AddItemAsync(RestItem item)
        {
            lastError = "";
            var uri = new Uri(string.Format(apiEndPoint, string.Empty));
            Console.WriteLine(uri.ToString());

                string json = "{}";
                using (var stream = new MemoryStream())
                {
                    DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings
                    {
                        DateTimeFormat = new DateTimeFormat("yyyy-MM-dd HH:mm:ss")
                    };
                    var ser = new DataContractJsonSerializer(typeof(ISerializable), Settings);

                        ser.WriteObject(stream, item);
                        json = Encoding.Default.GetString(stream.ToArray());
                }
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //if (!Guid.Empty.Equals(item.Guid))
                    //{
                    //    response = await _client.PostAsync(uri, content); // Existing
                    //}
                    //else
                    //{

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "RodnaPamet Mobile App");





            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            requestMessage.Content = content;
            HttpResponseMessage response = _client.SendAsync(requestMessage).Result;
            //}

            if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsStringAsync();
                if (resp == "")
                    return false;
                        ServerResponse r = JsonConvert.DeserializeObject<ServerResponse>(resp);

                if (r.Error != null && r.Error == 1)
                {
                    lastError = r.Message;
                }

                items = r.Data;
                        lastItems = items;
                        typedItems = new List<RestItem>((RestItem[])JsonConvert.DeserializeObject<RestItem[]>(r.Data.ToString()));

                    return await Task.FromResult(true);
                }
                else
                {
                    string resp = await response.Content.ReadAsStringAsync();
                    ServerResponse respo = JsonConvert.DeserializeObject<ServerResponse>(resp);
                if(respo != null)
                { 
                    lastError = respo.Message;
                    if (respo.Data != null)
                    {
                        items = respo.Data;
                        lastItems = items;
                        typedItems = new List<RestItem>((RestItem[])JsonConvert.DeserializeObject<RestItem[]>(respo.Data.ToString()));
                    }
                    if (respo.Error != null && respo.Error == 1)
                    {
                        lastError = respo.Message;
                    }
                    Debug.WriteLine(@"\User NOT saved. " + respo.Message);
                }
                else
                    Debug.WriteLine(@"\tUser NOT saved. Error thrown by server.");
            }
            return false;
        }

        public string GetLastMessage()
        {
            return lastError;
        }
        public List<RestItem> ItemList { get { return typedItems; } }

        public async Task<bool> UpdateItemAsync(RestItem item)
        {
            RestItem oldItem = null;
            for(int i = 0; i < ((List<RestItem>)items).Count; i++)
                if(item.Id.Equals(items[i].Id))
                    items.Remove(items[i]);
            items.Add(item);

            var uri = new Uri(string.Format(apiEndPoint));
            Console.WriteLine(uri.ToString());

                HttpContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "RodnaPamet Mobile App");





            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            requestMessage.Content = content;
            HttpResponseMessage response = _client.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
                {
                ServerResponse resp = JsonConvert.DeserializeObject<ServerResponse>(response.Content.ReadAsStringAsync().Result);
                if (resp.Success == 1)
                    lastError = "";
                else
                    lastError = resp.Message;
                    return resp.Success == 1;
                }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var uri = new Uri(string.Format(apiEndPoint));
            Console.WriteLine(uri.ToString());

            RestItem Rem = new RestItem();
            Rem.Id = id;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Rem), Encoding.UTF8, "application/json");

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "Midalidare Mobile App");

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            requestMessage.Content = content;
            HttpResponseMessage response = _client.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                ServerResponse resp = JsonConvert.DeserializeObject<ServerResponse>(response.Content.ReadAsStringAsync().Result);
                if (resp.Success == 1)
                {
                    lastError = "";

                    for (int i = 0; i < ((List<RestItem>)items).Count; i++)
                        if (id.Equals(items[i].Id))
                            items.Remove(items[i]);
                }
                else
                    lastError = resp.Message;
                return resp.Success == 1;
            }

            return await Task.FromResult(true);
        }

        public async Task<RestItem> GetItemAsync(string id)
        {
            ArrayList vars = new ArrayList();
            vars.Add("Id=" + id.ToString());

            var uri = new Uri(string.Format(apiEndPoint + "?Id=" + id.ToString()));
            Console.WriteLine(uri.ToString());


            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "RodnaPamet Mobile App");





            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = _client.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return await Task.FromResult((RestItem) JsonConvert.DeserializeObject<IList> (JsonConvert.DeserializeObject<ServerResponse>(content).Data.ToString())[0]);
                }

            return null;
        }

        public async Task<dynamic> InternalGetItemsAsync(KeyValuePair<string, object>[] filter, bool forceRefresh = false)
        {
            List<string> vars = new List<string>();
            foreach (KeyValuePair<string, object> kv in filter)
                if (kv.Key != null)
                    vars.Add(kv.Key+"="+kv.Value.ToString());

            string uri = apiEndPoint;
            if (vars.Count > 0)
                uri += "?" + string.Join("&", vars.ToArray());

//            if (Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
//                return "[]";

                TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
                {
                    eventArgs.SetObserved();
                    ((AggregateException)eventArgs.Exception).Handle(ex =>
                    {
                        Console.WriteLine("Exception type: {0}", ex.GetType());
                        return true;
                    });
                };

                _client.DefaultRequestHeaders.Add("Accept", "application/json");
                _client.DefaultRequestHeaders.Add("User-Agent", "RodnaPamet Mobile App");


            
            

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            HttpResponseMessage rm = _client.SendAsync(requestMessage).Result;


            //rm.OnCompleted = <lambda expression >;
            if (rm.IsSuccessStatusCode)
                {
                    dynamic list = (IList)JsonConvert.DeserializeObject(JsonConvert.DeserializeObject<ServerResponse>(await rm.Content.ReadAsStringAsync()).Data.ToString());
                    // TODO: add the type dynamically, so this sh*t knows what its dealing with...
                    return await Task.FromResult(list);
                }
                else
                {
                    Debug.WriteLine("RestService out call result " + rm.StatusCode.ToString());
                    Debug.WriteLine(rm.ReasonPhrase);
                }

            return "[]";
        }

        public async Task<dynamic> PlainGetItemsAsync(KeyValuePair<string, object>[] filter, bool forceRefresh = false)
        {
            List<string> vars = new List<string>();
            foreach (KeyValuePair<string, object> kv in filter)
                if (kv.Key != null)
                    vars.Add(kv.Key + "=" + kv.Value.ToString());

            string uri = apiEndPoint;
            if (vars.Count > 0)
                uri += "?" + string.Join("&", vars.ToArray());



                TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
                {
                    eventArgs.SetObserved();
                    ((AggregateException)eventArgs.Exception).Handle(ex =>
                    {
                        Console.WriteLine("Exception type: {0}", ex.GetType());
                        return true;
                    });
                };

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "RodnaPamet Mobile App");





            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage rm = _client.SendAsync(requestMessage).Result;

            //rm.OnCompleted = <lambda expression >;
            if (rm.IsSuccessStatusCode)
                {
                    dynamic list = (IList)JsonConvert.DeserializeObject(await rm.Content.ReadAsStringAsync());
                    // TODO: add the type dynamically, so this shit knows what its dealing with...
                    return await Task.FromResult(list);
                }
                else
                {
                    Debug.WriteLine("RestService out call result " + rm.StatusCode.ToString());
                    Debug.WriteLine(rm.ReasonPhrase);
                }

            return "[]";
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256Managed.Create())
            {

                //var enc = Encoding.GetEncoding(1255);
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.ASCII.GetBytes(rawData));

                //Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public Task<IEnumerable<RestItem>> GetItemsAsync(KeyValuePair<string, object>[] item, bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetCredentials(string c1, string c2)
        {
            MD5 md5Hash = MD5.Create();
            string hash = ComputeSha256Hash(c1 + ":" + c2);

            var authData = string.Format("{0}:{1}", c1, c2);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            items = new List<RestItem>();

            return Task.FromResult<bool>(true);
        }

        public Newtonsoft.Json.Linq.JContainer LastItems
        {
            get {
                return lastItems;
            }
        }

        public string Language { get; set; }
    }
    public enum RestServiceType
    { 
        Users,
        Messages,
        Other
    }
}