using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Kraken_Testserver
{

	public class TCPServer : Server
	{
		TcpClient Client;

		StreamReader sr;
		StreamWriter sw;

		public TCPServer(int port)
			: base(port)
		{
			Client = new TcpClient();
			sr = new StreamReader(Client.GetStream());
			sw = new StreamWriter(Client.GetStream());
			sw.AutoFlush = true;
		}

		// Setup

		public override async Task SetupListener()
		{
			Listener.Start();
			Console.WriteLine($"TCPListener listens on Port {Port}");
			Client = await Listener.AcceptTcpClientAsync();
			Console.WriteLine("1");

			while (Client.Connected)
			{
				Console.WriteLine("TCPClient connects to the Server");
				ReceiveMessage();
			}
		}

		// Connection-Using Functions

		public override void ReceiveMessage()
		{
			try
			{
				Console.WriteLine("Receiving Message: ");
				while ((Data = sr.ReadLine()) != null)
				{
					Console.WriteLine(Data);
					SendMessage(Data);
				}
			}catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("An error occured, initiate shutdownhook");
				Shutdown(this, Client);
			}
        }

		public override void SendMessage(string msg)
		{
			try
			{
				sw.WriteLine("Server responce: " + msg);
			}catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("An error occured, initiate shutdownhook");
				Shutdown(this, Client);
			}
		}

    }
}

