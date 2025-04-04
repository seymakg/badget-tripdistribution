using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Badger.Win
{
    public partial class ResultViewer : Form
    {
        public ResultViewer()
        {
            InitializeComponent();

            txtResult.Text = "Results:" + Environment.NewLine;
        }

        public void SetResult(string logs)
        {
            txtResult.Text += logs;
        }
    }
}
