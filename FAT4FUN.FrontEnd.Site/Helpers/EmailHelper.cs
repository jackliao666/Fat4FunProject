using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Helpers
{
    public class EmailHelper
    {
        public virtual void SendFromGmail(string from, string to, string subject, string body)
        {
            try
            {
                // 验证收件人邮箱是否为空
                if (string.IsNullOrWhiteSpace(to))
                {
                    throw new ArgumentException("收件人信箱不能為空");
                }

                // 使用简单的正则表达式或者内置的 `MailAddress` 来验证邮箱格式
                if (!IsValidEmail(to))
                {
                    throw new ArgumentException("收件人信箱格式不正確");
                }

                var smtpAccount = from;
                var smtpPassword = "yivv ndxd fwuc efav"; // 应用专用密码
                var smtpServer = "smtp.gmail.com";
                var SmtpPort = 587;

                var mms = new MailMessage
                {
                    From = new MailAddress(smtpAccount),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    SubjectEncoding = Encoding.UTF8
                };

                // 添加收件人邮箱
                mms.To.Add(new MailAddress(to));

                using (var client = new SmtpClient(smtpServer, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpAccount, smtpPassword); // 使用应用专用密码
                    client.Send(mms); // 发送邮件
                }
            }
            catch (Exception ex)
            {
                // 捕获并输出异常信息
                Console.WriteLine("郵件發送失敗: " + ex.Message);
                throw;
            }
        }

        // 使用 MailAddress 类来验证邮箱格式
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}