using System;
using System.IO;
using System.Text;

namespace SmtpParser
{
    public class MessageParser
    {
        private const string END_OF_LINE = "\r\n";
        private MessageBuilder _builder;
        
        public MessageParser(MessageBuilder builder) {
            _builder = builder;
        }

        public Message Parse(Stream messageStream) {
            StringBuilder messageData = new StringBuilder();
            Byte[] buffer = new Byte[1024];
            while (true)
            {
                int count = messageStream.Read(buffer, 0, buffer.Length);
                messageData.Append(Encoding.ASCII.GetString(buffer, 0, count));
                if (count < buffer.Length) break;
            }

            return Parse(messageData.ToString());
        }

        public Message Parse(Byte[] messageData) {
            return Parse(Encoding.UTF8.GetString(messageData));
        }

        public Message Parse(string messageData)
        {
            var messageFragments = messageData.Split(END_OF_LINE.ToCharArray());
            return Parse(messageFragments);
        }

        public Message Parse(string[] messageData)
        {
            //check if message is succeded
            foreach(var fragment in messageData)
            {
                _builder.SetSender(fragment);
                _builder.SetReceiver(fragment);
                _builder.SetSubject(fragment);
                _builder.SetDate(fragment);
                _builder.SetContentType(fragment);
                _builder.SetBody(fragment);
                _builder.SetAttachment(fragment);
            }
            return _builder.GetResult();
        }

    }
}
