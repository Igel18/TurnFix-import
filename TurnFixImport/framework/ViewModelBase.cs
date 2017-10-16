namespace TurnFixImport.framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Diagnostics;
    using System.Collections.Concurrent;

    // http://stackoverflow.com/questions/19507745/how-to-get-rid-of-repetitive-properties-in-wpf-mvvm-viewmodels

    [Serializable]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private ConcurrentDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

        protected T GetValue<T>(Expression<Func<T>> propertyExpresssion)
        {
            if (propertyExpresssion == null)
            {
                throw new ArgumentNullException("propertyExpression not set!");
            }

            var name = PropertyHelper.ExtractPropertyName(propertyExpresssion);

            return getValue<T>(name);
        }

        protected void SetValue<T>(Expression<Func<T>> propertyExpresssion, T value)
        {
            if (propertyExpresssion == null)
            {
                throw new ArgumentNullException("propertyExpression not set!");
            }

            var name = PropertyHelper.ExtractPropertyName(propertyExpresssion);

            var storedValue = getValue<T>(name);

            if (object.Equals(storedValue, value))
            {
                return;
            }

            _properties[name] = value;

            RaisePropertyChanged(name);
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = PropertyHelper.ExtractPropertyName(propertyExpresssion);
            this.RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private T getValue<T>(string name)
        {
            object obj;

            if (_properties.TryGetValue(name, out obj))
            {
                return (T)_properties[name];
            }

            return default(T);
        }
    }
}
