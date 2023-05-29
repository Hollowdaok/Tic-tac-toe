using System.ComponentModel;
using TicTacToe.Logic;

namespace TicTacToe.UI
{
    public sealed class ViewModel : INotifyPropertyChanged
    {
        private Game game = new();
        private string statusText = "X's turn";

        public ViewModel()
        {
            ResetCommand = new RelayCommand((_) => Reset());
            CellClickCommand = new RelayCommand((index) =>
            {
                string indexString = index as string;
                CellClick(int.Parse(indexString));
            });
        }

        public RelayCommand ResetCommand { get; private set; }
        public RelayCommand CellClickCommand { get; private set; }

        public char[] PlayerIcons
        {
            get
            {
                var array = new char[9];
                for (int i = 0; i < 9; i++)
                {
                    var x = i % 3;
                    var y = i / 3;
                    var value = game.GetPlayer(x, y);
                    if (value == Player.Empty) array[i] = ' ';
                    else if (value == Player.X) array[i] = 'X';
                    else array[i] = 'O';
                }
                return array;
            }
        }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        private void Reset()
        {
            game.Reset();
            StatusText = "X's turn";
            OnPropertyChanged(nameof(PlayerIcons));
        }

        private void CellClick(int index)
        {
            int x = index % 3;
            int y = index / 3;
            Player player = Player.X;
            Player ai = Player.O;

            if (game.MakeTurn(x, y, player))
            {
                var aiChoice = Minimax.GetBestMove(game, ai);
                game.MakeTurn(aiChoice, ai);

                switch (game.State)
                {
                    case GameState.Playing:
                        StatusText = "Playing";
                        break;
                    case GameState.XWon:
                        StatusText = "X won!";
                        break;
                    case GameState.OWon:
                        StatusText = "O won!";
                        break;
                    case GameState.Draw:
                        StatusText = "Draw!";
                        break;
                }

                OnPropertyChanged(nameof(PlayerIcons));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
