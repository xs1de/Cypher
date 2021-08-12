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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cypher
{
    /// <summary>
    /// Логика взаимодействия для Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        private int time = 1;
        private DispatcherTimer timer = new DispatcherTimer();
        public Splash()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation opacityA = new DoubleAnimation();
            opacityA.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            opacityA.From = 0;
            opacityA.To = 1;
            this.BeginAnimation(OpacityProperty, opacityA);

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            if (this.Opacity == 0)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            time--;
            if (time == 0)
            {
                DoubleAnimation opacityA = new DoubleAnimation();
                opacityA.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                opacityA.From = 1;
                opacityA.To = 0;
                this.BeginAnimation(OpacityProperty, opacityA);
                timer.Stop();
            }
        }
    }
}
