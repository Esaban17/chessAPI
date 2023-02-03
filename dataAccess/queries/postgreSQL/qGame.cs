namespace chessAPI.dataAccess.queries.postgreSQL;

public sealed class qGame : IQGame
{
    private const string _selectAll = @"
    SELECT id, winner, blacks, whites, turn, winner
    FROM public.game";

    private const string _selectOne = @"
    SELECT id, winner, blacks, whites, turn, winner 
    FROM public.game
    WHERE id=@ID";

    private const string _add = @"
    INSERT INTO public.player(winner, blacks, whites, turn)
	VALUES (@WINNER, @BLACKS, @WHITES, @TURN) RETURNING id";

    private const string _delete = @"
    DELETE FROM public.game 
    WHERE id = @ID";

    private const string _update = @"
    UPDATE public.game
	SET blacks=@BLACKS, whites=@WHITES, turn=@TURN, winner=@WINNER
	WHERE id=@ID";

    public string SQLGetAll => _selectAll;

    public string SQLDataEntity => _selectOne;

    public string NewDataEntity => _add;

    public string DeleteDataEntity => _delete;

    public string UpdateWholeEntity => _update;
}