using System;
using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    public class MessageSender : MessageWorkerBase, IMessageSender
    {
        private MessageProducer _msgProducer;

        public MessageSender(MessageService prv, string queueName) : base(prv, queueName)
        {
        }

        protected override void AfterReconnect()
        {
            _msgProducer = Session.CreateProducer(Destination);
        }

        public IMessage NewMessage()
        {
            CheckConnection();

            return new TibcoTextMessage(Session.CreateTextMessage());
        }

        public void Send(IMessage message)
        {
            CheckConnection();

            _msgProducer.Send(((TibcoTextMessage) message).TibcoMsg);
        }

        public void Send(IMessage message, Action<IMessage> sended)
        {
            CheckConnection();

            _msgProducer.Send(((TibcoTextMessage) message).TibcoMsg);
            if (sended != null)
                sended(message);
        }
    }
}