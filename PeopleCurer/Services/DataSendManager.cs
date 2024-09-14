using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Numerics;

namespace PeopleCurer.Services
{
    internal static class DataSendManager
    {
        private static readonly MailAddress fromAdr = new("studieergebnisse@gmail.com");
        private static readonly MailAddress toAdr = new("lenni.kremling@gmail.com");

        private const string subject = "Studie - Neue Ergebnisse - ";
        //private const string password = "studieErgebnisse_24";
        private const string password = "nahhnregntcyubly";

        public static bool SendData()
        {
            Stream[] streams = [];

            try
            {
                SmtpClient smtp = new()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAdr.Address, password)
                };

                MailMessage mailMessage = new(fromAdr.Address, toAdr.Address, subject + DateTime.Now.Date.ToShortDateString(), string.Empty);
                streams = SerializationManager.GetStreams();
                for (int i = 0; i < streams.Length; i++)
                {
                    ContentType t = new()
                    {
                        MediaType = MediaTypeNames.Application.Json
                    };

                    Attachment attachment = new(streams[i], t);

                    mailMessage.Attachments.Add(attachment);
                }

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                for (int i = 0; i < streams.Length; i++)
                {
                    streams[i].Close();
                    streams[i].Dispose();
                }
            }

            return true;
        }

    }
}
