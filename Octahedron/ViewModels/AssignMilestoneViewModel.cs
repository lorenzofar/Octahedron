using GalaSoft.MvvmLight.Command;
using Helper;
using Octahedron.Views.Dialogs;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using Template10.Mvvm;

namespace Octahedron.ViewModels
{
    public class AssignMilestoneViewModel : ViewModelBase
    {
        private List<Milestone> _milestones;
        public List<Milestone> milestones
        {
            get
            {
                return _milestones;
            }
            set
            {
                Set(ref _milestones, value);
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

        private string _loadingProgress = constants.r_loader.GetString("milestones_progress");
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

        private int _selectedIndex = 0;
        public int selectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                Set(ref _selectedIndex, value);
            }
        }

        public AssignMilestoneViewModel()
        {
            LoadMilestones();
        }

        private async void LoadMilestones()
        {
            loading = true;
            var viewModel = AssignMilestoneDialog.dataContext as IssueViewModel;
            var rawMilestones = (await constants.g_client.Issue.Milestone.GetAllForRepository(viewModel.issueData[0], viewModel.issueData[1])).ToList();
            rawMilestones.Insert(0, new Milestone());
            milestones = rawMilestones;
            selectedIndex = viewModel.issue.Milestone == null ? 0 : milestones.IndexOf(viewModel.issue.Milestone);
            loading = false;
        }

        private RelayCommand _ConfirmAssignment;
        public RelayCommand ConfirmAssignment
        {
            get
            {
                if(_ConfirmAssignment == null)
                {
                    _ConfirmAssignment = new RelayCommand(() =>
                    {
                        if (selectedIndex != -1)
                        {
                            var viewModel = AssignMilestoneDialog.dataContext as IssueViewModel;
                            viewModel.AddToMilestone.Execute(milestones[selectedIndex]);
                        }
                    });
                }
                return _ConfirmAssignment;
            }
        }
    }
}
