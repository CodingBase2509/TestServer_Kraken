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

		public static void ReadBytes(NetworkStream ns,ref byte[] buffer, int offset)
        {
			int index = offset;
			int tempByte;
			byte[] tempBuff = new byte[] {} ;

			while(!Equals(tempByte = ns.ReadByte(), null))
            {
				tempBuff[index] = (byte)tempByte;
				index++;
            }
			buffer = tempBuff;
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

