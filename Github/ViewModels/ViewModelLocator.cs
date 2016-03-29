﻿namespace Github.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
        }

        public MainViewModel Main
        {
            get
            {
                return new MainViewModel();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return new LoginViewModel();
            }
        }

        public ProfileViewModel Profile
        {
            get
            {
                return new ProfileViewModel();
            }
        }

        public ReposViewModel Repos
        {
            get
            {
                return new ReposViewModel();
            }
        }

        public RepoDetailViewModel RepoDetail
        {
            get
            {
                return new RepoDetailViewModel();
            }
        }
    }
}