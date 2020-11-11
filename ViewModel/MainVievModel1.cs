using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DiplomaStorage.UserControls;
using Microsoft.Win32;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage
{
    partial class MainVievModel
    {
        #region Prop

        public DiplomaStorageDataContext DataContext { get; set; }

        #region DiplomaTabs

        private ObservableCollection<Tab> diplomaTabs;

        /// <summary>список вкладок для диплома</summary>
        public ObservableCollection<Tab> DiplomaTabs { get => diplomaTabs; set => Set(ref diplomaTabs, value); }

        #endregion DiplomaTabs

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

        #endregion Prop

        #region Command

        public lamdaCommand AddGroup { get; private set; }
        public lamdaCommand AddStudent { get; private set; }
        public lamdaCommand AddTeacher { get; private set; }

        public lamdaCommand HideGroup { get; private set; }

        public lamdaCommand VisibleDiploma { get; private set; }
        public lamdaCommand VisibleGroup { get; private set; }
        public lamdaCommand VisibleStudent { get; private set; }
        public lamdaCommand VisibleTeacher { get; private set; }

        public lamdaCommand AddDiploma { get; private set; }
        public lamdaCommand AddDiplomaData { get; private set; }
        public lamdaCommand AddDiplomaDocumentation { get; private set; }
        public lamdaCommand AddDiplomaStatement { get; private set; }
        public lamdaCommand AddDiplomaAnnotation { get; private set; }

        public lamdaCommand CloseAddDiploma { get; private set; }
        public lamdaCommand CloseAddStudent { get; private set; }
        public lamdaCommand CloseAddTeacher { get; private set; }
        public lamdaCommand CloseAddGroup { get; private set; }

        public lamdaCommand<Group> AddTab { get; private set; }

        public lamdaCommand<Diploms> OpenDiploma { get; private set; }

        #endregion Command

        private void InitializeComponent()
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

            DiplomaTabs = new ObservableCollection<Tab>();
            DiplomaTabs.CollectionChanged += Tabs_CollectionChanged;

            AddTab = new lamdaCommand<Group>(OnAddTab);
            OpenDiploma = new lamdaCommand<Diploms>(OnOpenDiploma);
        }
    }
}