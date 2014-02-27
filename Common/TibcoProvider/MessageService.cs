using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    public class MessageService : Provider, IMessageService
    {
        internal const string ErrNoConnection = "Соединение с Tibco не инициализировано.";

        private readonly ExceptionListener _exeptionListenerer;

        private TibcoConnectionString _cs = null;

        private ConnectionFactory _connectionFactory;

        public MessageService()
        {
            Connection = null;
            _exeptionListenerer = new ExceptionListener(this);
        }

        public Connection Connection { get; private set; }

        protected override void Initialize(IDictionary<string, string> settings)
        {
            _cs = new TibcoConnectionString(ConnectionString);
            _connectionFactory = new ConnectionFactory(_cs.ServerUrl);
        }

        public override bool CheckConnection(out Exception ex)
        {
            ex = null;
            if (Connection == null)
            {
                ex = new Exception(ErrNoConnection);
                return false;
            }

            bool wasClosed = !Connected;
            try
            {
                Connect();
            }
            catch (Exception e)
            {
                ex = e;
                return false;
            }
            finally
            {
                if (wasClosed) Disconnect();
            }

            return true;
        }

        public void Connect()
        {
            if (Connected) return;

            Disconnect();

            try
            {
                Connection = _connectionFactory.CreateConnection(_cs.User, _cs.Password);
                Connection.ExceptionListener = _exeptionListenerer;
            }
            catch (Exception)
            {
                Disconnect();
                throw;
            }
        }

        protected virtual void AfterConnect()
        {

        }

        protected virtual void BeforeDisconnect()
        {

        }

        public void Disconnect()
        {
            try
            {
                BeforeDisconnect();
            }
            finally
            {
                if (Connection != null)
                {
                    if (!Connection.IsClosed)
                    {
                        Connection.Stop();
                        Connection.Close();
                    }
                }
            }
        }

        public bool Connected
        {
            get { return Connection != null && !Connection.IsDisconnected() && !Connection.IsClosed; }
        }

        public IMessageReader CreateReader(string queueName)
        {
            MessageReader retval = new MessageReader(this, queueName);
            retval.Reconnect();
            return retval;
        }

        public IMessageSender CreateSender(string queueName)
        {
            MessageSender retval = new MessageSender(this, queueName);
            retval.Reconnect();
            return retval;
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void OnException(EMSException exception)
        {
            Connect();
        }
    }
}