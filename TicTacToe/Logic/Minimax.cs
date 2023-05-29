using System;

namespace TicTacToe.Logic
{
    public static class Minimax
    {
        public static int GetBestMove(Game game, Player ai)
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            for (int i = 0; i < 9; i++)
            {
                if (game.GetPlayer(i) == Player.Empty)
                {
                    var clone = game.Clone();
                    clone.SetPlayer(i, ai);
                    int score = MinimaxAlgorithm(clone, ai);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i;
                    }
                }
            }

            return bestMove;
        }

        private static int MinimaxAlgorithm(Game game, Player ai, int depth = 0, bool isMaximizing = false)
        {
            game.CheckGameState();
            if (game.State != GameState.Playing)
            {
                return GetScore(game, ai);
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;

                for (int i = 0; i < 9; i++)
                {
                    if (game.GetPlayer(i) == Player.Empty)
                    {
                        var clone = game.Clone();
                        clone.SetPlayer(i, ai);
                        int score = MinimaxAlgorithm(clone, ai, depth + 1, false);
                        bestScore = Math.Max(score, bestScore);
                    }
                }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;

                for (int i = 0; i < 9; i++)
                {
                    if (game.GetPlayer(i) == Player.Empty)
                    {
                        var clone = game.Clone();
                        clone.SetPlayer(i, InvertPlayer(ai));
                        int score = MinimaxAlgorithm(clone, ai, depth + 1, true);
                        bestScore = Math.Min(score, bestScore);
                    }
                }

                return bestScore;
            }
        }

        private static int GetScore(Game game, Player player)
        {
            return game.State switch
            {
                GameState.XWon => player == Player.X ? 1 : -1,
                GameState.OWon => player == Player.O ? 1 : -1,
                GameState.Draw => 0,
                _ => throw new InvalidOperationException(),
            };
        }

        private static Player InvertPlayer(Player player)
        {
            return player switch
            {
                Player.X => Player.O,
                Player.O => Player.X,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
