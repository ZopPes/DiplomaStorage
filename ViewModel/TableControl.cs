using System.Windows;
using WPFMVVMHelper;

namespace DiplomaStorage
{
    public class TableControl<T> : peremlog where T : new()
    {
        #region New

        private T newprop;

        /// <summary>новая переменная</summary>
        public T New { get => newprop; set => Set(ref newprop, value); }

        #endregion New

        #region VisibletiControl

        private Visibility visibility;

        /// <summary>Отображение окна</summary>
        public Visibility VisibletiControl { get => visibility; set => Set(ref visibility, value); }

        #endregion VisibletiControl

        public lamdaCommand Visible { get; }
        public lamdaCommand Collapsed { get; }

        public TableControl()
        {
            VisibletiControl = Visibility.Collapsed;
            Visible = new lamdaCommand(() => VisibletiControl = Visibility.Visible);
            Collapsed = new lamdaCommand(() => VisibletiControl = Visibility.Collapsed);
            New = new T();
        }

        public TableControl(T @new)
        {
            VisibletiControl = Visibility.Collapsed;
            Visible = new lamdaCommand(() => VisibletiControl = Visibility.Visible);
            Collapsed = new lamdaCommand(() => VisibletiControl = Visibility.Collapsed);
            New = @new;
        }
    }
}