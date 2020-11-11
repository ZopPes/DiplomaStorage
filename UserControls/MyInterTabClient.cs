using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiplomaStorage.UserControls;
using Dragablz;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage.UserControls
{
    public class InterTabDiploma : MyInterTabClient
    {
        public ObservableCollection<Diploms> Diploms { get; }

        public lamdaCommand<Diploms> OpenDiploma { get; }

        public InterTabDiploma(IEnumerable<Diploms> diploms)
        {
            Diploms = new ObservableCollection<Diploms>(diploms);

            OpenDiploma = new lamdaCommand<Diploms>(OnOpenDiploma);
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
    }

    public class MyInterTabClient :  Tab, IInterTabClient
    {


        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var View = new testDragablz();
            
            return new NewTabHost<Window>(View, View.TabCon);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
