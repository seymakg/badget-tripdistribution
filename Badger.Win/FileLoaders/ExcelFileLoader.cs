using FileSystem.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Badger.Win.FileLoaders
{
    public partial class ExcelFileLoader : Form
    {
        public string ExcelFilePath { get; private set; }

        public double[,] CostMatrix { get; private set; }

        public double[,] TripMatrix { get; private set; }

        public string[] Names { get; private set; }

        public ExcelFileLoader(string filePath)
        {
            InitializeComponent();

            this.ExcelFilePath = filePath;
        }

        private void ExcelFileLoader_Load(object sender, EventArgs e)
        {
            BindSheetNames();
        }

        private void BindSheetNames()
        {
            using (ExcelFileReader excelFileReader = new ExcelFileReader(this.ExcelFilePath))
            {
                var sheets = excelFileReader.GetSheets();
                for (int i = 0; i < sheets.Length; i++)
                {
                    ExcelSheetListItem excelSheetListItem = new ExcelSheetListItem();
                    excelSheetListItem.Id = i;
                    excelSheetListItem.Name = sheets[i].Name;

                    cbCostSheetName.Items.Add(excelSheetListItem);
                    cbTripSheetName.Items.Add(excelSheetListItem);
                }

                cbCostSheetName.SelectedIndex = 0;
                cbTripSheetName.SelectedIndex = 1;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtZoneCount.Text))
            {
                MessageBox.Show("Please enter zone count.");
            }

            int zoneCount = int.MinValue;
            if (!int.TryParse(txtZoneCount.Text, out zoneCount) || zoneCount < 1)
            {
                MessageBox.Show("Please enter a numeric value which is greater than 1 for zone count.");
            }

            if (cbCostSheetName.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Cost Matrix sheet.");
            }

            if (cbTripSheetName.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Trip Matrix sheet.");
            }

            using (ExcelFileReader excelFileReader = new ExcelFileReader(this.ExcelFilePath))
            {
                ExcelSheetListItem costItem = (ExcelSheetListItem)cbCostSheetName.SelectedItem;
                object[,] costMatrix = excelFileReader.ReadSheet(costItem.Name, zoneCount, true, true);

                ExcelSheetListItem tripItem = (ExcelSheetListItem)cbTripSheetName.SelectedItem;
                object[,] tripMatrix = excelFileReader.ReadSheet(tripItem.Name, zoneCount, true, true);

                CostMatrix = new double[zoneCount, zoneCount];
                TripMatrix = new double[zoneCount, zoneCount];
                Names = new string[zoneCount];

                for (int i = 0; i < zoneCount + 1; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    for (int j = 0; j < zoneCount + 1; j++)
                    {
                        if (j == 0)
                        {
                            Names[i - 1] = costMatrix[i, j].ToString();
                            continue;
                        }

                        CostMatrix[i - 1, j - 1] = (double)costMatrix[i, j];
                        TripMatrix[i - 1, j - 1] = (double)tripMatrix[i, j];
                    }
                }
            }

            Close();
        }
    }

    class ExcelSheetListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
