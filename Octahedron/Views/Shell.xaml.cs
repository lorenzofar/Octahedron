﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Services.NavigationService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Octahedron.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Shell()
        {
            this.InitializeComponent();
            Messenger.Default.Register<MvvmMessaging.ProfileIconMessage>(this, message =>
            {
                profileIcon.ImageSource = new BitmapImage(new Uri(message.url));
            });
        }

        public Shell(INavigationService navigationService)
        {
            InitializeComponent();
            Messenger.Default.Register<MvvmMessaging.ProfileIconMessage>(this, message =>
            {
                profileIcon.ImageSource = new BitmapImage(new Uri(message.url));
            });
            Menu.NavigationService = navigationService;
        }
    }
}
