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
using RFIDDAL.Repositories.Contracts;

namespace RFIDBLL.Services.Services
{
    public class EmailService : IEmailService
    {
        protected IRepositoryWrapper _repoWrapper;
        public EmailService(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        public string SendTicketEmail(int id, string mailTo, int status)
        {
            string incryptId = id.ToString();
            string secretKey = "NT$12#LM$12";
            string incryptId256Hash = HashingUtils.ComputeSHA256Hash(incryptId, secretKey);
            string url = "https://tltevents.tlt.co.com/TicketReservationDetails/" + id + "/" + incryptId256Hash;


            var mailMessage = new MailMessage
            {
                From = new MailAddress("Tlt.events@tltconcepts.com"),
                Subject = "",
                Body = "",
                IsBodyHtml = true,
            };
            mailMessage.Subject = "TLT Events - Reservation Update";

            mailMessage.Body += "<div>";
            mailMessage.Body += "<p class='MsoNormal'>";
            mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
            mailMessage.Body += "&quot;Times New Roman&quot;;color:#232A2A'>Hello," + " </span><span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:&quot;Times New Roman&quot;;";
            mailMessage.Body += "color:black'><o:p></o:p></span></p>";

            mailMessage.Body += "<p class='MsoNormal'>";
            mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
            mailMessage.Body += "&quot;Times New Roman&quot;;color:black'><br /> <o:p></o:p></span></p>";

            if (status == 2) // pending
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking request has been submitted successfully. We will review it and get back to you soon. <br /> <br /> You can follow up your booking through the following link <br />" + url + " <br /><br /> <br />  <o:p></o:p></span></p>";
            }
            else if (status == 3) // In Payment
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking status has been updated. <br /> <br /> Please follow up your booking through the following link <br />" + url + " <br /><br /> <br />  <o:p></o:p></span></p>";
            }
            else if (status == 4)  // Confirmed
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Your payment has been received. Your booking now is confirmed. Please use the QR codes in your booking link for entrance. <br />" + url + " <br /><br /> <br />  <o:p></o:p></span></p>";
            }
            else if (status == 5)  // Rejected
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> We are sorry to inform you that your booking has been disconfirmed as we are fully booked. <br /><br />  <o:p></o:p></span></p>";
            }
            else if (status == 6)  // Cancelled
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking has been cancelled. <br /><br />  <o:p></o:p></span></p>";
            }
            else if (status == 9)  // Specific email msg
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Thank you for submitting your payment. <br /><br /> Our House Rules:  <br /><br /> -Doors close at 2:00 AM sharp.  <br /> -Misdemeanor upon entry or inside the venue entitles future entry to be unwelcome for future reservations. <br /> -Minimum age is 25. <br /> - All reservations are subject to DOOR SELECTION. <br /> - A QR Code must be presented upon entry.<br /> -If you are facing any difficulties with our payment or QR code please visit our Registration  Entrance in our venue upon arrival. <br /> -NO RE-ENRTY <br /> Please be present with a picture ID for reference. <o:p></o:p></span></p>";
            }
            else if (status == 10)  // Specific email msg
            {
                mailMessage.Body += "<p class='MsoNormal'>";
                mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
                mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Announcement  <br /> Unfortunately, Maceo Plex won't be able to make it tonight as he missed his connection flight.  <br /> But rest assured, we've arranged for Heavy Pins & Mohasseb to take over the decks along with Misty and Moenes to ensure a great night!  <br /><br /> If you wish to refund kindly note that you will be able to request a refund using your booking link within the coming 48 hours. <o:p></o:p></span></p>";
            }

            mailMessage.Body += "<p class='MsoNormal'>";
            mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
            mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Sincerely,<br /> TLT Events. <o:p></o:p></span></p>";


            mailMessage.Body += "</div>";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("Tlt.events@tltconcepts.com", "unedzwgohamyrkEvqj8c"),
                EnableSsl = true,
            };
            mailMessage.To.Add(mailTo);

            try
            {
                smtpClient.Send(mailMessage);
                return incryptId256Hash;

            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return incryptId256Hash;
            }

            //#endregion

        }

        //public string SendTicketEmail(int id, string mailTo, int status)
        //{
        //    string incryptId = id.ToString();
        //    string secretKey = "NT$12#LM$12";
        //    string incryptId256Hash = HashingUtils.ComputeSHA256Hash(incryptId, secretKey);

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("ritualsofzoya@tltconcepts.com"),
        //        Subject = "",
        //        Body = "",
        //        IsBodyHtml = true,
        //    };
        //    mailMessage.Subject = "Rituals Of Zoya";

        //    mailMessage.Body += "<div>";
        //    mailMessage.Body += "<p class='MsoNormal'>";
        //    mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //    mailMessage.Body += "&quot;Times New Roman&quot;;color:#232A2A'>Hello," + " </span><span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:&quot;Times New Roman&quot;;";
        //    mailMessage.Body += "color:black'><o:p></o:p></span></p>";

        //    mailMessage.Body += "<p class='MsoNormal'>";
        //    mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //    mailMessage.Body += "&quot;Times New Roman&quot;;color:black'><br /> <o:p></o:p></span></p>";

        //    if (status == 2) // pending
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking request has been submitted successfully. We will review it and get back to you soon. <br /> <br /> You can follow up your booking through the following link <br /> http://lemonada.tlt.co.com/Redirect/TicketDetails?id=" + id + "&resId=" + incryptId256Hash + " <br /><br /> <br />  <o:p></o:p></span></p>";
        //    }
        //    else if (status == 3) // In Payment
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking status has been updated. <br /> <br /> Please follow up your booking through the following link <br /> http://lemonada.tlt.co.com/Redirect/TicketDetails?id=" + id + "&resId=" + incryptId256Hash + " <br /><br /> <br />  <o:p></o:p></span></p>";
        //    }
        //    else if (status == 4)  // Confirmed
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Your payment has been received. Your booking now is confirmed. Please use the QR codes in your booking link for entrance. <br /> http://lemonada.tlt.co.com/Redirect/TicketDetails?id=" + id + "&resId=" + incryptId256Hash + " <br /><br /> <br />  <o:p></o:p></span></p>";
        //    }
        //    else if (status == 5)  // Rejected
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> We are sorry to inform you that your booking has been disconfirmed as we are fully booked. <br /><br />  <o:p></o:p></span></p>";
        //    }
        //    else if (status == 6)  // Cancelled
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Please be informed that your booking has been cancelled. <br /><br />  <o:p></o:p></span></p>";
        //    }
        //    else if (status == 9)  // Specific email msg
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Thank you for submitting your payment. <br /><br /> Our House Rules:  <br /><br /> -Doors close at 2:00 AM sharp.  <br /> -Misdemeanor upon entry or inside the venue entitles future entry to be unwelcome for future reservations. <br /> -Minimum age is 25. <br /> - All reservations are subject to DOOR SELECTION. <br /> - A QR Code must be presented upon entry.<br /> -If you are facing any difficulties with our payment or QR code please visit our Registration  Entrance in our venue upon arrival. <br /> -NO RE-ENRTY <br /> Please be present with a picture ID for reference. <o:p></o:p></span></p>";
        //    }
        //    else if (status == 10)  // Specific email msg
        //    {
        //        mailMessage.Body += "<p class='MsoNormal'>";
        //        mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //        mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Announcement  <br /> Unfortunately, Maceo Plex won't be able to make it tonight as he missed his connection flight.  <br /> But rest assured, we've arranged for Heavy Pins & Mohasseb to take over the decks along with Misty and Moenes to ensure a great night!  <br /><br /> If you wish to refund kindly note that you will be able to request a refund using your booking link within the coming 48 hours. <o:p></o:p></span></p>";
        //    }

        //    mailMessage.Body += "<p class='MsoNormal'>";
        //    mailMessage.Body += "<span style='font-family:&quot;Calibri&quot;,sans-serif;mso-fareast-font-family:";
        //    mailMessage.Body += "&quot;Times New Roman&quot;;color:black'> Sincerely,<br /> Rituals Of Zoya. <o:p></o:p></span></p>";


        //    mailMessage.Body += "</div>";

        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new System.Net.NetworkCredential("ritualsofzoya@tltconcepts.com", "rituals.12345"),
        //        EnableSsl = true,
        //    };
        //    mailMessage.To.Add(mailTo);

        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        return incryptId256Hash;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex);
        //        return incryptId256Hash;
        //    }

        //    //#endregion

        //}


    }
}
