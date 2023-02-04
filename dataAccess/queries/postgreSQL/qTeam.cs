namespace chessAPI.dataAccess.queries.postgreSQL
{
    public sealed class qTeam : IQTeam
    {
        private const string _selectAll = @"
        SELECT id, name, created_at
        FROM public.team";

        private const string _selectOne = @"
        SELECT id, name, created_at 
        FROM public.team
        WHERE id=@ID";

        private const string _add = @"
        INSERT INTO public.team(name)
	    VALUES (@NAME) RETURNING id";

        private const string _delete = @"
        DELETE FROM public.team 
        WHERE id = @ID";

        private const string _update = @"
        UPDATE public.team
	    SET name=@NAME
	    WHERE id=@ID";

        public string SQLGetAll => _selectAll;

        public string SQLDataEntity => _selectOne;

        public string NewDataEntity => _add;

        public string DeleteDataEntity => _delete;

        public string UpdateWholeEntity => _update;
    }
}
