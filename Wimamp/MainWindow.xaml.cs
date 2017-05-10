using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
