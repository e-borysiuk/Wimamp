using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wimamp
{
    // ciemniejszy #076CA6
    // jesniejszy #00AADD
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlaylistWindow playlist;
        public static MediaPlayer mediaPlayer = new MediaPlayer();
        private bool userIsDraggingSlider = false;
        private bool mediaPlayerIsPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            CommandBinding OpenFileCommandBind = new CommandBinding();
            OpenFileCommandBind.Command = ApplicationCommands.Open;
            OpenFileCommandBind.Executed += OpenMusicFile;
            this.CommandBindings.Add(OpenFileCommandBind);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            playlist = new PlaylistWindow {Owner = this};
            playlist.WindowStartupLocation = WindowStartupLocation.Manual;
            PlaylistPositioning();
            playlist.Width = this.Width + 2;
            playlist.Show(); 
        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            PlaylistPositioning();
        }

        private void PlaylistPositioning()
        {
            playlist.Top = this.Top + this.Height - 7;
            playlist.Left = this.Left - 1;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if(mediaPlayer.Source != null)
            {
                // mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss") <-- cała piosenka tyle trwa
                LbSongTime.Content = String.Format("{0}", mediaPlayer.Position.ToString(@"mm\:ss"));
            }
            else
            {
                LbSongTime.Content = "00:00";
            }
        }

        private void OpenMusicFile(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(dlg.FileName));
            }
            DispatcherTimer dt = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            dt.Tick += TimerTick;
            dt.Start();
            

            e.Handled = true;
        }

        private void BtPlay_Click(object sender, RoutedEventArgs e)
        {
            if(mediaPlayer.Source != null)
            {
                mediaPlayer.Play();
            }
        }

        private void BtStop_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                mediaPlayer.Stop();
            }
        }

        private void BtPause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                mediaPlayer.Pause();
            }
        }

        private void SlProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void SlProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(SlProgress.Value);
        }

        private void SlProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LbSongTime.Content = TimeSpan.FromSeconds(SlProgress.Value).ToString(@"mm\:ss");
        }
    }
}
