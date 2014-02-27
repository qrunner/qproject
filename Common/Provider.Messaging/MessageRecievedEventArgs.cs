using System;

namespace Provider.Messaging
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageRecievedEventArgs(IMessage message)
        {
            _message = message;
        }

        readonly IMessage _message;
        public IMessage Message { get { return _message; } }
    }
}