using DataGrid2DLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AutomataGP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string[,] FSM { get; set; }

        public MainWindow()
        {
            FSM = new string[20,20];
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    FSM[i, j] = Convert.ToString(i + j);
                }
            }
            InitializeComponent();
            FSM_View.DataContext = this;
        }

        

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
