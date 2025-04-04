using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Badger.Win
{
    public partial class EditForm : Form
    {
        private string editValue;
        private string editLabel;
        public string EditValue
        {
            set
            {
                editValue = value;
                valueTextBox.Text = value;
            }
        }
        public string EditLabel
        {
            set
            {
                editLabel = value;
                label1.Text = value + " :";
            }
        }

        public EditForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string input = valueTextBox.Text;

            Properties.Settings.Default["FileSaveLocation"] = input;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
