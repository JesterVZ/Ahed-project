using System.Windows;
using System.Windows.Input;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для GeometryTemplateWindow.xaml
    /// </summary>
    public partial class GeometryTemplateWindow : Window
    {
        public string imageUrl;
        public GeometryTemplateWindow(string url)
        {
            InitializeComponent();
            this.imageUrl = url;
            /*
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageUrl, UriKind.Absolute);
            bitmapImage.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bitmapImage;
            GeometryImage.Source = bitmapImage;*/
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }
    }
}
