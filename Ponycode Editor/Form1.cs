using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;

namespace Ponycode_Editor
{
    public partial class Form1 : Form
    {
        Pony pony = new Pony();

        public Form1()
        {
            InitializeComponent();
            lblStatus.Text = "";
            Text = Application.ProductName + " v" + Application.ProductVersion;
        }

        private void btnCalcPonycode_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                pony.Race = Convert.ToByte(txtBoxRace.Text);
                pony.Gender = Convert.ToByte(txtBoxGender.Text);
                pony.BodySize = Convert.ToSingle(txtBoxBodySize.Text);
                pony.HornSize = Convert.ToSingle(txtBoxHornSize.Text);
                pony.Name = txtBoxName.Text;
                pony.CutieMarks = txtBoxCutieMark.Text.Split('-').Select(s => Convert.ToInt32(s)).ToArray();

                txtBoxPonycodeNew.Text = pony.calculatePonyCode();
                txtBoxPonyDataNew.Text = pony.calculatePonyData();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private void txtBoxPonycode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    lblStatus.Text = "";
                    txtBoxPonyData.Text = "";
                    txtBoxPonycodeNew.Text = "";
                    txtBoxPonyDataNew.Text = "";

                    pony.PonyCode = Convert.FromBase64String(txtBoxPonycode.Text);
                    txtBoxRace.Text = pony.Race.ToString();
                    txtBoxGender.Text = pony.Gender.ToString();
                    txtBoxBodySize.Text = pony.BodySize.ToString();
                    txtBoxHornSize.Text = pony.HornSize.ToString();
                    txtBoxName.Text = pony.Name;
                    txtBoxCutieMark.Text = string.Join("-", pony.CutieMarks);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = ex.Message;
                }
            }
        }

        private void txtBoxPonyData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    lblStatus.Text = "";
                    txtBoxPonycode.Text = "";
                    txtBoxPonycodeNew.Text = "";
                    txtBoxPonyDataNew.Text = "";

                    pony.PonyData = Convert.FromBase64String(txtBoxPonyData.Text);
                    txtBoxRace.Text = pony.Race.ToString();
                    txtBoxGender.Text = pony.Gender.ToString();
                    txtBoxBodySize.Text = pony.BodySize.ToString();
                    txtBoxHornSize.Text = pony.HornSize.ToString();
                    txtBoxName.Text = pony.Name;
                    txtBoxCutieMark.Text = string.Join("-", pony.CutieMarks);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = ex.Message;
                }
            }
        }
    }
}
