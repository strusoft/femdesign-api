using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FemDesign.Results;

namespace FemDesign.Examples
{
    public partial class Form1 : Form
    {
        FemDesignConnection _connection = new FemDesignConnection();
        string _modelPath = Path.GetFullPath("sample_slab.struxml");

        public Form1()
        {
            InitializeComponent();

            label1.Text = Path.GetFileName(_modelPath);
        }

        ~Form1()
        {
            _connection.Dispose();
        }

        private void syncrounousButtonClick(object sender, EventArgs e)
        {
            // The following will block the main thread (UI) until the Task has completed then continue.
            var quantities = GetQuantitiesAsync(_modelPath).Result;

            DisplayResults(quantities);
        }

        private async void asyncrounousButtonClick(object sender, EventArgs e)
        {
            // By running in an async method we can instead 'await' the Task.
            // The following will not block the main thread and the UI will continue to be responsive.
            // When the results are awailable the method will continue.
            var quantities = await GetQuantitiesAsync(_modelPath);

            DisplayResults(quantities);
        }

        private Task<List<QuantityEstimationConcrete>> GetQuantitiesAsync(string modelPath)
        {
            var units = UnitResults.Default();
            units.Mass = Mass.t;

            // In order not to get stuck in a deadlock, the commands must be sent from a background thread.
            // Task.Run() will start a thread then open the model and return the quantity estimation.
            return Task.Run(() =>
            {
                _connection.Open(modelPath);
                return _connection.GetResults<QuantityEstimationConcrete>();
            });
        }

        private void DisplayResults(List<QuantityEstimationConcrete> quantities)
        {
            double total = quantities.Sum(r => r.TotalWeight);
            textBox1.AppendText($"Concrete weight: {total:0.0} ton.\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "FEM-Design files|*.str;*.struxml|All files (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                _modelPath = dialog.FileName;
                label1.Text = Path.GetFileName(_modelPath);
            }
        }
    }
}
