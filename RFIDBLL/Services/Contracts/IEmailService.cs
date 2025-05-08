using RFIDBLL.DTOs;
using RFIDDAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RFIDBLL.Services.Contracts
{
    public interface IEmailService
    {
        string SendTicketEmail(int id, string mailTo, int status);
    }
}
