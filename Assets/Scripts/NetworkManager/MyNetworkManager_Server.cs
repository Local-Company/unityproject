using Mirror;

public partial class MyNetworkManager {
    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        base.OnServerAddPlayer(conn);

        if (conn.identity.gameObject.TryGetComponent<NetworkPlayer>(out var player)) {
            player.SetRandomColor();
            player.SetName($"Player {this.numPlayers.ToString()}");
        }
    }
}