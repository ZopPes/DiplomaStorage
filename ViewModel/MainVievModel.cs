using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DiplomaStorage.UserControls;
using Microsoft.Win32;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage
{
    public class MainVievModel : peremlog
    {
        public DiplomaStorageDataContext DataContext { get; set; }

        private readonly ObservableCollection<Tab> tabs;//список вклабок

        public ICollection<Tab> Tabs { get; }

        #region Teacher

        private ObservableCollection<Teacher> teacher;

        /// <summary>список преподователей</summary>
        public ObservableCollection<Teacher> Teacher { get => teacher; set => Set(ref teacher, value); }

        #endregion Teacher

        #region Students

        private ObservableCollection<Student> students;

        /// <summary>Список студентов</summary>
        public ObservableCollection<Student> Students { get => students; set => Set(ref students, value); }

        #endregion Students

        #region BlurEffect

        private int blurEffect;

        /// <summary>Размытие фона</summary>
        public int BlurEffect { get => blurEffect; set => Set(ref blurEffect, value); }

        #endregion BlurEffect

        #region AuthorizVisible

        private Visibility visibility;

        /// <summary>Отображение формы авторизации</summary>
        public Visibility AuthorizVisible { get => visibility; set => Set(ref visibility, value); }

        #endregion AuthorizVisible

        #region NewDiploma

        private Diploms newDiploma;

        /// <summary>переменная для хранения данных о новом дипломе</summary>
        public Diploms NewDiploma { get => newDiploma; set => Set(ref newDiploma, value); }

        #endregion NewDiploma

        #region NewTeacher

        private Teacher newTeacher;

        /// <summary>переменная для хранения данных о новом дипломе</summary>
        public Teacher NewTeacher { get => newTeacher; set => Set(ref newTeacher, value); }

        #endregion NewTeacher

        #region VievDiploma

        private ObservableCollection<Diploms> viewDiploms;

        /// <summary>Колекция дипломов</summary>
        public ObservableCollection<Diploms> ViewDiploms { get => viewDiploms; set => Set(ref viewDiploms, value); }

        #endregion VievDiploma

        #region AddDiplomaVisible

        private Visibility Addvisibility;

        /// <summary>Отображение окна добавления диплома</summary>
        public Visibility AddDiplomaVisible { get => Addvisibility; set => Set(ref Addvisibility, value); }

        #endregion AddDiplomaVisible

        #region AddTeacherVisible

        private Visibility AddvisibilityTeacher;

        /// <summary>Отображение окна добавления диплома</summary>
        public Visibility AddTeacherVisible { get => AddvisibilityTeacher; set => Set(ref AddvisibilityTeacher, value); }

        #endregion AddTeacherVisible

        #region EnableMain

        private bool enablemain;

        /// <summary>включение и выключение главной фого окна</summary>
        public bool EnableMain { get => enablemain; set => Set(ref enablemain, value); }

        #endregion EnableMain

        #region AddStudentVisible

        private Visibility AddvisibilityStudent;

        /// <summary>Отображение окна добавления диплома</summary>
        public Visibility AddStudentVisible { get => AddvisibilityStudent; set => Set(ref AddvisibilityStudent, value); }

        #endregion AddStudentVisible

        #region AddGroupVisible

        private Visibility AddvisibilityGroup;

        /// <summary>Отображение окна добавления диплома</summary>
        public Visibility AddGroupVisible { get => AddvisibilityGroup; set => Set(ref AddvisibilityGroup, value); }

        #endregion AddGroupVisible

        public IEnumerable Groups
        {
            get => DataContext.Group.GroupBy(i => i.date, (i, g) => new { k = i, G = g });
        }

        public OpenFileDialog fileDialog;

        #region NewStudent

        private Student newStudent;

        /// <summary>Шаблон для новой группы</summary>
        public Student NewStudent
        {
            get => newStudent;
            set => Set(ref newStudent, value);
        }

        #endregion NewStudent

        #region NewGroup

        private Group newGroup;

        /// <summary>Шаблон для новой группы</summary>
        public Group NewGroup { get => newGroup; set => Set(ref newGroup, value); }

        #endregion NewGroup

        #region SelectedGroup

        private Group sGroup;

        /// <summary>Выбранная группа</summary>
        public Group SelectedGroup { get => sGroup; set => Set(ref sGroup, value); }

        #endregion SelectedGroup

        #region Command

        public lamdaCommand AddGroup { get; }
        public lamdaCommand AddStudent { get; }
        public lamdaCommand AddTeacher { get; }

        public lamdaCommand HideGroup { get; }

        public lamdaCommand VisibleDiploma { get; }
        public lamdaCommand VisibleGroup { get; }
        public lamdaCommand VisibleStudent { get; }
        public lamdaCommand VisibleTeacher { get; }

        public lamdaCommand AddDiploma { get; }
        public lamdaCommand AddDiplomaData { get; }
        public lamdaCommand AddDiplomaDocumentation { get; }
        public lamdaCommand AddDiplomaStatement { get; }
        public lamdaCommand AddDiplomaAnnotation { get; }

        public lamdaCommand CloseAddDiploma { get; }
        public lamdaCommand CloseAddStudent { get; }
        public lamdaCommand CloseAddTeacher { get; }
        public lamdaCommand CloseAddGroup { get; }

        public lamdaCommand<Group> AddTab { get; }

        public lamdaCommand AddInterTab { get; }

        public lamdaCommand<Diploms> OpenDiploma { get; }

        #endregion Command

        public ObservableCollection<InterTabDiploma> interTabs { get; set; }

        public MainVievModel()
        {
            AddDiplomaVisible = Visibility.Collapsed;
            AddGroupVisible = Visibility.Collapsed;
            AddStudentVisible = Visibility.Collapsed;
            AddTeacherVisible = Visibility.Collapsed;

            BlurEffect = 0;
            EnableMain = true;

            fileDialog = new OpenFileDialog();

            NewDiploma = new Diploms();
            newGroup = new Group();
            //Authorization authorization = new Authorization();
            //authorization.ShowDialog();
            //DataContext = new DiplomaStorageDataContext(authorization.sql);
            DataContext = new DiplomaStorageDataContext();
            //TODO: раскрыть вход
            ViewDiploms = new ObservableCollection<Diploms>(DataContext.Diploms);

            Students = new ObservableCollection<Student>(DataContext.Student);
            Teacher = new ObservableCollection<Teacher>(DataContext.Teacher);

            AddStudent = new lamdaCommand(OnAddStudent);

            AddTeacher = new lamdaCommand(OnAddTeacher);

            AddGroup = new lamdaCommand(OnAddGroup);

            AddDiploma = new lamdaCommand(OnAddDiploma);
            AddDiplomaAnnotation = new lamdaCommand(OnAddDiplomaAnnotation);
            AddDiplomaData = new lamdaCommand(OnAddDiplomaData);
            AddDiplomaDocumentation = new lamdaCommand(OnAddDiplomaDocumentation);
            AddDiplomaStatement = new lamdaCommand(OnAddDiplomaStatement);

            NewStudent = new Student() { Pioples = new Pioples() };

            NewTeacher = new Teacher() { Pioples = new Pioples() };

            NewGroup = new Group();
            HideGroup = new lamdaCommand(OnHideGroup);

            VisibleGroup = new lamdaCommand(OnVisibleGroup);
            CloseAddGroup = new lamdaCommand(OnCloseAddGroup);

            VisibleDiploma = new lamdaCommand(OnVisibleDiploma);
            CloseAddDiploma = new lamdaCommand(OnCloseAddDiploma);

            VisibleStudent = new lamdaCommand(OnVisibleStudent);
            CloseAddStudent = new lamdaCommand(OnCloseAddStudent);

            VisibleTeacher = new lamdaCommand(OnVisibleTeacher);
            CloseAddTeacher = new lamdaCommand(OnCloseAddTeacher);

            tabs = new ObservableCollection<Tab>();
            tabs.CollectionChanged += Tabs_CollectionChanged;

            Tabs = tabs;

            AddTab = new lamdaCommand<Group>(OnAddTab);
            OpenDiploma = new lamdaCommand<Diploms>(OnOpenDiploma);

            interTabs = new ObservableCollection<InterTabDiploma>();
            interTabs.CollectionChanged += Tabs_CollectionChanged;
            AddInterTab = new lamdaCommand(OnAddInterTab);

            var f = new InterTabDiploma(ViewDiploms);
            f.name = "test";
            interTabs.Add(f);

            var ff = new TabDiploma(ViewDiploms);
            ff.name = "test";
            Tabs.Add(ff);
        }

       

        private void OnAddInterTab()
        {
            InterTabDiploma tabDiploma = new InterTabDiploma(DataContext.Diploms);
            tabDiploma.name = tabDiploma.Diploms.Count.ToString();

            interTabs.Add(tabDiploma);
        }

        private void OnAddTab(Group obj)
        {
            TabDiploma tabDiploma = new TabDiploma(ViewDiploms.Where(a => a.Student.Group == obj));
            tabDiploma.name = obj.Fnumber;

            Tabs.Add(tabDiploma);
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

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;

                default:
                    break;
            }
        }

        private void OnCloseTab(object sender, EventArgs e)
        {
            Tab tab = (Tab)sender;
            tab.CloseRequested -= OnCloseTab;
            Tabs.Remove(tab);
        }

        private void OnOpenDiploma(Diploms obj)
        {
            if (!Directory.Exists(@"data\" + obj.id)) Directory.CreateDirectory(@"data\" + obj.id);
            if (obj.annotation != null) File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.annotation) + "." + obj.annot_type, obj.annotation.ToArray());
            if (obj.documentation != null) File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.documentation) + "." + obj.doc_type, obj.documentation.ToArray());
            if (obj.statement != null) File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.statement) + "." + obj.state_type, obj.statement.ToArray());
            File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.data) + ".rar", obj.statement.ToArray());
            Process.Start(@"data\" + obj.id);
        }

        private void OnCloseAddTeacher()
        {
            BlurEffect = 0;
            AddTeacherVisible = Visibility.Collapsed;
            EnableMain = true;
        }

        private void OnVisibleTeacher()
        {
            BlurEffect = 8;
            AddTeacherVisible = Visibility.Visible;
            EnableMain = false;
        }

        private void OnAddTeacher()
        {
            try
            {
                DataContext.Add_Teacher
                                (
                               NewTeacher.Pioples.lastname
                               ,
                               NewTeacher.Pioples.name
                               ,
                               NewTeacher.Pioples.patronymic
                                );
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, Teacher);
                Teacher = new ObservableCollection<Teacher>(DataContext.Teacher);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnCloseAddStudent()
        {
            BlurEffect = 0;
            AddStudentVisible = Visibility.Collapsed;
            EnableMain = true;
        }

        private void OnVisibleStudent()
        {
            BlurEffect = 8;
            AddStudentVisible = Visibility.Visible;
            EnableMain = false;
        }

        private void OnCloseAddGroup()
        {
            BlurEffect = 0;
            AddGroupVisible = Visibility.Collapsed;
            EnableMain = true;
        }

        private void OnVisibleGroup()
        {
            BlurEffect = 8;
            AddGroupVisible = Visibility.Visible;
            EnableMain = false;
        }

        private void OnAddDiplomaStatement()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                NewDiploma.statement = File.ReadAllBytes(fileDialog.FileName);
                NewDiploma.StatementP = fileDialog.FileName;
                OnPropertyChanged(nameof(NewDiploma.StatementP));
                NewDiploma.state_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaStatement(string path)
        {
            NewDiploma.statement = File.ReadAllBytes(path);
            NewDiploma.StatementP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(NewDiploma.StatementP));
            NewDiploma.state_type = Path.GetExtension(path).Substring(1);
        }

        public void OnAddDiplomaData(string path)
        {
            NewDiploma.data = File.ReadAllBytes(path);

            NewDiploma.DataP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(NewDiploma.DataP));
            NewDiploma.doc_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaDocumentation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                NewDiploma.documentation = File.ReadAllBytes(fileDialog.FileName);
                NewDiploma.DocP = fileDialog.FileName;
                OnPropertyChanged(nameof(NewDiploma.DocP));
                NewDiploma.doc_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaDocumentation(string path)
        {
            NewDiploma.documentation = File.ReadAllBytes(path);
            NewDiploma.DocP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(NewDiploma.DocP));
            NewDiploma.doc_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaData()
        {
            fileDialog.Filter = " rar архив (*.rar)| *.rar";
            if (fileDialog.ShowDialog() == true)
            {
                NewDiploma.data = File.ReadAllBytes(fileDialog.FileName);

                NewDiploma.DataP = fileDialog.FileName;
                OnPropertyChanged(nameof(NewDiploma.DataP));
                NewDiploma.doc_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaAnnotation(string path)
        {
            NewDiploma.annotation = File.ReadAllBytes(path);
            NewDiploma.AnnotationP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(NewDiploma.AnnotationP));

            NewDiploma.annot_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaAnnotation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                NewDiploma.annotation = File.ReadAllBytes(fileDialog.FileName);
                NewDiploma.AnnotationP = fileDialog.FileName;
                OnPropertyChanged(nameof(NewDiploma.AnnotationP));

                NewDiploma.annot_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        async private void OnAddDiploma()
        {
            OnCloseAddDiploma();
            await Task.Run(() =>
            {
                try
                {
                    DataContext.Add_Diploma(
                        sudent: NewDiploma.Sudent_id
                        ,
                        teacher: NewDiploma.Teacher_id
                        ,
                        date: NewDiploma.date
                        ,
                        data: NewDiploma.data
                        ,
                        documentation: NewDiploma.documentation
                        ,
                        doc_type: NewDiploma.doc_type
                        ,
                        statement: NewDiploma.statement
                        ,
                        state_type: NewDiploma.state_type
                        ,
                        annotation: NewDiploma.annotation
                        ,
                        annot_type: NewDiploma.annot_type
                        ,
                        comment: NewDiploma.comment
                        );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.Diploms);
            });
            ViewDiploms = new ObservableCollection<Diploms>(DataContext.Diploms);
        }

        private void OnCloseAddDiploma()
        {
            BlurEffect = 0;
            AddDiplomaVisible = Visibility.Collapsed;
            EnableMain = true;
        }

        private void OnVisibleDiploma()
        {
            BlurEffect = 8;
            AddDiplomaVisible = Visibility.Visible;
            EnableMain = false;
        }

        private void OnHideGroup()
        {
            DataContext.hide_Group
                (
                SelectedGroup.id
                );

            DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.Group);
            OnPropertyChanged(nameof(Groups));
        }

        private void OnAddStudent()
        {
            try
            {
                DataContext.Add_Student
                                (
                               newStudent.Pioples.lastname
                               ,
                               newStudent.Pioples.name
                               ,
                               newStudent.Pioples.patronymic
                               ,
                               newStudent.Group_id
                                );
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.Student);
                Students = new ObservableCollection<Student>(DataContext.Student);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnAddGroup()
        {
            try
            {
                DataContext.Add_Group
                 (

                 NewGroup.number, NewGroup.date, NewGroup.name
                 );
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.Group);
                OnPropertyChanged(nameof(Groups));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}