using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMDbLib.Client;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace MovieLib.Client.TMDBLibHelper
{
    class HttpClientRequest
    {
        #region Contants
        private const string TMDbKey = "a14c3a5a544bc93fc7dafbe6f97f5f7f";
        #endregion

        #region StaticVariable
        static TMDbClient client = new TMDbClient(TMDbKey);
        static UserSession userSession = client.AuthenticationGetUserSessionAsync("rlebranchu", "Imoariis2710_").Result;
        static string FilePathRating = Path.Combine(Path.GetFullPath(@"..\..\"), @"Resources\Json\RatedMovies.txt");
        static string FilePathWatched = Path.Combine(Path.GetFullPath(@"..\..\"), @"Resources\Json\WatchedMovies.txt");
        static string FilePathWishWatch = Path.Combine(Path.GetFullPath(@"..\..\"), @"Resources\Json\WishWatchMovies.txt");
        static string UrlBasePosterPath = "http://image.tmdb.org/t/p/w200/";
        #endregion

        #region StaticMethods
        /// <summary>
        /// Initialise la liste de base des films via la Web Api TmDB à partir d'un filtre texte sur le titre du film, d'une année
        /// </summary>
        /// <param name="search">String : le texte de recherche sur le titre du film</param>
        /// <param name="year">int : l'année du film</param>
        /// <returns>List-Movie- : La liste de films</returns>
        public static List<SearchMovie> GetMoviesBySearchAsync(String search, int year = 0)
        {
            List<SearchMovie> srchMovies = client.SearchMovieAsync(search, year: year).Result.Results;
            return SetListSearchMoviePosterPath(srchMovies);
        }

        /// <summary>
        /// Initialise la liste des films favoris du compte via la Web Api TmDB
        /// </summary>
        /// <returns>List-Movie- : La liste de films</returns>
        public static List<SearchMovie> GetFavoriteMoviesAsync()
        {
            client.SetSessionInformation(userSession.SessionId, SessionType.UserSession);
            return SetListSearchMoviePosterPath(client.AccountGetFavoriteMoviesAsync().Result.Results);
        }

        /// <summary>
        ///     Retourne le film via l'id et via la Web Api TmDB
        /// </summary>
        /// <param name="movieId">int : l'id du film</param>
        /// <returns>Movie : le film</returns>
        public static Movie GetMovie(int movieId)
        {
            client.SetSessionInformation(userSession.SessionId, SessionType.UserSession);
            Movie movie = client.GetMovieAsync(movieId).Result;
            movie.PosterPath = UrlBasePosterPath + movie.PosterPath;
            return movie;
        }

        /// <summary>
        /// Enregistre ou enlève un film comme favoris dans le compte
        /// </summary>
        /// <param name="movie">The movie to add or remove</param>
        /// <param name="isFavorite">Set to favorite or not</param>
        public static void SetMovieMyCollection(int movieId, bool isFavorite)
        {
            bool result = client.AccountChangeFavoriteStatusAsync(MediaType.Movie, movieId, !isFavorite).Result;
        }

        #region RatingMovie
        /// <summary>
        ///     Noter le film
        /// </summary>
        /// <param name="movie">The movie rated</param>
        /// <param name="rating">The note</param>
        public static void SetRatingMovie(int movieId, int rating)
        {
            Dictionary<int,int> list = JsonConvert.DeserializeObject<Dictionary<int, int>>(File.ReadAllText(FilePathRating));
            list = (list ?? new Dictionary<int, int>());
            if (list.TryGetValue(movieId, out _))
            {
                list[movieId] = rating;
            }
            else{
                list.Add(movieId, rating);
            }
            File.WriteAllText(FilePathRating, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        /// <summary>
        ///     Récupérer la note du film
        /// </summary>
        /// <param name="movie">Le film à rechercher</param>
        /// <returns>int : la note personnel du site</returns>
        public static int GetRatingNote(SearchMovie movie)
        {
            Dictionary<int, int> list = JsonConvert.DeserializeObject<Dictionary<int, int>>(File.ReadAllText(FilePathRating));
            return (list == null 
                            ? 0 
                            : (list.TryGetValue(movie.Id, out _) 
                                                    ? list[movie.Id] 
                                                    : 0)
                   );
        }
        #endregion

        #region WatchedMovies
        /// <summary>
        ///     Indiquer si l'utilisateur à vu le film ou non
        /// </summary>
        /// <param name="movieId">L'id du film</param>
        /// <param name="watched">Film vu ou non</param>
        public static void SetWatchedMovie(int movieId, bool watched)
        {
            List<int> list = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(FilePathWatched));
            list = (list ?? new List<int>());
            if (list.Contains(movieId))
            {
                if (!watched) list.Remove(movieId);
            }
            else
            {
                if (watched) list.Add(movieId);
            }
            File.WriteAllText(FilePathWatched, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        /// <summary>
        ///     Récupérer un booléen indiquant si le film a été marqué comme vu ou non
        /// </summary>
        /// <param name="movie">Le film à rechercher</param>
        /// <returns>Bool : Le film est vu ou non</returns>
        public static bool GetWatchedMovies(SearchMovie movie)
        {
            List<int> list = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(FilePathWatched));
            return (list != null ? list.Contains(movie.Id) : false);
        }
        #endregion

        #region WishWatchMovies
        /// <summary>
        ///     Indiquer si l'utilisateur souhaiter voir le film ou non
        /// </summary>
        /// <param name="movieId">L'id du film</param>
        /// <param name="wish">Bool : Souhait de visionnage</param>
        public static void SetWishWatchMovie(int movieId, bool wish)
        {
            List<int> list = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(FilePathWishWatch));
            list = (list ?? new List<int>());
            if (list.Contains(movieId))
            {
                if (!wish) list.Remove(movieId);
            }
            else
            {
                if (wish) list.Add(movieId);
            }
            File.WriteAllText(FilePathWishWatch, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        /// <summary>
        ///     Récupérer un booléen indiquant si l'utilisateur souhaite voir le film ou non
        /// </summary>
        /// <param name="movie">Le film à rechercher</param>
        /// <returns>Bool : Souhait de visionnage</returns>
        public static bool GetWishWatchMovies(SearchMovie movie)
        {
            List<int> list = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(FilePathWishWatch));
            return (list != null ? list.Contains(movie.Id) : false);
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">La liste des SearchMovies</param>
        /// <returns>List-SearchMovies- : formattage de l'url du poster</returns>
        private static List<SearchMovie> SetListSearchMoviePosterPath(List<SearchMovie> list)
        {
            if (list != null)
            {
                foreach (SearchMovie mv in list)
                {
                    mv.PosterPath = UrlBasePosterPath + mv.PosterPath;
                }
            }
            return list;
        }        
        #endregion
    }
}