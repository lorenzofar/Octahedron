﻿using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Github.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
        }

        public Shell(INavigationService navigationService)
        {
            InitializeComponent();
            Menu.NavigationService = navigationService;
        }
    }
}