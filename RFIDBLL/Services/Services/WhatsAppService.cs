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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Options;

namespace RFIDBLL.Services.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        //protected IRepositoryWrapper _repoWrapper;
        //private readonly string _accountSid;
        //private readonly string _authToken;
        //private readonly string _fromNumber;
        private readonly TwilioSettings _twilioSettings;

        public WhatsAppService(IOptions<TwilioSettings> twilioSettings)
        {
            //_repoWrapper = repoWrapper;
            //_accountSid = accountSid;
            //_authToken = authToken;
            //_fromNumber = fromNumber;
            //TwilioClient.Init(_accountSid, _authToken);
            _twilioSettings = twilioSettings.Value;
            TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
        }
        public void SendMessage(string toNumber, string mediaUrl)
        {
            var message = MessageResource.Create(
                body: "Here is your invoice",
                from: new PhoneNumber($"whatsapp:{_twilioSettings.FromNumber}"),
                to: new PhoneNumber($"whatsapp:{toNumber}")
                //mediaUrl: new List<Uri> { new Uri(mediaUrl) }
            );
        }

    }
}
