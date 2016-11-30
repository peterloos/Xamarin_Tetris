using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTetrisCross.Settings
{
    public static class SettingsManagement
    {
        private const String SettingsKeyHighScores = "settings_key_high_scores";
        private static readonly String SettingsKeyHighScoresDefault = "[]";

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static String AllHighScores
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKeyHighScores, SettingsKeyHighScoresDefault);
            }

            set
            {
                AppSettings.AddOrUpdateValue<String>(SettingsKeyHighScores, value);
            }
        }
    }
}
