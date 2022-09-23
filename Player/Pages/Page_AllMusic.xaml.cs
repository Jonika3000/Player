using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page_AllMusic.xaml
    /// </summary>
    public partial class Page_AllMusic : Page
    {
         bool gray = true;
        MainWindow w = Window.GetWindow(App.Current.MainWindow) as MainWindow;

        
        List<Song> songs = new List<Song>();
        List<StackPanel> Alltracks = new List<StackPanel>();


        public Page_AllMusic()
        {
            InitializeComponent();
        }
       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            songs = w.songs;
            foreach (var s in songs)
            {
                AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, s, ref gray); ;
            }
            SetGenre();
        }
         private void SetGenre()
        {
            foreach (var s in songs)
            {
                if (!w.Genre.Contains(s.Genre))
                    w.Genre.Add(s.Genre);
            }
        }
 
        private void ButtonRnd_Click(object sender, RoutedEventArgs e)
        {

            Random rnd = new Random();
            int i = rnd.Next(0, songs.Count - 1);
            w._timerName.Stop();
            w.PlaySong(songs[i]);
        }
        private void AddGenreToContextMenu()
        {
            foreach (var g in w.Genre)
            {
                if (g != string.Empty && g != null)
                {
                    MenuItem newQuery = new MenuItem();
                    newQuery.Header = g;
                    newQuery.Style = this.FindResource("MenuItemStyle") as Style;

                    newQuery.Click += NewQuery_Click;

                    ButtonGenre.ContextMenu.Items.Add(newQuery);
                }

            }
            MenuItem newQuery1 = new MenuItem();
            newQuery1.Header = "all genre";
            newQuery1.Style = this.FindResource("MenuItemStyle") as Style;

            newQuery1.Click += NewQuery_Click;

            ButtonGenre.ContextMenu.Items.Add(newQuery1);
        }

        private void NewQuery_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            MainStackPanel.Children.Clear();
            Alltracks.Clear();
            if (menuitem.Header.ToString() == "all genre")
            {
                foreach (var s in songs)
                {
                    AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, s, ref gray);  
                }
            }
            else
            {
                foreach (var s in songs)
                {
                    if (s.Genre == menuitem.Header.ToString())
                    {
                        AddSongToStackPanel.SetStackPanelSongs(MainStackPanel, s, ref gray);
                    }
                }
            }

        }

        private void ButtonGenre_Click(object sender, RoutedEventArgs e)
        {

            ButtonGenre.ContextMenu.Items.Clear();
            AddGenreToContextMenu();
            if (ButtonGenre.ContextMenu.Style == null)
                ButtonGenre.ContextMenu.Style = this.FindResource("ContextMenuStyle") as Style;

            ButtonGenre.ContextMenu.IsOpen = true;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
