using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            if (obj.annotation != null)
            {
                var annotation = DataContext.Get_File(obj.annotation).First();
                File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.annotation) + "." + annotation.type, annotation.data.ToArray());
            }
            if (obj.statement != null)
            {
                var statement = DataContext.Get_File(obj.statement).First();
                File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.statement) + "." + statement.type, statement.data.ToArray());
            }
            var documentation = DataContext.Get_File(obj.documentation).First();
            File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.documentation) + "." + documentation.type, documentation.data.ToArray());
            var data = DataContext.Get_File(obj.data).First();
            File.WriteAllBytes(@"data\" + obj.id + "\\" + nameof(obj.data) + "."+data.type, data.data.ToArray());
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
                OnAddDiplomaStatement(fileDialog.FileName);
        }

        public void OnAddDiplomaStatement(string path)
        {
            AddDiplomaVisibleControl.New.statement =
            DataContext.Add_File(File.ReadAllBytes(path), Path.GetExtension(path)).First().id;
        }

        public void OnAddDiplomaData(string path)
        {
            AddDiplomaVisibleControl.New.data =
            DataContext.Add_File(File.ReadAllBytes(path), Path.GetExtension(path)).First().id.Value;

        }

        private void OnAddDiplomaDocumentation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
                OnAddDiplomaDocumentation(fileDialog.FileName);
        }

        public void OnAddDiplomaDocumentation(string path)
        {
            AddDiplomaVisibleControl.New.documentation =
                DataContext.Add_File(File.ReadAllBytes(path), Path.GetExtension(path)).First().id.Value;

        }

    private void OnAddDiplomaData()
        {
            fileDialog.Filter = " rar архив (*.rar)| *.rar";
            if (fileDialog.ShowDialog() == true)
                OnAddDiplomaData(fileDialog.FileName);
        }

        public void OnAddDiplomaAnnotation(string path)
        {
            AddDiplomaVisibleControl.New.annotation =
            DataContext.Add_File(File.ReadAllBytes(path), Path.GetExtension(path)).First().id;
        }

        private void OnAddDiplomaAnnotation()
        {
            fileDialog.Filter = "odt (*.odt)| *.odt |" +
                " doc (*.doc)| *.doc|" +
                " docx (*.docx)| *.docx";
            if (fileDialog.ShowDialog() == true)
                OnAddDiplomaAnnotation(fileDialog.FileName);
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
                        statement: AddDiplomaVisibleControl.New.statement
                        ,
                        annotation: AddDiplomaVisibleControl.New.annotation
                        ,
                        comment: AddDiplomaVisibleControl.New.comment
                        );
                DataContext.Refresh(RefreshMode.OverwriteCurrentValues, DataContext.Diploms);
                 ViewDiploms = new ObservableCollection<Diploms>(DataContext.Diploms);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
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
                DataContext.Group.InsertOnSubmit(AddGroupVisibleControl.New);
                DataContext.SubmitChanges();
                Groups.Add(DataContext.Group.ToList().Last());
                AddGroupVisibleControl.New = new Group();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}