using System.ComponentModel;

namespace Common.Binding
{
    /// <summary>
    /// Предоставляет базовый класс, реализующий интерфейс INotifyPropertyChanged
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Возникает при изменении свойства объекта
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Вызывает событие PropertyChanged с указанным именем свойства 
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}