namespace PacMan.Systems
{
    /*
     * Our different network states
     */
    public enum NetworkStatusType
    {
        Disconnected,
        ConnectingToServer,
        InLobby,
        SearchingForMatch,
        InRoom,
    }
}