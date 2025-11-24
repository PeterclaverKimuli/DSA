using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int DurationInSeconds { get; set; }

        public Song(string title, string artist, string album, int durationInSeconds)
        {
            Title = title;
            Artist = artist;
            Album = album;
            DurationInSeconds = durationInSeconds;
        }

        public override string ToString()
        {
            return $"{Title} by {Artist} from album {Album} - {DurationInSeconds}";
        }
    }

    public class User {
        public string Name { get; set; }
        public int Credits { get; set; }

        public User(string name, int credits)
        {
            Name = name;
            Credits = credits;
        }

        public bool UseCredits() {
            if (Credits > 0) {
                Credits--;
                return true;
            }

            return false;
        }

        public void AddCredits(int amount) { 
            Credits += amount;
        }
    }

    public class Playlist { 
        public Queue<Song> Songs { get; set; }

        public void AddSong(Song song) { 
            Songs.Enqueue(song);
        }

        public Song GetNextSong() { 
            return Songs.Count > 0 ? Songs.Dequeue() : null;
        }

        public bool HasSongs => Songs.Count > 0;
    }

    public class MusicPlayer
    {
        private Song currentSong;

        public void Play(Song song) { 
            currentSong = song;

            Console.WriteLine($"\nNow playing: {currentSong.Title} by {currentSong.Artist}\n");
        }
    }

    public class JukeBox
    {
        private Playlist playlist = new Playlist();
        private List<Song> songLibrary = new List<Song>();
        private MusicPlayer musicPlayer = new MusicPlayer();

        public void AddSongToLibrary(Song song) { 
            songLibrary.Add(song);
        }

        public void showLibrary() {
            foreach (var song in songLibrary) { 
                song.ToString();
            }
        }

        public void AddSongToPlaylist(User user, string songTitle)
        {
            if (!user.UseCredits())
            {
                Console.WriteLine("Not enough credits to create a playlist.");
                return;
            }

            var song = songLibrary.FirstOrDefault(s => s.Title.Equals(songTitle, StringComparison.OrdinalIgnoreCase));
            if (song != null)
            {
                playlist.AddSong(song);
                Console.WriteLine("Song added to the playlist.");
            }
            else
            {
                Console.WriteLine("Song not found in the library.");
            }
        }

        public void PlayNextSong() {
            if (playlist.HasSongs)
            {
                var nextSong = playlist.GetNextSong();
                musicPlayer.Play(nextSong);
            }
            else { 
                Console.WriteLine("Playlist is empty. Please add songs to the playlist.");
            }
        }
    }

}
