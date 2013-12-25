using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TagLib;
using System.IO;
using System.Collections.Specialized;
using System.Windows;

namespace PiDu.Model
{
    public class Library : ILibrary
    {
        private IMusicDataProvider MusicDataProvider { get; set; }

        public ICollection<IAlbum> Albums { get; set; }
        public ICollection<ISong> Songs { get; set; }

        object albumLock = new object();
        object songLock = new object();

        public Library()
        {
            Albums = new ObservableCollection<IAlbum>();
            Songs = new ObservableCollection<ISong>();
            MusicDataProvider = new LastFMMusicDataProvider(String.Format(@"{0}\\Cache",Properties.Settings.Default.MusicRootDirectory));
        }

        public async Task Load()
        {
            await Load(PiDu.Properties.Settings.Default.MusicRootDirectory);
        }

        public async Task Load(string startingDirectory)
        {
            Albums.Clear();
            Songs.Clear();

            IEnumerable<string> musicFiles = Directory.EnumerateFiles(startingDirectory, "*.mp3", SearchOption.AllDirectories);
            System.Console.WriteLine(String.Format("Loading {0} files", musicFiles.Count()));
            
            //Parallel.ForEach<string>(musicFiles, musicFile =>

            await Task.Run(() =>
            {

                foreach (string musicFile in musicFiles)
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
                    catch (Exception exn)
                    {
                        //System.Console.WriteLine("QWE:" + exn.Message.ToString());
                    }

                    if (fileInfo != null)
                    {

                        Album album = new Album();
                        Song track;



                        string albumName = fileInfo.Name;
                        string albumArtists = "";

                        if (fileInfo.Tag.Album != null)
                            albumName = fileInfo.Tag.Album;

                        if (fileInfo.Tag.AlbumArtists != null)
                            albumArtists = String.Join(",", fileInfo.Tag.AlbumArtists);

                        if (
                            (album = (Album)this.Albums.SingleOrDefault(
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

                        track = new Song(album);
                        track.FileLocation = fileInfo.Name;
                        track.Name = fileInfo.Tag.Title;
                        track.TrackNumber = (int)fileInfo.Tag.Track;

                        //lock (album)
                        //{
                        if (album.Songs == null)
                            album.Songs = new ObservableCollection<ISong>();

                        ((ObservableCollection<ISong>)album.Songs).Add(track);
                        //}

                        //lock (songLock)
                        //{
                        this.Songs.Add(track);
                        //}
                    }
                    else
                    {
                        //System.Console.WriteLine("Could not load metadata for file: " + musicFile);
                    }
                }//);
            });

            if (this.LibraryUpdated != null)
            {
                this.LibraryUpdated(this, new EventArgs());
            }

            await Task.Run(() =>
            {
                Parallel.ForEach(this.Albums, (album) =>
                {
                    if (!MusicDataProvider.GetCachedAlbumData(album).Result)
                        MusicDataProvider.GetExternalAlbumData(album);
                });
            });

            
            
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

        public event EventHandler LibraryUpdated;
    }
}
