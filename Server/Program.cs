using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Servers

{
    class Program
    {

        static void Main(string[] args)
        {            
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ipAd, 8888);
            serverSocket.Start();
            TcpClient clientSocket = default(TcpClient);
            TcpClient producSocket = default(TcpClient);
            Console.Title = "Server";
            Console.WriteLine("**Server waiting for user connection**");
            clientSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine("Connection established with the Consumer Main");
            producSocket = serverSocket.AcceptTcpClient();
            Console.WriteLine("Connection established with the Producer Main");

            while (true)
            {
                try
                {
                    NetworkStream clientStream = clientSocket.GetStream();
                    NetworkStream ProducStream = producSocket.GetStream();
                   byte[] bytesFrom = new byte[102400];
                    if (clientStream.DataAvailable)
                    {
                        new Thread(() =>
                        {

                            Console.WriteLine("\nClient Thread");
                            int bytesRead = clientStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, bytesRead);
                            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dataFromClient);
                            ProducStream.Write(outStream, 0, outStream.Length);
                            Console.WriteLine("Data from client - " + dataFromClient);
                            Thread.Sleep(1000);
                        }).Start();
                        Console.WriteLine("\nThread off");
                        


                    }
                    else if (ProducStream.DataAvailable)
                    {
                        new Thread(() => 
                        {

                            Console.WriteLine("\nProduction Thread");
                            int bytesread = ProducStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                            string dataFromProduc = System.Text.Encoding.ASCII.GetString(bytesFrom, 0, bytesread);;
                            byte[] outStream1 = System.Text.Encoding.ASCII.GetBytes(dataFromProduc);
                            clientStream.Write(outStream1, 0, outStream1.Length);
                            Console.WriteLine("Data from production - " + dataFromProduc);
                            Thread.Sleep(1000);
                        }).Start();
                        Console.WriteLine("\nThread off");

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