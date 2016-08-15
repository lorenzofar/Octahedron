﻿using Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Octahedron.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReposPage : Page
    {
        public ReposPage()
        {
            this.InitializeComponent();
        }

        private void sort_btn_Click(object sender, RoutedEventArgs e)
        {
            sort_btn.Flyout.ShowAt(sort_btn);
        }

        private void filter_btn_Click(object sender, RoutedEventArgs e)
        {
            filter_btn.Flyout.ShowAt(filter_btn);
        }
    }
}