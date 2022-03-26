using HandyControl.Data;
using System.Windows.Media;
using HandyControl.Controls;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PowerUp.Windows;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;
using Newtonsoft.Json;

namespace PowerUp.Tools
{
    internal class SnapOCR
    {
        private OCRWindow resultWindow;

        public SnapOCR()
        {
            Screenshot.Snapped += Screenshot_Snapped;
            resultWindow = new OCRWindow();
        }

        public void Snap()
        {
            new Screenshot().Start();
        }

        public async void Screenshot_Snapped(object sender, FunctionEventArgs<ImageSource> e)
        {
            resultWindow.UpdateImage(e.Info);
            resultWindow.Show();

            var resultOCR = await GeneralBasicOCR(Base64Tool.Instance.ImageToBase64(e.Info));
            resultWindow.UpdateText(resultOCR);
        }

        private static async Task<string> GeneralBasicOCR(string base64str)
        {
            var result = await Task.Run(() =>
            {
                try
                {
                    Credential cred = new Credential
                    {
                        SecretId = SettingsTool.Instance.AppSettings.SnapOCRSettings.SecretID,
                        SecretKey = SettingsTool.Instance.AppSettings.SnapOCRSettings.SecretKey
                    };

                    ClientProfile clientProfile = new ClientProfile();
                    HttpProfile httpProfile = new HttpProfile();
                    httpProfile.Endpoint = ("ocr.tencentcloudapi.com");
                    clientProfile.HttpProfile = httpProfile;

                    OcrClient client = new OcrClient(cred, "ap-shanghai", clientProfile);
                    GeneralBasicOCRRequest req = new GeneralBasicOCRRequest();
                    req.ImageBase64 = base64str;
                    req.LanguageType = "zh_rare";
                    GeneralBasicOCRResponse resp = client.GeneralBasicOCRSync(req);
                    Response deserializedResponse = JsonConvert.DeserializeObject<Response>(AbstractModel.ToJsonString(resp));
                    List<TextDetection> textDetections = deserializedResponse.TextDetections;

                    string result = "";
                    foreach (TextDetection textDetection in textDetections)
                    {
                        result = result + textDetection.DetectedText + " ";
                    }
                    return result;
                }
                catch (Exception e)
                {
                    return "error: " + e.ToString();
                }
            });
            return result;
        }

        ~SnapOCR()
        {
            Screenshot.Snapped -= Screenshot_Snapped;
        }
    }

    // ResponseRoot myDeserializedClass = JsonConvert.DeserializeObject<ResponseRoot>(myJsonResponse);
    public class ItemPolygon
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Polygon
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class TextDetection
    {
        public string AdvancedInfo { get; set; }
        public int Confidence { get; set; }
        public string DetectedText { get; set; }
        public ItemPolygon ItemPolygon { get; set; }
        public List<Polygon> Polygon { get; set; }
        public List<object> WordCoordPoint { get; set; }
        public List<object> Words { get; set; }
    }

    public class Response
    {
        public double Angel { get; set; }
        public string Language { get; set; }
        public int PdfPageSize { get; set; }
        public string RequestId { get; set; }
        public List<TextDetection> TextDetections { get; set; }
    }
}