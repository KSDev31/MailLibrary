# Initialization

To start using the library, instantiate the "EmailSender" class, pass to the constructor the user and password of the email address that will be used to send emails.

# Usage

The library supports sending e-mails to a single user or to several users, only their e-mails must be provided.

The expected result is a tuple, which will indicate whether it mail was sent or not (boolean), and based on that the second value of the tuple will be the error code or a list of the users to whom the mail was sent.

The correct way to send mails is as follows ```<bool, string> sended = await mail.SendMail("Mail library", "<h1><TEXTOJP</h1>", null, emails, [attachment]);```
