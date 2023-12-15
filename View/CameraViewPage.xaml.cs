namespace _2dQrScanner.View;

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

	void camerView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs barcodeInput)
	{
		string barcodeInfo = barcodeInput.Result[0].Text;

        string newTextResult = "";
		string GTIN = "GTIN: ";
		string NDC = "NDC: ";
		string SN = "SN: ";
		string LOT = "LOT: ";
		string EXP = "EXP: ";
        MainThread.BeginInvokeOnMainThread(() =>
		{
			if (barcodeInput.Result[0].BarcodeFormat.ToString() == "DATA_MATRIX")
            {
                GTIN = DecodeBarcode(barcodeInfo, 3, 17, "GTIN");
                NDC = DecodeBarcode(barcodeInfo, 5, 16, "NDC");
                SN = DecodeBarcode(barcodeInfo, 19, 32,"SN");
                EXP = DecodeBarcode(barcodeInfo, 34, 40, "EXP");
				LOT = DecodeBarcode(barcodeInfo, 42, barcodeInfo.Length, "LOT");
            }

            barcodeResult.Text = $"{GTIN}\n{NDC}\n{SN}\n{EXP}\n{LOT}";
		});
	}


    private static string DecodeBarcode(string barcodeInfo, int startIndex, int endIndex, string barcodeName)
    {
		string decodedBarcodeInfo = $"{barcodeName}: ";
        for (int i = startIndex; i < endIndex; i++)
        {
            decodedBarcodeInfo += $"{barcodeInfo[i]}";
        }

        return decodedBarcodeInfo;
    }
}