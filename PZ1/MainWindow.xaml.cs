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
        private static List<object> objectList;
        private static List<object> clearList;
        private static Stack<object> stackUndo;
        public static List<Point> PolygonPoints;
        // promenljiva koja se aktivira kada je sve ocisnjeno
        // da bi mogli odjenom da vratimo sve
        public bool AllCleared;
        // posto se levim klikom na povrsinu 
        // otvara setting za podesavanje param
        // ova promenljiva sluzi da obezbedi
        // to otvaranja samo kada je potrebno
        private bool IsPolygon;
        // index elementa koji menjamo
        int index;
        // izmenjen objekat
        public static object tempObject;
        // za bojenje menija
        private SolidColorBrush menuItemBrush;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            objectList = new List<object>();
            // lista koja sluzi samo za ciscenje 
            // celog canvasa
            clearList = new List<object>();
            // koristimo Stack za undo/redo akciju
            // jer je O(1)
            stackUndo = new Stack<object>();
            PolygonPoints = new List<Point>();
            AllCleared = false;
            IsPolygon = false;
            index = -1;
            menuItemBrush = new SolidColorBrush();
            menuItemBrush.Color = Colors.LightGray;
        }
        #endregion

        #region Creating Elements
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (clickedName == "polygon")
            {
                IsPolygon = true;
                Point = Mouse.GetPosition(canvas);
                PolygonPoints.Add(Point);
            }
            else if (clickedName == "ellipse" || clickedName == "rectangle" || clickedName == "image")
            {
                // posto nije pologon aktiviran, on se mora za svaki slucaj deaktivirati
                IsPolygon = false;

                Point = Mouse.GetPosition(canvas);
                Settings settingsWindow = new Settings(clickedName, Point);
                settingsWindow.ShowDialog();
                if (Object != null)
                {
                    if (clickedName == "ellipse")
                    {
                        Ellipse ellipseTemp = (Ellipse)Object;
                        ellipseTemp.MouseLeftButtonDown += OnObjectClicked;
                        canvas.Children.Add(ellipseTemp);
                    }
                    else if (clickedName == "rectangle")
                    {
                        Rectangle rectangleTemp = (Rectangle)Object;
                        rectangleTemp.MouseLeftButtonDown += OnObjectClicked;
                        canvas.Children.Add(rectangleTemp);
                    }
                    else if (clickedName == "image")
                    {
                        Image image = (Image)Object;
                        image.MouseLeftButtonDown += OnObjectClicked;
                        canvas.Children.Add(image);
                    }
                    objectList.Add(Object);

                    // posto je nacrtano, ne zelimo da pamtimo
                    // koji je prethodni bio oblik
                    clickedName = null;

                    // posto smo dodali novi element
                    // cistimo undo listu, da redo ne bi
                    // mogao da radi
                    clearList.Clear();
                }
                ellipse.Background = null;
                rectangle.Background = null;
                image.Background = null;
            }
        }

        private void MenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = (MenuItem)sender;
            clickedName = clicked.Name;
            if (clickedName == "ellipse")
            {
                ellipse.Background = menuItemBrush;
                rectangle.Background = null;
                polygon.Background = null;
                image.Background = null;
            }
            else if (clickedName == "rectangle")
            {
                ellipse.Background = null;
                rectangle.Background = menuItemBrush;
                polygon.Background = null;
                image.Background = null;
            }
            else if (clickedName == "polygon")
            {
                ellipse.Background = null;
                rectangle.Background = null;
                polygon.Background = menuItemBrush;
                image.Background = null;
            }
            else if (clickedName == "image")
            {
                ellipse.Background = null;
                rectangle.Background = null;
                polygon.Background = null;
                image.Background = menuItemBrush;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPolygon == true)
            {
                Settings settingsWindow = new Settings(clickedName, PolygonPoints);
                settingsWindow.ShowDialog();
                if (Object != null)
                {
                    Polygon polygon = (Polygon)Object;
                    polygon.MouseLeftButtonDown += OnObjectClicked;
                    canvas.Children.Add(polygon);

                    PolygonPoints.Clear();

                    objectList.Add(Object);

                    // posto smo dodali novi element
                    // cistimo undo listu, da redo ne bi
                    // mogao da radi
                    clearList.Clear();

                    // posto je poligon iscrtan
                    // treba da ga deaktiviramo
                    IsPolygon = false;
                }
                polygon.Background = null;
            }
        }

        private void OnObjectClicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Ellipse ellipse = (Ellipse)sender;
                // pronalazak indexa elementa
                for (int i = 0; i < canvas.Children.Count; i++)
                {
                    if (ellipse == canvas.Children[i])
                        index = i;
                }
                Settings settings = new Settings(ellipse);
                settings.ShowDialog();
                // prepisivanje starog s novim
                canvas.Children[index] = (Ellipse)tempObject;
                index = -1;
            }
            catch
            {
                try
                {
                    Rectangle rectangle = (Rectangle)sender;
                    // pronalazak indexa elementa
                    for (int i = 0; i < canvas.Children.Count; i++)
                    {
                        if (rectangle == canvas.Children[i])
                            index = i;
                    }
                    Settings settings = new Settings(rectangle);
                    settings.ShowDialog();
                    // prepisivanje starog s novim
                    canvas.Children[index] = (Rectangle)tempObject;
                    index = -1;
                }
                catch
                {
                    try
                    {
                        Polygon polygon = (Polygon)sender;
                        // pronalazak indexa elementa
                        for (int i = 0; i < canvas.Children.Count; i++)
                        {
                            if (polygon == canvas.Children[i])
                                index = i;
                        }
                        Settings settings = new Settings(polygon);
                        settings.ShowDialog();
                        // prepisivanje starog s novim
                        canvas.Children[index] = (Polygon)tempObject;
                        index = -1;
                    }
                    catch
                    {
                        try
                        {
                            Image image = (Image)sender;
                            // pronalazak indexa elementa
                            for (int i = 0; i < canvas.Children.Count; i++)
                            {
                                if (image == canvas.Children[i])
                                    index = i;
                            }
                            Settings settings = new Settings(image);
                            settings.ShowDialog();
                            // prepisivanje starog s novim
                            canvas.Children[index] = (Image)tempObject;
                            index = -1;
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        #endregion

        #region Clear
        private void MenuItem_Clear(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse;
            Rectangle rectangle;
            Polygon polygon;
            Image image;

            foreach (var obj in objectList)
            {
                try
                {
                    ellipse = (Ellipse)obj;
                    //prvo dodamo u Undo listu da bi mogli da vratimo
                    clearList.Add(ellipse);
                    canvas.Children.Remove(ellipse);
                }
                catch
                {
                    try
                    {
                        rectangle = (Rectangle)obj;
                        //prvo dodamo u Undo listu da bi mogli da vratimo
                        clearList.Add(rectangle);
                        canvas.Children.Remove(rectangle);
                    }
                    catch
                    {
                        try
                        {
                            polygon = (Polygon)obj;
                            //prvo dodamo u Undo listu da bi mogli da vratimo
                            clearList.Add(polygon);
                            canvas.Children.Remove(polygon);
                        }
                        catch
                        {
                            try
                            {
                                image = (Image)obj;
                                //prvo dodamo u Undo listu da bi mogli da vratimo
                                clearList.Add(image);
                                canvas.Children.Remove(image);
                            }
                            catch 
                            {
                            }
                        }
                    }
                }
            }
            AllCleared = true;
        }
        #endregion

        #region Undo
        private void MenuItem_Undo(object sender, MouseButtonEventArgs e)
        {
            // ako je uradjen clear, vracamo sve odjednom
            if (AllCleared == true)
            {
                foreach (var obj in stackUndo)
                {
                    var poppedObj = stackUndo.Pop();
                    if (poppedObj is Ellipse)
                    {
                        Ellipse ell = (Ellipse)poppedObj;
                        canvas.Children.Add(ell);
                    }
                    else if (poppedObj is Rectangle)
                    {
                        Rectangle rec = (Rectangle)poppedObj;
                        canvas.Children.Add(rec);
                    }
                    else if (poppedObj is Polygon)
                    {
                        Polygon pol = (Polygon)poppedObj;
                        canvas.Children.Add(pol);
                    }
                    else if (poppedObj is Image)
                    {
                        Image img = (Image)poppedObj;
                        canvas.Children.Add(img);
                    }
                }
                /*
                foreach (var obj in clearList)
                {
                    try
                    {
                        canvas.Children.Add((Ellipse)obj);
                    }
                    catch
                    {
                        try
                        {
                            canvas.Children.Add((Rectangle)obj);
                        }
                        catch
                        {
                            try
                            {
                                canvas.Children.Add((Polygon)obj);
                            }
                            catch
                            {
                                try
                                {
                                    canvas.Children.Add((Image)obj);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                clearList.Clear();*/
                // posto je vracemo, onemogucavamo ovu naredbu
                AllCleared = false;
            }
            else if (canvas.Children.Count > 0)
            {
                //UndoList.Add(canvas.Children[canvas.Children.Count - 1]);

                // smestamo poslednji iz liste na stack
                stackUndo.Push(canvas.Children[canvas.Children.Count - 1]);

                // uklanjamo poslednji sa canvasa
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
        }
        #endregion

        #region Redo
        private void MenuItem_Redo(object sender, MouseButtonEventArgs e)
        {
            if (stackUndo.Count > 0 && AllCleared == false /*da ne moze da se pozove ako je prethodna naredba bila clear*/)
            {
                var poppedObj = stackUndo.Pop();
                if (poppedObj is Ellipse)
                {
                    Ellipse ell = (Ellipse)poppedObj;
                    canvas.Children.Add(ell);
                }
                else if (poppedObj is Rectangle)
                {
                    Rectangle rec = (Rectangle)poppedObj;
                    canvas.Children.Add(rec);
                }
                else if (poppedObj is Polygon)
                {
                    Polygon pol = (Polygon)poppedObj;
                    canvas.Children.Add(pol);
                }
                else if (poppedObj is Image)
                {
                    Image img = (Image)poppedObj;
                    canvas.Children.Add(img);
                }

                //try
                //{
                //    Ellipse ellipseTemp = (Ellipse)stackUndo.Pop();
                //    canvas.Children.Add(ellipseTemp);
                //    //canvas.Children.Add((Ellipse)UndoList[UndoList.Count - 1]);
                //    //UndoList.RemoveAt(UndoList.Count - 1);
                //}
                //catch
                //{
                //    try
                //    {
                //        Rectangle rectangleTemp = (Rectangle)stackUndo.Pop();
                //        canvas.Children.Add(rectangleTemp);
                //        //canvas.Children.Add((Rectangle)UndoList[UndoList.Count - 1]);
                //        //UndoList.RemoveAt(UndoList.Count - 1);
                //    }
                //    catch
                //    {
                //        try
                //        {
                //            Polygon polygonTemp = (Polygon)stackUndo.Pop();
                //            canvas.Children.Add(polygonTemp);
                //            //canvas.Children.Add((Polygon)UndoList[UndoList.Count - 1]);
                //            //UndoList.RemoveAt(UndoList.Count - 1);
                //        }
                //        catch
                //        {
                //            try
                //            {
                //                Image imageTemp = (Image)stackUndo.Pop();
                //                canvas.Children.Add(imageTemp);
                //                //canvas.Children.Add((Image)UndoList[UndoList.Count - 1]);
                //                //UndoList.RemoveAt(UndoList.Count - 1);
                //            }
                //            catch
                //            {
                //            }
                //        }
                //    }
                //}
            }
        }
        #endregion

        #region Methods
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }
        #endregion
    }
}
