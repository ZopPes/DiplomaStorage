using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DiplomaStorage.UserControls;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage
{
    public class DataContextVM : DiplomaStorageDataContext, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string Property = null)
        {
            if (Equals(field, value) || value == null) return false;

            field = value;
            OnPropertyChanged(Property);
            return true;
        }


        #region DiplomaTabs

        private ObservableCollection<Tab> diplomaTabs;

        /// <summary>список вкладок для диплома</summary>
        public ObservableCollection<Tab> DiplomaTabs { get => diplomaTabs; set => Set(ref diplomaTabs, value); }

        #endregion DiplomaTabs
        #region GroupVisibleControl

        private TableControlTest<Group> groupControl;

        /// <summary>обработчик окна групп</summary>
        public TableControlTest<Group> GroupControl { get => groupControl; set => Set(ref groupControl, value); }

        #endregion AddGroupVisibleControl

        

        /// <summary>
        /// добавление новой вкладки
        /// </summary>
        public lamdaCommand<Group> AddTab { get; private set; }

        /// <summary>
        /// Открытие выбранного диплома
        /// </summary>
        public lamdaCommand<Diploms> OpenDiploma { get; private set; }


        public DataContextVM() 
        {

            GroupControl = new TableControlTest<Group>
                (
                OnAddGroup
                );


            OpenDiploma = new lamdaCommand<Diploms>(OnOpenDiploma);

            DiplomaTabs = new ObservableCollection<Tab>();
            DiplomaTabs.CollectionChanged += Tabs_CollectionChanged;

            AddTab = new lamdaCommand<Group>(OnAddTab);

        }

        private void OnAddGroup(Group obj)
        {
            Group.InsertOnSubmit(obj);
            this.SubmitChanges();
        }

        private void OnAddTab(Group obj)
        {
            TabDiploma tabDiploma = new TabDiploma
                (
                obj.Student.SelectMany(s=>s.Diploms)
                    , 
                    OpenDiploma
                )
            {
                name = obj.Fnumber
            };

            DiplomaTabs.Add(tabDiploma);
        }



        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (Tab item in e.NewItems)
                    {
                        item.CloseRequested += OnCloseTab;
                    }
                    break;
            }
        }

        private void OnCloseTab(object sender, EventArgs e)
        {
            Tab tab = (Tab)sender;
            tab.CloseRequested -= OnCloseTab;
            DiplomaTabs.Remove(tab);
        }


        private void OnOpenDiploma(Diploms obj)
        {
            if (!Directory.Exists(@"data\" + obj.id)) Directory.CreateDirectory(@"data\" + obj.id);
            if (obj.annotation != null) File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.annotation) + "." + obj.annot_type, obj.annotation.ToArray());
            if (obj.statement != null) File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.statement) + "." + obj.state_type, obj.statement.ToArray());
            File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.documentation) + "." + obj.doc_type, obj.documentation.ToArray());
            File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.data) + ".rar", obj.statement.ToArray());
            Process.Start(@"data\" + obj.id);
        }
    }
}
