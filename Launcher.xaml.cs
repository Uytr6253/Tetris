using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Logique d'interaction pour Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        private bool WantToClose = false;
        private SoundPlayer sound = new($@"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\net6.0-windows\\", "")}Son//tetris launcher song.wav");
        private SoundPlayer Buttonsound = new($@"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\net6.0-windows\\", "")}Son//Launcher Button song.wav");
        private DispatcherTimer t = new();

        public Launcher()
        {
            InitializeComponent();
            sound.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            t.Tick += ActionClose;
            t.Interval = TimeSpan.FromMilliseconds(324);
            WantToClose = true;
            sound.Stop();
            Buttonsound.Play();
            t.Start();
        }

        private void ActionClose(object? sender, EventArgs e)
        {
            Close();
            t.Stop();
        }

        private void IsClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WantToClose)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
    }
}
