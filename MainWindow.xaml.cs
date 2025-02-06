using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ceddit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveAsFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSharp file|*.cs|C++ file|*.cpp";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "" && result == true)
            {
                TextRange range = new TextRange(pageContent.Document.ContentStart, pageContent.Document.ContentEnd);
                FileStream file = new FileStream(saveFileDialog.FileName, FileMode.CreateNew);
                range.Save(file, System.Windows.DataFormats.XamlPackage);
                file.Close();
            }

        }

        private void openFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSharp file|*.cs|C++ file|*.cpp";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "" && result == true)
            {
                var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                RichTextBox newPageContent;
                string text = new TextRange(pageContent.Document.ContentStart, pageContent.Document.ContentEnd).Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    newPageContent = pageContent;
                    mainTab.Header = $"{fileName}";
                }
                else
                {
                    newPageContent = newTab(fileName);
                }
                TextRange range = new TextRange(newPageContent.Document.ContentStart, newPageContent.Document.ContentEnd);
                FileStream file = new FileStream(openFileDialog.FileName, FileMode.Open);
                range.Load(file, System.Windows.DataFormats.XamlPackage);
                file.Close();
            }
        }

        private void newFile(object sender, RoutedEventArgs e)
        {
            newTab("Untitled");
        }

        private RichTextBox newTab(string? fileName)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = $"{fileName}";
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Name = "pageContent";
            tabItem.Content = richTextBox;
            fileTabControl.Items.Add(tabItem);
            return richTextBox;
        }

        public void saveAllFiles()
        {

        }

        private void MoveWindow(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                double windowWidth = this.Width;
                var curPos = Mouse.GetPosition(this);
                this.Top = 0;
                this.Left = curPos.X - (windowWidth / 2);
            }
            this.DragMove();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                BorderThickness = new Thickness(WindowState == WindowState.Maximized ? 8 : 0);
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }
    }
}