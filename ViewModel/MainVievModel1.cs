using System;
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

        public bool IsConnect=false;

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

        public IEnumerable Groups
        {
            get => DataContext.Group.GroupBy(i => i.date, (i, g) => new { k = i, G = g });
        }

        #region VievDiploma

        private ObservableCollection<Diploms> viewDiploms;

        /// <summary>Колекция дипломов</summary>
        public ObservableCollection<Diploms> ViewDiploms { get => viewDiploms; set => Set(ref viewDiploms, value); }

        #endregion VievDiploma

        #region AddDiplomaVisibleControl

        private TableControl<Diploms> DilomaControl;

        /// <summary>Обработка окна диплома</summary>
        public TableControl<Diploms> AddDiplomaVisibleControl { get => DilomaControl; set => Set(ref DilomaControl, value); }

        #endregion AddDiplomaVisibleControl

        #region AddTeacherVisibleControl

        private TableControl<Teacher> addTeacherVisibleControl;

        /// <summary>Обработка онка преподователя</summary>
        public TableControl<Teacher> AddTeacherVisibleControl { get => addTeacherVisibleControl; set => Set(ref addTeacherVisibleControl, value); }

        #endregion AddTeacherVisibleControl

        #region AddStudentVisibleControl

        private TableControl<Student> addStudentVisibleControl;

        /// <summary>обработка окна студента</summary>
        public TableControl<Student> AddStudentVisibleControl { get => addStudentVisibleControl; set => Set(ref addStudentVisibleControl, value); }

        #endregion AddStudentVisibleControl

        #region AddGroupVisibleControl

        private TableControl<Group> addGroupVisibleControl;

        /// <summary>обработчик окна групп</summary>
        public TableControl<Group> AddGroupVisibleControl { get => addGroupVisibleControl; set => Set(ref addGroupVisibleControl, value); }

        #endregion AddGroupVisibleControl

        public OpenFileDialog fileDialog;


        #endregion Prop

        #region Command

        public lamdaCommand AddGroup { get; private set; }
        public lamdaCommand AddStudent { get; private set; }
        public lamdaCommand AddTeacher { get; private set; }

        public lamdaCommand AddDiploma { get; private set; }
        public lamdaCommand AddDiplomaData { get; private set; }
        public lamdaCommand AddDiplomaDocumentation { get; private set; }
        public lamdaCommand AddDiplomaStatement { get; private set; }
        public lamdaCommand AddDiplomaAnnotation { get; private set; }

        public lamdaCommand<Group> AddTab { get; private set; }

        public lamdaCommand<Diploms> OpenDiploma { get; private set; }

        #endregion Command

        private void InitializeComponent()
        {
            Authorization authorization = new Authorization();
            if (authorization.ShowDialog() == true)
            {
                IsConnect = true;
                DataContext = new DiplomaStorageDataContext(authorization.sql);
            }
            else
            {
                Application.Current.Shutdown();
            return;
            }

            AddDiplomaVisibleControl = new TableControl<Diploms>();
            AddGroupVisibleControl = new TableControl<Group>();
            AddStudentVisibleControl = new TableControl<Student>
                (
                new Student() { Pioples = new Pioples() }
                );
            AddTeacherVisibleControl = new TableControl<Teacher>
                (
                new Teacher() { Pioples = new Pioples() }
                );

            fileDialog = new OpenFileDialog();

            ViewDiploms = new ObservableCollection<Diploms>(DataContext.Diploms);

            Students = new ObservableCollection<Student>(DataContext.Student);
            Teacher = new ObservableCollection<Teacher>(DataContext.Teacher);

            AddGroup = new lamdaCommand(OnAddGroup);
            AddStudent = new lamdaCommand(OnAddStudent);
            AddTeacher = new lamdaCommand(OnAddTeacher);
            AddDiploma = new lamdaCommand(OnAddDiploma);

            AddDiplomaData = new lamdaCommand(OnAddDiplomaData);
            AddDiplomaDocumentation = new lamdaCommand(OnAddDiplomaDocumentation);
            AddDiplomaStatement = new lamdaCommand(OnAddDiplomaStatement);
            AddDiplomaAnnotation = new lamdaCommand(OnAddDiplomaAnnotation);


            DiplomaTabs = new ObservableCollection<Tab>();
            DiplomaTabs.CollectionChanged += Tabs_CollectionChanged;

            AddTab = new lamdaCommand<Group>(OnAddTab);
            OpenDiploma = new lamdaCommand<Diploms>(OnOpenDiploma);
        }
    }
}