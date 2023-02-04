namespace chessAPI.models.team
{
    public sealed class clsTeam<TI>
        where TI : struct, IEquatable<TI>
    {
        public clsTeam(TI id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public TI id { get; set; }
        public string name { get; set; }

    }
}
