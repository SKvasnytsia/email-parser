using System.Collections.Generic;

namespace SmtpParser
{
    public class MessageBuilder
    {
        private const string SENDER = "From:";
        private const string RECEIVER = "To:";
        private const string SUBJECT = "Subject:";
        private const string DATE = "Date:";
        private const string BODY = "------=_NextPart";
        private const string CONTENT_TYPE = "Content-Type:";
        private const string CONTENT_TYPE_HTML = "text/html;";
        private const string CONTENT_TYPE_TXT = "text/plain;";
        private const string CHARSET = "charset";
        private const string CONTENT_TRANSFER_ENCODING = "Content-Transfer-Encoding";
        private const string ATTACHMENT_INFO = "Content-Disposition: attachment;";
        private const string FILENAME = "filename=";
        private const string ATTACHMENT_END = "--_";

        private Message _message;
        private bool _readingToFlag = false;
        private bool _readingMessageBodyFlag = false;
        private bool _readingAttachmentFlag = false;

        public MessageBuilder()
        {
            _message = new Message();
        }

        public void SetSender(string text)
        {
            if (text.StartsWith(SENDER))
                _message.From = text.Split('"')[1];
        }

        public void SetReceiver(string text)
        {
            if (text.StartsWith(RECEIVER))
            {
                _readingToFlag = true;
                _message.To = new List<string>() { text.Split('<')[1].Replace(">", "").Replace(",", "") };
            }
            if (!string.IsNullOrEmpty(text) && !text.StartsWith(RECEIVER) && !text.StartsWith(SUBJECT) && _readingToFlag)
            {
                _message.To.Add(text.Split('<')[1].Replace(">", "").Replace(",", ""));
            }
            if (text.StartsWith(SUBJECT))
            {
                _readingToFlag = false;
            }
        }

        public void SetSubject(string text)
        {
            if (text.StartsWith(SUBJECT))
                _message.Subject = text.Substring(SUBJECT.Length + 1);
        }

        public void SetDate(string text)
        {
            if (text.StartsWith(DATE))
                _message.SentDate = text.Substring(DATE.Length + 1);
        }

        public void SetContentType(string text)
        {
            if (text.StartsWith(CONTENT_TYPE) && 
                    (text.Substring(CONTENT_TYPE.Length + 1) == CONTENT_TYPE_TXT || 
                    text.Substring(CONTENT_TYPE.Length + 1) == CONTENT_TYPE_HTML))
            {
                _message.ContentType = text.Substring(CONTENT_TYPE.Length + 1);
                _readingMessageBodyFlag = true;
            }
        }

        public void SetBody(string text)
        {
            if (_readingMessageBodyFlag && text.StartsWith(BODY))
            {
                _readingMessageBodyFlag = false;
            }
            if (_readingMessageBodyFlag && !text.StartsWith(CONTENT_TYPE) && !text.Contains(CHARSET) && !text.Contains(CONTENT_TRANSFER_ENCODING))
            {
                _message.Body += text;
            }
        }

        public void SetAttachment(string text)
        {
            if (text.StartsWith(ATTACHMENT_INFO))
            {
                _readingAttachmentFlag = true;
                _message.Attachment = new Attachment();
                _message.Attachment.FileName = text.Split('\"')[1];
            }
            if (text.Contains(ATTACHMENT_END))
            {
                _readingAttachmentFlag = false;
            }
            if (_readingAttachmentFlag && !string.IsNullOrEmpty(text))
            {
                _message.Attachment.RawData += text;
            }
        }

        public Message GetResult()
        {
            return _message;
        }
    }
}
