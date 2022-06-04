using MC;

namespace PTSoft.BDSManager.Services;

public class PlayerService
{
    public int OnlinePlayerCount => Level.GetAllPlayers().Count;

    public IEnumerable<Player> AllPlayers => Level.GetAllPlayers();
}