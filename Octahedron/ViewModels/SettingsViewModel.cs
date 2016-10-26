using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _loading = false;
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

        #region PROFILE SECTION
        private string _bio;
        public string bio
        {
            get
            {
                return _bio;
            }
            set
            {
                Set(ref _bio, value);
            }
        }
        private string _blog;
        public string blog
        {
            get
            {
                return _blog;
            }
            set
            {
                Set(ref _blog, value);
            }
        }
        private string _company;
        public string company
        {
            get
            {
                return _company;
            }
            set
            {
                Set(ref _company, value);
            }
        }
        private string _location;
        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                Set(ref _location, value);
            }
        }
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(ref _name, value);
            }
        }
        private bool _hireable;
        public bool hireable
        {
            get
            {
                return _hireable;
            }
            set
            {
                Set(ref _hireable, value);
            }
        }

        private IReadOnlyList<EmailAddress> _emails;
        public IReadOnlyList<EmailAddress> emails
        {
            get
            {
                return _emails;
            }
            set
            {
                Set(ref _emails, value);
            }
        }
        
        private EmailAddress _selectedEmail;
        public EmailAddress selectedEmail
        {
            get
            {
                return _selectedEmail;
            }
            set
            {
                Set(ref _selectedEmail, value);
            }
        }

        private RelayCommand _UpdateProfile;
        public RelayCommand UpdateProfile
        {
            get
            {
                if (_UpdateProfile == null)
                {
                    _UpdateProfile = new RelayCommand(async () =>
                    {
                        loading = true;
                        loadingProgress = constants.r_loader.GetString("updateProfile_progress");
                        App.user = await constants.g_client.User.Update(new Octokit.UserUpdate
                        {
                            Bio = bio,
                            Blog = blog,
                            Company = company,
                            Email = selectedEmail.Email,
                            Hireable = hireable,
                            Location = location,
                            Name = name
                        });
                        loading = false;
                    });
                }
                return _UpdateProfile;
            }
        }
        #endregion

        private RelayCommand _LogOut;
        public RelayCommand LogOut
        {
            get
            {
                if (_LogOut == null)
                {
                    _LogOut = new RelayCommand(async () =>
                    {
                        await utilities.LogOut();
                        App.Current.NavigationService.ClearHistory();
                        Window.Current.Content = new Views.LoginPage();
                    });
                }
                return _LogOut;
            }
        }


        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            loading = true;
            loadingProgress = constants.r_loader.GetString("loadingSettings_progress");
            App.user = await constants.g_client.User.Current();
            emails = (await constants.g_client.User.Email.GetAll()).OrderByDescending(x => x.Email == App.user.Email).ToList();
            selectedEmail = emails.FirstOrDefault(x => x.Email == App.user.Email);
            bio = App.user.Bio;
            blog = App.user.Blog;
            company = App.user.Company;
            location = App.user.Location;
            name = App.user.Name;
            hireable = App.user.Hireable == null ? false : App.user.Hireable.Value;
            loading = false;
        }
    }
}
