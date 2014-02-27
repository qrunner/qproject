using TIBCO.EMS;

namespace Provider.Messaging.Tibco
{
    class ExceptionListener : IExceptionListener
    {
        private readonly IMessageService _service;

        public ExceptionListener(IMessageService service)
        {
            _service = service;
        }

        public void OnException(EMSException exception)
        {
            _service.Connect();
        }
    }
}