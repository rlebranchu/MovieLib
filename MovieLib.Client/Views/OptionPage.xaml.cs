using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MovieLib.Client.Views
{
    /// <summary>
    /// Logique d'interaction pour OptionPage.xaml
    /// </summary>
    public partial class OptionPage : UserControl
    {
        public OptionPage()
        {
            InitializeComponent();

            // Définition de la liste de couleurs
            ColorSwitchComboBox.ItemsSource = new List<string>()
            {
                "Red"
                ,"Green"
                ,"Blue"
                ,"Purple"
                ,"Orange"
                ,"Lime"
                ,"Emerald"
                ,"Teal"
                ,"Cyan"
                ,"Cobalt"
                ,"Indigo"
                ,"Violet"
                ,"Pink"
                ,"Magenta"
                ,"Crimson"
                ,"Amber"
                ,"Yellow"
                ,"Brown"
                ,"Olive"
                ,"Steel"
                ,"Mauve"
                ,"Taupe"
                ,"Sienna"
            };
            ColorSwitchComboBox.SelectedItem = "Teal";
        }

        /// <summary>
        ///     Listener permettant de passer d'un mode sombre à un mode clair et inversement
        /// </summary>
        /// <param name="sender">Le ToogleSwitch</param>
        /// <param name="e">L'Event</param>
        private void ThemeSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            if (ThemeSwitch.IsChecked == false)
            {
                ThemeManager.ChangeAppTheme(App.Current, "BaseLight");
            }
            else
            {
                ThemeManager.ChangeAppTheme(App.Current, "BaseDark");
            }
        }

        /// <summary>
        ///     Listener permettant de changer la couleur principale de l'application
        /// </summary>
        /// <param name="sender">La Combobox de couleur</param>
        /// <param name="e">L'event de selection</param>
        private void ColorSwitchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(
                App.Current,
                ThemeManager.GetAccent(ColorSwitchComboBox.SelectedItem as string),
                ThemeManager.DetectAppStyle().Item1
            );
        }
    }
}
