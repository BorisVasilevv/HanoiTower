using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AlgorithmComplexity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public class Tower
    {

        public Tower(Stack<Rectangle> rectangles, int coordinateX, int coordinateY)
        {
            Rings = rectangles;
            X = coordinateX;
            Y = coordinateY;
        }
        public Stack<Rectangle> Rings;
        public int X;
        public int Y;
    }

    public partial class MainWindow : Window
    {

        public static Canvas Canvas1;
        public static Color[] RectFill = new Color[] {Colors.Green, Colors.Blue,
            Colors.Gray, Colors.Honeydew, Colors.Olive, Colors.Red,
            Colors.Teal,Colors.Yellow,Colors.Aqua};

        public static readonly Tower Tower1 = new Tower(new Stack<Rectangle>(), 175, 300);
        public static readonly Tower Tower2 = new Tower(new Stack<Rectangle>(), 425, 300);
        public static readonly Tower Tower3 = new Tower(new Stack<Rectangle>(), 675, 300);
        public static List<Tower> AllTowers = new List<Tower>() { Tower1, Tower2, Tower3 };
        public static bool IsProgramStart = false;
        public static int AmountOfDisk = 5;
        public static List<(int, int)> Moves = new List<(int, int)>();
        public enum Towers
        {
            FirstTower = 0,
            SecondTower = 1,
            ThirdTower = 2
        }

        public MainWindow()
        {
            InitializeComponent();
            Canvas1 = canvas1;
            for (int i = 0; i < AmountOfDisk; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Fill = new SolidColorBrush(RectFill[i]);
                rectangle.Stroke = new SolidColorBrush(Colors.Black);
                rectangle.Width = 120 - 10 * i;
                rectangle.Height = 20;
                Tower tower = AllTowers[(int)Towers.FirstTower];
                Canvas.SetTop(rectangle, tower.Y - 20 * (i + 1));
                Canvas.SetLeft(rectangle, tower.X - rectangle.Width / 2);
                tower.Rings.Push(rectangle);
                canvas1.Children.Add(rectangle);
            }
        }

        class HanoiTower
        {
            //start - откуда кладем, end - куда кладем, temp - промежуточный стержень, disks - кол-во дисков
            public static void MoveDisks(int start, int temp, int end, int disks)
            {
                if (disks > 1)
                {
                    MoveDisks(start, end, temp, disks - 1);
                }
                //Console.WriteLine("{0} -> {1}", start, end);
                Moves.Add((start, end));
                if (disks > 1)
                {
                    MoveDisks(temp, start, end, disks - 1);
                }
            }
        }

        class DrawHelper
        {
            public static void DrawRingMove(int from, int to)
            {
                Rectangle rect = AllTowers[from].Rings.Pop();
                Stack<Rectangle> rings = AllTowers[to].Rings;
                rings.Push(rect);
                int amountOfRings = rings.Count();
                Canvas.SetTop(rect, AllTowers[to].Y - 20 * (amountOfRings));
                Canvas.SetLeft(rect, AllTowers[to].X - rect.Width / 2);
            }
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsProgramStart)
            {
                HanoiTower.MoveDisks(0, 1, 2, AmountOfDisk);
                IsProgramStart = true;
            }
        }

        public static int CounterOfMoves = 0;

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            if (CounterOfMoves < Moves.Count)
            {
                DrawHelper.DrawRingMove(Moves[CounterOfMoves].Item1, Moves[CounterOfMoves].Item2);
                CounterOfMoves++;
            }
        }

        private void automatically_button_Click(object sender, RoutedEventArgs e)
        {
            canvas1.MouseMove += MouseMoveAutomatically;
        }

        private void MouseMoveAutomatically(object sender, RoutedEventArgs e)
        {
            if (CounterOfMoves < Moves.Count)
            {
                DrawHelper.DrawRingMove(Moves[CounterOfMoves].Item1, Moves[CounterOfMoves].Item2);
                CounterOfMoves++;
                Thread.Sleep(500);
            }
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
