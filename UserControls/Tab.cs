using System;
using WPFMVVMHelper;

namespace DiplomaStorage.UserControls
{
    public abstract class Tab : peremlog
    {
        /// <summary>
        /// класс для кправления вкладками приложения
        /// </summary>
        public Tab()
        {
            CloseCommand = new lamdaCommand(() => CloseRequested?.Invoke(this, EventArgs.Empty));
        }

        /// <summary>
        /// название вкладки
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// комманда для закрытия вкладки
        /// </summary>
        public lamdaCommand CloseCommand { get; }

        /// <summary>
        /// событие какое-то
        /// </summary>
        public event EventHandler CloseRequested;
    }
}