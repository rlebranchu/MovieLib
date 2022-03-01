using MovieLib.Client.ViewModels;
using System;
using System.Windows.Controls;

namespace MovieLib.Client.Views
{
    /// <summary>
    /// Logique d'interaction pour LibrairyPage.xaml
    /// </summary>
    public partial class LibrairyPage : UserControl
    {
        public LibrairyPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Listener de la coche d'un film Vu ou Non : si le fuilm est vu, on enleve le film de la liste de souhait
        /// </summary>
        /// <param name="sender">La checkbox</param>
        /// <param name="e">L'event</param>
        public void CheckBoxWishWatch_Clicked(object sender, EventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                ((MainViewModel)DataContext).LibrairyViewModel.SetWishWatchedMovie();
            }
        }

        /// <summary>
        ///     Listener du bouton Youtube : rechercher à partir du nom
        /// </summary>
        /// <param name="sender">La checkbox</param>
        /// <param name="e">L'event</param>
        public void YoutubeButton_Clicked(object sender, EventArgs e)
        {
            ((MainViewModel)DataContext).LibrairyViewModel.SearchOnYoutube();
        }
    }
}
