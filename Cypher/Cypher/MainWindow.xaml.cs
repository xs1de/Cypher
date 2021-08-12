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
using System.Windows.Media.Animation;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Collections;
using System.IO;
using System.Windows.Threading;

namespace Cypher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Splash splash = new Splash();
            splash.Show();
        }

        private void CreateNotification(string text)
        {
            Rectangle bottomRect = new Rectangle();
            bottomRect.Height = 24;
            bottomRect.VerticalAlignment = VerticalAlignment.Bottom;
            bottomRect.HorizontalAlignment = HorizontalAlignment.Stretch;
            bottomRect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 62, 69));
            gridMain.Children.Add(bottomRect);

            Label textLabel = new Label();
            textLabel.Content = text;
            textLabel.HorizontalAlignment = HorizontalAlignment.Center;
            textLabel.VerticalAlignment = VerticalAlignment.Bottom;
            textLabel.FontFamily = new FontFamily("Montserrat");
            textLabel.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            gridMain.Children.Add(textLabel);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                gridMain.Children.Remove(bottomRect);
                gridMain.Children.Remove(textLabel);
            };
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation opacityA = new DoubleAnimation();
            opacityA.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            opacityA.From = 1;
            opacityA.To = 0;
            opacityA.Completed += new EventHandler(opacityA_Completed);
            this.BeginAnimation(OpacityProperty, opacityA);
        }

        private void opacityA_Completed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation opacityA = new DoubleAnimation();
            opacityA.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            opacityA.From = 0;
            opacityA.To = 1;
            this.BeginAnimation(OpacityProperty, opacityA);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void base64enc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = new TextRange(base64text.Document.ContentStart, base64text.Document.ContentEnd).Text;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
                base64res.Document.Blocks.Clear();
                base64res.Document.Blocks.Add(new Paragraph(new Run(System.Convert.ToBase64String(plainTextBytes))));
            }
            catch (Exception ex)
            {
                CreateNotification(ex.Message);
            }
        }

        private void base64dec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = new TextRange(base64text.Document.ContentStart, base64text.Document.ContentEnd).Text;
                var base64EncodedBytes = System.Convert.FromBase64String(text);

                base64res.Document.Blocks.Clear();
                base64res.Document.Blocks.Add(new Paragraph(new Run(System.Text.Encoding.UTF8.GetString(base64EncodedBytes))));
            }
            catch (Exception ex)
            {
                CreateNotification(ex.Message);
            }
        }

        public string GetMd5Hash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private void md5enc_Click(object sender, RoutedEventArgs e)
        {
            md5res.Document.Blocks.Clear();
            md5res.Document.Blocks.Add(new Paragraph(new Run(GetMd5Hash(new TextRange(md5text.Document.ContentStart, md5text.Document.ContentEnd).Text))));
        }

        static string GetSHA1Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        private void sha1enc_Click(object sender, RoutedEventArgs e)
        {
            sha1res.Document.Blocks.Clear();
            sha1res.Document.Blocks.Add(new Paragraph(new Run(GetSHA1Hash(new TextRange(sha1text.Document.ContentStart, sha1text.Document.ContentEnd).Text))));
        }

        static string GetSHA256Hash(string input)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private void sha256enc_Click(object sender, RoutedEventArgs e)
        {
            sha256res.Document.Blocks.Clear();
            sha256res.Document.Blocks.Add(new Paragraph(new Run(GetSHA256Hash(new TextRange(sha256text.Document.ContentStart, sha256text.Document.ContentEnd).Text))));
        }
        public static string GetSHA512Hash(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private void sha512enc_Click(object sender, RoutedEventArgs e)
        {
            sha512res.Document.Blocks.Clear();
            sha512res.Document.Blocks.Add(new Paragraph(new Run(GetSHA512Hash(new TextRange(sha512text.Document.ContentStart, sha512text.Document.ContentEnd).Text))));
        }
        public static string GetSHA384Hash(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA384.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
        private void sha384enc_Click(object sender, RoutedEventArgs e)
        {
            sha384res.Document.Blocks.Clear();
            sha384res.Document.Blocks.Add(new Paragraph(new Run(GetSHA384Hash(new TextRange(sha384text.Document.ContentStart, sha384text.Document.ContentEnd).Text))));
        }
    }
}
