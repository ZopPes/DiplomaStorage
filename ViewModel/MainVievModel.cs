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