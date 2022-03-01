using MovieLib.MVVM;

namespace MovieLib.Client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        #region Fields

        /// <summary>
        ///     Vue-modèle pour les libraries de films.
        /// </summary>
        private LibrairyViewModel _LibrairyViewModel;

        /// <summary>
        ///     Vue-modèle pour les options.
        /// </summary>
        private OptionViewModel _OptionViewModel;

        #endregion

        #region Properties

        /// <summary>
        ///     Obtient le vue-modèle pour les options.
        /// </summary>
        public LibrairyViewModel LibrairyViewModel { get => _LibrairyViewModel; private set => SetProperty(nameof(LibrairyViewModel), ref _LibrairyViewModel, value); }

        /// <summary>
        ///     Obtient le vue-modèle pour les libraries de films.
        /// </summary>
        public OptionViewModel OptionViewModel { get => _OptionViewModel; private set => SetProperty(nameof(OptionViewModel), ref _OptionViewModel, value); }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="ViewModelMain"/>.
        /// </summary>
        public MainViewModel()
        {
            LibrairyViewModel = new LibrairyViewModel();
            OptionViewModel = new OptionViewModel();
        }

        #endregion
    }
}
