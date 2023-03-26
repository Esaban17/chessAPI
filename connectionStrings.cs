namespace chessAPI;
public sealed class connectionStrings
{

        public connectionStrings()
        {
            relationalDBConn = "";
            mongoDbConn = "";
        }

        /// <summary>
        /// Cadena de conexión a base de datos relacional
        /// </summary>
        public string relationalDBConn { get; set; }
        public string mongoDbConn { get; set; }
}