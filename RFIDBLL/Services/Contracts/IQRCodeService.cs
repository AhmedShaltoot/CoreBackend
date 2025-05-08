using RFIDBLL.DTOs;
using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RFIDBLL.Services.Contracts
{
    public interface IQRCodeService
    {
        string CreateTicketQRCodeImage(string itemSerial);
        string FileRenameWithOutExtenision(string fName);
    }
}
