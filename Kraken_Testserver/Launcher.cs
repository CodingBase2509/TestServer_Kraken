using System.Reflection;
using System.Text;

namespace Kraken_Testserver;

public class Launcher
{
    static Server? server1;
    static Server? server2;

    public static void Main(string[] args)
    {
        while (true)
        {
            SetupServer<TCPServer>(server1!);
            Console.WriteLine("Restart Server");
            Console.WriteLine();
        }
    }

    private static void func(string[] args)
    {
        do
        {
            if (args.Length == 0)
                SetupAll();
            else
            {
                var tcpName = nameof(TCPServer);
                var socketName = nameof(SocketServer);

                if (args.Contains("-s"))
                {
                    int serverIndex = Array.IndexOf(args, "-s");
                    string nameoftype = args[serverIndex + 1];

                    if (nameoftype == tcpName)
                        SetupServer<TCPServer>(server1!);
                    else if (nameoftype == socketName)
                        SetupServer<SocketServer>(server1!);

                }
                else if (args.Contains("-h"))
                    Help();

            }
        } while (Console.ReadLine() != "x");
    }

    public static void SetupAll()
    {
        server2 = new TCPServer(3333);
        server1 = new SocketServer(4444);

        server1!.SetupListener();
        server2!.SetupListener();
    }

    public static void SetupServer<T>(Server server) where T: Server
    {
        var t = typeof(T);

        if (t == typeof(TCPServer))
            server = new TCPServer(3333);
        else if (t == typeof(SocketServer))
            server = new SocketServer(4444);

        server!.SetupListener();
    }

    public static void Help()
    {
        var s = new StringBuilder();

        s.AppendLine("Existing commands:");
        s.AppendLine(string.Empty);
        s.AppendLine("-s    spezifies the type of the Server an starts the server of the given type");
        s.AppendLine("      valid server-types are: TCPServer, SocketServer");
        s.AppendLine("      The TCPServer runs on 127.0.0.1, port 3333");
        s.AppendLine("      The SocketServer runs on 127.0.0.1, port 4444");
        s.AppendLine(string.Empty);
        s.AppendLine("start without flags will start both servers");
        s.AppendLine(string.Empty);
        s.AppendLine("-h    opens this help page");

        Console.WriteLine(s);

        // first possibile way to restart app
        var fileName = Assembly.GetExecutingAssembly().Location;
        System.Diagnostics.Process.Start(fileName);
        Environment.Exit(0);
    }

}