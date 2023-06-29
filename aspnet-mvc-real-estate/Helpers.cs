using aspnet_mvc_real_estate.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

namespace aspnet_mvc_real_estate
{
    public static class MessageHelpers
    {
        // SMTP Config here ...
        private static string FromEmail = "luunhatthanha3@gmail.com";
        private static string FromName = "[Skylands] Support Team";
        private static string Host = "smtp.gmail.com";
        private static int Port = 587;
        private static string UserName = "luunhatthanha3@gmail.com";
        private static string Password = "ondsnqvgplsksgdg";

        // Send Email using SMTP server settings
        public static void SendEmail(EmailModels models)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(models.To);
            mail.From = new MailAddress(FromEmail, FromName);
            mail.Subject = models.Subject;
            string Body = models.Body;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.Port = Port;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        // Send email for multi receiver
        public static void SendEmail(EmailModels models, List<string> ccmail)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(models.To);
            mail.From = new MailAddress(FromEmail, FromName);
            mail.Subject = models.Subject;
            string Body = models.Body;
            mail.Body = Body;
            foreach (string ccmailitem in ccmail)
            {
                mail.CC.Add(ccmailitem);
            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.Port = Port;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
    public static class HashHelpers
    {
        // Calculate SHA-512 hash of the input string
        public static string ComputeSha512Hash(string rawData)
        {
            var bytes = Encoding.UTF8.GetBytes(rawData);
            using (var sha512Hash = SHA512.Create())
            {
                var hashBytes = sha512Hash.ComputeHash(bytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hashString;
            }
        }

        // Verify if the hash of the input string is equal to a given hash
        public static bool VerifySha512Hash(string rawData, string hashString)
        {
            var bytes = Encoding.UTF8.GetBytes(rawData);
            using (var sha512Hash = SHA512.Create())
            {
                var hashBytes = sha512Hash.ComputeHash(bytes);
                var hashStringToCompare = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return (hashStringToCompare == hashString);
            }
        }
    }
    public static class GenerateHelpers
    {
        public static string GenerateStrongPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            var random = new Random();
            char[] passwordArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordArray[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(passwordArray);
        }
        public static string GenerateSlug(string input)
        {
            if (input == null) return "";
            // Chuẩn hóa chuỗi
            string output = input.Normalize(NormalizationForm.FormKD);
            // Loại bỏ các dấu gạch dưới, dấu chấm, dấu hỏi đầu câu, các kí tự đặc biệt khác
            output = Regex.Replace(output, @"[\p{Pd}\p{Pc}\p{Po}]+", " ");
            // Đổi các kí tự viết hoa thành kí tự viết thường
            output = output.ToLower();
            // Thay thế khoảng trắng và kí tự đặc biệt bằng dấu gạch ngang
            output = Regex.Replace(output, @"[\s_]+", "-");
            // Loại bỏ các kí tự không thuộc bảng mã ASCII (basic Latin)
            output = Regex.Replace(output, @"[^\p{IsBasicLatin}]", "");
            return output;
        }

        public static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
    public static class DataTypeHelpers
    {
        public static string FormatCurrency(Decimal amount)
        {
            if (amount < 1000000)
            {
                return amount.ToString() + "đ";
            }
            else if (amount >= 1000000 && amount < 1000000000)
            {
                Decimal millions = amount / 1000000;
                return millions.ToString() + " Triệu";
            }
            else if (amount >= 1000000000)
            {
                Decimal billions = amount / 1000000000;
                return billions.ToString().Replace(". ", ", ") + " Tỷ";
            }
            else
            {
                return "Error";
            }
        }
    }
    public static class GetHelpers
    {
        public static string GetIDYoutube(string youtubeUrl)
        {
            string[] parts = youtubeUrl.Split(new char[] { '=', '&' });
            string videoId = parts[1];
            return videoId;
        }
        public static string GetCurrentUrl()
        {
            var httpContext = HttpContext.Current;
            var request = httpContext.Request;
            var urlBuilder = new UriBuilder(request.Url.AbsoluteUri)
            {
                Path = request.Url.AbsolutePath,
                Query = request.QueryString.ToString()
            };
            string scheme = urlBuilder.Scheme;
            string host = urlBuilder.Host;
            int port = urlBuilder.Port;
            if (port != 80 && port != 443)
            {
                host += ":" + port;
            }
            string pathAndQuery = urlBuilder.Uri.PathAndQuery;
            string fullUrl = $"{scheme}://{host}{pathAndQuery}";

            return fullUrl;
        }

    }
}