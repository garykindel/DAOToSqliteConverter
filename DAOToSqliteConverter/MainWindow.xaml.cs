using Microsoft.Win32;
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

namespace DAOToSqliteConverter
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

        private void btnLookupMDB_Click(object sender, RoutedEventArgs e)
        {
            LoadDatabase();
        }

        void LoadDatabase()
        {                      
            var filePath = OpenFileDialog(null, null, null);
            var accessHelper = new AccessHelper();
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                txtAccessDatabase.Text = System.IO.Path.GetFileName(filePath);
                cboTable.ItemsSource = accessHelper.GetAccessTableList(filePath);
                cboQuery.ItemsSource = accessHelper.GetAccessQueryList(filePath);
            }            
        }

        string OpenFileDialog(string initalFolder, string filename, string filterlist)
        {
            string wFilter = string.IsNullOrEmpty(filterlist) ? @"All files (*.*)|*.*" : filterlist;
            var dialog = new OpenFileDialog
            {
                Filter = wFilter,
                Title = @"Select file"
            };

            if (!string.IsNullOrEmpty(initalFolder))
                dialog.InitialDirectory = initalFolder;

            if (string.IsNullOrEmpty(filename))
                dialog.FileName = filename;

            bool? ret  = dialog.ShowDialog();                       

            if (ret.HasValue)
                return dialog.FileName;
            return null;
            
            
        }

    }
}
