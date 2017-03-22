using System;
using Data = Ingesup.Maze.Server.Core.Entities;

namespace Ingesup.Maze.Server.Web.Helper
{
    public static class SecretMessageHelper
    {
        /* ************************************************* *\
         * Here are the normal steps to unlock secrets:
         *   A. Finish his own game:
         *       1. in Easy mode              => "Want new challenge ? Try to make harder mode..."
         *       2. in Medium mode            => "Want new challenge ? Try to make harder mode..."
         *       3. in Hard mode              => "Have you ever tried to beat someone else on his ground ? Try this: {0}"
         *       4. in Extreme mode           => "Have you ever tried to beat someone else on his ground ? Try this: {0}"
         *
         *   B. Finish other player game:
         *       1. Loose                     => "Only winners will be awarded..."
         *       2. Win
         *           a. in Easy mode          => "It's not fair in easy mode :)"
         *           b. in Medium mode        => "My secret does not match with medium players..."
         *           c. in Hard mode          => "Need to be a super hero ? Try this: {0}"
         *           d. in Extreme mode       => "Need to be a super hero ? Try this: {0}"
         * 
         *   C. Finish a multiplayer game with all players in distinct powers and Extreme mode:
         *       1. Yes                       => "Did you open your chest ?" | "Want more challenge ?\nTry this: {0}"
         *       2. No                        => "Regroup all the power players in a single game to defeat the extreme mode"
         *   
         *   D. Finish a game with all powers => "You found all the in-game challenges. Can you find the best single power ?"
         * ************************************************* */

        public static string ForWcfResponse(Data.Player player)
        {
            if (!player.HasFinished)
            {
                return null;
            }

            if (player.HasAllPowers)
            {
                return "You found all the in-game challenges. Can you find the best single power ?";
            }

            if (player.HasPower)
            {
                if (player.Game.Difficulty == Data.Difficulty.Extreme && player.Game.ContainsAllPowerPlayers())
                {
                    return "Did you open your chest ?";
                }

                return "Regroup all the power players in a single game to defeat the extreme mode";
            }

            Data.Player creator = player.Game.GetCreator();
            if (creator == null)
            {
                return null;
            }

            if (!player.IsCreator)
            {
                if (!player.IsGameWinner)
                {
                    return "Only winners will be awarded...";
                }

                switch (player.Game.Difficulty)
                {
                    case Data.Difficulty.Easy: return "It's not fair in easy mode :)";
                    case Data.Difficulty.Medium: return "My secret does not match with medium players...";
                    default: return string.Format("Need to be a super hero ? Try this: {0}", UrlHelper.PowerServiceUrl);
                }
            }

            switch (player.Game.Difficulty)
            {
                case Data.Difficulty.Hard:
                case Data.Difficulty.Extreme:
                    return string.Format("Have you ever tried to beat someone else on his ground ? Try this: {0}", UrlHelper.AllGamesUrl);

                default: return "Want new challenge ? Try to make harder mode...";
            }
        }

        public static string ForWebApiResponse(Data.Game game)
        {
            if (!game.ContainsWinner())
            {
                return "Mum didn't tell you it's not polite to cheat :)";
            }

            if (game.Difficulty == Data.Difficulty.Extreme && game.ContainsAllPowerPlayers())
            {
                return string.Format("Want more challenge ?\nTry this: {0}", UrlHelper.ResolveGameUrl(Configuration.Instance.LastGameKey));
            }

            return null;
        }
    }
}