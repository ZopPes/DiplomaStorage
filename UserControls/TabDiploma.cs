using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;
using WPFMVVMHelper;

namespace DiplomaStorage.UserControls
{
    public class TabDiploma : Tab
    {
        public ObservableCollection<Diploms> Diploms { get; }

        public lamdaCommand<Diploms> OpenDiploma { get; }

        public TabDiploma(IEnumerable<Diploms> diploms, lamdaCommand<Diploms> openDiploma)
        {
            Diploms = new ObservableCollection<Diploms>(diploms);
            OpenDiploma = openDiploma;
        }
    }
}