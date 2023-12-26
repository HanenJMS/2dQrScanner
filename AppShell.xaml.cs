namespace PharmacyDataMatrixScanner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(View.CameraViewPage), typeof(View.CameraViewPage));
        }
    }
}
