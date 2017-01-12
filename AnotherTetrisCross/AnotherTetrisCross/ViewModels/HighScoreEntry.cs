namespace AnotherTetrisCross.ViewModels
{
    using System;

    public class HighScoreEntry
    {
        public HighScoreEntry()
        {
            this.Name = String.Empty;
            this.Position = 0;
            this.Score = 0;
        }

        public String Name { get; set; }
        public int Position { get; set; }
        public int Score { get; set; }
    }
}
