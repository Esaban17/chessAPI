namespace chessAPI.models.game;

public sealed class clsNewGame
{
    public clsNewGame()
    {
        started = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        turn = false;
        winner = 0;
        whites = 0;
        blacks = 0;
    }

    public string started { get; set; }
    public bool turn { get; set; }
    public int winner { get; set; }
    public int whites { get; set; }
    public int blacks { get; set; }

}