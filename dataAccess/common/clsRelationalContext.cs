using System.Data;
using System.Globalization;
using chessAPI.dataAccess.interfaces;

namespace chessAPI.dataAccess.common;

public sealed class clsRelationalContext<TC> : IRelationalContext<TC>
        where TC : struct
{
    private readonly IDbConnection dbConn;
    private bool connActive;

    public IDbTransaction trn { get; private set; } = null!;
    public ILogger<clsRelationalContext<TC>> logger { get; private set; }
    public IDBConcurrencyHandler<TC> concurrencyHandler { get; private set; }

    public clsRelationalContext(IDbConnection dbConn,
                                ILogger<clsRelationalContext<TC>> logger,
                                IDBConcurrencyHandler<TC> concurrencyHandler)
    {
        this.dbConn = dbConn;
        this.logger = logger;
        this.concurrencyHandler = concurrencyHandler;
        connActive = false;
    }

    public void getTransactionContext()
    {
        if (!connActive)
        {
            dbConn.Open();
            connActive = true;
        }
        trn = dbConn.BeginTransaction();
    }

    public void rollbackTransaction(ApplicationException ex)
    {
        if (connActive)
        {
            if (trn != null)
            {
                trn.Rollback();
                logger.LogWarning("DB Transaction rolledback due to the following condition {0}", ex?.Message);
            }
        }
    }

    public void rollbackTransaction(Exception ex)
    {
        if (connActive)
        {
            if (trn != null)
            {
                trn.Rollback();
                logger.LogError("DB Transaction rolledback due to an nonexpected error");
            }
            var msg = "state conn exists?" + (dbConn != null).ToString(CultureInfo.CurrentCulture) + " dbConnState: " + (dbConn != null ? dbConn.State.ToString() : " non existent.");
            logger.LogError(msg);
        }
    }

    public void commitTransactionContext()
    {
        if (connActive)
        {
            if (trn != null)
            {
                trn.Commit();
            }
        }
    }

    public void closeTransactionContext()
    {
        if (connActive)
        {
            if (dbConn.State == ConnectionState.Open) dbConn.Close();
            connActive = false;
        }
    }
}