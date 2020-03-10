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
        #region Fields
        private string eT;
        private Point pt;
        private List<Point> points;
        // promenljiva koja govori da je stigao
        // element koji se menja
        private bool changingElement;
        private Ellipse ellipsePriv;
        private Rectangle rectanglePriv;
        private Polygon polygonPriv;
        #endregion

        #region Constructors
        public Settings(string elementType, Point point)
        {
            InitializeComponent();
            eT = elementType;
            pt = point;

            SetComboBox();
        }

        public Settings(string elementType, List<Point> lpoints)
        {
            InitializeComponent();
            eT = elementType;
            points = lpoints;

            SetComboBox();
        }

        public Settings(Ellipse ellipse)
        {
            InitializeComponent();
            eT = "ellipse";
            ellipsePriv = ellipse;
            tbHeight.Text = ellipse.Height.ToString();
            tbWidth.Text = ellipse.Width.ToString();
            tbBorderThickness.Text = ellipse.StrokeThickness.ToString();
            LockAndSet();
            SetComboBox();
        }

        public Settings(Rectangle rectangle)
        {
            InitializeComponent();
            eT = "rectangle";
            rectanglePriv = rectangle;
            tbHeight.Text = rectangle.Height.ToString();
            tbWidth.Text = rectangle.Width.ToString();
            tbBorderThickness.Text = rectangle.StrokeThickness.ToString();
            LockAndSet();
            SetComboBox();
        }

        public Settings(Polygon polygon)
        {
            InitializeComponent();
            eT = "polygon";
            polygonPriv = polygon;
            tbHeight.Text = "0";
            tbWidth.Text = "0";
            tbBorderThickness.Text = polygon.StrokeThickness.ToString();
            LockAndSet();
            SetComboBox();
        }
        #endregion

        #region DrawingShapes
        private Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX;// - (width / 2);
            double top = desiredCenterY;// - (height / 2);

            ellipse.Margin = new Thickness(left, top, 0, 0);
            return ellipse;
        }

        private Rectangle CreateRectangle(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Rectangle rectangle = new Rectangle { Width = width, Height = height };
            double left = desiredCenterX;
            double top = desiredCenterY;

            rectangle.Margin = new Thickness(left, top, 0, 0);
            return rectangle;
        }

        private Polygon DrawLine(List<Point> points)
        {
            Polygon line = new Polygon();
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
        #endregion

        #region Buttons
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
                if (changingElement == true)
                {
                    ellipsePriv.StrokeThickness = borderThickness;
                    MainWindow.tempObject = ellipsePriv.StrokeThickness;
                }
                else
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
            }
            else if (eT == "rectangle")
            {
                if (changingElement == true)
                {
                    rectanglePriv.StrokeThickness = borderThickness;
                    MainWindow.tempObject = rectanglePriv.StrokeThickness;
                }
                else
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
            }
            else if (eT == "polygon")
            {
                if (changingElement == true)
                {
                    polygonPriv.StrokeThickness = borderThickness;
                    MainWindow.tempObject = polygonPriv.StrokeThickness;
                }
                else
                {
                    SolidColorBrush blueBrush = new SolidColorBrush();
                    blueBrush.Color = Colors.Blue;

                    Polygon polygon = DrawLine(points);
                    polygon.StrokeThickness = borderThickness;
                    polygon.Fill = blueBrush;
                    MainWindow.Object = polygon;
                }
            }

            this.Close();
        }
        #endregion

        #region Methods
        private void SetComboBox()
        {
            cbBorderColor.Items.Add("Blue");
            cbBorderColor.Items.Add("Red");
            cbBorderColor.Items.Add("Green");

            cbFillColor.Items.Add("Blue");
            cbFillColor.Items.Add("Red");
            cbFillColor.Items.Add("Green");
            cbFillColor.Items.Add("Black");
        }

        private void LockAndSet()
        {
            tbHeight.IsReadOnly = true;
            tbWidth.IsReadOnly = true;
            changingElement = true;
            btDraw.Content = "Change";
        }
        #endregion
    }
}
