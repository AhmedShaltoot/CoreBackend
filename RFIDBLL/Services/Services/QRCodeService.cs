using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using RFIDBLL.HelperClasses;
using RFIDBLL.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace RFIDBLL.Services.Services
{
    public class QRCodeService : IQRCodeService
    {
        private IHostingEnvironment Environment;
        public QRCodeService(IHostingEnvironment environment)
        {
            Environment = environment;
        }

        public string CreateTicketQRCodeImage(string itemSerial)
        {
            string dir = Path.Combine(Environment.ContentRootPath, "UploadedDocs\\TicketQR", ClientDateService.HostingYear(), ClientDateService.HostingMonth(), ClientDateService.HostingDay());

            if (ClientDateService.CheckDirectory(dir))
            {
                string QRCodeimage = $"{itemSerial}.png";

                while (File.Exists(Path.Combine(dir, QRCodeimage)))
                {
                    QRCodeimage = $"{ClientDateService.FileRenameWithOutExtenision(QRCodeimage)}.png";
                }

                try
                {
                    string qr = itemSerial;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qr, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    string qrImagePath = Path.Combine(dir, QRCodeimage);
                    qrCodeImage.Save(qrImagePath, ImageFormat.Png);

                    string relativePath = Path.Combine("UploadedDocs", "TicketQR", ClientDateService.HostingYear(), ClientDateService.HostingMonth(), ClientDateService.HostingDay(), QRCodeimage);
                    return relativePath.Replace("\\", "/");
                }
                catch (Exception)
                {
                    throw new Exception("Failure");
                }
            }
            else
            {
                throw new Exception("Failure");
            }
        }
        public string FileRenameWithOutExtenision(string fName)
        {
            string[] nameWithExt = fName.Split('.');
            string newFileName = nameWithExt[0] + "_" + DateTime.Now.Second + "." + nameWithExt[1];
            return newFileName;
        }

    }
}
