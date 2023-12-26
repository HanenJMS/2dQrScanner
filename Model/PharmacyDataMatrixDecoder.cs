
using System.Text;
using ZXing;

namespace PharmacyDataMatrixScanner.Model
{
    static class PharmacyDataMatrixDecoder
    {
        public static string DecodingDataMatrix(byte[] barcodeInfo, BarcodeFormat barcodeType)
        {
            string newBarcodeStringStates = "";
            Dictionary<int, Queue<byte>> fncSeperator = new();
            Queue<byte> byteQueue = new Queue<byte>(barcodeInfo);
            for(int key = -1; byteQueue.Count > 0;)
            {
                byte nextByte = byteQueue.Dequeue();
                if (nextByte == 232) key++;
                else
                {
                    if(!fncSeperator.ContainsKey(key)) fncSeperator.Add(key, new());
                    fncSeperator[key].Enqueue(nextByte);
                }
            }

            if (barcodeType == BarcodeFormat.DATA_MATRIX)
            {
                for(int fncIndex = 0; fncIndex < fncSeperator.Count; fncIndex++)
                {
                    while (fncSeperator[fncIndex].Count > 0)
                    {
                        string barcodeAI = ParseBarcodeAI(fncSeperator[fncIndex]);
                        int decodeLength = GetAILength(barcodeAI);

                        string barcodeAIType = GetApplicationIdentifierType(barcodeAI);
                        string codeInfo = "";
                        for (int i = 0; i < decodeLength; i++)
                        {
                            byte newByte;
                            if (fncSeperator[fncIndex].TryDequeue(out newByte))
                                codeInfo += ConvertCodeWordUsingGS1Standard(newByte);
                        }
                        if (barcodeAIType != "")
                        {
                            newBarcodeStringStates += $"{barcodeAIType}: {codeInfo}\n";
                        }
                    }
                }
                
            }
            return newBarcodeStringStates;
        }
        //static string ReturnInfoToViewableFormat()
        //{
        //    StringBuilder str = new();
        //    foreach()
        //}

        static string ParseBarcodeAI(Queue<byte> byteQueue)
        {
            if (byteQueue.Count() <= 0) return "";
            return ConvertCodeWordUsingGS1Standard(byteQueue.Dequeue());
        }
        static string DecodeBarcode(string barcodeInfo, int decodeLength)
        {
            return barcodeInfo.Remove(0, decodeLength);
        }
        static string GetApplicationIdentifierType(string parsedDataType)
        {
            string qrDataTypeName = "";
            if (parsedDataType == "01")
            {
                qrDataTypeName = "GTIN";
            }
            if (parsedDataType == "21")
            {
                qrDataTypeName = "SN";
            }
            if (parsedDataType == "17")
            {
                qrDataTypeName = "EXP";
            }
            if(parsedDataType == "10")
            {
                qrDataTypeName = "LOT";
            }
            return qrDataTypeName;
        }
        static int GetAILength(string parsedDataType)
        {
            int length = 0;
            if (parsedDataType == "01")
            {
                length = 7;
            }
            if (parsedDataType == "10")
            {
                length = 5;
            }
            if (parsedDataType == "21")
            {
                length = 5;
            }
            if (parsedDataType == "17")
            {
                length = 3;
            }
            if(parsedDataType == "]d2")
            {
                length = 0;
            }
            return length;
        }

        public static void DecodeRawBytes(byte[] rawBytesData)
        {
            for (int i = 0; i < rawBytesData.Length; i++)
            {
                ParsingByteCodeWord(rawBytesData[i]);
            }
        }
        //static string ReturnInfoToViewableFormat()
        //{
        //    StringBuilder str = new();
        //    foreach()
        //}

        static string ParsingByteCodeWord(byte byteInfo)
        {
            return ConvertCodeWordUsingGS1Standard(byteInfo);
        }
        static string ConvertCodeWordUsingGS1Standard(byte byteInfo)
        {
            string codeWordConverted = "";
            if (byteInfo == 232)
            {
                codeWordConverted = "]d2";
            }
            if (byteInfo >= 130 && byteInfo <= 229)
            {
                codeWordConverted = $"{ConvertCodeWord_130_229(byteInfo)}";
            }
            if (byteInfo >= 1 && byteInfo <= 128)
            {
                codeWordConverted = $"{ConvertCodeWord_1_129(byteInfo)}";
            }
            return codeWordConverted;
        }
        static string ConvertCodeWord_130_229(byte byteInfo)
        {
            return $"{byteInfo - 130:00}";
        }
        static string ConvertCodeWord_1_129(byte byteInfo)
        {
            return char.ConvertFromUtf32(byteInfo - 1);
        }
    }
}
