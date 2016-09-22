using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Octahedron.Views.Dialogs
{
    public sealed partial class AssignMilestoneDialog : ContentDialog
    {
        public static object dataContext { get; set; }

        public AssignMilestoneDialog(object vmDataContext)
        {
            dataContext = vmDataContext;
            this.InitializeComponent();
        }
    }
}
