using DellyBelly.Application.Interfaces;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Domain.Entities;
using DellyBelly.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DellyBelly.Application.Services
{
    /**
     * GmailEmailService
     * ─────────────────────────────────────────────────────────────────────────────
     * Universal Email Engine for Delly Belly.
     * Uses Standard Gmail SMTP with SSL (Safe for Production).
     * Now features Global Quota Tracking sync'd via Database.
     * ─────────────────────────────────────────────────────────────────────────────
     */
    public class GmailEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public GmailEmailService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string purpose, bool isHtml = true)
        {
            bool isSuccess = false;
            try
            {
                using var client = GetSmtpClient();
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:FromEmail"], "Delly Belly Bakery"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                isSuccess = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL ERROR]: {ex.Message}");
                return false;
            }
            finally
            {
                // Always log the attempt to history
                await LogEmailAsync(toEmail, subject, purpose, isSuccess);
            }
        }

        public async Task<int> SendBroadcastAsync(IEnumerable<string> toEmails, string subject, string body, string purpose)
        {
            int successCount = 0;
            foreach (var email in toEmails)
            {
                var success = await SendEmailAsync(email, subject, body, purpose);
                if (success) successCount++;
                await Task.Delay(100); 
            }
            return successCount;
        }

        public async Task LogEmailAsync(string recipient, string subject, string purpose, bool success)
        {
            try 
            {
                var log = new EmailLog 
                { 
                    Recipient = recipient, 
                    Subject = subject, 
                    Purpose = purpose, 
                    SentAt = DateTimeHelper.GetIndianTime(), // Using Indian time helper
                    IsSuccess = success
                };
                _context.EmailLogs.Add(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL LOG ERROR]: {ex.Message}");
            }
        }

        public async Task<int> GetRemainingQuotaAsync()
        {
            try
            {
                var today = DateTimeHelper.GetIndianTime().Date;
                var sentTodayCount = await _context.EmailLogs
                    .Where(l => l.SentAt >= today && l.IsSuccess)
                    .CountAsync();

                return Math.Max(0, 500 - sentTodayCount);
            }
            catch
            {
                return 500;
            }
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _config["EmailSettings:FromEmail"], 
                    _config["EmailSettings:AppPassword"]
                )
            };
        }
    }
}
