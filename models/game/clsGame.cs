namespace chessAPI.models.game;

public sealed class clsGame<TI>
    where TI : struct, IEquatable<TI>
{
    public clsGame(TI id, string started, bool turn, int winner, int whites, int blacks)
    {
        this.id = id;
        this.started = started;
        this.turn = turn;
        this.winner = winner;
        this.whites = whites;
        this.blacks = blacks;
    }

    public TI id { get; set; }
    public string started { get; set; }
    public bool turn { get; set; }
    public int winner { get; set; }
    public int whites { get; set; }
    public int blacks { get; set; }
}