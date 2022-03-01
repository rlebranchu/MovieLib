using System.ComponentModel;

namespace MovieLib.MVVM
{
    /// <summary>
    ///     Classe qui implémente un système de notification sur le changement de valeur des propriétés.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Events

        /// <summary>
        ///     Se produit en cas de modification d'une valeur de propriété.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Se produit lorsqu'une propriété s'apprète à changer de valeur.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Methods

        /// <summary>
        ///     Déclenche l'événement <see cref="PropertyChanging"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui va changer.</param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        ///     Déclenche l'événement <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changé.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Assigne une propriété et déclenche les événements <see cref="PropertyChanging"/> et <see cref="PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="T">Type de la propriété.</typeparam>
        /// <param name="propertyName">Nom de la propriété.</param>
        /// <param name="field">Référence du champ à assigner.</param>
        /// <param name="value">Valeur à assigner.</param>
        protected void SetProperty<T>(string propertyName, ref T field, T value)
        {
            if ((field == null && value != null) || field?.Equals(value) == false)
            {
                OnPropertyChanging(propertyName);
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}