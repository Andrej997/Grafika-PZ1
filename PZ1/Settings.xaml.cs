using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        private Image imagePriv;
        private SolidColorBrush borderColor;
        private SolidColorBrush fillColor;
        #endregion

        #region Constructors
        public Settings(string elementType, Point point)
        {
            InitializeComponent();
            eT = elementType;
            if (eT == "image")
            {
                lBorderThickness.Content = "Choose image";
                cbBorderColor.Visibility = Visibility.Hidden;
                lFillColor.Visibility = Visibility.Hidden;
                lBorderColor.Visibility = Visibility.Hidden; 
                cbFillColor.Visibility = Visibility.Hidden;
                tbBorderThickness.Visibility = Visibility.Hidden;
            }
            else
            {
                btImage.Visibility = Visibility.Hidden;
            }
            pt = point;
        }

        public Settings(string elementType, List<Point> lpoints)
        {
            InitializeComponent();
            eT = elementType;
            points = lpoints;
            tbHeight.IsReadOnly = true;
            tbWidth.IsReadOnly = true;
            btImage.Visibility = Visibility.Hidden;
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
        }

        public Settings(Image image)
        {
            InitializeComponent();
            eT = "image";
            imagePriv = image;
            tbHeight.Text = image.Height.ToString();
            tbWidth.Text = image.Width.ToString();
            LASFCI();
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
        }

        #endregion

        #region DrawingShapes
        private Ellipse CreateEllipse(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Ellipse ellipse = new Ellipse { Width = width, Height = height };
            double left = desiredCenterX;// - (width / 2); // deo ako zelimo da tracka bude u centru
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

        private Image CreateImage(double width, double height, double desiredCenterX, double desiredCenterY)
        {
            Image image = new Image { Width = width, Height = height };
            double left = desiredCenterX;
            double top = desiredCenterY;

            image.Margin = new Thickness(left, top, 0, 0);
            image.Stretch = Stretch.Fill;
            return image;
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

            return line;
        }
        #endregion

        #region Buttons
        private void BtCancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void BtDraw_Click(object sender, RoutedEventArgs e)
        {
            #region Settings parameters
            double height = 0;
            Double.TryParse(tbHeight.Text, out height);
            double width = 0;
            Double.TryParse(tbWidth.Text, out width);
            double borderThickness = 0; // ovo moze biti 0, jer mozda korisnik ne zeli okvir
            Double.TryParse(tbBorderThickness.Text, out borderThickness);

            // visina i sirina ne smeju da budu manja od 0, ali nemaju ni smisla da budu 0,
            // ali to ne vazi za poligon!
            if ((height <= 0 || width <= 0 || borderThickness < 0) && eT != "polygon")
            {
                System.Windows.MessageBox.Show("You need to enter number parametars!", "Warning");
                return;
            }
            #endregion

            if (eT == "ellipse")
            {
                if (changingElement == true)
                {
                    ellipsePriv.StrokeThickness = borderThickness;
                    if (borderColor != null)
                        ellipsePriv.Stroke = borderColor;
                    if (fillColor != null)
                        ellipsePriv.Fill = fillColor;

                    MainWindow.tempObject = ellipsePriv.StrokeThickness;
                }
                else
                {
                    Ellipse ellipse = CreateEllipse(width, height, pt.X, pt.Y);
                    CheckColors();
                    ellipse.StrokeThickness = borderThickness;
                    ellipse.Stroke = borderColor;
                    ellipse.Fill = fillColor;

                    MainWindow.Object = ellipse;
                }
            }
            else if (eT == "rectangle")
            {
                if (changingElement == true)
                {
                    rectanglePriv.StrokeThickness = borderThickness;
                    if (borderColor != null)
                        rectanglePriv.Stroke = borderColor;
                    if (fillColor != null)
                        rectanglePriv.Fill = fillColor;
                    MainWindow.tempObject = rectanglePriv.StrokeThickness;
                }
                else
                {
                    Rectangle rectangle = CreateRectangle(width, height, pt.X, pt.Y);
                    CheckColors();
                    rectangle.StrokeThickness = borderThickness;
                    rectangle.Stroke = borderColor;
                    rectangle.Fill = fillColor;

                    MainWindow.Object = rectangle;
                }
            }
            else if (eT == "polygon")
            {
                if (changingElement == true)
                {
                    polygonPriv.StrokeThickness = borderThickness;
                    if (fillColor != null)
                        polygonPriv.Fill = fillColor;
                    if (borderColor != null)
                    polygonPriv.Stroke = borderColor;
                    MainWindow.tempObject = polygonPriv.StrokeThickness;
                }
                else
                {
                    Polygon polygon = DrawLine(points);
                    CheckColors();
                    polygon.StrokeThickness = borderThickness;
                    polygon.Fill = fillColor;
                    polygon.Stroke = borderColor;
                    MainWindow.Object = polygon;
                }
            }
            else if (eT == "image")
            {
                if (changingElement == true)
                {
                    MainWindow.tempObject = imagePriv;
                }
            }

            this.Close();
        }

        private void BtImage_Click(object sender, RoutedEventArgs e)
        {
            double height = 0;
            Double.TryParse(tbHeight.Text, out height);
            double width = 0;
            Double.TryParse(tbWidth.Text, out width);

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFileName = dlg.FileName;
                //lBorderThickness.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();

                if (changingElement == true)
                {
                    imagePriv.Source = bitmap;
                    MainWindow.tempObject = imagePriv;
                    this.Close();
                }
                else
                {
                    Image dynamicImage = CreateImage(width, height, pt.X, pt.Y);
                    dynamicImage.Source = bitmap;

                    MainWindow.Object = dynamicImage;
                }
            }
        }
        #endregion

        #region Methods
        private void CheckColors()
        {
            // korisniku je dozvoljeno da ne unese boje,
            // ali je onda podrazumevano da je u boji canvasa, odnosno belo.
            if (borderColor == null)
                borderColor = new SolidColorBrush(Colors.White);
            if (fillColor == null)
                fillColor = new SolidColorBrush(Colors.White);
        }

        private void LockAndSet()
        {
            tbHeight.IsReadOnly = true;
            tbWidth.IsReadOnly = true;
            changingElement = true;
            btDraw.Content = "Change";
            btImage.Visibility = Visibility.Hidden;
        }

        // Lock and Set For Changing Image
        private void LASFCI()
        {
            lBorderThickness.Content = "Choose image";
            cbBorderColor.Visibility = Visibility.Hidden;
            lFillColor.Visibility = Visibility.Hidden;
            lBorderColor.Visibility = Visibility.Hidden;
            cbFillColor.Visibility = Visibility.Hidden;
            tbBorderThickness.Visibility = Visibility.Hidden;
            tbHeight.IsReadOnly = true;
            tbWidth.IsReadOnly = true;
            btDraw.Content = "Change";
            changingElement = true;
        }

        #endregion

        #region Comboboxes
        private void CbBorderColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color borderClr = (Color)(cbBorderColor.SelectedItem as PropertyInfo).GetValue(1, null);
            borderColor = new SolidColorBrush(borderClr);
        }

        private void CbFillColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color fillClr = (Color)(cbFillColor.SelectedItem as PropertyInfo).GetValue(1, null);
            fillColor = new SolidColorBrush(fillClr);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbBorderColor.ItemsSource = typeof(Colors).GetProperties();
            cbFillColor.ItemsSource = typeof(Colors).GetProperties();
        }
        #endregion
    }
}
