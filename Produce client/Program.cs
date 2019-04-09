using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ProducerClient";
            Console.WriteLine("**Producer Main**");
            System.Net.Sockets.TcpClient producSocket = new System.Net.Sockets.TcpClient();
            producSocket.Connect("127.0.0.1", 8888);
            Console.WriteLine("Connection established with the server");
            Console.WriteLine("Type in hi");
            while (true)
            {
                try
                {
                    NetworkStream serverStream = producSocket.GetStream();
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    if (serverStream.DataAvailable)
                    {

                        byte[] inStream = new byte[102400];
                        serverStream.Read(inStream, 0, (int)producSocket.ReceiveBufferSize);
                        string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                        returndata = returndata.Substring(0, returndata.IndexOf('\0'));
                        Console.WriteLine("\n Response from Consumer Client: " + returndata);
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
