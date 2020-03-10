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
using System.Windows.Shapes;

namespace PZ1
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private string eT;
        private Point pt;
        private List<Point> points;
        public Settings(string elementType, Point point)
        {
            InitializeComponent();
            eT = elementType;
            pt = point;
            
            cbBorderColor.Items.Add("Blue");
            cbBorderColor.Items.Add("Red");
            cbBorderColor.Items.Add("Green");

            cbFillColor.Items.Add("Blue");
            cbFillColor.Items.Add("Red");
            cbFillColor.Items.Add("Green");
            cbFillColor.Items.Add("Black");
        }

        public Settings(string elementType, List<Point> lpoints)
        {
            InitializeComponent();
            eT = elementType;
            points = lpoints;

            cbBorderColor.Items.Add("Blue");
            cbBorderColor.Items.Add("Red");
            cbBorderColor.Items.Add("Green");
            cbBorderColor.Items.Add("Black");
        }

        private Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            return ellipse;
        }

        private Rectangle CreateRectangle(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Rectangle rectangle = new Rectangle { Width = width, Height = height };
            double left = desiredCenterX - (width / 2);
            double top = desiredCenterY - (height / 2);

            rectangle.Margin = new Thickness(left, top, 0, 0);
            return rectangle;
        }

        private Polyline DrawLine(List<Point> points)
        {
            Polyline line = new Polyline();
            PointCollection collection = new PointCollection();
            foreach (Point p in points)
            {
                collection.Add(p);
            }
            collection.Add(points[0]);
            line.Points = collection;
            line.Stroke = new SolidColorBrush(Colors.Black);

            return line;
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtDraw_Click(object sender, RoutedEventArgs e)
        {
            #region Settings parameters
            double height = Double.Parse(tbHeight.Text);
            double width = Double.Parse(tbWidth.Text);
            double borderThickness = Int32.Parse(tbBorderThickness.Text);
            #endregion

            if (eT == "ellipse")
            {
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;

                Ellipse ellipse = CreateEllipse(width, height, pt.X, pt.Y);
                ellipse.StrokeThickness = borderThickness;
                ellipse.Stroke = blackBrush;

                ellipse.Fill = blueBrush;

                MainWindow.Object = ellipse;
            }
            else if (eT == "rectangle")
            {
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;

                Rectangle rectangle = CreateRectangle(width, height, pt.X, pt.Y);
                rectangle.StrokeThickness = borderThickness;
                rectangle.Stroke = blackBrush;

                rectangle.Fill = blueBrush;

                MainWindow.Object = rectangle;
            }
            else if (eT == "polygon")
            {
                Polyline polyline = DrawLine(points);
                polyline.StrokeThickness = borderThickness;
                MainWindow.Object = polyline;
            }

            this.Close();
        }
    }
}
