using RodnaPamet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace RodnaPamet.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
            };

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(path, "rpdata");
            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter form = new BinaryFormatter();
                        IList<Item> s = (IList<Item>)form.Deserialize(fs);
                        items = (List<Item>) s;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void Persist()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = Path.Combine(path, "rpdata");
            try
            {
                BinaryFormatter form = new BinaryFormatter();
                using (var fs = File.Open(filePath, FileMode.Create, FileAccess.Write))
                {
                    form.Serialize(fs, items);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);
            Persist();
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
            Persist();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => id == arg.Id).FirstOrDefault();
            items.Remove(oldItem);
            Persist();
            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(KeyValuePair<string, object>[] item, bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}