using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class NewMilestoneViewModel : ViewModelBase
    {
        public static bool editing { get; set; }
        private Milestone editingMilestone { get; set; }

        private bool dueDateChanged { get; set; }

        private string[] _repoData;
        public string[] repoData
        {
            get
            {
                return _repoData;
            }
            set
            {
                Set(ref _repoData, value);
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

        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
                CreateMilestone.RaiseCanExecuteChanged();
            }
        }

        private string _description;
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                Set(ref _description, value);
            }
        }

        private Nullable<DateTime> _dueDate = null;
        public Nullable<DateTime> dueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                Set(ref _dueDate, value);
                dueDateChanged = true;
                ClearDueDate.RaiseCanExecuteChanged();
            }
        }


        private RelayCommand _ClearDueDate;
        public RelayCommand ClearDueDate
        {
            get
            {
                if (_ClearDueDate == null)
                {
                    _ClearDueDate = new RelayCommand(() =>
                    {
                        dueDate = DateTime.Now;
                        dueDateChanged = false;
                        ClearDueDate.RaiseCanExecuteChanged();
                    }, () => dueDateChanged == true);
                }
                return _ClearDueDate;
            }
        }

        private RelayCommand _CreateMilestone;
        public RelayCommand CreateMilestone
        {
            get
            {
                if (_CreateMilestone == null)
                {
                    _CreateMilestone = new RelayCommand(async () =>
                    {
                        loading = true;
                        loadingProgress = constants.r_loader.GetString(editing ? "editMilestone_progress" : "addMilestone_progress");
                        Milestone milestone = new Milestone();
                        if (!editing)
                        {
                            NewMilestone newMilestone = new NewMilestone(title) { Description = description };
                            if (dueDateChanged)
                            {
                                newMilestone.DueOn = new DateTimeOffset(dueDate.Value);
                            }
                            milestone = await constants.g_client.Issue.Milestone.Create(int.Parse(repoData[2]), newMilestone);
                        }
                        else
                        {
                            MilestoneUpdate update = new MilestoneUpdate() { Title = title, Description = description };
                            if (dueDateChanged)
                            {
                                update.DueOn = new DateTimeOffset(dueDate.Value);
                            }
                            milestone = await constants.g_client.Issue.Milestone.Update(int.Parse(repoData[2]), editingMilestone.Number, update);
                        }
                        loading = false;
                        App.Current.NavigationService.GoBack(); //JUST A PLACEHOLDER, IT SHOULD OPEN THE MILESTONE'S PAGE
                    }, () => !string.IsNullOrWhiteSpace(title));
                }
                return _CreateMilestone;
            }
        }

        private RelayCommand _Cancel;
        public RelayCommand Cancel
        {
            get
            {
                if (_Cancel == null)
                {
                    _Cancel = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.GoBack();
                    });
                }
                return _Cancel;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                var request = parameter as Dictionary<string, string>;
                editing = int.Parse(request["kind"].ToString()) != 0;
                repoData = new string[] { request["owner"], request["name"], request["id"] };
                if (editing)
                {
                    var milestoneNumber = int.Parse(request["issueNumber"]);
                    editingMilestone = await constants.g_client.Issue.Milestone.Get(int.Parse(repoData[2]), milestoneNumber);
                    title = editingMilestone.Title;
                    description = editingMilestone.Description;
                    dueDate = editingMilestone.DueOn.Value.Date != null ? editingMilestone.DueOn.Value.Date : DateTime.Now;
                }
            }
        }
    }
}
