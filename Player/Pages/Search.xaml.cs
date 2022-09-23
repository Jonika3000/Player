using System;
using System.Collections.Generic;
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

namespace Player.Pages
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        string text;
        bool gray = true;
        public Search()
        {
            InitializeComponent();
        }
        public Search(string text)
        {
            InitializeComponent();
            this.text = text;
            search();
        }
        private void search()
        {
            bool Is = false;
            var w = Window.GetWindow(App.Current.MainWindow) as MainWindow;
            foreach(var s in w.songs)
            {
                if(s.Name.ToLower().Contains(text.ToLower()) || (string.Join(" ", s.Artists).ToLower().Contains(text.ToLower())))
                    {
                  Is = true;
                   AddSongToStackPanel.SetStackPanelSongs(MainStackPanel,s,ref gray);
                }

            }
            if(!Is)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Nothing found";
                textBlock.FontSize = 25;
                textBlock.Foreground = Brushes.White;
                textBlock.FontWeight = FontWeights.Light;
                textBlock.Margin = new Thickness(-50);
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                MainStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                MainStackPanel.VerticalAlignment = VerticalAlignment.Center;
                MainStackPanel.Children.Add(textBlock);
            }    
        }
         
    }
}
