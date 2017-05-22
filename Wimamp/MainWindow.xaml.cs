using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public static Song CurrentSong { get; set; }
        private PlaylistWindow playlist;
        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += Timer_Tick;
            timer.Start();
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

        //

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((MePlayer.Source != null) && (MePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                SlProgress.Minimum = 0;
                SlProgress.Maximum = MePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SlProgress.Value = MePlayer.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                    MePlayer.Source = new Uri(openFileDialog.FileName);
                    TbSongName.Text = openFileDialog.SafeFileName;
            }

        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MePlayer != null) && (MePlayer.Source != null);
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Play();
            mediaPlayerIsPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.Stop();
            mediaPlayerIsPlaying = false;
        }

        private void SlProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void SlProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MePlayer.Position = TimeSpan.FromSeconds(SlProgress.Value);
        }

        private void SlProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LbSongTime.Content = TimeSpan.FromSeconds(SlProgress.Value).ToString(@"mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void MuteVolume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MePlayer.IsMuted = !MePlayer.IsMuted;
            SlVolume.IsEnabled = (!MePlayer.IsMuted);
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //to do with playlist EMIL
        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //to do with playlist EMIL
        }

        private void Stack_OnLoaded(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (Storyboard)this.stack.FindResource("slide"); stack.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => { sb.Begin(); }));
        }
    }
    
}
