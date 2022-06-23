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

        static SerialPort sp = new SerialPort();

        
        [HttpGet]
        public string nhantt()
        {

            sp.PortName = "COM21";
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.StopBits = StopBits.One;
            sp.BaudRate = 115200;

            sp.DataReceived += Sp_DataReceived;

            sp.Open();

            sp.Write("AT+CGMI" + Environment.NewLine);

            manualResetEvent.WaitOne();


            return "Tên thiết bị là: " + ndl;
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
