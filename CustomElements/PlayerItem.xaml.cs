using System;
using System.Windows.Controls;

namespace CheatMusic_Try2_
{
    /// <summary>
    /// Логика взаимодействия для PlayerItem.xaml
    /// </summary>
    public partial class PlayerItem : UserControl
    {
        public PlayerItem(string title, int count, TimeSpan TotalTime)
        {
            InitializeComponent();
            lblTrack_Name.Text = title;
            Count.Text = Convert.ToString(count);
            lblDuration_Track.Text = TotalTime.ToString(@"mm\:ss");
        }

        public int NumberInLIst
        {
            get { return Convert.ToInt32(Count.Text); }
            set
            {
                Count.Text = Convert.ToString(value + 1);
            }
        }

        public string Path { get; set; }
    }


}
