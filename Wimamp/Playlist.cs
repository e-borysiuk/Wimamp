using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace Wimamp
{
    [XmlRoot(Namespace = "Wimamp")]
    [XmlInclude(typeof(Song))]
    [Serializable]
    public class Playlist
    {
        public int currentIndex = 0;

        public Collection<object> songs = new ObservableCollection<object>();
        private List<Song> songsSave = new List<Song>();
        public ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(songs);
            }
        }

        public Playlist()
        {
            
        }

        public bool loadPlaylist()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Playlist Files (*.dat) | *.dat"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);
                List<Song> list = (List<Song>)serializer.Deserialize(reader);
                foreach (Song o in list)
                {
                    songs.Add(o);
                }
                return true;
            }
            return false;
        }

        public void savePlaylist()
        {
            foreach (object o in songs)
            {
                songsSave.Add((Song)o);
            }
            XmlSerializer serializer = new XmlSerializer(songsSave.GetType());
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "playlist.dat",
                Filter = "Playlist files (*.dat) | *.dat",
                DefaultExt = ".dat"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    serializer.Serialize(writer, songsSave);
                }
            }
        }

        public Uri Play()
        {
            if (songs.Count > 0)
            {
                var song = (songs.Cast<Song>().ToArray())[0];
                var uri = new Uri(song.Uri);
                Application.Current.Windows.OfType<MainWindow>().First().TbSongName.Text = song.Name;
                return uri;
            }
            return null;
        }

        public Uri Next()
        {

            if (currentIndex != songs.Count - 1)
            {
                var song = (songs.Cast<Song>().ToArray())[++currentIndex];
                var uri = new Uri(song.Uri);
                Application.Current.Windows.OfType<MainWindow>().First().TbSongName.Text = song.Name;
                return uri;
            }
            return null;
        }

        public Uri Previous()
        {
            if (currentIndex != 0)
            {
                var song = (songs.Cast<Song>().ToArray())[--currentIndex];
                var uri = new Uri(song.Uri);
                Application.Current.Windows.OfType<MainWindow>().First().TbSongName.Text = song.Name;
                return uri;
            }
            return null;
        }

        public Uri Random()
        {
            return null;

        }

        public void Add(Uri u)
        {
            

        }

        public void Remove(Uri u)
        {
            
          
        }

    }
}
