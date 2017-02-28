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
using MahApps.Metro.Controls;

using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace AutomataGP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public List<List<string>> FSM { get; set; }
        public List<List<string>> dummy { get; set; }

        private void increaseSize()
        {
            List<string> newRow = new List<string>();
            for (int i = 0; i < FSM.Count; i++) newRow.Add("");
            FSM.Add(newRow);
            foreach (List<string> oc in FSM)
            {
                oc.Add(" ");
            }
            Binding datagrid2dBinding = new Binding();
            datagrid2dBinding.Path = new PropertyPath("dummy");
            FSM_View.SetBinding(DataGrid2D.ItemsSource2DProperty, datagrid2dBinding);

            datagrid2dBinding = new Binding();
            datagrid2dBinding.Path = new PropertyPath("FSM");
            FSM_View.SetBinding(DataGrid2D.ItemsSource2DProperty, datagrid2dBinding);
        }

        public MainWindow()
        {
            FSM = new List<List<string>>();
            dummy = new List<List<string>>();
            InitializeComponent();
            FSM_View.DataContext = this;
            increaseSize();
            increaseSize();
            increaseSize();
        }

        
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);

            byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Png);

            GraphVizHost.Source = ImageHelper.LoadImage(output);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            increaseSize();
            Console.WriteLine("ADD !");
        }
    }
}
