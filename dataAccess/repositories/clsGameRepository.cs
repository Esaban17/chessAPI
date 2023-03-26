using chessAPI.dataAccess.common;
using chessAPI.dataAccess.interfaces;
using chessAPI.dataAccess.models;
using chessAPI.models.game;
using Dapper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace chessAPI.dataAccess.repositores;

public sealed class clsGameRepository : IGameRepository
{
    private readonly IMongoCollection<clsGameEntityModel> gameCollection;

    public clsGameRepository(IMongoCollection<clsGameEntityModel> gameCollection)
    {
        this.gameCollection = gameCollection;
    }

    private async Task<long> getLastGame()
    {
        //Empty document tells the driver to count all the documents in the collection
        return await gameCollection.CountDocumentsAsync(new BsonDocument());
    }

    public async Task addGame(clsNewGame newGame)
    {
        var newId = await getLastGame().ConfigureAwait(false) + 1;
        await gameCollection.InsertOneAsync(new clsGameEntityModel(newGame, newId)).ConfigureAwait(false);
    }

    public async Task<clsGameEntityModel?> getGame(long id)
    {
        var filter = Builders<clsGameEntityModel>.Filter.Eq(r => r.id, id);
        return await gameCollection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task swapTurn(long id)
    {
        var gameToSwap = await getGame(id).ConfigureAwait(false);
        if (gameToSwap != null)
        {
            var update = Builders<clsGameEntityModel>.Update.Set(g => g.turn, !gameToSwap.turn);
            var filter = Builders<clsGameEntityModel>.Filter.Eq(r => r.id, id);
            await gameCollection.UpdateOneAsync(filter, update);
        }
    }

}