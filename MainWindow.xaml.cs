using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
//son
using System.Media;
using System.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd = new();

        private DispatcherTimer t = new();
        private DispatcherTimer t2 = new();
        private DispatcherTimer tSound = new();
        private DispatcherTimer timer = new();

        private bool colision = false;
        private int dropSpeed = 43;
        private int rotation = 0;

        private List<string> Pieces = new() { "S", "Z", "L", "I", "[]", "reverseL", "T" };
        private string Piece = "";
        private string prochainePiece = "";

        private bool line = false;
        private int int_line = 0;
        private int lineScore = 0;
        public long score = 0;
        private int niveau = 0;
        private double temps = 2000;
        private bool first = true;
        private int TimerS = 0;
        private int TimerMin = 0;
        private int TimerH = 0;

        private List<Rectangle> rectangles = new();
        private List<Rectangle> Top2 = new();
        private List<Rectangle> Top45 = new();
        private List<Rectangle> Top88 = new();
        private List<Rectangle> Top131 = new();
        private List<Rectangle> Top174 = new();
        private List<Rectangle> Top217 = new();
        private List<Rectangle> Top260 = new();
        private List<Rectangle> Top303 = new();
        private List<Rectangle> Top346 = new();
        private List<Rectangle> Top389 = new();
        private List<Rectangle> Top432 = new();
        private List<Rectangle> Top475 = new();
        private List<Rectangle> Top518 = new();
        private List<Rectangle> Top561 = new();
        private List<Rectangle> Top604 = new();
        private List<Rectangle> Top647 = new();
        private List<Rectangle> Top690 = new();
        private List<Rectangle> Top733 = new();

        private SoundPlayer sound = new($@"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\net6.0-windows\\", "")}Son//tetrisSong.wav");
        private SoundPlayer soundDeath = new($@"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\net6.0-windows\\", "")}Son//[ONTIVA.COM] Game Over (8-Bit Music)-HQ.wav");

        public bool wantClose = false;

        public MainWindow()
        {
            InitializeComponent();

            Launcher launcher = new();
            launcher.ShowDialog();

            t.Tick += GameEngine;
            t.Interval = TimeSpan.FromMilliseconds(temps);
            t2.Tick += GameEngine;
            t2.Interval = TimeSpan.FromMilliseconds(20);
            tSound.Tick += MainTheme;
            tSound.Interval = TimeSpan.FromSeconds(103);
            sound.Play();
            tSound.Start();
            timer.Tick += TiMeR;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            MettrePiece(Pieces[rnd.Next(0, Pieces.Count - 1)]);

            ProchainePiece();

            t.Start();
            t2.Stop();
        }

        private void TiMeR(object? sender, EventArgs e)
        {
            TimerS += 1;
            if (TimerS >= 60)
            {
                TimerS -= 60;
                TimerMin++;
            }
            if (TimerMin >= 60)
            {
                TimerMin -= 60;
                TimerH++;
            }
            if (TimerH <= 0)
            {
                if (TimerMin < 10)
                {
                    if (TimerS < 10)
                        TIMER.Content = $"Timer = 0{TimerMin}:0{TimerS}";
                    else
                        TIMER.Content = $"Timer = 0{TimerMin}:{TimerS}";
                }
                else
                {
                    if (TimerS < 10)
                        TIMER.Content = $"Timer = {TimerMin}:0{TimerS}";
                    if (TimerS < 10)
                        TIMER.Content = $"Timer = {TimerMin}:{TimerS}";
                }
            }
            else
            {
                if (TimerH < 10)
                {
                    if (TimerMin < 10)
                    {
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = 0{TimerH}:0{TimerMin}:0{TimerS}";
                        else
                            TIMER.Content = $"Timer = 0{TimerH}:0{TimerMin}:{TimerS}";
                    }
                    else
                    {
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = 0{TimerH}:{TimerMin}:0{TimerS}";
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = 0{TimerH}:{TimerMin}:{TimerS}";
                    }
                }
                else
                {
                    if (TimerMin < 10)
                    {
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = {TimerH}:0{TimerMin}:0{TimerS}";
                        else
                            TIMER.Content = $"Timer = {TimerH}:0{TimerMin}:{TimerS}";
                    }
                    else
                    {
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = {TimerH}:{TimerMin}:0{TimerS}";
                        if (TimerS < 10)
                            TIMER.Content = $"Timer = {TimerH}:{TimerMin}:{TimerS}";
                    }
                }
            }
        }

        private void MainTheme(object? sender, EventArgs e)
        {
            sound.Play();
        }

        private void Position(Rectangle rectangle, double left, double top)
        {
            Canvas.SetLeft(rectangle, left);
            Canvas.SetTop(rectangle, top);
        }

        private void Couleur(Rectangle r1, Rectangle r2, Rectangle r3, Rectangle r4, string piece)
        {
            SolidColorBrush couleur = new();
            if (piece == "Z")
                couleur = new SolidColorBrush(Colors.Red);
            if (piece == "L")
                couleur = new SolidColorBrush(Colors.Orange);
            if (piece == "[]")
                couleur = new SolidColorBrush(Colors.Yellow);
            if (piece == "S")
                couleur = new SolidColorBrush(Colors.LightGreen);
            if (piece == "I")
                couleur = new SolidColorBrush(Colors.LightBlue);
            if (piece == "reverseL")
                couleur = new SolidColorBrush(Colors.Blue);
            if (piece == "T")
                couleur = new SolidColorBrush(Colors.Purple);

            r1.Fill = couleur;
            r2.Fill = couleur;
            r3.Fill = couleur;
            r4.Fill = couleur;
        }

        private void MettrePiece(string piece)
        {
            Couleur(carré_1, carré_2, carré_3, carré_4, piece);

            switch (piece)
            {
                #region I
                case "I":
                    Piece = "I";

                    Position(carré_1, 392, 45);
                    Position(carré_2, 442, 45);
                    Position(carré_3, 492, 45);
                    Position(carré_4, 542, 45);
                    break;
                #endregion

                #region L
                case "L":
                    Piece = "L";

                    Position(carré_1, 442, 2);
                    Position(carré_2, 442, 45);
                    Position(carré_3, 442, 88);
                    Position(carré_4, 492, 88);
                    break;
                #endregion

                #region S
                case "S":
                    Piece = "S";

                    Position(carré_1, 392, 45);
                    Position(carré_2, 442, 45);
                    Position(carré_3, 442, 2);
                    Position(carré_4, 492, 2);
                    break;
                #endregion

                #region T
                case "T":
                    Piece = "T";

                    Position(carré_1, 392, 2);
                    Position(carré_2, 442, 2);
                    Position(carré_3, 442, 45);
                    Position(carré_4, 492, 2);
                    break;
                #endregion

                #region Z
                case "Z":
                    Piece = "Z";

                    Position(carré_1, 392, 2);
                    Position(carré_2, 442, 2);
                    Position(carré_3, 442, 45);
                    Position(carré_4, 492, 45);
                    break;
                #endregion

                #region []
                case "[]":
                    Piece = "[]";

                    Position(carré_1, 442, 2);
                    Position(carré_2, 492, 2);
                    Position(carré_3, 442, 45);
                    Position(carré_4, 492, 45);
                    break;
                #endregion

                #region reverseL
                case "reverseL":
                    Piece = "reverseL";

                    Position(carré_1, 442, 88);
                    Position(carré_2, 492, 88);
                    Position(carré_3, 492, 45);
                    Position(carré_4, 492, 2);
                    break;
                    #endregion
            }
        }

        private void ProchainePiece()
        {
            string pieceActuelle = Pieces[rnd.Next(0, Pieces.Count)];

            Couleur(prochain1, prochain2, prochain3, prochain4, pieceActuelle);

            switch (pieceActuelle)
            {
                #region []
                case "[]":
                    prochainePiece = "[]";

                    Position(prochain1, 823, 492);
                    Position(prochain2, 873, 492);
                    Position(prochain3, 823, 535);
                    Position(prochain4, 873, 535);
                    break;
                #endregion

                #region reverseL
                case "reverseL":
                    prochainePiece = "reverseL";

                    Position(prochain1, 875, 470);
                    Position(prochain2, 875, 513);
                    Position(prochain3, 875, 556);
                    Position(prochain4, 825, 556);
                    break;
                #endregion

                #region L
                case "L":
                    prochainePiece = "L";

                    Position(prochain1, 825, 470);
                    Position(prochain2, 825, 513);
                    Position(prochain3, 825, 556);
                    Position(prochain4, 875, 556);
                    break;
                #endregion

                #region I
                case "I":
                    prochainePiece = "I";

                    Position(prochain1, 770, 512);
                    Position(prochain2, 820, 512);
                    Position(prochain3, 870, 512);
                    Position(prochain4, 920, 512);
                    break;
                #endregion

                #region T
                case "T":
                    prochainePiece = "T";

                    Position(prochain1, 800, 490);
                    Position(prochain2, 850, 490);
                    Position(prochain3, 900, 490);
                    Position(prochain4, 850, 533);
                    break;
                #endregion

                #region S
                case "Z":
                    prochainePiece = "Z";

                    Position(prochain1, 800, 490);
                    Position(prochain2, 850, 490);
                    Position(prochain3, 850, 533);
                    Position(prochain4, 900, 533);
                    break;
                #endregion

                #region Z
                case "S":
                    prochainePiece = "S";

                    Position(prochain1, 800, 533);
                    Position(prochain2, 850, 533);
                    Position(prochain3, 850, 490);
                    Position(prochain4, 900, 490);
                    break;
                    #endregion
            }
        }

        private void GameOver(object? sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void GameEngine(object? sender, EventArgs e)
        {
            Canvas.SetTop(carré_1, Canvas.GetTop(carré_1) + dropSpeed);
            Canvas.SetTop(carré_2, Canvas.GetTop(carré_2) + dropSpeed);
            Canvas.SetTop(carré_3, Canvas.GetTop(carré_3) + dropSpeed);
            Canvas.SetTop(carré_4, Canvas.GetTop(carré_4) + dropSpeed);
            foreach (Rectangle x in rectangles)
            {
                Rect rect1 = new(Canvas.GetLeft(carré_1), Canvas.GetTop(carré_1), carré_1.Width, carré_1.Height);
                Rect rect2 = new(Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2), carré_2.Width, carré_2.Height);
                Rect rect3 = new(Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3), carré_3.Width, carré_3.Height);
                Rect rect4 = new(Canvas.GetLeft(carré_4), Canvas.GetTop(carré_4), carré_4.Width, carré_4.Height);
                Rect rect5 = new(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                if (rect1.IntersectsWith(rect5) || rect2.IntersectsWith(rect5) || rect3.IntersectsWith(rect5) || rect4.IntersectsWith(rect5))
                {
                    colision = true;
                }
            }
            if (Canvas.GetTop(carré_1) > 774.0 - carré_1.Height || Canvas.GetTop(carré_2) > 774.0 - carré_2.Height || Canvas.GetTop(carré_3) > 774.0 - carré_3.Height || 
                Canvas.GetTop(carré_4) > 774.0 - carré_4.Height || colision)
            {
                Canvas.SetTop(carré_1, Canvas.GetTop(carré_1) - carré_1.Height - 2.0);
                Canvas.SetTop(carré_2, Canvas.GetTop(carré_2) - carré_2.Height - 2.0);
                Canvas.SetTop(carré_3, Canvas.GetTop(carré_3) - carré_3.Height - 2.0);
                Canvas.SetTop(carré_4, Canvas.GetTop(carré_4) - carré_4.Height - 2.0);

                #region NewPiece
                Rectangle rectangle1 = new Rectangle();
                Rectangle rectangle2 = new Rectangle();
                Rectangle rectangle3 = new Rectangle();
                Rectangle rectangle4 = new Rectangle();

                rectangle1.Height = carré_1.Height;
                rectangle2.Height = carré_2.Height;
                rectangle3.Height = carré_3.Height;
                rectangle4.Height = carré_4.Height;

                rectangle1.Width = carré_1.Width;
                rectangle2.Width = carré_2.Width;
                rectangle3.Width = carré_3.Width;
                rectangle4.Width = carré_4.Width;

                rectangle1.Fill = carré_1.Fill;
                rectangle2.Fill = carré_2.Fill;
                rectangle3.Fill = carré_3.Fill;
                rectangle4.Fill = carré_4.Fill;

                myCanvas.Children.Add(rectangle1);
                myCanvas.Children.Add(rectangle2);
                myCanvas.Children.Add(rectangle3);
                myCanvas.Children.Add(rectangle4);

                rectangles.Add(rectangle1);
                rectangles.Add(rectangle2);
                rectangles.Add(rectangle3);
                rectangles.Add(rectangle4);

                Canvas.SetTop(rectangle1, Canvas.GetTop(carré_1));
                Canvas.SetLeft(rectangle1, Canvas.GetLeft(carré_1));

                Canvas.SetTop(rectangle2, Canvas.GetTop(carré_2));
                Canvas.SetLeft(rectangle2, Canvas.GetLeft(carré_2));

                Canvas.SetTop(rectangle3, Canvas.GetTop(carré_3));
                Canvas.SetLeft(rectangle3, Canvas.GetLeft(carré_3));

                Canvas.SetTop(rectangle4, Canvas.GetTop(carré_4));
                Canvas.SetLeft(rectangle4, Canvas.GetLeft(carré_4));

                Top(rectangle1);
                Top(rectangle2);
                Top(rectangle3);
                Top(rectangle4);

                MettrePiece(prochainePiece);

                ProchainePiece();
                rotation = 0;
                colision = false;

                void Top(Rectangle rectangle)
                {
                    switch (Canvas.GetTop(rectangle))
                    {
                        case 2:
                            Top2.Add(rectangle);
                            break;

                        case 45:
                            Top45.Add(rectangle);
                            break;

                        case 88:
                            Top88.Add(rectangle);
                            break;

                        case 131:
                            Top131.Add(rectangle);
                            break;

                        case 174:
                            Top174.Add(rectangle);
                            break;

                        case 217:
                            Top217.Add(rectangle);
                            break;

                        case 260:
                            Top260.Add(rectangle);
                            break;

                        case 303:
                            Top303.Add(rectangle);
                            break;

                        case 346:
                            Top346.Add(rectangle);
                            break;

                        case 389:
                            Top389.Add(rectangle);
                            break;

                        case 432:
                            Top432.Add(rectangle);
                            break;

                        case 475:
                            Top475.Add(rectangle);
                            break;

                        case 518:
                            Top518.Add(rectangle);
                            break;

                        case 561:
                            Top561.Add(rectangle);
                            break;

                        case 604:
                            Top604.Add(rectangle);
                            break;

                        case 647:
                            Top647.Add(rectangle);
                            break;

                        case 690:
                            Top690.Add(rectangle);
                            break;

                        case 733:
                            Top733.Add(rectangle);
                            break;
                    }
                }
                #endregion

                #region Line

                #region descendre
                if (Top88.Count == 10)
                {
                    ToutDescendre(Top88, 16);
                }
                if (Top131.Count == 10)
                {
                    ToutDescendre(Top131, 15);
                }
                if (Top174.Count == 10)
                {
                    ToutDescendre(Top174, 14);
                }
                if (Top217.Count == 10)
                {
                    ToutDescendre(Top217, 13);
                }
                if (Top260.Count == 10)
                {
                    ToutDescendre(Top260, 12);
                }
                if (Top303.Count == 10)
                {
                    ToutDescendre(Top303, 11);
                }
                if (Top346.Count == 10)
                {
                    ToutDescendre(Top346, 10);
                }
                if (Top389.Count == 10)
                {
                    ToutDescendre(Top389, 9);
                }
                if (Top432.Count == 10)
                {
                    ToutDescendre(Top432, 8);
                }
                if (Top475.Count == 10)
                {
                    ToutDescendre(Top475, 7);
                }
                if (Top518.Count == 10)
                {
                    ToutDescendre(Top518, 6);
                }
                if (Top561.Count == 10)
                {
                    ToutDescendre(Top561, 5);
                }
                if (Top604.Count == 10)
                {
                    ToutDescendre(Top604, 4);
                }
                if (Top647.Count == 10)
                {
                    ToutDescendre(Top647, 3);
                }
                if (Top690.Count == 10)
                {
                    ToutDescendre(Top690, 2);
                }
                if (Top733.Count == 10)
                {
                    ToutDescendre(Top733, 1);
                }
                #endregion

                #region game over
                if (Top2.Count >= 6 || Top45.Count >= 6)
                {
                    t.Stop();
                    t2.Stop();
                    tSound.Stop();
                    sound.Stop();
                    DispatcherTimer tDeath = new DispatcherTimer();
                    tDeath.Tick += GameOver;
                    tDeath.Interval = TimeSpan.FromSeconds(14.5);
                    tDeath.Start();
                    soundDeath.Play();
                    foreach (var x in myCanvas.Children.OfType<Rectangle>())
                        x.Visibility = Visibility.Hidden;
                    foreach (var x in myCanvas.Children.OfType<Label>())
                        x.Visibility = Visibility.Hidden;
                    
                    Loose.Visibility = Visibility.Visible;
                    scoreLoose.Visibility = Visibility.Visible;
                    bg.Visibility = Visibility.Visible;
                    scoreLoose.Content = $"Your score : {score}";
                }
                #endregion

                void ToutDescendre(List<Rectangle> Top, int top)
                {
                    lineScore += 1;
                    line = true;

                    // manque animation
                    foreach (Rectangle x in Top)
                    {
                        myCanvas.Children.Remove(x);
                        rectangles.Remove(x);
                    }

                    Top.Clear();

                    if (top <= 16)
                    {
                        if (top <= 15)
                        {
                            if (top <= 14)
                            {
                                if (top <= 13)
                                {
                                    if (top <= 12)
                                    {
                                        if (top <= 11)
                                        {
                                            if (top <= 10)
                                            {
                                                if (top <= 9)
                                                {
                                                    if (top <= 8)
                                                    {
                                                        if (top <= 7)
                                                        {
                                                            if (top <= 6)
                                                            {
                                                                if (top <= 5)
                                                                {
                                                                    if (top <= 4)
                                                                    {
                                                                        if (top <= 3)
                                                                        {
                                                                            if (top <= 2)
                                                                            {
                                                                                foreach (Rectangle x in this.Top690)
                                                                                {
                                                                                    Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                                                }
                                                                                Top733.AddRange(Top690);
                                                                                Top690.Clear();
                                                                            }
                                                                            foreach (Rectangle x in this.Top647)
                                                                            {
                                                                                Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                                            }
                                                                            Top690.AddRange(Top647);
                                                                            Top647.Clear();
                                                                        }
                                                                        foreach (Rectangle x in this.Top604)
                                                                        {
                                                                            Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                                        }
                                                                        Top647.AddRange(Top604);
                                                                        Top604.Clear();
                                                                    }
                                                                    foreach (Rectangle x in this.Top561)
                                                                    {
                                                                        Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                                    }
                                                                    Top604.AddRange(Top561);
                                                                    Top561.Clear();
                                                                }
                                                                foreach (Rectangle x in this.Top518)
                                                                {
                                                                    Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                                }
                                                                Top561.AddRange(Top518);
                                                                Top518.Clear();
                                                            }
                                                            foreach (Rectangle x in this.Top475)
                                                            {
                                                                Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                            }
                                                            Top518.AddRange(Top475);
                                                            Top475.Clear();
                                                        }
                                                        foreach (Rectangle x in this.Top432)
                                                        {
                                                            Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                        }
                                                        Top475.AddRange(Top432);
                                                        Top432.Clear();
                                                    }
                                                    foreach (Rectangle x in this.Top389)
                                                    {
                                                        Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                    }
                                                    Top432.AddRange(Top389);
                                                    Top389.Clear();
                                                }
                                                foreach (Rectangle x in this.Top346)
                                                {
                                                    Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                                }
                                                Top389.AddRange(Top346);
                                                Top346.Clear();
                                            }
                                            foreach (Rectangle x in this.Top303)
                                            {
                                                Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                            }
                                            Top346.AddRange(Top303);
                                            Top303.Clear();
                                        }
                                        foreach (Rectangle x in this.Top260)
                                        {
                                            Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                        }
                                        Top303.AddRange(Top260);
                                        Top260.Clear();
                                    }
                                    foreach (Rectangle x in this.Top217)
                                    {
                                        Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                    }
                                    Top260.AddRange(Top217);
                                    Top217.Clear();
                                }
                                foreach (Rectangle x in this.Top174)
                                {
                                    Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                                }
                                Top217.AddRange(Top174);
                                Top174.Clear();
                            }
                            foreach (Rectangle x in this.Top131)
                            {
                                Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                            }
                            Top174.AddRange(Top131);
                            Top131.Clear();
                        }
                        foreach (Rectangle x in this.Top88)
                        {
                            Canvas.SetTop(x, Canvas.GetTop(x) + x.Height + 2);
                        }
                        Top131.AddRange(Top88);
                        Top88.Clear();
                    }

                    int_line++;

                    if (temps > 10)
                        temps -= 10;

                    t.Interval = TimeSpan.FromMilliseconds(temps);
                    niveau = (int)Math.Floor((double)int_line / 10);

                    lineLabel.Content = $"Line : {int_line}";
                    niveauLabel.Content = $"Niveau : {niveau}";
                    line = false;
                }

                switch (lineScore)
                {
                    case 1:
                        score += 40;
                        break;

                    case 2:
                        score += 100;
                        break;

                    case 3:
                        score += 300;
                        break;

                    case 4:
                        score += 1200;
                        break;
                }

                lineScore = 0;
                scoreLabel.Content = $"Score : {score}";
                #endregion
            }
        }

        public void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && Canvas.GetLeft(carré_1) - carré_1.Width > 240)
            {
                Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2.0);
                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) - carré_2.Width - 2.0);
                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) - carré_3.Width - 2.0);
                Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) - carré_4.Width - 2.0);
                foreach (Rectangle rectangle in rectangles)
                {
                    if (Canvas.GetLeft(carré_1) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_1) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2.0);
                    }
                    else if (Canvas.GetLeft(carré_2) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_2) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2.0);
                    }
                    else if (Canvas.GetLeft(carré_3) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_3) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2.0);
                    }
                    else if (Canvas.GetLeft(carré_4) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_4) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2.0);
                    }
                }
            }

            if (e.Key == Key.Right && Canvas.GetLeft(carré_4) + carré_4.Width < 740.0)
            {
                Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2.0);
                Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2.0);
                foreach (Rectangle rectangle in rectangles)
                {
                    if (Canvas.GetLeft(carré_1) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_1) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) - carré_2.Width - 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) - carré_3.Width - 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) - carré_4.Width - 2.0);
                    }
                    else if (Canvas.GetLeft(carré_2) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_2) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) - carré_2.Width - 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) - carré_3.Width - 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) - carré_4.Width - 2.0);
                    }
                    else if (Canvas.GetLeft(carré_3) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_3) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) - carré_2.Width - 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) - carré_3.Width - 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) - carré_4.Width - 2.0);
                    }
                    else if (Canvas.GetLeft(carré_4) == Canvas.GetLeft(rectangle) && Canvas.GetTop(carré_4) == Canvas.GetTop(rectangle))
                    {
                        Canvas.SetLeft(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2.0);
                        Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_2) - carré_2.Width - 2.0);
                        Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_3) - carré_3.Width - 2.0);
                        Canvas.SetLeft(carré_4, Canvas.GetLeft(carré_4) - carré_4.Width - 2.0);
                    }
                }
            }

            void Bug(bool gauche)
            {
                foreach (Rectangle x in rectangles)
                {
                    if ((Canvas.GetLeft(x) == Canvas.GetLeft(carré_1) && Canvas.GetTop(x) == Canvas.GetTop(carré_1)) ||
                       (Canvas.GetLeft(x) == Canvas.GetLeft(carré_2) && Canvas.GetTop(x) == Canvas.GetTop(carré_2)) ||
                       (Canvas.GetLeft(x) == Canvas.GetLeft(carré_3) && Canvas.GetTop(x) == Canvas.GetTop(carré_3)) ||
                       (Canvas.GetLeft(x) == Canvas.GetLeft(carré_4) && Canvas.GetTop(x) == Canvas.GetTop(carré_4)) ||
                       Canvas.GetLeft(carré_1) < 240 || Canvas.GetLeft(carré_4) > 740)
                    {
                        if (gauche)
                            RotationDroite();
                        else
                            RotationGauche();
                    }
                }
            }

            void RotationGauche()
            {
                if (rotation == 0)
                    rotation = 3;
                else
                    rotation--;
                switch (Piece)
                {
                    #region I
                    case "I":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_2) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Position(carré_3, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_1.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_1) - carré_1.Height - 2);
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) + carré_3.Height + 2);
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_4.Height + 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region L
                    case "L":
                        switch (rotation)
                        {
                            #region 3
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Position(carré_3, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_2) - carré_4.Height - 2);
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_4));
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_1));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_4));
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_3));
                                Position(carré_4, Canvas.GetLeft(carré_4) + carré_4.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 0
                            case 0:
                                Position(carré_1, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_3.Height - 2);
                                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_3));
                                Canvas.SetTop(carré_3, Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_3));
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region S
                    case "S":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_2) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) - carré_3.Height - 2);
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_3));
                                Position(carré_3, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_3) + carré_3.Height + 2.0);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region T
                    case "T":
                        switch (rotation)
                        {
                            #region 0
                            case 0:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_1) + carré_1.Width + 2.0);
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_1));
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Position(carré_1, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3));
                                Position(carré_2, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_1) - carré_2.Height - 2);
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_4));
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_3));
                                Position(carré_2, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_1));
                                Position(carré_3, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_4));
                                Position(carré_4, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                break;
                            #endregion

                            #region 3
                            case 3:
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_1) + carré_1.Height + 2);
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) + carré_2.Height + 2);
                                Position(carré_4, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region Z
                    case "Z":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_2) - carré_1.Width - 2, Canvas.GetTop(carré_4));
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_1));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3));
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_1) + carré_1.Height + 2);
                                Position(carré_3, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_1));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_3.Height - 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region []
                    case "[]":
                        estearEgg.Content = "????????? WHY ?????????";
                        break;
                    #endregion

                    #region reverseL
                    case "reverseL":
                        switch (rotation)
                        {
                            #region 0
                            case 0:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_3) + carré_3.Height + 2.0);
                                Position(carré_2, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_1));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_4.Height - 2);
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Position(carré_1, Canvas.GetLeft(carré_3) - carré_1.Width - 2, Canvas.GetTop(carré_3));
                                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_1));
                                Canvas.SetTop(carré_3, Canvas.GetTop(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_2));
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_4));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) - carré_3.Height - 2);
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 3
                            case 3:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_3));
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_1));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2) + carré_2.Width + 2.0);
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                break;
                                #endregion
                        }
                        break;
                        #endregion
                }
                Bug(true);
            }

            void RotationDroite()
        {
                if (rotation == 3)
                    rotation = 0;
                else
                    rotation++;
                switch (Piece)
                {
                    #region I
                    case "I":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                if (Canvas.GetLeft(carré_2) > 290.0 && Canvas.GetLeft(carré_2) < 640.0)
                                    Position(carré_1, Canvas.GetLeft(carré_2) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Position(carré_3, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_2));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_1) - carré_1.Height - 2);
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) + carré_3.Height + 2);
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_4.Height + 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region L
                    case "L":
                        switch (rotation)
                        {
                            #region 0
                            case 0:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_4));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) + carré_2.Height + 2);
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_4));
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_1) - carré_1.Height - 2);
                                Position(carré_3, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_2) - carré_1.Height - 2.0);
                                Position(carré_2, Canvas.GetLeft(carré_1) + carré_1.Width + 2, Canvas.GetTop(carré_1));
                                Canvas.SetTop(carré_3, Canvas.GetTop(carré_2) + carré_2.Height + 2.0);
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                break;
                            #endregion

                            #region 3
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2) - carré_3.Width - 2, Canvas.GetTop(carré_3));
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_1));
                                Position(carré_3, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_4.Height - 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region S
                    case "S":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_3));
                                Position(carré_2, Canvas.GetLeft(carré_1) + carré_1.Width + 2, Canvas.GetTop(carré_1));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_1) - carré_3.Height - 2);
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) - carré_2.Height - 2);
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_4) + carré_2.Height + 2.0);
                                Position(carré_3, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region T
                    case "T":
                        switch (rotation)
                        {
                            #region 0
                            case 0:
                                Position(carré_2, Canvas.GetLeft(carré_4), Canvas.GetTop(carré_1));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_2) + carré_2.Height + 2);
                                Position(carré_4, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Position(carré_1, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3));
                                Canvas.SetLeft(carré_2, Canvas.GetLeft(carré_4));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_4));
                                Position(carré_2, Canvas.GetLeft(carré_1) + carré_1.Width + 2, Canvas.GetTop(carré_1));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                break;
                            #endregion

                            #region 3
                            case 3:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_3) - carré_1.Height - 2.0);
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_3));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_4));
                                Position(carré_4, Canvas.GetLeft(carré_2) + carré_2.Width + 2, Canvas.GetTop(carré_2));
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region Z
                    case "Z":
                        switch (rotation)
                        {
                            #region 0/2
                            case 0:
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_1) - carré_1.Width - 2, Canvas.GetTop(carré_4));
                                Position(carré_2, Canvas.GetLeft(carré_1) + carré_1.Width + 2, Canvas.GetTop(carré_1));
                                Position(carré_3, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_1) + carré_1.Height + 2);
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 1/3
                            case 1:
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2), Canvas.GetTop(carré_1) + carré_1.Height + 2);
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_1) + carré_1.Height + 2);
                                Position(carré_3, Canvas.GetLeft(carré_1) + carré_1.Width + 2, Canvas.GetTop(carré_1));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_3.Height - 2);
                                break;
                                #endregion
                        }
                        break;
                    #endregion

                    #region []
                    case "[]":
                        estearEgg.Content = "????????? WHY ?????????";
                        break;
                    #endregion

                    #region reverseL
                    case "reverseL":
                        switch (rotation)
                        {
                            #region 0
                            case 0:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_4));
                                Canvas.SetTop(carré_2, Canvas.GetTop(carré_1));
                                Canvas.SetLeft(carré_3, Canvas.GetLeft(carré_2));
                                Position(carré_4, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) - carré_4.Height - 2);
                                break;
                            #endregion

                            #region 1
                            case 1:
                                Canvas.SetTop(carré_1, Canvas.GetTop(carré_4));
                                Position(carré_2, Canvas.GetLeft(carré_1), Canvas.GetTop(carré_3));
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 2
                            case 2:
                                Position(carré_1, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3) + carré_3.Height + 2);
                                Position(carré_2, Canvas.GetLeft(carré_3), Canvas.GetTop(carré_3));
                                Canvas.SetTop(carré_3, Canvas.GetTop(carré_2) - carré_3.Height - 2.0);
                                Position(carré_4, Canvas.GetLeft(carré_3) + carré_3.Width + 2, Canvas.GetTop(carré_3));
                                break;
                            #endregion

                            #region 3
                            case 3:
                                Position(carré_1, Canvas.GetLeft(carré_2) - carré_1.Width - 2, Canvas.GetTop(carré_2));
                                Position(carré_3, Canvas.GetLeft(carré_2) + carré_3.Width + 2, Canvas.GetTop(carré_2));
                                Canvas.SetTop(carré_4, Canvas.GetTop(carré_3) + carré_4.Height + 2.0);
                                break;
                                #endregion
                        }
                        break;
                        #endregion
                }
                Bug(false);
            }

            if (e.Key == Key.A)
            {
                RotationGauche();
            }

            if (e.Key == Key.S)
            {
                RotationDroite();
            }

            if (e.Key == Key.Down && !line)
            {
                t.Stop();
                t2.Start();
            }
        }

        public void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && !line)
            {
                t2.Stop();
                t.Start();
            }
            estearEgg.Content = "";
        }
    }
}
