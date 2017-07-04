using System.Collections.Generic;

namespace SmtpParser
{
    public class Message
    {
        public string From { get; set; }

        public List<string> To { get; set; }

        public string Subject { get; set; }

        public string ContentType { get; set; }

        public string Body { get; set; }

        public string SentDate { get; set; }

        //may be list of attachments
        public Attachment Attachment { get; set; }

    }

    public class Attachment
    {
        public string FileName { get; set; }

        public string RawData { get; set; }
    }
}
