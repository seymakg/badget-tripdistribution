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
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        public void SetText(string text) {
            label1.Text = text;
        }

        public string GetText()
        {
            return label1.Text;
        }
    }
}
