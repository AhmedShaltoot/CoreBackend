using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using RFIDBLL.Services.Contracts;
using RFIDDAL.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RFIDBLL.Services.Services
{
    public class NotificationService : INotificationService
    {
        protected IRepositoryWrapper _repoWrapper;
        public NotificationService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public async Task<string> SendOneClientMobileMessage(string mobile, string message)
        {
            //SaveNotifications(message);
            try
            {
                if (mobile.StartsWith("0"))
                {
                    mobile = "2" + mobile;
                }
                else if (mobile.StartsWith("20"))
                {
                    mobile = mobile;
                }
                else
                {
                    mobile = "20" + mobile;
                }
                var values = new Dictionary<string, string> { };

                var queryParams = HttpUtility.ParseQueryString(string.Empty);
                queryParams["environment"] = "1";
                queryParams["username"] = "2jIGfHso";
                queryParams["password"] = "pQPSg3qeuW";
                queryParams["language"] = "1";
                queryParams["sender"] = "5552fa9319f3972b3b9f298365a0834e89772df5c1b1642c1d3ba2a48ca1b7be";
                queryParams["mobile"] = mobile;
                queryParams["message"] = message;
                queryParams["DelayUntil"] = "";
                var responseString = "";
                string baseUrl = "https://smsmisr.com/api/SMS/";
                string fullUrl = baseUrl + "?" + queryParams.ToString();

                using (var clientshop = new HttpClient())
                {
                    var content = new StringContent("", System.Text.Encoding.UTF8, "application/json");
                    try
                    {
                        var response = await clientshop.PostAsync(fullUrl, content);
                        // Process the response as needed
                        responseString = await response.Content.ReadAsStringAsync();
                    }
                    catch
                    {
                        return "";
                    }
                }
                return responseString;
            }
            catch (Exception ex)
            {
                string str = "error";
                return str;
            }
        }
    }
}
