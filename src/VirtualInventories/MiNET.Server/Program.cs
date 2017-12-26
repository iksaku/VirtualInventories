using System;
using System.Collections.Generic;

namespace MiNET.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting MiNET Server...");
            
            MiNetServer server = new MiNetServer();
            server.StartServer();
            
            Console.WriteLine("MiNET Server started! Press <Enter> to exit");
            Console.ReadLine();
            Console.WriteLine("Stopping MiNET Server...");
            
            server.StopServer();
        }
    }
}