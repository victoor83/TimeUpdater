namespace TimeUpdater
{
    public class Paths
    {
        public string TimeFilePath { get; set; }

        public string BackupFileFolder { get; set; }

        /// <summary>
        /// The changes will be saved first here, because target time file is overriden during start by program
        /// </summary>
        public string BrokerTimeFile { get; set; }

        public string ExcelTimeFile { get; set; }
    }
}
