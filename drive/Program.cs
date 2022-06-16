using System;
using System.IO.Ports;
using System.Threading;


namespace drive
{
   class Program
   {

        static void Dosomething(int seconds)
        {
            Console.WriteLine("Di ngu 1 chut");
            Thread.Sleep(seconds);
            Console.WriteLine("Day hoc tiep thoi");

        }

        private static SerialPort sp; 
        
    
        static void Main()
        {


            sp = new SerialPort();

            //lay thong tin de gui
            Console.WriteLine("So dien thoai:");
            int sdt = Convert.ToInt32(Console.ReadLine()); //nhap với số
            Console.WriteLine("Noi dung:");
            string contenGD = Console.ReadLine(); //nhap voi string 

            Console.WriteLine("Sdt: " + sdt);
            Console.WriteLine("Noi dung: " + contenGD);
            

            //nghi ngoi 1 chut
            Dosomething(1000);

            sp.PortName = "COM";

            sp.Open();
            sp.WriteLine("AT" + Environment.NewLine);    //lệnh AT dùng để sử dụng điều khiển modem
            Dosomething(1000);
            sp.WriteLine("AT+CMGP=1" + Environment.NewLine);    //chuyen(mở) che do gui tin nhan,
            Dosomething(1000);

            sp.WriteLine("AT+CSCS=\"GSM\"" + Environment.NewLine);    //Chọn bộ ký tự của thiết bị AT+CSCS, GSM bảng chữ cái mặc định
            Dosomething(1000);
            sp.WriteLine("AT+CMGS=\"" + sdt + "\"" + Environment.NewLine);    //AT+CMGS đc sử dụng để gửi tin nhắn
            Dosomething(1000);
            sp.WriteLine(contenGD + Environment.NewLine);
            Dosomething(1000);
            sp.Write(new byte[] {26}, 0, 1);
            Dosomething(1000);
            var response = sp.ReadExisting();

            if(response.Contains("ERROR"))
            {
                Console.WriteLine("SMS that bai roi");
            }
            else
            {
                Console.WriteLine("SMS da gui thanh cong");
            }



           




       }
   }
}