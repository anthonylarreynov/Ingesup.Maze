using System.ServiceModel;
using Model = Ingesup.Maze.Server.Web.Models;

namespace Ingesup.Maze.Server.Web.WCF.Contract
{
    [ServiceContract]
    public interface IGame
    {
        [OperationContract]
        Model.PlayerGame CreateGame(Model.Difficulty difficulty, string playerName);

        [OperationContract]
        Model.Game LoadGame(string gameKey);

        [OperationContract]
        Model.Game ResetGame(string gameKey);

        [OperationContract]
        Model.Player AddPlayer(string gameKey, string playerName);

        [OperationContract]
        Model.Player MovePlayer(string gameKey, string playerKey, Model.Direction direction);
    }
}