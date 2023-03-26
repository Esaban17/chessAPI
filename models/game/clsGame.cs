namespace chessAPI.models.game;

public sealed class clsGame
{
    public long id { get; set; }
    public string started { get; set; }
    public int whites { get; set; }
    public int blacks { get; set; }
    public bool turn { get; set; }
    public int winner { get; set; }
}