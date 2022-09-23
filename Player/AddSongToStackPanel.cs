using Player.Pages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Player
{
    public static class AddSongToStackPanel
    {
        static MainWindow w = Window.GetWindow(Application.Current.MainWindow) as MainWindow;

        public static void SetStackPanelSongs(StackPanel MainStackPanel, Song song, ref bool gray)
        {
            if (song.Name == null)
                return;
            var bc = new BrushConverter();
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.MouseRightButtonDown += StackPanel_MouseRightButtonDown;  
            ContextMenu menu1 = new ContextMenu();
            menu1.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle");
            menu1.Items.Add(new MenuItem());
            stackPanel.ContextMenu = menu1;

            if (gray)
            {
                stackPanel.Background = (Brush)bc.ConvertFrom("#252525");
                gray = false;
            }
            else
                gray = true;

            Image image = new Image();
            image.Width = 50;
            image.Height = 50;
            image.Source = song.image.Source;



            TextBlock textBlockName = new TextBlock();
             textBlockName.Text = song.Name;
         
            ContextMenu menu2 = new ContextMenu();
            menu2.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle");
            menu2.Items.Add(new MenuItem());
            textBlockName.ContextMenu = menu2;
            textBlockName.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("TextBlockNameSong");
            //Style st = new Style();
            //Style style = new Style(typeof(TextBlock), textBlockName.Style); 
            //Trigger t = new Trigger();
            //t.Property = TextBlock.IsMouseOverProperty;
            //t.Value = true;
            //Setter setter = new Setter();
            //setter.Property = TextBlock.ContextMenuProperty.PropertyType.iso;
            //setter.Value = true;
            //t.Setters.Add(setter); 
            //style.Triggers.Add(t); 
            //textBlockName.Style = style;



            Button buttonPlay = new Button();
            buttonPlay.Width = 30;
            buttonPlay.Height = 35;
            buttonPlay.Margin = new Thickness(0, 0, 20, 0);
            buttonPlay.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ButtonStyle");
            buttonPlay.Content = StckForButtonPlay();
            buttonPlay.Click += ButtonPlay_Click;


            Button buttonAdd = new Button();
            buttonAdd.Width = 30;
            buttonAdd.Height = 35;
            buttonAdd.Margin = new Thickness(0, 0, 20, 0);
            buttonAdd.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ButtonStyle");
            buttonAdd.Click += ButtonAdd_Click;
            buttonAdd.Content = StckForButtonAdd();
            ContextMenu menu = new ContextMenu();
            menu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle");
            menu.Items.Add(new MenuItem());
            buttonAdd.ContextMenu = menu;


            Button buttonArtist = new Button();
            buttonArtist.Name = "Artist";
            buttonArtist.Width = 80;
            buttonArtist.Height = 35;
            buttonArtist.VerticalAlignment = VerticalAlignment.Center;
            buttonArtist.Cursor = Cursors.Arrow;
            buttonArtist.Margin = new Thickness(0, 0, 20, 0);
            buttonArtist.Background = Brushes.Transparent;
            buttonArtist.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ButtonStyle");
            buttonArtist.BorderThickness = new Thickness(0);
            buttonArtist.Content = stckArtistButton(song);
            buttonArtist.Click += ButtonArtist_Click;

            TextBlock Genre = new TextBlock();
            if (song.Genre == string.Empty || song.Genre == null)
                Genre.Text = "Unknown genre";
            else
                Genre.Text = song.Genre;
            Genre.Margin = new Thickness(0, 0, 20, 0);
            Genre.Width = 120;
            Genre.VerticalAlignment = VerticalAlignment.Center;
            Genre.HorizontalAlignment = HorizontalAlignment.Center;
            Genre.Background = Brushes.Transparent;
            Genre.Foreground = (Brush)bc.ConvertFrom("#7F7F7F");

            TextBlock time = new TextBlock();
            time.HorizontalAlignment = HorizontalAlignment.Center;
            time.VerticalAlignment = VerticalAlignment.Center;
            time.Foreground = (Brush)bc.ConvertFrom("#7F7F7F");
            time.Text = string.Format("{0:m\\:ss}", TimeSpan.FromSeconds(song.Time.TotalSeconds).Duration());
            time.Width = 40;

            TextBlock Album = new TextBlock();
            Album.HorizontalAlignment = HorizontalAlignment.Center;
            Album.VerticalAlignment = VerticalAlignment.Center;
            if (song.Album == string.Empty || song.Album == null)
                Album.Text = "Unknown album";
            else
                Album.Text = song.Album;
            Album.Width = 100;
            Album.Margin = new Thickness(0, 0, 20, 0);
            Album.Background = Brushes.Transparent;
            Album.Foreground = (Brush)bc.ConvertFrom("#7F7F7F");
            if (w.Container.Content != null)
                if ((w.Container.Content as Page).Title == "RecentlyPage")
                    stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlockName);
            stackPanel.Children.Add(buttonPlay);
            stackPanel.Children.Add(buttonAdd);
            stackPanel.Children.Add(buttonArtist);
            stackPanel.Children.Add(Album);
            stackPanel.Children.Add(Genre);
            stackPanel.Children.Add(time);
            stackPanel.Height = 60;

            stackPanel.Tag = song.FullPath;
            buttonAdd.Tag = song.FullPath;
            buttonPlay.Tag = song.FullPath;


            MainStackPanel.Children.Add(stackPanel);
        }

        private static void TextBlockName_MouseLeave(object sender, MouseEventArgs e)
        {
            var b = sender as TextBlock;
            b.ContextMenu.IsOpen = false;
        }

        private static void TextBlockName_MouseEnter(object sender, MouseEventArgs e)
        {
            var b = sender as TextBlock;
            b.ContextMenu.Items.Clear();

            MenuItem newQuery = new MenuItem();
            newQuery.Header = b.Text;
            newQuery.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("MenuItemStyle") as Style; 

            b.ContextMenu.Items.Add(newQuery);

            if (b.ContextMenu.Style == null)
                b.ContextMenu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle") as Style;

            b.ContextMenu.IsOpen = true;
        }

        private static void StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (w.Container.Content != null)
            {
                if ((w.Container.Content as Page).Title == "PlayList")
                {
                    var b = sender as StackPanel;
                    b.ContextMenu.Items.Clear();

                    MenuItem newQuery = new MenuItem();
                    newQuery.Header = "Remove song from playlist";
                    newQuery.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("MenuItemStyle") as Style;
                    newQuery.Tag = b.Tag;
                    newQuery.Click += NewQuery_Click2; ;
                    newQuery.Tag = b.Tag;

                    b.ContextMenu.Items.Add(newQuery);

                    if (b.ContextMenu.Style == null)
                        b.ContextMenu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle") as Style;

                    b.ContextMenu.IsOpen = true;
                }
            }
        }

        private static void ButtonArtist_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var s = b.Content as StackPanel;
            var t = s.Children[0] as TextBlock;
            ArtistSongs Artist = new ArtistSongs(t.Text);
            w.Container.Navigate(Artist);
        }

        private static void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            bool Is = true;
            b.ContextMenu.Items.Clear();
            foreach (var p in w.playLists)
            {
                if (p.Name != string.Empty && p.Name != null)
                {
                    var c = p.Songs.Where(q => q.FullPath == b.Tag).FirstOrDefault();
                    if (c == null)
                    {
                        Is = true;
                        MenuItem newQuery = new MenuItem();
                        newQuery.Header = p.Name;
                        newQuery.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("MenuItemStyle") as Style;
                        newQuery.Tag = b.Tag;
                        newQuery.Click += NewQuery_Click;

                        b.ContextMenu.Items.Add(newQuery);
                    }
                    else
                        Is = false;
                }
            }
            if (b.ContextMenu.Style == null)
                b.ContextMenu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle") as Style;
            if(Is)
            b.ContextMenu.IsOpen = true;
        }

        private static void NewQuery_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            foreach (var p in w.playLists)
            {
                if(p.Name == menuitem.Header)
                {
                    p.Songs.Add(w.songs.Where(s => s.FullPath == menuitem.Tag).FirstOrDefault());
                    break;
                }
            }
        }

        private static StackPanel StckForButtonPlay()
        {
            StackPanel stackPanelbutton = new StackPanel();
            Image image = new Image();
            image.Source = GetButtonPlayImage();
            image.Height = 30;
            image.Width = 30;
            stackPanelbutton.Children.Add(image);
            return stackPanelbutton;
        }
        private static StackPanel stckArtistButton(Song s)
        {
            StackPanel stackPanelbutton = new StackPanel();
            TextBlock Name = new TextBlock();
            Name.Text = string.Join(" ", s.Artists);
            Name.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("GrayWhenMouseOver");
            if (Name.Text == string.Empty)
            {
                Name.Text = "Unknown artist";

            }
            stackPanelbutton.Children.Add(Name);
            return stackPanelbutton;
        }
        private static BitmapImage GetButtonPlayImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"Image_For_Page/icons8_play_24px.png", UriKind.Relative);
            logo.EndInit();
            return logo;
        }
        private static StackPanel StckForButtonAdd()
        {
            StackPanel stackPanelbutton = new StackPanel();
            Image image = new Image();
            image.Width = 30;
            image.Height = 25;
            image.Source = GetButtonAddImage();
            stackPanelbutton.Children.Add(image);
            return stackPanelbutton;
        }
        private static BitmapImage GetButtonAddImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@"Image_For_Page/icons8_Plus_24px.png", UriKind.Relative);
            logo.EndInit();
            return logo;
        }
        private static void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            foreach (var song in w.songs)
            {
                if (song.FullPath == b.Tag)
                {
                    if ((w.Container.Content as Page).Title != "PlayList")  
                    {
                        song.lastopenedplace = (w.Container.Content as Page).Title;
                    }
                    else
                    {
                        var page = w.Container.Content as Page;
                        Grid g = (Grid)page.Content;
                        Grid g1 = (Grid)g.Children[0];
                        StackPanel st = (StackPanel)g1.Children[0];
                        TextBox text = (TextBox)st.Children[1];
                        song.lastopenedplace = text.Text;
                    }
                    w._timerName.Stop();
                    w.PlaySong(song);

                }
            }
        }
        public static Button CreateButtonPlayList(PlayList playList)
        {
            Button button = new Button();
            button.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("menuButtonLeft");
            button.Width = 300;
            button.Tag = playList.Name;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Click += Button_Click1; ;

            StackPanel s = new StackPanel();
            s.Background = Brushes.Transparent;
            s.Orientation = Orientation.Horizontal;
            s.Margin = new Thickness(15, 0, 0, 0);
            s.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("Background");

            Image img = new Image();
            img.Source = playList.image.Source;
            img.Height = 24;
            img.Width = 24;

            TextBlock t = new TextBlock();
            t.Text = "   " + playList.Name;
            t.Cursor = Cursors.Hand;
            t.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("TextBlockMenu");

            s.Children.Add(img);
            s.Children.Add(t);

            ContextMenu menu = new ContextMenu();
            menu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle");
            menu.Items.Add(new MenuItem());
            button.ContextMenu = menu;

            button.MouseRightButtonDown += Button_MouseRightButtonDown;  

            button.Content = s;

            return button;
        }

        private static void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var b = sender as Button;
            b.ContextMenu.Items.Clear();

            MenuItem newQuery = new MenuItem();
            newQuery.Header = "Delete playlist";
            newQuery.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("MenuItemStyle") as Style;
            newQuery.Tag = b.Tag;
            newQuery.Click += NewQuery_Click1;
            newQuery.Tag = b.Tag;

            b.ContextMenu.Items.Add(newQuery);

            if (b.ContextMenu.Style == null)
                b.ContextMenu.Style = (Style)(((MainWindow)Application.Current.MainWindow)).FindResource("ContextMenuStyle") as Style;

            b.ContextMenu.IsOpen = true;
        }

        
        private static void NewQuery_Click1(object sender, RoutedEventArgs e)
        {
            var m = sender as MenuItem;
            foreach (Button lpl in w.ListPlaylist.Children)
            {
                if(lpl.Tag == m.Tag)
                {
                    w.ListPlaylist.Children.Remove(lpl);
                    break;
                }
            }
            w.playLists.Remove(w.playLists.Where(p => p.Name == m.Tag).FirstOrDefault());
         }

        private static void Button_Click1(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            PlayListPage p = new PlayListPage(
                w.playLists.Where(q => q.Name == b.Tag).FirstOrDefault());

            w.Container.Navigate(p);
        }
 
        private static void NewQuery_Click2(object sender, RoutedEventArgs e)
        {
            string playlistname;
            var m = sender as MenuItem;

            var page = w.Container.Content as Page;
            Grid g = (Grid)page.Content;
            Grid g1 = (Grid)g.Children[0];
            StackPanel st = (StackPanel)g1.Children[0];
            TextBox text = (TextBox)st.Children[1];
            playlistname = text.Text;

            foreach(var pls in w.playLists)
            {
                if(pls.Name == playlistname)
                {
                    foreach(var songdel in pls.Songs)
                    {
                        if(songdel.FullPath ==m.Tag)
                        {
                            pls.Songs.Remove(songdel);
                            break;
                        }
                    }
                }
            }

            var page1 = w.Container.Content as PlayListPage;
            page1.UpdateList();
        }
    }
}
