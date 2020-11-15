using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DiplomaStorage
{
    class DataContextVM : DiplomaStorageDataContext, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string Property = null)
        {
            if (!Equals(field, value) || value == null) return false;

            field = value;
            OnPropertyChanged();
            return true;
        }
    }
}
