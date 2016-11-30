namespace AnotherTetrisCross.ViewModels
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class BoardCellModel : ViewModelBase
    {
        private TetrisViewPageModel model;
        private Color cellColor;

        public BoardCellModel(TetrisViewPageModel model, Color cellColor)
        {
            this.model = model;
            this.cellColor = cellColor;
        }

        public Color CellColor
        {
            get
            {
                return this.cellColor;
            }

            set
            {
                if (this.cellColor != value)
                {
                    this.cellColor = value;
                    this.RaisePropertyChanged(() => CellColor);
                }
            }
        }

        public ICommand TapCommand
        {
            get
            {
                return new Command<String>((key) =>
                {
                    this.model.TapCommand.Execute(key);
                });
            }
        }
    }
}
