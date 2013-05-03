using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TagLib;
using System.IO;

namespace PiDu
{
    public class Library
    {
        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        object albumLock = new object();
        object songLock = new object();

        public void Load()
        {
            Load(@"C:\Users\Vivek\Music");
        }

        public void Load(string startingDirectory)
        {
            Albums = new ObservableCollection<Album>();
            Songs = new ObservableCollection<Song>();
            
            List<string> musicFiles = Directory.EnumerateFiles(startingDirectory, "*.mp3", SearchOption.AllDirectories).ToList();
            System.Console.WriteLine(String.Format("Loading {0} files", musicFiles.Count()));
            
            //Parallel.ForEach<string>(musicFiles, musicFile =>
            foreach(string musicFile in musicFiles)
            {
                TagLib.File fileInfo = null;
                try
                {
                    if (!System.IO.File.Exists(musicFile))
                    {
                        System.Console.WriteLine("File: " + musicFile + " does not exist");
                    }

                    fileInfo = TagLib.File.Create(musicFile);
                }
                catch(Exception exn){
                    System.Console.WriteLine("QWE:" + exn.Message.ToString());
                }

                if (fileInfo != null)
                {

                    Album album = new Album();
                    Song track = new Song();

                    track.FileLocation = fileInfo.Name;
                    track.Name = fileInfo.Tag.Title;

                    string albumName = fileInfo.Name;
                    string albumArtists = "";

                    if (fileInfo.Tag.Album != null)
                    {
                        albumName = fileInfo.Tag.Album;
                    }
                    if(fileInfo.Tag.AlbumArtists != null){
                        albumArtists = String.Join(",",fileInfo.Tag.AlbumArtists);
                    }

                    if (
                        (album = this.Albums.SingleOrDefault(
                            x => x.Title.ToLower().Equals(albumName.ToLower())
                            && x.Artist.ToLower().Equals(albumArtists.ToLower()))
                        ) == null)
                    {
                        album = new Album();
                        album.Artist = albumArtists;
                        album.Title = albumName;

                        //lock (albumLock)
                        //{
                            this.Albums.Add(album);
                        //}
                    }

                    track.Album = album;

                    //lock (album)
                    //{
                        if (album.Songs == null)
                        {
                            album.Songs = new ObservableCollection<Song>();
                        }

                        album.Songs.Add(track);
                    //}

                    //lock (songLock)
                    //{
                        this.Songs.Add(track);
                    //}
                }
                else
                {
                    System.Console.WriteLine("Could not load metadata for file: " + musicFile);
                }
            }//);

            
        }


        public TagLib.File ScanFile(string file)
        {
            TagLib.File fileInfo;

            try
            {
                fileInfo = TagLib.File.Create(file);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("QWE:" + e.Message.ToString());
                fileInfo = null;
            }

            if (fileInfo.Tag.Title == null)
            {
                fileInfo = null;
            }

            System.Console.WriteLine("Returning file: " + file);
            return fileInfo;
        }
    }
}
