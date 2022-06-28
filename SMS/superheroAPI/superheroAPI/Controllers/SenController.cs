using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;



/*
    Get lấy dữ liệu
    Post đẩy dữ liệu mới lên
    put Pack sửa dl
    Dele xóa dữ liệu
 */



namespace superheroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SenController : ControllerBase
    {
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public static string? ndl { get; set; }

        public static string[]? cccom { get; set; }

        public static string[]? tttb { get; set; }

        static SerialPort sp = new SerialPort();



        [HttpGet("conhungcongCOMnao")]
        public string[] getCom()
        {
            cccom = SerialPort.GetPortNames();
            return cccom;
        }

        
        [HttpGet("laytttb")]
        public string laytttb()
        {
            foreach(string a in cccom)
            {
                sp.PortName = a;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.StopBits = StopBits.One;
                sp.BaudRate = 115200;
                
                sp.DataReceived += Sp_DataReceived;
                
                sp.Open();
                
                sp.Write("AT+CGMI" + Environment.NewLine);
                
                manualResetEvent.WaitOne();
                
                sp.Close();
            }

            //sp.PortName = "COM21";      //viettel
            //sp.PortName = "COM9";         //vina
            //sp.DataBits = 8;
            //sp.Parity = Parity.None;
            //sp.StopBits = StopBits.One;
            //sp.BaudRate = 115200;
            //
            //sp.DataReceived += Sp_DataReceived;
            //
            //sp.Open();
            //
            //sp.Write("AT+CGMI" + Environment.NewLine);
            //
            //manualResetEvent.WaitOne();
            //
            //sp.Close();


            return "Tên thiết bị là: " + ndl;
        }



        [HttpGet("{nd}&{sdt}")]
        public string guitn(string nd, int sdt)
        {
            sp.PortName = "COM9";
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.BaudRate = 115200;

            sp.Open();
            Thread.Sleep(1000);

            sp.WriteLine("AT" + Environment.NewLine);    //lệnh AT dùng để sử dụng điều khiển modem
            Thread.Sleep(100);

            sp.WriteLine("AT+CMGF=1" + Environment.NewLine);    //chuyen(mở) che do gui tin nhan,
            Thread.Sleep(100);

            sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);    //Chọn bộ ký tự của thiết bị AT+CSCS, GSM bảng chữ cái mặc định
            Thread.Sleep(100);
            sp.WriteLine("AT+CMGS=\"" + "+84" + sdt + "\"" + Environment.NewLine);    //AT+CMGS đc sử dụng để gửi tin nhắn
            Thread.Sleep(100);
            sp.WriteLine(nd + Environment.NewLine);
            Thread.Sleep(100);
            sp.Write(new byte[] { 26 }, 0, 1);
            Thread.Sleep(100);
            sp.Close();
            return nd;
        }



        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string noidung = sp.ReadExisting();


            Task t1 = new Task(
                () =>
                {
                    string[] newText = Regex.Split(noidung, @"\s+");

                    foreach (string s in newText)
                    {
                        if ((s != "AT+CGMI") && (s != "OK"))
                            //Console.WriteLine(s);
                            ndl += " "+ s;
                    }
                    //Console.WriteLine("o phan bat su kien " + ndl);
                    manualResetEvent.Set();
                });


            t1.Start();
            //manualResetEvent.Set();

        }
    }
}
