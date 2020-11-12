using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DiplomaStorage.UserControls;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage
{
    public partial class MainVievModel : peremlog
    {
        public MainVievModel()
        {
            InitializeComponent();
        }

        private void OnAddTab(Group obj)
        {
            TabDiploma tabDiploma = new TabDiploma(ViewDiploms.Where(a => a.Student.Group == obj), OpenDiploma)
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
            DiplomaTabs.Remove(tab);
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

        private void OnAddTeacher()
        {
            try
            {
                DataContext.Add_Teacher
                                (
                               addTeacherVisibleControl.New.Pioples.lastname
                               ,
                               addTeacherVisibleControl.New.Pioples.name
                               ,
                               addTeacherVisibleControl.New.Pioples.patronymic
                                );
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, Teacher);
                Teacher = new ObservableCollection<Teacher>(DataContext.Teacher);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnAddDiplomaStatement()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                AddDiplomaVisibleControl.New.statement = File.ReadAllBytes(fileDialog.FileName);
                AddDiplomaVisibleControl.New.StatementP = fileDialog.FileName;
                OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.StatementP));
                AddDiplomaVisibleControl.New.state_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaStatement(string path)
        {
            AddDiplomaVisibleControl.New.statement = File.ReadAllBytes(path);
            AddDiplomaVisibleControl.New.StatementP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.StatementP));
            AddDiplomaVisibleControl.New.state_type = Path.GetExtension(path).Substring(1);
        }

        public void OnAddDiplomaData(string path)
        {
            AddDiplomaVisibleControl.New.data = File.ReadAllBytes(path);

            AddDiplomaVisibleControl.New.DataP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.DataP));
            AddDiplomaVisibleControl.New.doc_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaDocumentation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                AddDiplomaVisibleControl.New.documentation = File.ReadAllBytes(fileDialog.FileName);
                AddDiplomaVisibleControl.New.DocP = fileDialog.FileName;
                OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.DocP));
                AddDiplomaVisibleControl.New.doc_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaDocumentation(string path)
        {
            AddDiplomaVisibleControl.New.documentation = File.ReadAllBytes(path);
            AddDiplomaVisibleControl.New.DocP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.DocP));
            AddDiplomaVisibleControl.New.doc_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaData()
        {
            fileDialog.Filter = " rar архив (*.rar)| *.rar";
            if (fileDialog.ShowDialog() == true)
            {
                AddDiplomaVisibleControl.New.data = File.ReadAllBytes(fileDialog.FileName);

                AddDiplomaVisibleControl.New.DataP = fileDialog.FileName;
                OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.DataP));
                AddDiplomaVisibleControl.New.doc_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        public void OnAddDiplomaAnnotation(string path)
        {
            AddDiplomaVisibleControl.New.annotation = File.ReadAllBytes(path);
            AddDiplomaVisibleControl.New.AnnotationP = Path.GetFileNameWithoutExtension(path);
            OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.AnnotationP));

            AddDiplomaVisibleControl.New.annot_type = Path.GetExtension(path).Substring(1);
        }

        private void OnAddDiplomaAnnotation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
            {
                AddDiplomaVisibleControl.New.annotation = File.ReadAllBytes(fileDialog.FileName);
                AddDiplomaVisibleControl.New.AnnotationP = fileDialog.FileName;
                OnPropertyChanged(nameof(AddDiplomaVisibleControl.New.AnnotationP));

                AddDiplomaVisibleControl.New.annot_type = fileDialog.FileName.Substring(fileDialog.FileName.LastIndexOf('.') + 1);
            }
        }

        async private void OnAddDiploma()
        {
            await Task.Run(() =>
            {
                try
                {
                    DataContext.Add_Diploma(
                        sudent: AddDiplomaVisibleControl.New.Sudent_id
                        ,
                        teacher: AddDiplomaVisibleControl.New.Teacher_id
                        ,
                        date: AddDiplomaVisibleControl.New.date
                        ,
                        data: AddDiplomaVisibleControl.New.data
                        ,
                        documentation: AddDiplomaVisibleControl.New.documentation
                        ,
                        doc_type: AddDiplomaVisibleControl.New.doc_type
                        ,
                        statement: AddDiplomaVisibleControl.New.statement
                        ,
                        state_type: AddDiplomaVisibleControl.New.state_type
                        ,
                        annotation: AddDiplomaVisibleControl.New.annotation
                        ,
                        annot_type: AddDiplomaVisibleControl.New.annot_type
                        ,
                        comment: AddDiplomaVisibleControl.New.comment
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
                               AddStudentVisibleControl.New.Pioples.lastname
                               ,
                               AddStudentVisibleControl.New.Pioples.name
                               ,
                               AddStudentVisibleControl.New.Pioples.patronymic
                               ,
                               AddStudentVisibleControl.New.Group_id
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

                 AddGroupVisibleControl.New.number
                 ,
                 AddGroupVisibleControl.New.date
                 ,
                 AddGroupVisibleControl.New.name
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