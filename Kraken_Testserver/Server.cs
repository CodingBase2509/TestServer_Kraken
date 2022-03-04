using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Kraken_Testserver
{
	public abstract class Server
	{
		// Properties
		public int Port { get; set; }

		public string Data { get; set; }

		public string Host => "127.0.0.1";

		public TcpListener Listener;

		// Constructor
		public Server(int port)
		{
			Port = port;
			Listener = new TcpListener(IPAddress.Parse(Host), port);
		}

		// Connection establishment

		public abstract Task SetupListener();

		// Data Transfer Methods

		public abstract Task ReceiveMessage();

		public abstract Task SendMessage(string msg);

		public static void ReadBytes(NetworkStream ns,ref byte[] buffer, byte? firstbyte)
        {
			try
			{
				int tempByte;
				List<byte> tempBuffer = new List<byte>();

				if (!Equals(firstbyte, null))
				{
					tempBuffer.Add((byte)firstbyte);
				}

				while (ns.DataAvailable)
				{
					tempByte = ns.ReadByte();
					tempBuffer.Add((byte)tempByte);
				}

				buffer = tempBuffer.ToArray();
			}catch(Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				Console.WriteLine("An error occured, initiate shutdownhook");
			}
        }

		public static async Task<byte> AwaitDataReceive(NetworkStream ns)
        {
			int b;
			// waits for reicive a Byte
			while ((b = ns.ReadByte()) == -1) { }

			return (byte)b;
		}

		// Shutdown Functions

		public static void Shutdown(Server obj, Socket con)
		{
			con.Close();
			Console.WriteLine("Socket closed");
			con.Disconnect(true);
			Console.WriteLine("Socket disconnected");
			con.Dispose();
			Console.WriteLine("Socket disposed");
			DisposeListener(obj);
		}

		public static void Shutdown(Server obj, TcpClient con)
		{
			con.Close();
			Console.WriteLine("Client closed");
			con.Dispose();
			Console.WriteLine("Client disposed");
			DisposeListener(obj);
		}

		private static void DisposeListener(Server serv)
        {
			serv.Listener.Stop();
            Console.WriteLine("Listener stoped");
        }
	}
}

