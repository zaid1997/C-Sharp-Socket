using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketClient
{
    IPHostEntry host = Dns.GetHostEntry("localhost");
    public static IPAddress ipAddress = System.Net.IPAddress.Parse("192.168.3.147");
    public static IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5050);
    public static Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


    public static int Main(String[] args)
    {
        StartClient();
        return 0;
    }

    public static void SendMessage()
    {
        string inputstring = Console.ReadLine();
        byte[] msg = Encoding.ASCII.GetBytes(inputstring);
        sender.Send(msg);
    }

    public static void ReceiveMessage()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = sender.Receive(bytes);
        Console.WriteLine("Server: {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));
    }

    public static void StartClient()
    {
        byte[] bytes = new byte[1024];

        try
        {
            try
            {
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                while (true)
                {
                    Thread receive = new Thread(ReceiveMessage);
                    receive.Start();
                    SendMessage();
                }

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
