using System;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace Iris_patch
{
    class Program
    {
        private static HttpListener httpListener = new HttpListener();
        static void Main(string[] args)
        {
            var ipAddress = Dns.GetHostAddresses("iristech.co");
            if (ipAddress[0].ToString() != "127.0.0.1")
            {
                try
                {
                    string hostspath = @"C:\WINDOWS\system32\drivers\etc\hosts";
                    File.SetAttributes(hostspath, FileAttributes.Normal);
                    FileStream hosts = new FileStream(hostspath, FileMode.Append);
                    StreamWriter sw = new StreamWriter(hosts);
                    sw.WriteLine("127.0.0.1    iristech.co");
                    sw.Close();
                    hosts.Close();
                }
                catch
                {
                    Console.WriteLine("屏蔽hosts失败!" );
                    Console.WriteLine("请手动将");
                    Console.WriteLine("127.0.0.1    iristech.co");
                    Console.WriteLine("添加到hosts中");
                }
            }
            
            Console.WriteLine("by:muruoxi");
            Console.WriteLine("https://www.muruoxi.com/");
            Console.WriteLine("请输入任意激活码完成激活");
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Prefixes.Add("http://iristech.co/custom-code/");
            httpListener.Start();
            new Thread(new ThreadStart(delegate
            {
                try
                {
                    WaitListener(httpListener);
                }
                catch (Exception)
                {
                    httpListener.Stop();
                }
            })).Start();



        }
        private static void WaitListener(HttpListener httpListenner)
        {
            while (true)
            {
                HttpListenerContext context = httpListenner.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                if (request.HttpMethod == "GET")
                {
                    var buffer = Encoding.UTF8.GetBytes("SUCCESS");
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                response.Close();
                Environment.Exit(0);
            }
        }
    }
}
