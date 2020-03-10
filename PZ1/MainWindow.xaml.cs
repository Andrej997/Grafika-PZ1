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

namespace PZ1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        public static Point Point;
        public static string clickedName;
        public static object Object;
        public static List<object> List;
        public static List<Point> PolygonPoints;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            List = new List<object>();
            PolygonPoints = new List<Point>();
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (clickedName == "polygon")
            {
                Point = Mouse.GetPosition(canvas);
                PolygonPoints.Add(Point);
            }
            else
            {
                Point = Mouse.GetPosition(canvas);
                Settings settingsWindow = new Settings(clickedName, Point);
                settingsWindow.ShowDialog();
                if (clickedName == "ellipse")
                    canvas.Children.Add((Ellipse)Object);
                else if (clickedName == "rectangle")
                    canvas.Children.Add((Rectangle)Object);
                List.Add(Object);
            }
        }

        private void MenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = (MenuItem)sender;
            clickedName = clicked.Name;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Settings settingsWindow = new Settings(clickedName, PolygonPoints);
            settingsWindow.ShowDialog();

            canvas.Children.Add((Polyline)Object);
            PolygonPoints.Clear();

            List.Add(Object);
        }
    }
}
