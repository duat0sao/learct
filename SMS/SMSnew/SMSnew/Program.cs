using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Text.RegularExpressions;



namespace SMSnew
{



    internal class Program
    {

        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);


        static SerialPort sp = new SerialPort();

        static string ndl { get; set; }


        static void Main(string[] args)
        {


            //lay thong tin de gui
            //Console.WriteLine("So dien thoai:");
            //int sdt = Convert.ToInt32(Console.ReadLine()); //nhap với số
            //Console.WriteLine("Noi dung:");
            //string contenGD = Console.ReadLine(); //nhap voi string 
            //
            //Console.WriteLine("Sdt: " + sdt);
            //Console.WriteLine("Noi dung: " + contenGD);

            //nghi ngoi 1 chut
            //Thread.Sleep(100);

            sp.PortName = "COM21";
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.BaudRate = 115200;

            //sp.DataReceived += Sp_DataReceived;
            
            sp.Open();

            //Console.ReadKey();

            //sp.WriteLine("AT" + Environment.NewLine);    //lệnh AT dùng để sử dụng điều khiển modem
            //Thread.Sleep(100);

            //sp.Write("AT+CGMR"+Environment.NewLine);
            //Thread.Sleep(1000);
            //Console.ReadKey();
            //sp.Write("AT+CGMI" + Environment.NewLine);      //tên thiết bị


            //t1.Start();

            //string noidung = sp.ReadExisting();

            Thread.Sleep(100);


            //manualResetEvent.WaitOne();


            //Console.WriteLine("sau khi nghi" + ndl);


            Thread.Sleep(1000);
            
            sp.WriteLine("AT" + Environment.NewLine);    //lệnh AT dùng để sử dụng điều khiển modem
            Thread.Sleep(100);
            
            sp.WriteLine("AT+CMGF=1" + Environment.NewLine);    //chuyen(mở) che do gui tin nhan,
            Thread.Sleep(100);
            
            sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);    //Chọn bộ ký tự của thiết bị AT+CSCS, GSM bảng chữ cái mặc định
            Thread.Sleep(100);
            sp.WriteLine("AT+CMGS=\"" + "+84396192800" + "\"" + Environment.NewLine);    //AT+CMGS đc sử dụng để gửi tin nhắn
            Thread.Sleep(100);
            sp.WriteLine("gg" + Environment.NewLine);
            Thread.Sleep(100);
            sp.Write(new byte[] { 26 }, 0, 1);
            Thread.Sleep(100);
            //var response = sp.ReadExisting();
            
            //if (response.Contains("ERROR"))
            //{
            //    Console.WriteLine("SMS that bai roi");
            //}
            //else
            //{
            //    Console.WriteLine("SMS da gui thanh cong");
            //}
            Thread.Sleep(10000);
            //sp.Close();

            //Task t1 = Task1();

        }

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string noidung = sp.ReadExisting();
            //Console.WriteLine($"noi dung {noidung}");

            //string[] newText = Regex.Split(noidung, @"\s+");
            //
            //
            //foreach(string s in newText)
            //{
            //    if ((s != "AT+CGMI") && (s!="OK"))
            //        //Console.WriteLine(s);
            //        ndl += " " + s;
            //}
            //Console.WriteLine(ndl);


            Task t1 = new Task(
                () =>
                {
                    string[] newText = Regex.Split(noidung, @"\s+");
            
                    foreach (string s in newText)
                    {
                        if ((s != "AT+CGMI") && (s != "OK"))
                            //Console.WriteLine(s);
                            ndl += " " + s;
                    }
                    Console.WriteLine("o phan bat su kien " + ndl);
                    manualResetEvent.Set();
                });
            
            
            t1.Start();
            manualResetEvent.Set();


        }
    }
}