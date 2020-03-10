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
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            List = new List<object>();
            UndoList = new List<object>();
            PolygonPoints = new List<Point>();
            AllCleared = false;
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
            Settings settingsWindow = new Settings(clickedName, PolygonPoints);
            settingsWindow.ShowDialog();

            canvas.Children.Add((Polyline)Object);
            PolygonPoints.Clear();

            List.Add(Object);

            // posto smo dodali novi element
            // cistimo undo listu, da redo ne bi
            // mogao da radi
            UndoList.Clear();
        }

        private void MenuItem_Clear(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse;
            Rectangle rectangle;
            Polyline polyline;

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
                            polyline = (Polyline)obj;
                            //prvo dodamo u Undo listu da bi mogli da vratimo
                            UndoList.Add(polyline);
                            canvas.Children.Remove(polyline);
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
                                canvas.Children.Add((Polyline)obj);
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
                            canvas.Children.Add((Polyline)UndoList[0]);
                            UndoList.RemoveAt(0);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}
