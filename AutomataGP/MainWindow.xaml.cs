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
using MahApps.Metro.Controls.Dialogs;

using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
using System.Diagnostics;

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
            refreshDataGrid();

            G = new Graph(3);
        }

        
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //AboutFlyout.IsOpen = true;
            //this.ShowMessageAsync("Welcome !", "this will burn your wools");
            
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            increaseSize();
            refreshDataGrid();
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
                        //Console.Write("key:" + s);
                        if (s.Contains(","))
                        {
                            string[] sa = s.Split(',');
                            foreach(string ss in sa)
                            {
                                G.At(i).addOutEdge(ss[0], G.At(j));
                            }
                        }
                        else
                        {
                            G.At(i).addOutEdge(s[0], G.At(j));
                        }
                    }
                    j++;
                }
                i++;
            }

            //Configure Initial State
            if (initial_s_text.Text != "")
            {
                G.At(Convert.ToInt32(initial_s_text.Text)).isInitial = true;
                G.initialState = Convert.ToInt32(initial_s_text.Text);
            }

            //Configure Final States
            if (final_s_text.Text != "")
            {
                string[] segments = final_s_text.Text.Split(',');
                foreach (string s in segments)
                {
                    G.At(Convert.ToInt32(s)).isFinal = true;
                }
            }

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

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutFlyout.IsOpen = !AboutFlyout.IsOpen;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                string[] lines = System.IO.File.ReadAllLines(filename);
                FSM = new List<List<string>>();
                int i_size = Convert.ToInt32(lines[0]);
                int i_initState = Convert.ToInt32(lines[1]);
                List<int> i_finalStates = new List<int>();
                string[] fss = lines[2].Split(',');
                foreach(string fssi in fss)
                {
                    i_finalStates.Add(Convert.ToInt32(fssi));
                }
                for(int i=0;i< i_size; i++)
                {
                    increaseSize();
                }
                for(int i = 0; i < i_size; i++)
                {
                    string[] ttr = lines[3 + i].Split(' ');
                    for(int j=0; j< i_size; j++)
                    {
                        if(ttr[j] != "_")
                        {
                            FSM[i][j] = ttr[j];
                        }
                    }
                }
                refreshDataGrid();
            }
        }

        private async void String_Accept_Click(object sender, RoutedEventArgs e)
        {
            string str = await this.ShowInputAsync("String Acceptor", "please enter a string :");
            if(str != null)
            {
                if(G.acceptString(str))
                    await this.ShowMessageAsync("Yes !", "string was accepted");
                else
                    await this.ShowMessageAsync("No !", "string was not accepted");

            }
        }
    }
}
