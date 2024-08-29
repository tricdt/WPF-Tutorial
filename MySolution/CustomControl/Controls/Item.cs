using System.ComponentModel;

namespace CustomControl.Controls
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        protected void OnPropertyChanged(string PropertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this,
                     new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
