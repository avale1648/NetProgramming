using NPexw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPexwClient
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (radioButtonCross.Checked)
                new CrossesNoughts { MySign = NPexwLib.Sign.Cross }.Show();
            else
                new CrossesNoughts { MySign = NPexwLib.Sign.Nought }.Show();
        }
    }
}
