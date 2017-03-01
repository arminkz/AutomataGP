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
        private Graph G;

        int size = 0;
        private void increaseSize()
        {
            List<string> newRow = new List<string>();
            for (int i = 0; i < FSM.Count; i++) newRow.Add("");
            FSM.Add(newRow);
            foreach (List<string> oc in FSM)
            {
                oc.Add("");
            }
            size++;

            refreshDataGrid();
        }

        private void refreshDataGrid()
        {
            //TODO : fix this tricky code !
            //Change Binding to null and reset to FSM for force revalidation
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

            G = new Graph(3);
        }

        
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            increaseSize();
        }

        private void UpdateGraph()
        {
            G = new Graph(size);

            int i = 0;
            foreach(List<string> ls in FSM)
            {
                int j = 0;
                foreach (string s in ls)
                {
                    if (s != "")
                    {
                        Console.Write("key:" + s);
                        G.At(i).addOutEdge(s, G.At(j));
                    }
                    j++;
                }
                i++;
            }

            //refreshDataGrid();
        }

        private void GraphVizDraw()
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface
            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);
            string dot = G.toDOT();
            Console.Write(dot);
            byte[] output = wrapper.GenerateGraph(dot, Enums.GraphReturnType.Png);
            GraphVizHost.Source = ImageHelper.LoadImage(output);
            
        }

        private void Matrix_OK(object sender, RoutedEventArgs e)
        {
            UpdateGraph();
            GraphVizDraw();
        }

        private void CellEdited(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            
        }
    }
}
