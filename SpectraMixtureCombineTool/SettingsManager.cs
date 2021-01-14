﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SpectraMixtureCombineTool.WPF
{
    internal class SettingsManager<T>
        where T : class, new()
    {
        private readonly string filePath;

        public SettingsManager(string fileName, string folderName)
        {
            var directory = GetOrCreateDirectory(folderName);
            filePath = Path.Combine(directory.FullName, fileName);
            if (!File.Exists(filePath))
            {
                SaveSettings(new T());
            }
        }

        private DirectoryInfo GetOrCreateDirectory(string folderName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = Path.Combine(appData, "Aunir", folderName);
            return Directory.CreateDirectory(directoryPath);
        }

        public T LoadSettings()
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(filePath));
        }

        public void SaveSettings(T settings)
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(filePath, json);
        }
    }
}
