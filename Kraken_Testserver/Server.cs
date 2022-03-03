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
			int tempByte;
			List<byte> tempBuff = new List<byte>();

			if (!Equals(firstbyte, null))
				tempBuff.Add((byte)firstbyte);

			while(!Equals(tempByte = ns.ReadByte(), null))
            {
				tempBuff.Add((byte)tempByte);
            }

			buffer = tempBuff.ToArray();
        }

		public static async Task<byte> AwaitDataReceive(NetworkStream ns)
        {
			int b;
			// waits for reicive a Byte
			do {} while ((b = ns.ReadByte()) == -1);

			await new Task<bool>( () => { return true; } );

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

