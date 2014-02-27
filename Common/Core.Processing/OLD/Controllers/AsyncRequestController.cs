using OpenBank.SOZ.DataModel.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Processing.Controllers
{
    public abstract class AsyncRequestController<DataObject, RequestObject, CtrlContext, ExecContext, ThreadContext> : ControllerBase<DataObject, CtrlContext, ExecContext, ThreadContext>
        where ExecContext : IExecutionContext<CtrlContext, ThreadContext>
        where CtrlContext : class
    {
        public AsyncRequestController(CtrlContext context, IExceptionHandler<ExecContext> _exceptionHandler, ILogger<ExecContext> _logger) : 
            base(context, _exceptionHandler, _logger) { }

        public AsyncRequestController(CtrlContext context,
            ILoader<DataObject, ExecContext> _dataObjectLoader,
            ISaver<DataObject, ExecContext> _dataObjectSaver,
            IValidator<DataObject> _dataObjectValidator,
            IBuilder<DataObject, RequestObject, ExecContext> _requestGenerator,
            ISaver<RequestObject, ExecContext> _requestSaver,
            IRequestSender<RequestObject, ExecContext> _requestSender,
            ITransformer<RequestObject, DataObject, ExecContext> _processingTransformer,
            IExceptionHandler<ExecContext> _exceptionHandler,
            ILogger<ExecContext> _logger) :
            base(context, _exceptionHandler, _logger)
        {
            dataObjectLoader = _dataObjectLoader;
            dataObjectSaver = _dataObjectSaver;
            dataObjectValidator = _dataObjectValidator; // сделать валидаторы списком
            requestBuilder = _requestGenerator;
            requestSaver = _requestSaver;
            requestSender = _requestSender;
            processingTransformer = _processingTransformer;
        }
        /// <summary>
        /// Формирование объекта модели данных
        /// </summary>
        protected ILoader<DataObject, ExecContext> dataObjectLoader = null;
        /// <summary>
        /// Сохранение объекта модели данных в хранилище
        /// </summary>
        protected ISaver<DataObject, ExecContext> dataObjectSaver = null;
        protected IValidator<DataObject> dataObjectValidator = null; // сделать валидаторы списком
        protected IBuilder<DataObject, RequestObject, ExecContext> requestBuilder = null;
        protected ISaver<RequestObject, ExecContext> requestSaver = null;
        protected IRequestSender<RequestObject, ExecContext> requestSender = null;
        protected ITransformer<RequestObject, DataObject, ExecContext> processingTransformer = null;
        //protected IExceptionHandler<ExecContext> exceptionHandler = null;

        public override sealed void ExecuteInternal(DataObject obj, ExecContext execContext)
        {
            // валидация модели
            IList<string> validationMessages = new List<string>();
            if (dataObjectValidator != null && !dataObjectValidator.Validate(obj, ref validationMessages))
                throw new ValidationException(string.Format("Ошибка при валидации объекта <{0}>.", obj), validationMessages);

            // формирование запроса
            RequestObject request = requestBuilder.Transform(obj, execContext);

            // сохранение запроса
            if (requestSaver != null)
                Task.Factory.StartNew(() => { requestSaver.Save(request, execContext); });

            // отправка запроса            
            requestSender.SendRequest(request, execContext);

            // обновление модели (процессинг)            
            processingTransformer.Transform(request, obj, execContext);

            // сохранение изменений модели в хранилище            
            dataObjectSaver.Save(obj, execContext);
        }
    }
}