using System.Text;
using MessagePack;
using NekoLib.Scenes;
using NekoRay.Diagnostics.Model;
using WatsonWebsocket;

namespace NekoRay.Diagnostics; 

public class Server {
    public WatsonWsServer WsServer;
    
    public Server() {
        WsServer = new WatsonWsServer();
        WsServer.ClientConnected += OnClientConnected;
        WsServer.ClientDisconnected += OnClientDisconnected;
        WsServer.MessageReceived += OnMessageReceived;
    }
    
    public void Start() => WsServer.Start();

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs args) {
        Console.WriteLine($"Message received from {args.Client}: " + Encoding.UTF8.GetString(args.Data));
    }

    private void OnClientDisconnected(object? sender, DisconnectionEventArgs args) {
        Console.WriteLine("Client disconnected: " + args.Client);
    }

    private void OnClientConnected(object? sender, ConnectionEventArgs args) {
        Console.WriteLine("Client connected: " + args.Client);
    }

    public void Broadcast(byte[] data) {
        foreach (var client in WsServer.ListClients()) {
            WsServer.SendAsync(client.Guid, data);
        }
    }

    public void Update() {
    }
}