using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace socket
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            // establish the local end point for the socket 
            IPHostEntry ipHost = Dns.Resolve("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // create a Tcp/Ip Socket 
            Socket sListener = new Socket(AddressFamily.InterNetwork,
                                            SocketType.Stream, ProtocolType.Tcp);

            // bind the socket to the local endpoint and 
            // listen to the incoming sockets 

            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Start listening for connections 

                while (true)
                {
                    Console.WriteLine("Waiting for a connection on port {0}", ipEndPoint);

                    // program is suspended while waiting for an incoming connection 
                    Socket handler = sListener.Accept();

                    string data = null;

                    // we got the client attempting to connect 
                    while (true)
                    {
                        byte[] bytes = new byte[1024];

                        int bytesRec = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<theend>") > -1)
                        {
                            break;
                        }
                    }
                    int pos = data.IndexOf("<theend>");
                    if (pos >= 0)
                    {
                        // String after founder  
                        data = data.Remove(pos);
                        
                    }
                       
                    Perso monPerso = JsonSerializer.Deserialize<Perso>(data);

                    
                    // show the data on the console 
                    Console.WriteLine(monPerso.age);
                    Console.ReadLine();
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }

        } // end of Main 
    }
}


        