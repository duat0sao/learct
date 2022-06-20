using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;


namespace smsnewnew
{
    internal class Program
    {
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        static SerialPort sp = new SerialPort();

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string noidung = sp.ReadExisting();
            Console.WriteLine(noidung);

        }
        static void Main(string[] args)
        {

            sp.PortName = "COM9";
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.BaudRate = 115200;

            sp.DataReceived += Sp_DataReceived;

            sp.Open();

            sp.Write("AT+CGMI" + Environment.NewLine);      //tên thiết bị
            Thread.Sleep(1000);
        }
    }
}
