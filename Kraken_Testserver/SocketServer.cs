using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Kraken_Testserver
{
	public class SocketServer: Server
	{
		Socket Socket;

		byte[] Buffer = new byte[] { };

		public SocketServer(int port)
			:base(port)
		{
			Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

        public override async Task SetupListener()
        {
			Socket.Bind(new IPEndPoint(IPAddress.Parse(Host), Port));
			Socket.Listen();
			Console.WriteLine($"Socket listens on Port {Port}");
			//Socket = await Listener.AcceptSocketAsync();
			Console.WriteLine("2");

			while (Socket.Connected)
			{ 
				Console.WriteLine("Socket connects to the Server");
				ReceiveMessage();
			}
		}

        public override void ReceiveMessage()
        {
			try
			{
				Socket.Receive(Buffer);
				Data = Encoding.UTF8.GetString(Buffer, 0, Buffer.Length);
				Buffer = new byte[] { };
				SendMessage(Data);
			}catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("An error occured, initiate shutdownhook");
				Shutdown(this, Socket);
            }
		}

        public override void SendMessage(string msg)
        {
			try
			{
				string temp = "Server replied " + msg;
				Buffer = Encoding.UTF8.GetBytes(temp, 0, temp.Count());
				Socket.Send(Buffer);
				Buffer = new byte[] { };
			}catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("An error occured, initiate shutdownhook");
				Shutdown(this, Socket);
			}
        }

        
    }
}

