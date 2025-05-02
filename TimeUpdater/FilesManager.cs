using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace TimeUpdater
{
    public class FilesManager
    {
        private const string _CONFIG_FILE = "config.json";

        public FilesManager()
        {
            FilePaths = LoadConfigFile();
        }

        public Paths FilePaths { get; }

        public bool SaveTimesForSingleDay(List<double> dateTimes)
        {
            try
            {
                CreateBackupFile();

                //Save new times to broker file
                List<string> lines = File.ReadAllLines(FilePaths.BrokerTimeFile).ToList();

                int i = 0;
                foreach(var dateTime in dateTimes)
                {
                    //0 - start work  1 - start break  2- end break (start work time)  3 - end workday
                    TimeType timeType = (i == 0 || i == 2) ? TimeType.Start : TimeType.Stop;

                    lines.Insert(lines.Count - 1, GetTimeLine(dateTime, timeType));

                    i++;
                }

                File.WriteAllLines(FilePaths.BrokerTimeFile, lines);

                //Copy broker file to WordTimeRecorder
                File.Copy(FilePaths.BrokerTimeFile, FilePaths.TimeFilePath, true);

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

        public int GetLastSavedTime()
        {
            if(!File.Exists(FilePaths.BrokerTimeFile))
            {
                MessageBox.Show($"File '{FilePaths.BrokerTimeFile}' doesn't exists.");
                return -1;
            }

            var lastLine = File.ReadLines(FilePaths.BrokerTimeFile).TakeLast(2).SkipLast(1).First();
            //Get number (unix time)  from string
            var unixTime = Convert.ToInt32(new string(lastLine.Where(char.IsDigit).ToArray()));

            return unixTime;
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
            using(StreamReader file = File.OpenText(_CONFIG_FILE))
            {
                JsonSerializer serializer = new JsonSerializer();
                paths = (Paths)serializer.Deserialize(file, typeof(Paths));
            }

            return paths;
        }
    }
}
