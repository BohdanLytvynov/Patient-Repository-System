using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModelBaseLib.VM
{

    public abstract class ViewModelBaseClass : INotifyPropertyChanged, IDataErrorInfo
    {
        protected bool[] m_ValidationArray;

        public virtual string this[string columnName] => throw new NotImplementedException();

        [JsonIgnore]
        public virtual string Error { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            var temp = Volatile.Read(ref PropertyChanged);

            temp?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public bool Set<T>(ref T field, T value, string prop)
        {
            if (field == null)
            {
                return false;
            }

            if (field.Equals(value))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(prop);
                return true;
            }
        }

        protected virtual bool CheckValidArray(int start, int end)
        {
            for(int i = start; i < end; i ++)
            {
                if (m_ValidationArray[i] == false)
                {
                    return false;
                }
            }

            return true;
        }

    }
}

