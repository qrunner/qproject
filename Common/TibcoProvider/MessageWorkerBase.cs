using System;
using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    public abstract class MessageWorkerBase : IDisposable
    {
        protected Session Session = null;
        protected Destination Destination = null;
        protected MessageService Provider = null;
        protected string QueueName = null;

        protected MessageWorkerBase(MessageService prv, string queueName)
        {
            Provider = prv;
            QueueName = queueName;
        }

        public void Reconnect()
        {
            if (Session != null && !Session.IsClosed)
                Session.Close();

            Provider.Connect();
            Session = Provider.Connection.CreateSession(false, SessionMode.AutoAcknowledge);
            Destination = Session.CreateQueue(QueueName);
            AfterReconnect();
        }

        protected void CheckConnection()
        {
            if (!Provider.Connected || Session.IsClosed) Reconnect();
            if (!Provider.Connected || Session.IsClosed) throw new Exception(MessageService.ErrNoConnection);
        }

        protected abstract void AfterReconnect();

        public void Dispose()
        {
            Session.Close();
        }
    }
}