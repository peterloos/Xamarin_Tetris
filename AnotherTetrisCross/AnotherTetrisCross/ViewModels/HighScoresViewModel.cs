namespace AnotherTetrisCross.ViewModels
{
    using AnotherTetrisCross.Settings;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Views;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class HighScoresViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        private ObservableCollection<HighScoreEntry> highScorers;

        private String nameOfPlayer;
        private int lastScore;

        public HighScoresViewModel(INavigationService navigationService)
        {
            if (navigationService == null)
                throw new ArgumentNullException("NavigationService not provided");

            Debug.WriteLine("==============================> HighScoresViewModel c'tor");

            this.navigationService = navigationService;

            this.nameOfPlayer = String.Empty;
            this.lastScore = -1;

            this.ReadHighScoreListFromSettings();
        }

        public int LastScore
        {
            get
            {
                Debug.WriteLine("get LastScore");
                return this.lastScore;
            }

            set
            {
                Debug.WriteLine("set LastScore");

                if (this.lastScore != value)
                {
                    this.lastScore = value;
                    this.RaisePropertyChanged(() => LastScore);
                }
            }
        }

        public String NameOfPlayer
        {
            get
            {
                Debug.WriteLine("get NameOfPlayer");
                return this.nameOfPlayer;
            }

            set
            {
                Debug.WriteLine("set NameOfPlayer");

                if (this.nameOfPlayer != value)
                {
                    this.nameOfPlayer = value;
                    this.RaisePropertyChanged(() => NameOfPlayer);
                }
            }
        }

        public ICommand NavigateCommand
        {
            get
            {
                return new Command(() => {
                    this.SaveHighScoreListToSettings();
                    this.navigationService.GoBack();
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new Command(() => {
                    this.UpdateHighScoreList(this.NameOfPlayer, this.LastScore);
                });
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new Command(() => {
                    this.NameOfPlayer = String.Empty;
                });
            }
        }

        public ObservableCollection<HighScoreEntry> HighScorers
        {
            get
            {
                Debug.WriteLine("get HighScorers: {0} elements ...", this.highScorers.Count);
                return this.highScorers;
            }
        }

        // private helper methods
        private void SaveHighScoreListToSettings()
        {
            List<HighScoreEntry> list = this.highScorers.ToList<HighScoreEntry>();
            String jsonList = JsonConvert.SerializeObject(list);
            SettingsManagement.AllHighScores = jsonList;
            Debug.WriteLine("Saved: {0}", jsonList);
        }

        private void ReadHighScoreListFromSettings()
        {
            String settingsJson = SettingsManagement.AllHighScores;
            Debug.WriteLine("Read: {0}", settingsJson);
            List<HighScoreEntry> list = JsonConvert.DeserializeObject<List<HighScoreEntry>>(settingsJson);

            // sort list
            List<HighScoreEntry> sortedList = list.OrderBy(key => key.Score).ToList();

            // create observable collection
            int position = 1;
            this.highScorers = new ObservableCollection<HighScoreEntry>();
            foreach (HighScoreEntry entry in sortedList)
            {
                entry.Position = position;
                this.highScorers.Add(entry);
                position++;
            }
        }

        private void UpdateHighScoreList(String name, int score)
        {
            HighScoreEntry newEntry = new HighScoreEntry() { Name = name, Score = score };

            // add new entry to high score list
            List<HighScoreEntry> list = this.highScorers.ToList<HighScoreEntry>();
            list.Add(newEntry);

            // sort list
            List<HighScoreEntry> sortedList = list.OrderBy(key => key.Score).ToList();

            // create updated observable collection in sorted order
            int position = sortedList.Count;
            this.highScorers.Clear();
            foreach (HighScoreEntry entry in sortedList)
            {
                entry.Position = position;
                this.highScorers.Insert(0, entry);
                position--;
            }
        }
    }
}
