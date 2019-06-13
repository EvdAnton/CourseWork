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
using System.Windows.Documents;
using System.Threading;

namespace CheatMusic_Try2_
{
    /// <summary> 
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {

        private static AutoResetEvent waitHandle = new AutoResetEvent(false);


        const char SONG_SEPARATOR = ' ';
        const char DOWN_SEPARATOR = '_';

        public static SongText myFormText = null;

        List<PlayerItem> items = new List<PlayerItem>();
        private MediaElement mediaElement = new MediaElement();

        bool isRepeat = false;

        public MainWindow()
        {
            InitializeComponent();


            HiddenForm.Click += (s, e) => WindowState = WindowState.Minimized;
            PowerOff.Click += (s, e) => {
                if (myFormText != null)
                    myFormText.Close();
                Close();
            };
        }
     

        private void Timer_Tick(object sender, EventArgs e)
        {
            volum.Value = mediaElement.Position.TotalSeconds;
            currentDuration.Text = mediaElement.Position.ToString(@"mm\:ss");
        }


        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
            Stop.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
        }
       

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri((sender as PlayerItem).Path,UriKind.RelativeOrAbsolute);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.Play();
            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                mediaElement.Play();
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
            listMusic.Items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].NumberInLIst = i;
                listMusic.Items.Add(items[i]);
            }
        }

        private void ChooseTrack_MouseDown(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
                mediaElement.Stop();
            mediaElement.Volume = volumeSlider.Value;

            var myItem = (sender as PlayerItem);
            string myPath = myItem.Path;
            trackName.Text = myItem.songName;

            mediaElement.Source = new Uri(myPath, UriKind.RelativeOrAbsolute);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;

            mediaElement.MediaOpened += GetDuration_MediaOpened;
            mediaElement.Play();

            volum.Value = mediaElement.Position.TotalSeconds;
            currentDuration.Text = mediaElement.Position.ToString(@"mm\:ss");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            Play.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Visible;

        }

        void GetDuration_MediaOpened(object sender, RoutedEventArgs e)
        {
            volum.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            TimeSpan ts = TimeSpan.FromSeconds(volum.Maximum);
            Full_Duration.Text = ts.ToString(@"mm\:ss");
        }


        private void ListMusic_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] dropPath = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (string dropFilePath in dropPath)
                    AddMusic(dropFilePath);
            }
        }

        private void AddMusic(string dropFilePath)
        {
            if (Path.GetExtension(dropFilePath).Contains(".mp3") || Path.GetExtension(dropFilePath).Contains(".wav"))
            {
                Mp3FileReader reader = new Mp3FileReader(Path.GetFullPath(dropFilePath));
                TimeSpan ts = reader.TotalTime;
                PlayerItem pi = new PlayerItem(Path.GetFileNameWithoutExtension(dropFilePath), items.Count + 1, ts);
                pi.Path = Path.GetFullPath(dropFilePath);
                bool IsFind = FindInList(pi);
                if (!IsFind)
                {
                    pi.TrackItem.MouseDown += ChooseTrack_MouseDown;
                    pi.TrackItem.MouseWheel += GetSongText_MouseWheel;
                    items.Add(pi);
                    listMusic.Items.Add(pi);
                }
            }
        }

        private bool FindInList(PlayerItem item)
        {
            bool isFind = false;
            foreach(var allItems in items)
            {
                if (allItems.Path == item.Path)
                    isFind = true;
            }
            return isFind;
        }

        private void NextTrack_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                bool IsFind = false;
                int i = 0;
                if (items.Count > 1)
                {
                    do
                    {
                        if (items[i].Path == mediaElement.Source.LocalPath.ToString())
                            IsFind = true;

                        i++;
                    } while ((i < items.Count - 1) && (!IsFind));
                    if (i < items.Count)
                        mediaElement.Source = new Uri(items[i].Path, UriKind.RelativeOrAbsolute);
                    else
                        mediaElement.Stop();
                }
                else
                    mediaElement.Stop();
                mediaElement.LoadedBehavior = MediaState.Manual;
                mediaElement.UnloadedBehavior = MediaState.Manual;

                trackName.Text = Path.GetFileNameWithoutExtension(items[i].Path);
                volum.Value = mediaElement.Position.TotalSeconds;
                currentDuration.Text = mediaElement.Position.ToString(@"mm\:ss");

                mediaElement.Play();
            }
        }

        private void PredTrack_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                bool isFind = false;
                int i = items.Count - 1;
                if (i > 0)
                {
                    do
                    {
                        if (items[i].Path == mediaElement.Source.LocalPath.ToString())
                            isFind = true;

                        i--;
                    } while ((i > 0) && (!isFind));
                    if (i >= 0)
                        mediaElement.Source = new Uri(items[i].Path, UriKind.RelativeOrAbsolute);
                    else
                        mediaElement.Stop();
                }
                else
                    mediaElement.Stop();
                mediaElement.LoadedBehavior = MediaState.Manual;
                mediaElement.UnloadedBehavior = MediaState.Manual;

                trackName.Text = Path.GetFileNameWithoutExtension(items[i].Path);
                volum.Value = mediaElement.Position.TotalSeconds;
                currentDuration.Text = mediaElement.Position.ToString(@"mm\:ss");

                mediaElement.Play();
            }
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Source != null)
            {
                if (Replay.IsChecked.Value)
                {
                    isRepeat = true;
                    Replay.Background = Brushes.DimGray;
                }
                else
                    Replay.Background = Brushes.Black;
            }
        }

        private void Volum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement.Source != null)
            {
                TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
                mediaElement.Position = ts;

                if ((volum.Value == volum.Maximum) && (isRepeat))
                {
                    mediaElement.Stop();
                    mediaElement.Play();
                }
                if ((currentDuration.Text == Full_Duration.Text)&&(!isRepeat))
                    NextTrack_Click(sender, e);
                
                volum.Value = mediaElement.Position.TotalSeconds;
                currentDuration.Text = mediaElement.Position.ToString(@"mm\:ss");

            }
        }

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            if (volume.IsChecked.Value)
                volumeSlider.Visibility = Visibility.Visible;
            else
                volumeSlider.Visibility = Visibility.Hidden;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement.Source != null)
            {
                mediaElement.Volume = volumeSlider.Value;
                if (mediaElement.Volume == 0)
                {
                    volume.Visibility = Visibility.Hidden;
                    mute.Visibility = Visibility.Visible;
                }
                else
                {
                    volume.Visibility = Visibility.Visible;
                    mute.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (!mute.IsChecked.Value)
                volumeSlider.Visibility = Visibility.Hidden;
            else
                volumeSlider.Visibility = Visibility.Visible;
        }

        private void SearchField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchField.Text == "Search for music in Internet")
            {
                searchField.Text = "";
                searchField.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB0C4D9"));
            }
        }

        private void SearchField_LostFocus(object sender, RoutedEventArgs e)
        {
            searchField.Text = "Search for music in Internet";
            searchField.Foreground = Brushes.Gray;
        }

        private string PrepearSongName(string songname)
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
            string songName = searchField.Text;

            if ((e.Key == Key.Enter) && (songName != "Search for music in Internet") && (songName != ""))
            {
                Thread getMP3Thread = new Thread(new ParameterizedThreadStart(GetMP3Async));
                getMP3Thread.IsBackground = true;
                getMP3Thread.Start(songName);
                
            }
        }

        private void GetMP3Async(object obj)
        {
            string songName = obj as string;
            string response = ConnectionWithLastFm(songName);

            if (response != string.Empty)
                try
                {
                    string correctName = ArtistSongNames(response);

                    string answer = ConnectionWithYoutube(response);
                    SaveMP3(answer, correctName);

                }
                catch (ArgumentException)
                {
                    MessageBox.Show($"\"{songName}\" not found");
                }
        }

        private string ConnectionWithLastFm(string songName)
        {
            string result = string.Empty;
            var myRequest = PrepearSongName(songName);
            string url = $"http://ws.audioscrobbler.com/2.0/?method=track.search&track={myRequest}&api_key=57ee3318536b23ee81d6b27e36997cde&format=json&limit=1";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                result = streamReader.ReadToEnd();
            }
            catch(Exception)
            {
                MessageBox.Show("No connection to the internet");
            }

            return result;    
        }

        private string ConnectionWithYoutube(string response)
        {
            string subString = "https://www.last.fm/music/";
            string answer = GetUrlFromResponse(response, subString);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(answer);
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var streamReader = new StreamReader(httpWebResponse.GetResponseStream());

            response = streamReader.ReadToEnd();

            subString = "https://www.youtube.com/watch";
            answer = GetUrlFromResponse(response, subString);
            return answer;
        }

        private string GetUrlFromResponse(string response, string subString)
        {
            string answer = string.Empty;

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

        private string ArtistSongNames(string response)
        {
            const string SUBSTR_A = "artist";
            const string SUBSTR_S = "name";

            string authorSong = GetParamsFromJson(SUBSTR_A, response);
            authorSong += "-" + GetParamsFromJson(SUBSTR_S, response);
            return authorSong;
        }

        private string GetParamsFromJson(string substring, string response)
        {
            string result = string.Empty;
            int index = response.IndexOf(substring);
            if (index > 0)
            {
                index += substring.Length + 3;
                while (response[index] != '"')
                {
                    if (response[index] == SONG_SEPARATOR)
                        result += DOWN_SEPARATOR;
                    else
                        result += response[index];
                    index++;
                }
            }
            return result;
        }

        private void SaveMP3(string VideoURL, string artistSongNames)
        {

            artistSongNames = artistSongNames.Replace(DOWN_SEPARATOR, SONG_SEPARATOR);

            try
            {
                var youtube = YouTube.Default;
                var video = youtube.GetVideo(VideoURL);
                
                File.WriteAllBytes(artistSongNames, video.GetBytes());//Error in mscorlib.dll

                var inputFile = new MediaFile { Filename = $"{artistSongNames}" };
                var outputFile = new MediaFile { Filename = $"{artistSongNames}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }
                Dispatcher.BeginInvoke(new ThreadStart(delegate { AddMusic(outputFile.Filename); }));
            }
            catch (Exception)
            {
                MessageBox.Show($"your song {artistSongNames} not found");
            }
        }

        private static Thread thread = null;

        private void GetSongText_MouseWheel(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread = new Thread(new ParameterizedThreadStart(ShowText));
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start(sender);
            }

        }

        private void ShowText(object sender)
        {
            string songText = PrepearForBD((sender as PlayerItem).songName, sender);
            Dispatcher.BeginInvoke(new ThreadStart(delegate { InitializeNewForm(sender, songText); }));
            waitHandle.WaitOne();
            thread = null;
        }


        private string PrepearForBD(string url,object sender)
        {
            int hash = (sender as PlayerItem).ArtistSongHash;
            const char SEPARATOR = '-';
            const char URL_SEPARATOR = '/';

            url = url.Replace(SEPARATOR, URL_SEPARATOR).ToLower();
            url = url.Replace(SONG_SEPARATOR, DOWN_SEPARATOR);

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://en.lyrsense.com/{url}");
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpWebResponse.GetResponseStream());

                string response = streamReader.ReadToEnd();
                return TryToGetLiryc(response);
            }
            catch (Exception)
            {
                MessageBox.Show($"No connection to the internet or lirics for {url} not found");
            }
            return string.Empty;
        }

        private string TryToGetLiryc(string response)
        {
            const int OFFSET = 11;
            int index = 1;

            string lirics = string.Empty;
            string substring = $"highlightLine puzEng line{index}";
            
            int offset = response.IndexOf(substring);
            while (offset > 0)
            {
                offset += substring.Length + OFFSET;
                while (response[offset] != '<')
                {
                    if (response[offset] != '>')
                        lirics += response[offset++];
                    else offset++;
                }
                lirics = lirics + (char)13 + (char)10;

                substring = $"highlightLine puzEng line{++index}";

                if ((offset = response.IndexOf(substring)) < 0)
                {
                    lirics = lirics + (char)13 + (char)10;
                    substring = $"highlightLine puzEng line{++index}";
                    offset = response.IndexOf(substring);
                }
            }
            return lirics.Remove(lirics.LastIndexOf((char)13));
        }

        private void InitializeNewForm(object sender, string songText)
        {
            if (!PlayerItem.WindowTextActivate)
            {
                PlayerItem.WindowTextActivate = true;
                myFormText = new SongText();
                myFormText.Width = 250;
                myFormText.Left = Left + Width;
                myFormText.Top = Top + (Height - myFormText.Height) / 2;
                myFormText.Show();
            }

            myFormText.songText_Bottom.Document.Blocks.Clear();
            myFormText.songText_Bottom.Document.Blocks.Add(new Paragraph(new Run(songText)));
            myFormText.songText_Bottom.HorizontalContentAlignment = HorizontalAlignment.Center;

            string MyPath = (sender as PlayerItem).Path;
            myFormText.song_Name_Top.Text = Path.GetFileNameWithoutExtension(MyPath);
            waitHandle.Set();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (myFormText != null)
            {
                myFormText.Left = Left + Width;
                myFormText.Top = Top + (Height - myFormText.Height) / 2;
            }
        }
    }
}