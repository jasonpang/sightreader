using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class FormSandbox : Form
    {
        public FormSandbox()
        {
            InitializeComponent();
        }

        private void FormSandbox_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var parser = new Song();
            //var score = parser.LoadFile(@"C:\Users\Jason\Documents\OCR Sheet Music Converted\The Dark Knight Rises (Mark Fowler Cover) - Medley (Piano Score) (1).xml");
            var a = 1;
        }
    }
}
