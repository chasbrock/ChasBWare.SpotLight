using ChasBWare.SpotLight.Definitions.Utility;
using ChasBWare.SpotLight.Install;

namespace ChasBWare.SpotLight
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
            
        internal Navigator? Navigator { get;  set; }
     
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (Navigator != null)
            {
                Navigator.LastPage = args.Current?.Location;
            }
            // Cancel any back navigation.
            if (args.Source == ShellNavigationSource.Pop)
            {
                args.Cancel();
            }
        }
    }
}
