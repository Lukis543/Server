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
            Thread client = new Thread(Client);
            client.Start();
            Thread produc = new Thread(Creat);
            produc.Start();


        }

        static void Client()
        {
            ///loop
            while ((true))
            {
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    TcpClient producSocket = default(TcpClient);
                    NetworkStream clientStream = clientSocket.GetStream();
                    NetworkStream ProducStream = producSocket.GetStream();
                    byte[] bytesFrom = new byte[102400];
                    if (clientStream.DataAvailable)
                    {

                        clientStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                        string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf('\0'));
                        Console.WriteLine("Data from client - " + dataFromClient);
                        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dataFromClient);
                        ProducStream.Write(outStream, 0, outStream.Length);
                        ProducStream.Flush();
                        clientStream.Flush();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        static void Creat()
        {
            while ((true))
            {
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    TcpClient producSocket = default(TcpClient);
                    NetworkStream clientStream = clientSocket.GetStream();
                    NetworkStream ProducStream = producSocket.GetStream();
                    byte[] bytesFrom = new byte[102400];
                    if (ProducStream.DataAvailable)
                    {

                        ProducStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                        string dataFromProduc = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromProduc = dataFromProduc.Substring(0, dataFromProduc.IndexOf('\0'));
                        Console.WriteLine("Data from production - " + dataFromProduc);
                        byte[] outStream1 = System.Text.Encoding.ASCII.GetBytes(dataFromProduc);
                        clientStream.Write(outStream1, 0, outStream1.Length);
                        clientStream.Flush();
                        ProducStream.Flush();


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