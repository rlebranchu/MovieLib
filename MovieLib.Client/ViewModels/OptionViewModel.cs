using MovieLib.MVVM;

namespace MovieLib.Client.ViewModels
{
    public class OptionViewModel : ObservableObject
    {
        #region Fields
        /// <summary>
        ///     Taille d'affichage des poster dans les librairies de film
        /// </summary>
        private int _PosterSize;
        #endregion

        #region Properties
        /// <summary>
        ///     Accessseurs à la taille des poster de films
        /// </summary>
        public int PosterSize { get => _PosterSize; set => SetProperty(nameof(PosterSize), ref _PosterSize, value); }
        #endregion

        #region Constructors
        public OptionViewModel()
        {
            PosterSize = 170;    // Taille des poster par défaut
        }
        #endregion
    }
}
