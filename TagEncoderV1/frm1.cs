using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagEncoderV1
{
    public partial class frm1 : Form
    {
       
        public frm1()
        {
            InitializeComponent();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bool selected = false;
            int option = -1;
            //Check if user has selected any of the options
            if (rbfile.Checked)
            {
                option = 1;
                selected = true;
            }
            else if(rbCopy.Checked){
                option = 2;
                selected = true;
            }
            else if (rbOneByOne.Checked)
            {
                option = 3;
                selected = false;
            }
            else
            {
                MessageBox.Show("Please select from the above options", "Select...", MessageBoxButtons.OK, MessageBoxIcon.Information);

                option = -1;
                selected = false;
            }

            //setting the wizart type value in the main form.
            if (selected)
            {
                frmMain fParent = (frmMain)this.Owner;
               
            }
        }
    }
}
