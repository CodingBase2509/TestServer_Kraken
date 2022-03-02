namespace Kraken_Testserver;

public class Launcher
{
    static Server? server;

    public static void Main(string[] args)
    {
        server = new SocketServer(3333);
        server!.SetupListener();
    }    
}