using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class ActivityViewModel : ViewModelBase
    {
        private IReadOnlyList<Activity> _activity;
        public IReadOnlyList<Activity> user_activity
        {
            get
            {
                return _activity;
            }
            set
            {
                Set(ref _activity, value);
            }
        }

        private IReadOnlyList<Activity> _public_activity;
        public IReadOnlyList<Activity> public_activity
        {
            get
            {
                return _public_activity;
            }
            set
            {
                Set(ref _public_activity, value);
            }
        }

        private bool _loading;
        public bool loading
        {
            get
            {
                return _loading;
            }
            set
            {
                Set(ref _loading, value);
            }
        }

        private string _loadingProgress;
        public string loadingProgress
        {
            get
            {
                return _loadingProgress;
            }
            set
            {
                Set(ref _loadingProgress, value);
            }
        }

        public IReadOnlyList<Activity> Public_activity { get => _public_activity; set => _public_activity = value; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            loading = true;
            loadingProgress = constants.r_loader.GetString("activity_progress");
            user_activity = await constants.g_client.Activity.Events.GetAllUserPerformed(App.user.Login, new ApiOptions { PageSize = 30, PageCount = 1 });
            public_activity = await constants.g_client.Activity.Events.GetAllUserReceived(App.user.Login, new ApiOptions { PageSize = 30, PageCount = 1 });
            loading = false;
        }
    }
}
