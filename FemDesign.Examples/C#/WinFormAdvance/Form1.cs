using FemDesign;
using FemDesign.Calculate;
using FemDesign.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WinFormAdvance
{
    public partial class Form : System.Windows.Forms.Form
    {
        FemDesign.FemDesignConnection connection;
        string _filePath;
        Comb Comb;
        Model model;

        FemDesign.Calculate.Analysis analysisType = FemDesign.Calculate.Analysis.StaticAnalysis();

        public Form()
        {
            InitializeComponent();
            this.connection = new FemDesign.FemDesignConnection(minimized: true);
            this.label1.Text = "No file selected";
            this.comboBox1.SelectedIndex = 2;
            this.radioButtonNLE.Checked = true;
        }

        ~Form()
        {
            this.connection.Dispose();
            Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // open a browser to the FemDesign API documentation
            System.Diagnostics.Process.Start("https://femdesign-api-docs.onstrusoft.com/");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // open a file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "FEM-Design files|*.str;*.struxml";
            openFileDialog.InitialDirectory = "C:\\Users\\Marco\\OneDrive - StruSoft AB\\FD API\\00_API_Development\\23\\23.3.0\\420-mass-point\\FEM-Design API";
            openFileDialog.Title = "Select a FemDesign file";

            // store the selected file path
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _filePath = openFileDialog.FileName;
                this.label1.Text = Path.GetFileName(_filePath);

                await OpenFemDesignFile(_filePath);
                await GetModel();
            }
        }

        // wrap all the femdesign calls in a task to avoid blocking the UI
        private async Task OpenFemDesignFile(string filePath)
        {
            await Task.Factory.StartNew(() =>
            {
                connection.Open(filePath);
            });
        }

        private async Task OpenFemDesignModel(Model model)
        {
            await Task.Factory.StartNew(() =>
            {
                connection.Open(model);
            });
        }

        // write an async task to get the model
        private async Task GetModel()
        {
            await Task.Factory.StartNew(() =>
            {
                model = connection.GetModel();
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonLE_CheckedChanged(object sender, EventArgs e)
        {
            setComb();
        }

        private void radioButtonNLE_CheckedChanged(object sender, EventArgs e)
        {
            setComb();
        }

        private void setComb()
        {
            if (radioButtonNLE.Checked)
            {
                var comb = new Comb();
                var NLE = new CombItem(NLE: true, PL: true);
                comb.CombItem = new List<CombItem> { NLE };
                Comb = comb;
            }

            if (radioButtonLE.Checked)
            {
                var comb = new Comb();
                var LE = new CombItem(NLE: false, PL: false);
                comb.CombItem = new List<CombItem> { LE };
                Comb = comb;
            }
        }




        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        private async void buttonRunOptimisation_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            var index = comboBox1.SelectedIndex;
            var iteration = (int)comboBox1.Items[index];

            // await makes sure that the task is completed before the next line is executed
            await RunOptimisation(iteration);

            // it will be called after the task is completed
            textBox1.AppendText($"Optimisation completed");
        }


        private async Task RunOptimisation(int iteration)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= iteration; i++)
                {
                    // update the thickness of all the slabs in the model
                    foreach (var slab in model.Entities.Slabs)
                    {
                        slab.UpdateThickness(i * 0.01);
                    }
                    connection.Open(model);
                    var analysis = new Analysis(comb: Comb, calcCase: true, calcComb: true);
                    connection.RunAnalysis(analysis);

                    var quantities = connection.GetQuantities<FemDesign.Results.QuantityEstimationSteel>();
                    double total = quantities.Sum(r => r.TotalWeight);

                    // display the results comes from a different thread so it is necessary to use Invoke
                    DisplayResults(i, total);
                }
            });
        }



        private void DisplayResults(int i, double weight)
        {
            textBox1.Invoke((MethodInvoker)(() =>
            {
                textBox1.AppendText($"Iteration: {i}.\r\n");
                textBox1.AppendText($"Steel weight: {weight:0.0} ton.\r\n");
            }));
        }


        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
