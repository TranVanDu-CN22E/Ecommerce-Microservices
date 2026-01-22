using IdentityService.Application.Abstractions.Services;
using IdentityService.Domain.Aggregates.UserAggregate;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;

namespace IdentityService.Infrastructure.Mail
{
    public class EmailSender : IEmailService
    {
        private readonly EmailSetting _setting;
        public EmailSender(IOptions<EmailSetting> options) => _setting = options.Value;
        public async Task SendWelcomeEmailAsync(string email, string userName, CancellationToken ct)
        {
            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(_setting.SenderEmail, _setting.SenderName),
                    Subject = "Chào mừng bạn đến với chúng tôi.",
                    IsBodyHtml = true,
                    Body = $@"
                    <html>
                        <body style='font-family: Roboto, sans-serif; font-size: 18px; color: #333; line-height: 1.6; width: 100%;'>
                            <h2 style='color: #2E86C1; text-align: center;'>Xin chào {userName}</h2>
                            <p>Bạn đã đăng ký tài khoản thành công trên hệ thống của <b>Ecommerce-Microservices</b>.</p>
                            <p>Vui lòng nhấn vào liên kết dưới đây để kích hoạt tài khoản của bạn:</p>
                            <p style='text-align: center; font-weight: bolder; text-decoration: underline;'><a href='http://example.com/activate?code=123456' style='color: #f60505; text-decoration: none;'>Kích hoạt tài khoản</a></p>
                            <p>Nếu bạn không đăng ký tài khoản này, vui lòng bỏ qua email này.</p>
                            <p>Trân trọng,<br/>Đội ngũ hỗ trợ khách hàng</p>
                        </body>
                    </html>"
                };
                mail.To.Add(email);

                using var smtp = new SmtpClient(_setting.SmtpServer, _setting.Port)
                {
                    Credentials = new NetworkCredential(_setting.SenderEmail, _setting.SenderPassword),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail, ct);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task SendAccountLockedEmailAsync(string email, string userName, string reason, DateTime? lockedUntil, CancellationToken ct)
        {
            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(_setting.SenderEmail, _setting.SenderName),
                    Subject = "Thông báo về tài khoản của bạn.",
                    IsBodyHtml = true,
                    Body = $@"
                    <html>
                        <body style='font-family: Roboto, sans-serif; font-size: 18px; color: #333; line-height: 1.6; width: 100%;'>
                            <h2 style='color: #2E86C1; text-align: center;'>Xin chào {userName}</h2>
                            <p>Tài khoản của bạn trên hệ thống của <b>Ecommerce-Microservices</b> đã bị khóa.</p>
                            <p>Tài khoản của bạn đã vi phạm {reason} và bị khóa vào lúc {lockedUntil}.</p>
                            <p>Trân trọng,<br/>Đội ngũ hỗ trợ khách hàng</p>
                        </body>
                    </html>"
                };
                mail.To.Add(email);

                using var smtp = new SmtpClient(_setting.SmtpServer, _setting.Port)
                {
                    Credentials = new NetworkCredential(_setting.SenderEmail, _setting.SenderPassword),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail, ct);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task SendAccountOtpEmailAsync(string email, string otp, CancellationToken ct)
        {
            try
            {
                using var mail = new MailMessage
                {
                    From = new MailAddress(_setting.SenderEmail, _setting.SenderName),
                    Subject = "Mã xác thực của bạn.",
                    IsBodyHtml = true,
                    Body = $@"
                    <html>
                        <body style='font-family: Roboto, sans-serif; font-size: 18px; color: #333; line-height: 1.6; width: 100%;'>
                            <p>Mã otp của bạn là: </p>
                            <h2 style='color: #2E86C1; text-align: center;'>{otp}</h2>
                            <p>Nếu không phải bạn đăng nhập, vui lòng bỏ qua email này.</p>
                            <p>Trân trọng,<br/>Đội ngũ hỗ trợ khách hàng</p>
                        </body>
                    </html>"
                };
                mail.To.Add(email);

                using var smtp = new SmtpClient(_setting.SmtpServer, _setting.Port)
                {
                    Credentials = new NetworkCredential(_setting.SenderEmail, _setting.SenderPassword),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail, ct);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
