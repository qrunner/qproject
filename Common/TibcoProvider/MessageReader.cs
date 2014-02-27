using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    public class MessageReader : MessageWorkerBase, IMessageReader
    {
        private MessageConsumer _msgConsumer;

        public MessageReader(MessageService prv, string queueName) : base(prv, queueName)
        {
        }

        protected override void AfterReconnect()
        {
            _msgConsumer = Session.CreateConsumer(Destination);
            Session.Connection.Start();
        }

        public IMessage Read()
        {
            CheckConnection();

            TextMessage tm = null;
            try
            {
                tm = (TextMessage) _msgConsumer.ReceiveNoWait();
            }
            catch (IllegalStateException)
            {
                Reconnect();
                tm = (TextMessage)_msgConsumer.ReceiveNoWait();
            }

            return tm != null ? new TibcoTextMessage(tm) : null;
        }

        public IEnumerable<IMessage> ReadAll()
        {
            CheckConnection();

            while (true)
            {
                TextMessage tm = (TextMessage)_msgConsumer.ReceiveNoWait();
                if (tm != null)
                    yield return new TibcoTextMessage(tm);
                else
                    yield break;
            }
        }

        public event EventHandler<MessageRecievedEventArgs> OnMessageRecieved;
    }
}