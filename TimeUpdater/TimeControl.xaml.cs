using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TimeUpdater
{
    /// <summary>
    /// Interaction logic for TimeControl.xaml
    /// </summary>
    public partial class TimeControl : UserControl
    {
        public TimeControl()
        {
            InitializeComponent();
            cmbHours.ItemsSource = GetHours();
            cmbMinutes.ItemsSource = GetMinutes();

            int randomSecond;
            cmbSeconds.ItemsSource = GetSeconds(out randomSecond);
            cmbSeconds.Text = randomSecond.ToString();
        }

        public void SetCaption(string text)
        {
            labTitel.Content = text;
        }

        private List<int> GetHours()
        {
            var hours = new List<int>();
            for(int i = 0; i <= 24; i++)
            {
                hours.Add(i);
            }

            return hours;
        }

        private List<int> GetMinutes()
        {
            var hours = new List<int>();
            for(int i = 0; i <= 59; i++)
            {
                hours.Add(i);
            }

            return hours;
        }

        private List<int> GetSeconds(out int randomSecond)
        {
            var seconds = new List<int>(); //todo: random
            for(int i = 0; i <= 59; i++)
            {
                seconds.Add(i);
            }

            randomSecond = seconds[new Random().Next(seconds.Count)];

            return seconds;
        }
    }
}
