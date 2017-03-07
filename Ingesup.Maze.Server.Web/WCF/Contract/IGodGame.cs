using System.ServiceModel;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.WCF.Contract
{
    [ServiceContract]
    public interface IGodGame
    {
        [OperationContract]
        Model.UrlPlayerGame CreateGame(Model.Difficulty difficulty, string playerName);

        [OperationContract]
        Model.UrlGame LoadGame(string gameKey);

        [OperationContract]
        Model.UrlGame ResetGame(string gameKey);

        [OperationContract]
        Model.Player AddPlayer(string gameKey, string playerName);

        [OperationContract]
        Model.Player MovePlayer(string gameKey, string playerKey, Model.Direction direction);

        [OperationContract]
        Model.Player AddClone(string gameKey, string playerKey);

        [OperationContract]
        void RemoveClone(string gameKey, string playerKey);
    }
}