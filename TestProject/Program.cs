using MailLibrary;
using System.Net.Mail;
string customHtml = await File.ReadAllTextAsync("customHtml.html");
EmailSender mail = new("test@gmail.com", "vxyj masg pwzj hxes", customHtml);


Attachment attachment = new("bootstrap.css");

List<string> emails = ["test@gmail.com", "test@hotmail.com"];
Tuple<bool, string> sended = await mail.SendMail("Mail library", "<h1>TEXTOJP</h1>", null, emails, [attachment]);

if (!sended.Item1)
{
    Console.WriteLine("no se envió");
}
else
{
    Console.WriteLine("Se envió a: " + sended.Item2);
}