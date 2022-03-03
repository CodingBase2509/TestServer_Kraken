using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Kraken_Testserver
{

	public class TCPServer : Server
	{
		TcpClient Client;

		NetworkStream ns;

		public TCPServer(int port)
			: base(port)
		{ }

		// Setup

		public override async Task SetupListener()
		{
			Listener.Start();
			Console.WriteLine($"TCPListener listens on Port {Port}");
			Client = Listener.AcceptTcpClient();

			if (Client.Connected)
			{ 
				ns = Client.GetStream();
				Console.WriteLine("TCPClient connects to the Server");
				await ReceiveMessage();
			}
		}

		// Connection-Using Functions

		public override async Task ReceiveMessage()
		{
			byte[] tempBuff = new byte[] {};
			//ns.Seek(0, SeekOrigin.Begin);
			try
			{
				Console.WriteLine("Receiving Messages: ");

				var firstByteReceived = await AwaitDataReceive(ns);

				if (ns.DataAvailable && ns.CanRead)
				{
					ReadBytes(ns, ref tempBuff, firstByteReceived);
					
					if(!Equals(tempBuff.Length, 0))
						Data = Encoding.UTF8.GetString(tempBuff, 0, tempBuff.Length);
				}

				if (Equals(tempBuff.Length, 0))
				{
					ReceiveMessage();
					return;
				}
				
				tempBuff = null!;

				Console.WriteLine(Data);
				await SendMessage(Data);

				ReceiveMessage();
				return;

			}catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("An error occured, initiate shutdownhook");
			    Shutdown(this, Client);
			}
        }

		public override async Task SendMessage(string msg)
		{
			try
			{
				var tempBuff = Encoding.UTF8.GetBytes(msg);
				await ns.WriteAsync(tempBuff, 0, tempBuff.Length);
				tempBuff = null!;
				//sw.WriteLine("Server responce: " + msg);

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

