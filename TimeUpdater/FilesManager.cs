using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TimeUpdater
{
    public class FilesManager
    {
        private const string ConfigFile = "config.json";

        public FilesManager()
        {
            FilePaths = LoadConfigFile();
        }

        public Paths FilePaths { get; }

        public void SaveTimesForSingleDay(List<double> dateTimes)
        {
            CreateBackupFile();

            List<string> lines = File.ReadAllLines(FilePaths.TimeFilePath).ToList();

            int i = 0;
            foreach(var dateTime in dateTimes)
            {
                TimeType timeType = (i == 0 || i == 2) ? TimeType.Start : TimeType.Stop;

                lines.Insert(lines.Count - 1, GetTimeLine(dateTime, timeType));

                i++;
            }

            File.WriteAllLines(FilePaths.TimeFilePath, lines);
        }

        private string GetTimeLine(double eventTime, TimeType timeType)
        {
            return $"<Event time = '{eventTime}' type='{timeType}' description='Magnus'/>";
        }

        private void CreateBackupFile()
        {
            var timeFile = Path.Combine(FilePaths.BackupFileFolder, $"{Resources.ResourceManager.GetString("TimeFileName")}_{DateTime.Now:dd_MM_yyyy}");
            File.Copy(FilePaths.TimeFilePath, Path.Combine(FilePaths.BackupFileFolder, timeFile));
        }

        private Paths LoadConfigFile()
        {
            Paths paths = null;
            // deserialize JSON directly from a file
            using(StreamReader file = File.OpenText(ConfigFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                paths = (Paths)serializer.Deserialize(file, typeof(Paths));
            }

            return paths;
        }
    }
}
