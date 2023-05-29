using System;

namespace TicTacToe.Logic
{
    public enum Player
    {
        Empty,
        X,
        O
    }

    public enum GameState
    {
        Playing,
        XWon,
        OWon,
        Draw
    }

    public sealed class Game
    {
        private Player[,] board = new Player[3, 3];
        public GameState State = GameState.Playing;

        public Game()
        {
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = Player.Empty;
                }
            }

            State = GameState.Playing;
        }

        public Player GetPlayer(int x, int y)
        {
            return board[x, y];
        }

        public Player GetPlayer(int index)
        {
            int x = index % 3;
            int y = index / 3;
            return GetPlayer(x, y);
        }

        public void SetPlayer(int x, int y, Player player)
        {
            board[x, y] = player;
        }

        public void SetPlayer(int index, Player player)
        {
            int x = index % 3;
            int y = index / 3;
            SetPlayer(x, y, player);
        }

        public bool MakeTurn(int x, int y, Player player)
        {
            if (State != GameState.Playing)
            {
                return false;
            }

            if (board[x, y] != Player.Empty)
            {
                return false;
            }

            board[x, y] = player;

            CheckGameState();

            return true;
        }

        public bool MakeTurn(int index, Player player)
        {
            int x = index % 3;
            int y = index / 3;
            return MakeTurn(x, y, player);
        }

        public GameState GetWinner()
        {
            Player winner = Player.Empty;

            // horizontal
            for (var i = 0; i < 3; i++)
                if (Equals3(board[i, 0], board[i, 1], board[i, 2]))
                    winner = board[i, 0];

            // vertical
            for (var i = 0; i < 3; i++)
                if (Equals3(board[0, i], board[1, i], board[2, i]))
                    winner = board[0, i];

            // diagonal
            if (Equals3(board[0, 0], board[1, 1], board[2, 2]))
                winner = board[0, 0];

            if (Equals3(board[2, 0], board[1, 1], board[0, 2]))
                winner = board[2, 0];

            var openSpots = 0;
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (board[i, j] == Player.Empty)
                    {
                        openSpots++;
                    }
                }
            }

            if (winner == Player.Empty)
            {
                if (openSpots == 0)
                    return GameState.Draw;
                return GameState.Playing;
            }
            else
            {
                if (winner == Player.X)
                    return GameState.XWon;
                return GameState.OWon;
            }
        }

        public GameState CheckGameState()
        {
            var state = GetWinner();
            State = state;
            return state;
        }

        public static bool Equals3(Player a, Player b, Player c)
        {
            return a == b && b == c && a != Player.Empty;
        }

        public Game Clone()
        {
            return new Game
            {
                board = board.Clone() as Player[,] ?? throw new Exception(),
            };
        }
    }
}
