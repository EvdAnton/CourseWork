using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using NAudio.Wave;
using System.Net;
using System.IO;
using VideoLibrary;
using MediaToolkit.Model;
using MediaToolkit;

namespace CheatMusic_Try2_
{
    /// <summary> 
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {

        List<PlayerItem> items = new List<PlayerItem>();
        private MediaElement media_Element = new MediaElement();
        bool Repeat = false;

        public MainWindow()
        {
            InitializeComponent();
            HiddenForm.Click += (s, e) => WindowState = WindowState.Minimized;
            PowerOff.Click += (s, e) => Close();
        }
       
     

        void timer_Tick(object sender, EventArgs e)
        {
            Volum.Value = media_Element.Position.TotalSeconds;
            Current_Duration.Text = media_Element.Position.ToString(@"mm\:ss");
        }


        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            media_Element.Pause();
            Stop.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }

       

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            media_Element.Source = new Uri((sender as PlayerItem).Path,UriKind.RelativeOrAbsolute);
            media_Element.LoadedBehavior = MediaState.Manual;
            media_Element.UnloadedBehavior = MediaState.Manual;
            media_Element.Play();
            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;
        }


        private void Click_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (media_Element.Source != null)
            {
                media_Element.Play();
                Play.Visibility = Visibility.Hidden;
                Stop.Visibility = Visibility.Visible;
            }
        }

        private void SwapItemsInList(int i, int itemsIndex)
        {
            var temp = items[i];
            items[i] = items[itemsIndex];
            items[itemsIndex] = temp;
        }

        private void Shaker_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for(int i = 0; i < items.Count; i++)
            {
                int itemsIndex = rnd.Next(items.Count);
                SwapItemsInList(i, itemsIndex);
            }
            ListMusic.Items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].NumberInLIst = i;
                ListMusic.Items.Add(items[i]);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Choose_Track(object sender, RoutedEventArgs e)
        {
            if (media_Element.Source != null)
                media_Element.Stop();
            media_Element.Volume = Volume_Slider.Value;
            string MyPath = (sender as PlayerItem).Path;
            Track_name.Text = Path.GetFileNameWithoutExtension(MyPath);
            media_Element.Source = new Uri(MyPath, UriKind.RelativeOrAbsolute);
            media_Element.LoadedBehavior = MediaState.Manual;
            media_Element.UnloadedBehavior = MediaState.Manual;
            media_Element.MediaOpened += GetDuration_MediaOpened;
            media_Element.Play();

            Volum.Value = media_Element.Position.TotalSeconds;
            Current_Duration.Text = media_Element.Position.ToString(@"mm\:ss");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;

        }

        void GetDuration_MediaOpened(object sender, RoutedEventArgs e)
        {
            Volum.Maximum = media_Element.NaturalDuration.TimeSpan.TotalSeconds;
            TimeSpan Ts = TimeSpan.FromSeconds(Math.Round(media_Element.NaturalDuration.TimeSpan.TotalSeconds));
            Full_Duration.Text = Ts.ToString(@"mm\:ss");

        }


        private void ListMusic_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] DropPath = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (string dropfilepath in DropPath)
                    AddMusic(dropfilepath);
            }
        }       

        private void AddMusic(string dropfilepath)
        {
            if ((Path.GetExtension(dropfilepath).Contains(".mp3")) || (Path.GetExtension(dropfilepath).Contains(".wav")))
            {
                Mp3FileReader reader = new Mp3FileReader(Path.GetFullPath(dropfilepath));
                TimeSpan Ts = reader.TotalTime;
                PlayerItem pi = new PlayerItem(Path.GetFileNameWithoutExtension(dropfilepath), items.Count + 1, Ts);
                pi.Path = Path.GetFullPath(dropfilepath);
                bool IsFind = Find_in_List(pi);
                if (!IsFind)
                {
                    pi.TrackItem.MouseDown += Choose_Track;
                    items.Add(pi);
                    ListMusic.Items.Add(pi);
                }
            }
        }

        private bool Find_in_List(PlayerItem item)
        {
            bool isFind = false;
            foreach(var allitems in items)
            {
                if (allitems.Path == item.Path)
                    isFind = true;
            }
            return isFind;
        }

        private void Next_Track_Click(object sender, RoutedEventArgs e)
        {
            if (media_Element.Source != null)
            {
                bool IsFind = false;
                int i = 0;
                if (items.Count > 1)
                {
                    do
                    {
                        if (items[i].Path == media_Element.Source.LocalPath.ToString())
                            IsFind = true;

                        i++;
                    } while ((i < items.Count - 1) && (!IsFind));
                    if (i < items.Count)
                        media_Element.Source = new Uri(items[i].Path, UriKind.RelativeOrAbsolute);
                    else
                        media_Element.Stop();
                }
                else
                    media_Element.Stop();
                media_Element.LoadedBehavior = MediaState.Manual;
                media_Element.UnloadedBehavior = MediaState.Manual;

                Track_name.Text = System.IO.Path.GetFileNameWithoutExtension(items[i].Path);
                Volum.Value = media_Element.Position.TotalSeconds;
                Current_Duration.Text = media_Element.Position.ToString(@"mm\:ss");

                media_Element.Play();
            }
        }

        private void Pred_Track_Click(object sender, RoutedEventArgs e)
        {
            if (media_Element.Source != null)
            {
                bool IsFind = false;
                int i = items.Count - 1;
                if (i > 0)
                {
                    
                    do
                    {
                        if (items[i].Path == media_Element.Source.LocalPath.ToString())
                            IsFind = true;

                        i--;
                    } while ((i > 0) && (!IsFind));
                    if (i >= 0)
                        media_Element.Source = new Uri(items[i].Path, UriKind.RelativeOrAbsolute);
                    else
                        media_Element.Stop();
                }
                else
                    media_Element.Stop();
                media_Element.LoadedBehavior = MediaState.Manual;
                media_Element.UnloadedBehavior = MediaState.Manual;

                Track_name.Text = System.IO.Path.GetFileNameWithoutExtension(items[i].Path);
                Volum.Value = media_Element.Position.TotalSeconds;
                Current_Duration.Text = media_Element.Position.ToString(@"mm\:ss");

                media_Element.Play();
            }
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            if (media_Element.Source != null)
            {
                if (Replay.IsChecked.Value)
                {
                    Repeat = true;
                    Replay.Background = Brushes.DimGray;
                }
                else
                    Replay.Background = Brushes.Black;
            }
        }

        private void Volum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (media_Element.Source != null)
            {
                TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
                media_Element.Position = ts;
                
                if ((Current_Duration.Text == Full_Duration.Text)&&(!Repeat))
                    Next_Track_Click(sender, e);
                else if ((Current_Duration.Text == Full_Duration.Text) && (Repeat))
                {
                    media_Element.Stop();
                    media_Element.Play();
                }
                Volum.Value = media_Element.Position.TotalSeconds;
                Current_Duration.Text = media_Element.Position.ToString(@"mm\:ss");

            }
        }

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            if (Volume.IsChecked.Value)
                Volume_Slider.Visibility = Visibility.Visible;
            else
                Volume_Slider.Visibility = Visibility.Hidden;
        }

        private void Volume_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (media_Element.Source != null)
            {
                media_Element.Volume = Volume_Slider.Value;
                if (media_Element.Volume == 0)
                {
                    Volume.Visibility = Visibility.Hidden;
                    Mute.Visibility = Visibility.Visible;
                }
                else
                {
                    Volume.Visibility = Visibility.Visible;
                    Mute.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (!Mute.IsChecked.Value)
                Volume_Slider.Visibility = Visibility.Hidden;
            else
                Volume_Slider.Visibility = Visibility.Visible;
        }

        private void SearchField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchField.Text == "Search for music in Internet")
            {
                SearchField.Text = "";
                SearchField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0C4D9"));
            }
        }

        private void SearchField_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchField.Text = "Search for music in Internet";
            SearchField.Foreground = Brushes.Gray;
        }

        private string MyParse(string songname)
        {
            string result = "";
            for(int i = 0; i < songname.Length; i++)
            {
                if (songname[i] == ' ')
                {
                    result += "+";
                }
                else
                    result += songname[i];
            }
            return result;
        }


        private void SearchField_KeyDown(object sender, KeyEventArgs e)
        {
            string answer = "";
            string SongName = SearchField.Text;

            if ((e.Key == Key.Enter)&&(SongName != "Search for music in Internet")&&(SongName != ""))
            {
                var MyRequest = MyParse(SongName);
                string url = $"http://ws.audioscrobbler.com/2.0/?method=track.search&track={MyRequest}&api_key=57ee3318536b23ee81d6b27e36997cde&format=json&limit=1";

                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    string response;
                    try
                    {
                        var streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                        response = streamReader.ReadToEnd();
                        string subString = "https://www.last.fm/music/";
                        answer = GetUrlFromResponse(response, subString);

                        httpWebRequest = (HttpWebRequest)WebRequest.Create(answer);
                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        streamReader = new StreamReader(httpWebResponse.GetResponseStream());

                        response = streamReader.ReadToEnd();
                        subString = "https://www.youtube.com/watch";
                        answer = GetUrlFromResponse(response, subString);

                        SaveMP3Async(answer);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show($"\"{SongName}\" not found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No connection to the internet");
                }

            }
        }

        private string GetUrlFromResponse(string response, string subString)
        {
            string answer = "";
            
            int indexOfSubstring = response.IndexOf(subString);
            if (indexOfSubstring > 0)
            {
                int i = indexOfSubstring;
                char findSymbol = '"';
                while ((response[i] != findSymbol) && (i < response.Length))
                {
                    answer += response[i];
                    i++;
                }
                if (i >= response.Length)
                    throw new ArgumentException();

                return answer;
            }
            else
                throw new ArgumentException();
        }

        private async void SaveMP3Async(string VideoURL)
        {
            try
            {
                var youtube = YouTube.Default;
                var vid = await youtube.GetVideoAsync(VideoURL);

                File.WriteAllBytes(vid.FullName, await vid.GetBytesAsync());

                var inputFile = new MediaFile { Filename = vid.FullName };
                var outputFile = new MediaFile { Filename = $"{vid.FullName}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    engine.Convert(inputFile, outputFile);
                }
                AddMusic(outputFile.Filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Your song not found");
            }
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

