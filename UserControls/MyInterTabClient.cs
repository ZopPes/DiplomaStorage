using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiplomaStorage.UserControls;
using Dragablz;

namespace DiplomaStorage
{
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
