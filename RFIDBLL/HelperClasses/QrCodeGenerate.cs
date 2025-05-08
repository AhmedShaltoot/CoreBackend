using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class QrCodeGenerate
    {

        #region Based Method


        public string Generate(string Base64)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Base64, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = new object();
            
                qrCodeImage = qrCode.GetGraphic(20);


            using (var ms = new MemoryStream())
            {
                using (var bitmap = new Bitmap((Bitmap)qrCodeImage))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var SigBase64 = Convert.ToBase64String(ms.GetBuffer()); //Get Base64

                    return SigBase64;
                }
            }
        }
        public string GenerateNew(string Base64)
        {

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(Base64, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var bitmap = qrCode.GetGraphic(20))
                    {
                        using (var stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            var qrCodeImage = stream.ToArray();
                            var filePath = Path.Combine("E:\\New Volume D\\Siru\\SiruBabAPI\\SiruBabCore_Admin\\SiruBabCoreAPI\\Invoices\\QRCode", $"testNewQr.png");
                            System.IO.File.WriteAllBytes(filePath, qrCodeImage);
                            return "testNewQr.png"; // File(qrCodeImage, "image /png");
                        }
                    }
                }
            }
        }


        #endregion





    }
}
