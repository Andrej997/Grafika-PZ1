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
        public static List<object> UndoList;
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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            List = new List<object>();
            UndoList = new List<object>();
            PolygonPoints = new List<Point>();
            AllCleared = false;
            IsPolygon = false;
            index = -1;
        }

        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (clickedName == "polygon")
            {
                IsPolygon = true;
                Point = Mouse.GetPosition(canvas);
                PolygonPoints.Add(Point);
            }
            else if(clickedName == "ellipse" || clickedName == "rectangle")
            {
                // posto nije pologon aktiviran, on se mora za svaki slucaj deaktivirati
                IsPolygon = false; 

                Point = Mouse.GetPosition(canvas);
                Settings settingsWindow = new Settings(clickedName, Point);
                settingsWindow.ShowDialog();
                if (clickedName == "ellipse")
                {
                    Ellipse ellipse = (Ellipse)Object;
                    ellipse.MouseLeftButtonDown += OnObjectClicked; 
                    canvas.Children.Add(ellipse);
                }
                else if (clickedName == "rectangle")
                {
                    Rectangle rectangle = (Rectangle)Object;
                    rectangle.MouseLeftButtonDown += OnObjectClicked;
                    canvas.Children.Add(rectangle);
                }
                List.Add(Object);

                // posto smo dodali novi element
                // cistimo undo listu, da redo ne bi
                // mogao da radi
                UndoList.Clear();
            }
        }

        private void MenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = (MenuItem)sender;
            clickedName = clicked.Name;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPolygon == true) 
            {
                Settings settingsWindow = new Settings(clickedName, PolygonPoints);
                settingsWindow.ShowDialog();

                Polygon polygon = (Polygon)Object;
                polygon.MouseLeftButtonDown += OnObjectClicked;
                canvas.Children.Add(polygon);

                PolygonPoints.Clear();

                List.Add(Object);

                // posto smo dodali novi element
                // cistimo undo listu, da redo ne bi
                // mogao da radi
                UndoList.Clear();

                // posto je poligon iscrtan
                // treba da ga deaktiviramo
                IsPolygon = false;
            }
        }

        private void MenuItem_Clear(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse;
            Rectangle rectangle;
            Polygon polygon;

            foreach (var obj in List)
            {
                try
                {
                    ellipse = (Ellipse)obj;
                    //prvo dodamo u Undo listu da bi mogli da vratimo
                    UndoList.Add(ellipse);
                    canvas.Children.Remove(ellipse);
                }
                catch
                {
                    try
                    {
                        rectangle = (Rectangle)obj;
                        //prvo dodamo u Undo listu da bi mogli da vratimo
                        UndoList.Add(rectangle);
                        canvas.Children.Remove(rectangle);
                    }
                    catch 
                    {
                        try
                        {
                            polygon = (Polygon)obj;
                            //prvo dodamo u Undo listu da bi mogli da vratimo
                            UndoList.Add(polygon);
                            canvas.Children.Remove(polygon);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            AllCleared = true;
        }

        private void MenuItem_Undo(object sender, MouseButtonEventArgs e)
        {
            // ako je uradjen clear, vracamo sve odjednom
            if (AllCleared == true)
            {
                foreach (var obj in UndoList)
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
                            }
                        }
                    }
                }
                UndoList.Clear();
                // posto je vracemo, onemogucavamo ovu naredbu
                AllCleared = false;
            }
            else if (canvas.Children.Count > 0)
            {
                    UndoList.Add(canvas.Children[canvas.Children.Count - 1]);
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);
                
            }
        }

        private void MenuItem_Redo(object sender, MouseButtonEventArgs e)
        {
            if (UndoList.Count > 0 && AllCleared == false /*da ne moze da se pozove ako je prethodna naredba bila clear*/)
            {
                try
                {
                    canvas.Children.Add((Ellipse)UndoList[0]);
                    UndoList.RemoveAt(0);
                }
                catch
                {
                    try
                    {
                        canvas.Children.Add((Rectangle)UndoList[0]);
                        UndoList.RemoveAt(0);
                    }
                    catch
                    {
                        try
                        {
                            canvas.Children.Add((Polygon)UndoList[0]);
                            UndoList.RemoveAt(0);
                        }
                        catch
                        {
                        }
                    }
                }
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
                    }
                }
            }
        }
    }
}
