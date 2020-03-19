using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketListener
{
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }

    public static IPAddress ipAddress = System.Net.IPAddress.Parse("192.168.3.147");
    public static IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5050);
    public static Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    public static Socket handler;

    public static void ReceiveMessage()
    {
        try
        {
            while(true)
            {
                string data = null;
                byte[] bytes = null;
                bytes = new byte[1024];

                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Client: {0}", data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static void StartServer()
    {
        listener.Bind(localEndPoint);
        listener.Listen(10);
        Console.WriteLine("Waiting for a connection...");

        try
        {
            handler = listener.Accept();
            string inputstring;

            while (true)
            {
                Thread receive = new Thread(ReceiveMessage);
                receive.Start();

                inputstring = Console.ReadLine();
                byte[] msg = Encoding.ASCII.GetBytes(inputstring);
                handler.Send(msg);
            }

            /*
            byte[] msg = Encoding.ASCII.GetBytes(data);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            */


        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }
}