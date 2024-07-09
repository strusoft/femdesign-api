using Microsoft.Win32;
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
using FemDesign;
using FemDesign.Calculate;
using System.Security.Policy;
using System.Threading.Tasks;
using FemDesign.Results;

namespace Practical_example___WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FemDesign.FemDesignConnection connection;
        string _filePath;
        Comb Comb = null;
        Model model = null;

        public MainWindow()
        {
            InitializeComponent();
            this.connection = new FemDesign.FemDesignConnection(minimized: false);
        }

        ~MainWindow()
        {
            this.connection.Dispose();
        }


        private async Task OpenFemDesignFile(string filePath)
        {
            await Task.Factory.StartNew(() =>
            {
                connection.Open(filePath);
            });
        }
        private async Task GetModel()
        {
            await Task.Factory.StartNew(() =>
            {
                model = connection.GetModel();
            });
        }
        private async Task RunOptimisation(int iteration, double thicknessStep)
        {
            await Task.Factory.StartNew(() =>
            {

                // create a while loop with a condition that will be true until the utilisation is less than 1
                double utilisation = 2;
                int i = 1;

                

                while(utilisation > 1 && i < iteration)
                {
                    var thickness = i * thicknessStep / 1000;
                    // update the thickness of all the slabs in the model
                    foreach (var slab in model.Entities.Slabs)
                    {
                        slab.UpdateThickness(i * thickness);
                    }

                    connection.Open(model);
                    var analysis = new Analysis(comb: Comb, calcCase: true, calcComb: true);
                    connection.RunAnalysis(analysis);

                    var quantities = connection.GetQuantities<FemDesign.Results.QuantityEstimationSteel>();
                    double total = quantities.Sum(r => r.TotalWeight);

                    var units = UnitResults.Default();
                    units.Stress = Stress.MPa;
                    var maxStress = connection.GetResults<ShellStress>(units, options: new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints, 0.5)).Select(x => Math.Abs(x.SigmaVM)).Max();
                    utilisation = maxStress / 355;

                    DisplayResults(i, thickness, total, utilisation);
                    i++;
                }





                //for (int i = 1; i <= iteration; i++)
                //{
                //    // update the thickness of all the slabs in the model
                //    foreach (var slab in model.Entities.Slabs)
                //    {
                //        slab.UpdateThickness(i * thickness / 1000);
                //    }
                //    connection.Open(model);
                //    var analysis = new Analysis(comb: Comb, calcCase: true, calcComb: true);
                //    connection.RunAnalysis(analysis);

                //    var quantities = connection.GetQuantities<FemDesign.Results.QuantityEstimationSteel>();
                //    double total = quantities.Sum(r => r.TotalWeight);

                //    var units = UnitResults.Default();
                //    units.Stress = Stress.MPa;
                //    var maxStress = connection.GetResults<ShellStress>(options: new Options(BarResultPosition.ResultPoints, ShellResultPosition.ResultPoints, 0.5)).Select(x => Math.Abs(x.SigmaVM)).Max();
                //    var utilisation = 355 / maxStress;

                //    DisplayResults(i, thickness, total, utilisation);
                //}
            });
        }
        private void DisplayResults(int i, double thickness, double weight, double utilisation)
        {
            Dispatcher.Invoke(() =>
            {
                TextBox.AppendText($"Iteration: {i}\r\n");
                TextBox.AppendText($"thickness: {thickness}\r\n");
                TextBox.AppendText($"Steel utilisation: {utilisation * 100:0.0} %.\r\n");
                TextBox.AppendText($"Steel weight: {weight:0.0} ton.\r\n");
                TextBox.AppendText($"\r\n");
            });
        }

        #region Events
        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "FEM-Design files|*.str;*.struxml";
            openFileDialog.InitialDirectory = "C:\\GitHub\\femdesign-api\\FemDesign.Examples\\C#\\Practical example - WPF";
            openFileDialog.Title = "Select a FemDesign file";

            // store the selected file path
            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                this.label1.Content = System.IO.Path.GetFileName(_filePath);

                await OpenFemDesignFile(_filePath);
                await GetModel();
                
            }
        }
        private void Picture_MouseDown(object sender, MouseEventArgs e)
        {
            // open a browser to the FemDesign API documentation
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd", "/c start " + "https://femdesign-api-docs.onstrusoft.com/");
            psi.CreateNoWindow = true;
            System.Diagnostics.Process.Start(psi);
        }
        private async void RunOptimisation_Click(object sender, RoutedEventArgs e)
        {
            if(_filePath == null)
            {
                MessageBox.Show("Please open a FemDesign file first");
                return;
            }

            Run_Optimisation.Content = "Processing...";
            Run_Optimisation.IsEnabled = false;

            TextBox.Clear();

            int iteration = int.Parse(ComboBox.Text);
            double thickness = double.Parse(Thk.Text);

            // await makes sure that the task is completed before the next line is executed
            await RunOptimisation(iteration, thickness);

            // it will be called after the task is completed
            TextBox.AppendText($"Optimisation completed");

            Run_Optimisation.Content = "Run Optimisation";
            Run_Optimisation.IsEnabled = true;
        }
        private void setComb()
        {
            if (Radio_button_NLE.IsChecked == true)
            {
                var comb = new Comb();
                var NLE = new CombItem(NLE: true, PL: true);
                comb.CombItem = new List<CombItem> { NLE };
                Comb = comb;
            }

            if (Radio_button_LE.IsChecked == true)
            {
                var comb = new Comb();
                var LE = new CombItem(NLE: false, PL: false);
                comb.CombItem = new List<CombItem> { LE };
                Comb = comb;
            }
        }
        private void Radio_button_LE_Checked(object sender, RoutedEventArgs e)
        {
            setComb();
        }
        private void Radio_button_NLE_Checked(object sender, RoutedEventArgs e)
        {
            setComb();
        }

        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}