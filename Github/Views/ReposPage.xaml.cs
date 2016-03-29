using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Github.Views
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