using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading;

namespace CheatMusic_Try2_
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class SongText : Window
    {
        public SongText()
        {
            InitializeComponent();
        }

        private void ButtonTextClose_Click(object sender, RoutedEventArgs e)
        {
            var callback = new TimerCallback(CloseForm);
            Timer timer = new Timer(callback, 0, 0, 300);
        }

        private void CloseForm(object obj)
        {
            Thread.Sleep(300);
            PlayerItem.WindowTextActivate = false;
            MainWindow.myFormText = null;
            Dispatcher.BeginInvoke(new ThreadStart(delegate { TextWindow.Close(); }));
        }
    }
}
