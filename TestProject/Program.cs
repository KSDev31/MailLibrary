using MailLibrary;
string customHtml = await File.ReadAllTextAsync("customHtml.html");
EmailSender mail = new("musiccorreo225@gmail.com", "vxyj masg pwzj hxes", customHtml);
List<string> emails = ["josepaz12.32@gmail.com", "josepaz12.32@hotmail.com"];
Tuple<bool, string> sended = await mail.SendMail("Mail library", "<h1>TEXTOJP</h1>", null, emails);

if (!sended.Item1)
{
    Console.WriteLine("no se envió");
}
else
{
    Console.WriteLine("Se envió a: " + sended.Item2);
}