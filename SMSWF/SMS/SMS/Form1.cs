using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System;


namespace SMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //public class SerialPort : System.ComponentModel.Component

        private void btnGui_Click(object sender, EventArgs e)
        {
            
            SerialPort sp = new SerialPort();
            sp.PortName = 999;
            sp.Open();
            sp.WriteLine("AT" + Environment.NewLine);
            Thread.Sleep(100);
            sp.WriteLine("AT+CMGP=1" + Environment.NewLine);
            Thread.Sleep(100);
            sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);
            Thread.Sleep(100);
            sp.WriteLine("AT+CMGS=\"" + txtSdt.Text + "\"" + Environment.NewLine);
            Thread.Sleep(100);
            sp.WriteLine(txtConten.Text + Environment.NewLine);
            Thread.Sleep(100);
            sp.Write(new byte[] { 26 }, 0, 1);
            Thread.Sleep(100);
            var response = sp.ReadExisting();
            if(response.Contains("ERROR"))
            {
                MessageBox.Show("Failed", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thanh cong", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
    }
}