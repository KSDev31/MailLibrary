using System.Net.Mail;
using MailLibrary.Constants;
using System.Net;

namespace MailLibrary
{
    public class EmailSender
    {
        private string _from { get; set; } = string.Empty;
        private string _password { get; set; } = string.Empty;

        private List<string> _allowedFormats = 
        [
            ".JSON",
            ".PDF",
            ".DOC",
            ".DOCX",
            ".XLS",
            ".XLSX",
            ".PPT",
            ".PPTX"
        ];

        private int _maxFileSize = 20000; // In KB

        // ToDo: No agarra los estilos que se le ponen en el html - Solo si son ingresados a mano? Why?
        private string? _body { get; set; } = null;

        public EmailSender(string from, string password, string? body = null)
        {
            _from = from;
            _password = password;
            _body = body;
        }

        public async Task<Tuple<bool, string>> SendMail(string subject, string contentToInsertInHtml, string? to = null, List<string>? recipientList = null, List<Attachment>? attachments = null)
        {
            try
            {
                string html = await HandleAndReplaceContentHTML(_body is not null, contentToInsertInHtml);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (SmtpClient stmpClient = new(EmailSmtp.Gmail, EmailPort.Gmail))
                {
                    // Set the credentials and enable ssl
                    stmpClient.EnableSsl = true;
                    stmpClient.Credentials = new NetworkCredential(_from, _password);

                    MailAddress? fromAddress = null;
                    List<MailAddress>? addresses = ValidateEmailAddresses(to, recipientList);
                    if (addresses is null || !addresses.Any())
                    {
                        return new(false, "-1");
                    }
                    bool validFromAddress = MailAddress.TryCreate(_from, out fromAddress);
                    if (!validFromAddress)
                    {
                        return new(false, "-2");
                    }
                    MailMessage mailmessage = new()
                    {
                        From = fromAddress!,
                        Subject = subject,
                        Body = html,
                        IsBodyHtml = true,
                    };
                    foreach (MailAddress toAddress in addresses)
                    {
                        mailmessage.To.Add(toAddress);
                    }
                    List<Attachment>? confirmedAttachments = new();
                    if (attachments is not null)
                    {
                        confirmedAttachments = ValidateAttachments(attachments);
                        if (confirmedAttachments is null)
                        {
                            return new(false, "-3");
                        }
                        else
                        {
                            foreach (Attachment attachment in confirmedAttachments)
                            {
                                mailmessage.Attachments.Add(attachment);
                            }
                        }
                    }
                    await stmpClient.SendMailAsync(mailmessage);
                    return new(true, addresses.Count.ToString());
                }
            }
            catch
            {
                return new(false, "-4");
            }
        }

        private List<MailAddress>? ValidateEmailAddresses(string? to = null, List<string>? recipientList = null)
        {
            try
            {
                // Validate if the data received is correct for create the correct mail recipient list
                if ((!string.IsNullOrWhiteSpace(to) && recipientList is not null) 
                    || (recipientList is not null && !recipientList.Any())
                    || (string.IsNullOrEmpty(to) && (recipientList is null || !recipientList.Any()))
                    || (string.IsNullOrEmpty(to) && (recipientList is null || !recipientList.Any())))
                {
                    return null;
                }

                List<MailAddress> recipientListConfirmed = new();
                if (recipientList is not null)
                {
                    foreach (string potentialMail in recipientList)
                    {
                        bool validAddress = MailAddress.TryCreate(potentialMail, out MailAddress? address);
                        if (validAddress)
                        {
                            recipientListConfirmed.Add(address!);
                        }
                    }
                }
                else
                {
                    bool validAddress = MailAddress.TryCreate(to, out MailAddress? address);
                    if (validAddress)
                    {
                        recipientListConfirmed.Add(address!);
                    }
                }
                return recipientListConfirmed;
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> HandleAndReplaceContentHTML(bool customBody, string content)
        {
            try
            {
                string html = string.Empty;
                if (customBody)
                {
                    html = _body!.Replace("{{Content}}", content); 
                }
                else
                {
                    html = await File.ReadAllTextAsync("defaultBody.html");
                    html = html.Replace("{{Content}}", content);
                }
                return html;
            }
            catch
            {
                return string.Empty;
            }
        }

        private List<Attachment>? ValidateAttachments(List<Attachment> attachments)
        {
            try
            {
                List<Attachment> confirmedAttachments = [];
                foreach (Attachment attachment in attachments)
                {
                    if (!_allowedFormats.Any(x => Path.GetExtension(attachment.Name!).ToLower().Equals(x.ToLower())))
                    {
                        return null;
                    }
                    if (attachment.ContentStream.Length > (_maxFileSize * 1024))
                    {
                        return null;
                    }
                    confirmedAttachments.Add(attachment);
                }
                return attachments;
            }
            catch
            {
                return null;
            }
        }
    }
}