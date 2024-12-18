using Unity.Netcode;
using UnityEngine;

public class NetworkButtonManager : MonoBehaviour
{
    private const int maxClients = 4; // 최대 클라이언트 수 (호스트 포함)

    public void StartServer()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Server started.");
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        else
        {
            Debug.LogError("Failed to start server.");
        }
    }

    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started by: " + NetworkManager.Singleton.LocalClientId);
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    public void ConnectClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client connecting...");
        }
        else
        {
            Debug.LogError("Failed to connect client.");
        }
    }

    public void DisconnectClient()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Client disconnected: " + NetworkManager.Singleton.LocalClientId);
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        int currentClients = NetworkManager.Singleton.ConnectedClientsList.Count;
        Debug.Log($"Client connected: {clientId}. Total clients: {currentClients}");

        if (currentClients > maxClients)
        {
            Debug.Log($"Max clients reached. Disconnecting client: {clientId}");
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandleButtonPressServerRpc(ulong clientId, int buttonId)
    {
        Debug.Log($"Server: Player {clientId} pressed Button {buttonId}");
        NotifyClientsClientRpc(clientId, buttonId);
    }

    [ClientRpc]
    public void NotifyClientsClientRpc(ulong clientId, int buttonId)
    {
        Debug.Log($"Client: Player {clientId} pressed Button {buttonId}");
    }
}
