This library is designed to send mails in a fast and easy way, all suggestions will be accepted and valued to continue improving the tool.

At the moment the mail is just the content to send, but we are working to send it in a customizable mail format (in html).

# Initialization

To start using the library, instantiate the "EmailSender" class, pass to the constructor the user and password of the email address that will be used to send emails.

# Usage

The library supports sending e-mails to a single user or to several users, only their e-mails must be provided.

The expected result is a tuple, which will indicate whether it mail was sent or not (boolean), and based on that the second value of the tuple will be the error code or a list of the users to whom the mail was sent.

The basic operation consists of only sending the subject, the content of the mail and to which user it will be sent.<br>
```Tuple<bool, string> sended = await mail.SendMail("subject", "content", "mail@gmail.com")```

It is possible to send it to several users.<br>
```Tuple<bool, string> sended = await mail.SendMail("subject", "content", "mail@gmail.com")```

If you want to send the mail to several users, you must send a list of type chain containing all the mails.<br>
```Tuple<bool, string> sended = await mail.SendMail("subject", "content", null, recipientList)```

It is possible to send documents in the mail, you must send a list of type "Attachment", which is offered by Microsoft. 
The allowed file formats are Json, Pdf, Xls, Xlsx, Ppt, Pptx, Doc, Docx and the maximum size is 20 MB (per file).<br>
```Tuple<bool, string> sended = await mail.SendMail("subject", "content", null, recipientList, [attachments])```



