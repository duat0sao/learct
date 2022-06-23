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



namespace superheroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class guiController : ControllerBase
    {
        static SerialPort sp = new SerialPort();

        [HttpGet("{nd}/{sdt}")]
        public string guitn(string nd, int sdt)
        {
            sp.PortName = "COM21";
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
            sp.WriteLine("AT+CMGS=\"" + "+84"+sdt + "\"" + Environment.NewLine);    //AT+CMGS đc sử dụng để gửi tin nhắn
            Thread.Sleep(100);
            sp.WriteLine(nd + Environment.NewLine);
            Thread.Sleep(100);
            sp.Write(new byte[] { 26 }, 0, 1);
            Thread.Sleep(100);
            return nd;
        }

    }
}
