using System;
using System.Collections.Generic;
using System.Windows;

namespace TimeUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilesManager _filesManager;

        public MainWindow()
        {
            InitializeComponent();

            SetDefaultTimes();
            SetDefaultDate();

            _filesManager = new FilesManager();
            labFilePath.Content = _filesManager.FilePaths.TimeFilePath;
            labBackupFolder.Content = _filesManager.FilePaths.BackupFileFolder;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var datetime = Dp_Date.SelectedDate;

            if(datetime == null)
            {
                MessageBox.Show("Date is not selected.", "Time error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            else
            {
                _filesManager.SaveTimesForSingleDay(TimeConverter.ConvertToUnixDateTime(GetDateTimes()));
            }
        }

        private void SetDefaultDate()
        {
            DateTime defaultDate = DateTime.Today.AddDays(-1);
            DayOfWeek today = DateTime.Today.DayOfWeek;

            if(DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                defaultDate.AddDays(-2); //if monday set last friday
            }

            Dp_Date.SelectedDate = defaultDate;
        }

        private void SetDefaultTimes()
        {
            UcStart.SetCaption("Start");
            UcStart.cmbHours.Text = 7.ToString();
            UcStart.cmbMinutes.Text = 0.ToString();

            UcPauseBegin.SetCaption("Pause begin");
            UcPauseBegin.cmbHours.Text = 13.ToString();
            UcPauseBegin.cmbMinutes.Text = 0.ToString();

            UcPauseEnd.SetCaption("Pause end");
            UcPauseEnd.cmbHours.Text = 13.ToString();
            UcPauseEnd.cmbMinutes.Text = 0.ToString();

            UcEnd.SetCaption("End");
            UcEnd.cmbHours.Text = 16.ToString();
            UcEnd.cmbMinutes.Text = 0.ToString();
        }

        private List<DateTime> GetDateTimes()
        {
            //Start
            DateTime time1 = new DateTime(Dp_Date.SelectedDate.Value.Year, Dp_Date.SelectedDate.Value.Month, Dp_Date.SelectedDate.Value.Day, Convert.ToInt32(UcStart.cmbHours.Text),
                Convert.ToInt32(UcStart.cmbMinutes.Text), Convert.ToInt32(UcStart.cmbSeconds.Text), DateTimeKind.Local);

            //Pause begin
            DateTime time2 = new DateTime(Dp_Date.SelectedDate.Value.Year, Dp_Date.SelectedDate.Value.Month, Dp_Date.SelectedDate.Value.Day,
                Convert.ToInt32(UcPauseBegin.cmbHours.Text), Convert.ToInt32(UcPauseBegin.cmbMinutes.Text), Convert.ToInt32(UcPauseBegin.cmbSeconds.Text), DateTimeKind.Local);

            //Pause end
            DateTime time3 = new DateTime(Dp_Date.SelectedDate.Value.Year, Dp_Date.SelectedDate.Value.Month, Dp_Date.SelectedDate.Value.Day,
                Convert.ToInt32(UcPauseEnd.cmbHours.Text), Convert.ToInt32(UcPauseEnd.cmbMinutes.Text), Convert.ToInt32(UcPauseEnd.cmbSeconds.Text), DateTimeKind.Local);

            //End
            DateTime time4 = new DateTime(Dp_Date.SelectedDate.Value.Year, Dp_Date.SelectedDate.Value.Month, Dp_Date.SelectedDate.Value.Day, Convert.ToInt32(UcEnd.cmbHours.Text),
                Convert.ToInt32(UcEnd.cmbMinutes.Text), Convert.ToInt32(UcEnd.cmbSeconds.Text), DateTimeKind.Local);

            return new List<DateTime> {time1, time2, time3, time4};
        }
    }
}
