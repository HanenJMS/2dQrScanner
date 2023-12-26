using PharmacyDataMatrixScanner.Model;
using Camera.MAUI.ZXingHelper;
using ZXing;
using ZXing.Datamatrix;
using ZXing.OneD;
namespace PharmacyDataMatrixScanner.View;

public partial class CameraViewPage : ContentPage
{
    public CameraViewPage()
    {
        InitializeComponent();
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count <= 0) return;

        cameraView.Camera = cameraView.Cameras.First();

        MainThread.BeginInvokeOnMainThread(async () =>
        {

            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    void camerView_BarcodeDetected(object sender, BarcodeEventArgs barcodeInput)
    {
        string barcodeData = "";
        MainThread.BeginInvokeOnMainThread(() =>
        {
            barcodeData = PharmacyDataMatrixDecoder.DecodingDataMatrix(barcodeInput.Result[0].RawBytes, barcodeInput.Result[0].BarcodeFormat);
            barcodeResult.Text = barcodeData;
        });
        
    }


}