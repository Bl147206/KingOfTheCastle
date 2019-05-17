using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class StateObject {
    // Client  socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 1024;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class Server {
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    static List<StateObject> clients = new List<StateObject>();

    public void start() {
        new Thread(() => {
            var info = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = IPAddress.Parse("0.0.0.0"); // dummy ip
            foreach (var address in info.AddressList) {
                if (address.AddressFamily == AddressFamily.InterNetwork) {
                    ip = address;
                    //KingOfTheCastle.KingOfTheCastle.serverStatus = "IP Address @ " + ip;
                    break;
                }
            }
            var endPoint = new IPEndPoint(ip, 1337);

            var listener = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try {
                listener.Bind(endPoint);
                listener.Listen(3);

                while (true) {
                    allDone.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }).Start();
    }

    public static void AcceptCallback(IAsyncResult ar) {
        // Signal the main thread to continue.  
        allDone.Set();

        // Get the socket that handles the client request.  
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        // Create the state object.  
        StateObject state = new StateObject();
        state.workSocket = handler;
        clients.Add(state);
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public static void ReadCallback(IAsyncResult ar) {
        String content = String.Empty;

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.   
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) {
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read   
            // more data.  
            content = state.sb.ToString();
            if (content.IndexOf("EOF") > -1) {
                // All the data has been read from the   
                // client. Display it on the console.  
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content);

                // Echo the data back to the client.  
                Send(handler, content);
            } else {
                // Not all data received. Get more.  
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
    }

    // Send data to all clients
    public static void Send(String data) {
        foreach (var client in clients) {
            Send(client.workSocket, data);
        }
    }

    private static void Send(Socket handler, String data) {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

}
