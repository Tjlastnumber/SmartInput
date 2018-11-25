using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInput
{
    public static class FileHelper
    {
        private readonly static string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SmartInput");
        public static string FileName = "config.js";
        public static string FilePath = Path.Combine(FolderPath, FileName);

        public static void SaveJosn(object o)
        {
            string jsonString = JsonConvert.SerializeObject(o);
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            File.WriteAllText(FilePath, jsonString, Encoding.UTF8);
        }

        public static T ReadJosn<T>(string path = "")
        {
            var jsonPath = !string.IsNullOrWhiteSpace(path) ? path : FilePath;
            if (File.Exists(jsonPath))
            {
                string jsonString = File.ReadAllText(jsonPath);

                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            return default(T);
        }

        public static Task<T> AsyncReadJosn<T>(string path = "")
        {
            return Task.Run(() =>
            {
                var jsonPath = !string.IsNullOrWhiteSpace(path) ? path : FilePath;
                if (File.Exists(jsonPath))
                {
                    string jsonString = File.ReadAllText(jsonPath);

                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                return default(T);
            });
        }
    }
}
