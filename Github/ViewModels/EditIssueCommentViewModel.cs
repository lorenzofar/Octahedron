using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Octokit;
using GalaSoft.MvvmLight.Command;
using Github.Views.Dialogs;
using Windows.UI.Xaml.Navigation;
using Helper;

namespace Github.ViewModels
{
    public class EditIssueCommentViewModel : ViewModelBase
    {
        private int commentId;

        private string _newComment;
        public string newComment
        {
            get
            {
                return _newComment;
            }
            set
            {
                Set(ref _newComment, value);
            }
        }

        private RelayCommand _EditComment;
        public RelayCommand EditComment
        {
            get
            {
                if(_EditComment == null)
                {
                    _EditComment = new RelayCommand(() =>
                    {
                        var viewModel = EditIssueCommentDialog.dataContext as IssueViewModel;
                        string[] info = { commentId.ToString(), newComment };
                        viewModel.EditComment.Execute(info);
                    }, () => !string.IsNullOrWhiteSpace(newComment));
                }
                return _EditComment;
            }
        }

        public EditIssueCommentViewModel()
        {
            commentId = EditIssueCommentDialog.comment.Id;
            newComment = EditIssueCommentDialog.comment.Body;
        }        
    }
}
