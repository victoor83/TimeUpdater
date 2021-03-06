﻿using System;
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

            //Save new times to broker file
            List<string> lines = File.ReadAllLines(FilePaths.BrokerTimeFile).ToList();

            int i = 0;
            foreach(var dateTime in dateTimes)
            {
                TimeType timeType = (i == 0 || i == 2) ? TimeType.Start : TimeType.Stop;

                lines.Insert(lines.Count - 1, GetTimeLine(dateTime, timeType));

                i++;
            }

            File.WriteAllLines(FilePaths.BrokerTimeFile, lines);

            //Copy broker file to WordTimeRecorder
            File.Copy(FilePaths.BrokerTimeFile, FilePaths.TimeFilePath, true);
        }

        private string GetTimeLine(double eventTime, TimeType timeType)
        {
            return $"<Event time = '{eventTime}' type='{timeType}' description='Magnus'/>";
        }

        private void CreateBackupFile()
        {
            FileInfo fInfo = new FileInfo(Resources.ResourceManager.GetString("TimeFileName"));
            var fileName = Path.GetFileNameWithoutExtension(fInfo.Name);

            var timeFile = Path.Combine(FilePaths.BackupFileFolder, $"{fileName}_{DateTime.Now:dd_MM_yyyy}{fInfo.Extension}");
            File.Copy(FilePaths.TimeFilePath, Path.Combine(FilePaths.BackupFileFolder, timeFile), true);
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
