using System.Windows;
using System.Windows.Controls;
using System.IO;


namespace DiplomaStorage.UserControls
{
    /// <summary>
    /// Логика взаимодействия для AddDiplomaControl.xaml
    /// </summary>
    public partial class AddDiplomaControl : UserControl
    {
        public AddDiplomaControl()
        {
            InitializeComponent();
        }

        private void DropDadaDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".rar")
                {
                    DiplomaDatatext.Text = Path.GetFileNameWithoutExtension(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                    ((MainVievModel)((Grid)sender).DataContext).OnAddDiplomaData(((string[])p.GetData(DataFormats.FileDrop))[0]);
                }
            }
        }

        private void DropAnnotationDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".docx"
                    ||
                    ptype == ".doc"
                    ||
                    ptype == ".odt")
                {
                    DiplomaAnnotationtext.Text = Path.GetFileNameWithoutExtension(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                    ((MainVievModel)((Grid)sender).DataContext).OnAddDiplomaAnnotation(((string[])p.GetData(DataFormats.FileDrop))[0]);
                }
            }
        }

        private void DropDocumentationDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".docx"
                    ||
                    ptype == ".doc"
                    ||
                    ptype == ".odt")
                {
                    DiplomaDocumentationtext.Text = Path.GetFileNameWithoutExtension(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                    ((MainVievModel)((Grid)sender).DataContext).OnAddDiplomaDocumentation(((string[])p.GetData(DataFormats.FileDrop))[0]);
                }
            }
        }

        private void DropStatementDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".docx"
                    ||
                    ptype == ".doc"
                    ||
                    ptype == ".odt")
                {
                    DiplomaStatementtext.Text = Path.GetFileNameWithoutExtension(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                    ((MainVievModel)((Grid)sender).DataContext).OnAddDiplomaStatement(((string[])p.GetData(DataFormats.FileDrop))[0]);
                }
            }
        }

        private void DragEnterWordDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".docx"
                    ||
                    ptype == ".doc"
                    ||
                    ptype == ".odt")
                    e.Effects = DragDropEffects.Copy;
                return;
            }
            e.Handled = false;
        }

        private void DragEnterDadaDiloma(object sender, DragEventArgs e)
        {
            var p = e.Data;
            if (p.GetDataPresent(DataFormats.FileDrop))
            {
                var ptype = Path.GetExtension(((string[])p.GetData(DataFormats.FileDrop))[0]);
                if (ptype == ".rar")
                    e.Effects = DragDropEffects.Copy;
                return;
            }
            e.Handled = false;
        }
    }
}
