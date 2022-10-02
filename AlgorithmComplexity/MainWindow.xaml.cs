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
            Colors.Teal,Colors.Yellow,Colors.Aqua,Colors.DarkGoldenrod,
        Colors.DarkViolet, Colors.DeepPink, Colors.ForestGreen, Colors.MintCream, Colors.Navy};

        public static readonly Tower Tower1 = new Tower(new Stack<Rectangle>(), 175, 300);
        public static readonly Tower Tower2 = new Tower(new Stack<Rectangle>(), 425, 300);
        public static readonly Tower Tower3 = new Tower(new Stack<Rectangle>(), 675, 300);
        public static List<Tower> AllTowers = new List<Tower>() { Tower1, Tower2, Tower3 };
        public static bool IsProgramStart = false;
        public static bool IsProgramReady = false;
        public static int AmountOfDisk;
        public static List<(int, int)> Moves = new List<(int, int)>();
        public static readonly int RingHeigh = 15;



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
            bool isProgramReady = false;

            TextBlock textBlock = new TextBlock();
            textBlock.Width = 300;
            textBlock.Height = 20;
            textBlock.Text = "Input amount of rings from 2 to 15 and press Enter";
            canvas1.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, (canvas1.Width - textBlock.Width) / 2);
            Canvas.SetTop(textBlock, 10);
            Canvas.SetZIndex(textBlock, 15);

            Rectangle startRect = new Rectangle();
            startRect.Width = canvas1.Width;
            startRect.Height = canvas1.Height;
            startRect.Fill = new SolidColorBrush(Colors.Brown);
            canvas1.Children.Add(startRect);
            Canvas.SetLeft(startRect, 0);
            Canvas.SetTop(startRect, 0);
            Canvas.SetZIndex(startRect, 12);


            TextBox textBox = new TextBox();
            textBox.Width = 300;
            textBox.Height = 20;

            canvas1.Children.Add(textBox);
            Canvas.SetLeft(textBox, (canvas1.Width - textBox.Width) / 2);
            Canvas.SetTop(textBox, 30);
            Canvas.SetZIndex(textBox, 15);

            textBox.KeyDown += TextBox_KeyEnterDown; ;
            

            
            
            
        }

        private void TextBox_KeyEnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            TextBox textBox = sender as TextBox;
            if (int.TryParse(textBox.Text, out AmountOfDisk)&& AmountOfDisk >= 2 && AmountOfDisk <= 15)
            {
                IsProgramReady=true;
                canvas1.Children.Clear();
            }
            else
            {
                TextBlock textBlockErr = new TextBlock();
                textBlockErr.Width = 200;
                textBlockErr.Height = 20;
                textBlockErr.Text = "Input error try one more time";
                canvas1.Children.Add(textBlockErr);
                Canvas.SetLeft(textBlockErr, (canvas1.Width - textBlockErr.Width) / 2);
                Canvas.SetTop(textBlockErr, 50);
                Canvas.SetZIndex(textBlockErr, 15);
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

                Canvas.SetTop(rect, AllTowers[to].Y - RingHeigh * (amountOfRings));
                Canvas.SetLeft(rect, AllTowers[to].X - rect.Width / 2);

            }
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            if(!IsProgramReady) return;
            if (!IsProgramStart)
            {
                for (int i = 0; i < AmountOfDisk; i++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = new SolidColorBrush(RectFill[i]);
                    rectangle.Stroke = new SolidColorBrush(Colors.Black);
                    rectangle.Width = 170 - 10 * i;
                    rectangle.Height = RingHeigh;
                    Tower tower = AllTowers[(int)Towers.FirstTower];
                    Canvas.SetTop(rectangle, tower.Y - RingHeigh * (i + 1));
                    Canvas.SetLeft(rectangle, tower.X - rectangle.Width / 2);
                    tower.Rings.Push(rectangle);
                    canvas1.Children.Add(rectangle);
                }
                HanoiTower.MoveDisks(0, 1, 2, AmountOfDisk);
                IsProgramStart = true;
            }
        }

        public static int CounterOfMoves = 0;

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramStart)
            {
                if (CounterOfMoves < Moves.Count)
                {
                    DrawHelper.DrawRingMove(Moves[CounterOfMoves].Item1, Moves[CounterOfMoves].Item2);
                    CounterOfMoves++;

                }
            }
        }

        private void automatically_button_Click(object sender, RoutedEventArgs e)
        {
            if (IsProgramStart)
            {
                canvas1.MouseMove += MouseMoveAutomatically;

                TextBlock textBlockErr = new TextBlock();
                textBlockErr.Width = 200;
                textBlockErr.Height = 20;
                textBlockErr.Text = "Move mouse to program's work";
                canvas1.Children.Add(textBlockErr);
                Canvas.SetLeft(textBlockErr, (canvas1.Width - textBlockErr.Width) / 2);
                Canvas.SetTop(textBlockErr, 50);
                Canvas.SetZIndex(textBlockErr, 15);
            }
        }

        private void MouseMoveAutomatically(object sender, RoutedEventArgs e)
        {

            if (CounterOfMoves < Moves.Count)
            {
                DrawHelper.DrawRingMove(Moves[CounterOfMoves].Item1, Moves[CounterOfMoves].Item2);
                CounterOfMoves++;
                Thread.Sleep(50);
            }
        }

        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
