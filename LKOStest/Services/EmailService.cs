using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LKOStest.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using SmtpServer;

namespace LKOStest.Services
{
    public interface IEmailService
    {
        bool SendEmail(EmailRequest request);
        void LogSentEmail(SentEmail email);
        List<SentEmail> GetClientEmails(string clientId);
        bool SendAfterLogin(EmailRequest emailRequest);
    }

    public class EmailService : IEmailService
    {
        public TripContext tripContext;

        public EmailService(TripContext tripContext)
        {
            this.tripContext = tripContext;
        }

        public void LogSentEmail(SentEmail email)
        {
            tripContext.SentEmails.Add(email);
            var client = tripContext.Clients.FirstOrDefault(c => c.Id == email.Client.Id);
            client.Notified = true;
            tripContext.Clients.Update(client);
            tripContext.SaveChanges();
        }

        public List<SentEmail> GetClientEmails(string clientId)
        {
            var sentEmails = tripContext.SentEmails.Include(s=>s.Trip).Where(s => s.Client.Id == clientId).ToList();
            return sentEmails;
        }

        public bool SendAfterLogin(EmailRequest request)
        {
            var ctx = new ValidationContext(request);

            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, ctx, results, true))
            {
                if (results.Any())
                {
                    return false;
                }
            }

            var message = new MimeMessage();

            var from = new MailboxAddress("Tavis",
                "em.zareckis@gmail.com");
            message.From.Add(from);

            var to = new MailboxAddress(request.Receiver,
                request.ReceiverEmail);
            message.To.Add(to);

            message.Subject = "Access to your trip";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h4>Hello {request.Receiver},</h4>\r\n<p>Thank You for using TAVIS services.</p>\r\n<p>You have been assigned to trip {request.TripTitle}.</p>\r\n<hr>\r\n\r\n<p>Your credentials are the same as you have used previously.</p>\r\n<hr>\r\n<p>For any questions please contact {request.SenderEmail}.</p>\r\n<p>Best regards,</p>\r\n<p>TAVIS team.</p>",
                TextBody = "TextBody!"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            client.Connect("smtp-relay.sendinblue.com", 587, false);
            var user = "em.zareckis@gmail.com";
            var password = "0IzLbEqjhFH6gsfD";

            client.Authenticate(user, password);

            client.Send(message);

            client.Disconnect(true);

            return true;
        }

        public bool SendEmail(EmailRequest request)
        {
            var ctx = new ValidationContext(request);

            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, ctx, results, true))
            {
                if (results.Any())
                {
                    return false;
                }
            }

            var message = new MimeMessage();

            var from = new MailboxAddress("Tavis",
                "em.zareckis@gmail.com");
            message.From.Add(from);

            var to = new MailboxAddress(request.Receiver,
                request.ReceiverEmail);
            message.To.Add(to);

            message.Subject = "Access to your trip";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h4>Hello {request.Receiver},</h4>\r\n<p>Thank You for using TAVIS services.</p>\r\n<p>You have been assigned to trip {request.TripTitle}.</p>\r\n<hr>\r\n<p>Your personal login credentials to TAVIS application:</p>\r\n<p>Username: {request.ReceiverEmail}</p>\r\n<p>One-time password: {request.GeneratedPassword}</p>\r\n<p>Change password to your own as soon as you log in to application.</p>\r\n<hr>\r\n<p>For any questions please contact {request.SenderEmail}.</p>\r\n<p>Best regards,</p>\r\n<p>TAVIS team.</p>", 
                TextBody = "TextBody!"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            client.Connect("smtp-relay.sendinblue.com", 587, false);
            var user = "em.zareckis@gmail.com";
            var password = "0IzLbEqjhFH6gsfD";
            
            client.Authenticate(user, password);

            client.Send(message);

            client.Disconnect(true);

            return true;
        }
    }

    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; }
        [Required]
        public string TripTitle { get; set; }
        
        public string GeneratedPassword { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        [EmailAddress]
        public string ReceiverEmail { get; set; }

    }
}
