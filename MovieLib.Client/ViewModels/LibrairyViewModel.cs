using MovieLib.Client.TMDBLibHelper;
using MovieLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace MovieLib.Client.ViewModels
{
    public class LibrairyViewModel : ObservableObject
    {
        #region Fields
        /// <summary>
        ///     Liste sur laquelle va s'appuyer la page LibrairyPage pour afficher sa liste de films
        /// </summary>
        private ObservableCollection<SearchMovie> _ListMovies;

        /// <summary>
        ///     Liste sur laquelle va s'appuyer la page l'onglet Ma Collection pour afficher la liste de films favories
        /// </summary>
        private ObservableCollection<SearchMovie> _ListMyCollectionMovies;

        /// <summary>
        ///     Liste sur laquelle va s'appuyer le splitButton pour afficher les années (mininum -> année du tout premier film de l'histoire : 1888 / max : l'année actuelle)
        /// </summary>
        private List<int> _ListYears;

        /// <summary>
        ///     Terme de la recherche.
        /// </summary>
        private string _SearchTitle;

        /// <summary>
        ///     Année de recherche.
        /// </summary>
        private int _SearchYear;

        /// <summary>
        ///     Film sélectionné dans la liste pour voir la description du film.
        /// </summary>
        private SearchMovie _SelectedSearchMovie;

        /// <summary>
        ///     Film affiché dans le flyout pour voir la description du film.
        /// </summary>
        private Movie _SelectedMovie;

        /// <summary>
        ///     Note votée pour le film en cours
        /// </summary>
        private int _RatingNote;

        /// <summary>
        ///     Le film est vu ou non
        /// </summary>
        private bool _WatchedMovie;

        /// <summary>
        ///     L'utilisateur souhaite voir le film
        /// </summary>
        private bool _WishWatchMovie;

        /// <summary>
        ///     Commande pour gérer la recherche de films
        /// </summary>
        private RelayCommand _SearchMoviesCommand;

        /// <summary>
        ///     Commande pour ajouter ou enlever un film de sa collection
        /// </summary>
        private RelayCommand _SetMovieMyCollectionCommand;
        #endregion

        #region Properties
        /// <summary>
        ///     Accessseurs à la liste de films
        /// </summary>
        public ObservableCollection<SearchMovie> ListMovies { get => _ListMovies; private set => SetProperty(nameof(ListMovies), ref _ListMovies, value); }

        /// <summary>
        ///     Accessseurs à la liste de ma collection
        /// </summary>
        public ObservableCollection<SearchMovie> ListMyCollectionMovies { get => _ListMyCollectionMovies; private set => SetProperty(nameof(ListMyCollectionMovies), ref _ListMyCollectionMovies, value); }

        /// <summary>
        ///     Getter à la liste d'année
        /// </summary>
        public List<int> ListYears { get => _ListYears; }

        /// <summary>
        ///     Obtient ou définit le terme de la recherche.
        /// </summary>
        public string SearchTitle { get => _SearchTitle; set => SetProperty(nameof(SearchTitle), ref _SearchTitle, value); }

        /// <summary>
        ///     Obtient ou définit la date de la recherche.
        /// </summary>
        public int SearchYear
        {
            get => _SearchYear;
            set
            {
                SetProperty(nameof(SearchYear), ref _SearchYear, value);
                SearchMoviesCommand.Execute(value);
            }
        }

        /// <summary>
        ///     Obtient le film sélectionné dans la liste de film
        /// </summary>
        public SearchMovie SelectedSearchMovie
        {
            get => _SelectedSearchMovie;
            set
            {
                SetProperty(nameof(SelectedSearchMovie), ref _SelectedSearchMovie, value);
                if(value != null)
                {
                    SelectedMovie = HttpClientRequest.GetMovie(((SearchMovie)value).Id);
                    RatingNote = HttpClientRequest.GetRatingNote(((SearchMovie)value));
                    WatchedMovie = HttpClientRequest.GetWatchedMovies(((SearchMovie)value));
                    WishWatchMovie = HttpClientRequest.GetWishWatchMovies(((SearchMovie)value));
                }
            }
        }

        /// <summary>
        ///     Obtient le film affiché dans le flyout
        /// </summary>
        public Movie SelectedMovie { get => _SelectedMovie; set => SetProperty(nameof(SelectedMovie), ref _SelectedMovie, value); }

        /// <summary>
        ///     Obtient la note personnelle du film en cours
        /// </summary>
        public int RatingNote
        {
            get => _RatingNote;
            set
            {
                SetProperty(nameof(RatingNote), ref _RatingNote, value);
                HttpClientRequest.SetRatingMovie(_SelectedMovie.Id, value);
            }
        }

        /// <summary>
        ///     Obtient l'indication si le film a été vu ou non
        /// </summary>
        public bool WatchedMovie
        {
            get => _WatchedMovie;
            set
            {
                SetProperty(nameof(WatchedMovie), ref _WatchedMovie, value);
                HttpClientRequest.SetWatchedMovie(_SelectedMovie.Id, value);
            }
        }

        /// <summary>
        ///     Obtient l'indication si l'utilisateur souhaite voir le film en cours
        /// </summary>
        public bool WishWatchMovie
        {
            get => _WishWatchMovie;
            set
            {
                SetProperty(nameof(WishWatchMovie), ref _WishWatchMovie, value);
                HttpClientRequest.SetWishWatchMovie(_SelectedMovie.Id, value);
            }
        }

        /// <summary>
        ///     Obtient la commande pour recherche des films
        /// </summary>
        public RelayCommand SearchMoviesCommand => _SearchMoviesCommand;

        /// <summary>
        ///     Obtient la commande pour ajouter ou enlever des films de la collection
        /// </summary>
        public RelayCommand SetMovieMyCollectionCommand => _SetMovieMyCollectionCommand;
        #endregion

        #region Constructor
        public LibrairyViewModel()
        {
            _ListMovies = new ObservableCollection<SearchMovie>(HttpClientRequest.GetMoviesBySearchAsync("Harry Potter"));
            _ListMyCollectionMovies = new ObservableCollection<SearchMovie>(HttpClientRequest.GetFavoriteMoviesAsync());
            _ListYears = new List<int>();
            _ListYears.Add(0);
            for (int i = DateTime.Now.Year; i >= 1888; i--)
            {
                _ListYears.Add(i);
            }
            _SearchMoviesCommand = new RelayCommand(SearchMoviesCommandExecute);
            _SetMovieMyCollectionCommand = new RelayCommand(SetMovieMyCollectionCommandExecute);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Enlever de la liste de souhait lorsque le film est vu
        /// </summary>
        public void SetWishWatchedMovie()
        {
            if (_WishWatchMovie)
            {
                WishWatchMovie = false;
            }
        }

        /// <summary>
        ///     Rechercher sur Youtube via le titrez du film
        /// </summary>
        public void SearchOnYoutube()
        {
            if(_SelectedMovie != null)
            {
                System.Diagnostics.Process.Start("https://www.youtube.com/results?search_query=" + _SelectedMovie.Title);
            }
        }

        #region SearchMoviesCommandExecute
        /// <summary>
        ///     Méthode d'exécution de la commande <see cref="SearchMoviesCommand"/>.
        /// </summary>
        /// <param name="commandParameter">-</param>
        protected virtual void SearchMoviesCommandExecute(object commandParameter)
        {
            ListMovies = new ObservableCollection<SearchMovie>(HttpClientRequest.GetMoviesBySearchAsync(
                                        (String.IsNullOrWhiteSpace(_SearchTitle) ? "Harry Potter" : _SearchTitle)
                                        , _SearchYear)
            );

            ListMyCollectionMovies = new ObservableCollection<SearchMovie>(HttpClientRequest.GetFavoriteMoviesAsync().Where(
                sm => (String.IsNullOrWhiteSpace(_SearchTitle)) ? true : sm.Title.Contains(_SearchTitle)
            ));

            if (String.IsNullOrWhiteSpace(_SearchTitle) && _SearchYear != 0)
            {
                ListMyCollectionMovies = new ObservableCollection<SearchMovie>(ListMyCollectionMovies.Where(
                    sm => (sm.ReleaseDate != null ? sm.ReleaseDate.Value.Year == _SearchYear : false)
            ));
            }
        }
        #endregion

        #region SetMovieMyCollectionCommandExecute
        /// <summary>
        ///     Méthode d'exécution de la commande <see cref="SetMovieMyCollectionCommand"/>.
        /// </summary>
        /// <param name="commandParameter">The movie stared or unstared</param>
        protected virtual void SetMovieMyCollectionCommandExecute(object commandParameter)
        {
            if (commandParameter is SearchMovie sm)
            {
                HttpClientRequest.SetMovieMyCollection(sm.Id, ListMyCollectionMovies.Any(item => item.Id == sm.Id));
                ListMyCollectionMovies = new ObservableCollection<SearchMovie>(HttpClientRequest.GetFavoriteMoviesAsync());
            }
        }
        #endregion
        #endregion
    }
}
