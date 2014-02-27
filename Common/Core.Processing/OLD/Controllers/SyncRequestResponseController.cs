using Core.Processing.Interfaces;
using OpenBank.SOZ.DataModel.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Processing.Controllers
{    
    public abstract class SyncRequestResponseController<DataObject, RequestObject, ResponseObject, CtrlContext, ExecContext, ThreadContext> :
        ControllerBase<DataObject, CtrlContext, ExecContext, ThreadContext>
        //,ISyncRequestResponseController<DataObject, RequestObject, ResponseObject, CtrlContext, ExecContext, ThreadContext>
        //where ExecContext : IExecutionContext<CtrlContext, ThreadContext>
        where CtrlContext : class
    {
        public SyncRequestResponseController(CtrlContext context, IExceptionHandler<ExecContext> _exceptionHandler, ILogger<ExecContext> _logger) : 
            base(context, _exceptionHandler, _logger) { }

        public SyncRequestResponseController(
            CtrlContext context,
            IExceptionHandler<ExecContext> _exceptionHandler,
            ILogger<ExecContext> _logger,
            ISaver<DataObject, ExecContext> _dataObjectSaver,
            IValidator<DataObject> _dataObjectValidator,
            IValidator<ResponseObject> _responseValidator,
            IBuilder<DataObject, RequestObject, ExecContext> _requestBuilder,
            ITransformer<ResponseObject, DataObject, ExecContext> _responseTransformer,
            //ITransformer<RequestObject, DataObject, ExecContext> _requestTransformer,
            IReqRespSender<RequestObject, ResponseObject, ExecContext> _requestSender,
            ISaver<RequestObject, ExecContext> _requestSaver,
            ISaver<ResponseObject, ExecContext> _responseSaver
            )
            : base(context, _exceptionHandler, _logger)
        {

            dataObjectValidator = _dataObjectValidator;
            requestBuilder = _requestBuilder;
            requestSaver = _requestSaver;
            requestSender = _requestSender;
            //requestTransformer = _requestTransformer;
            responseValidator = _responseValidator;
            responseTransformer = _responseTransformer;
            responseSaver = _responseSaver;
            dataObjectSaver = _dataObjectSaver;
            logger = _logger;
        }

        /// <summary>
        /// Формирование объекта модели данных
        /// </summary>
        //protected ILoader<DataObject> dataObjectLoader = null;
        /// <summary>
        /// Сохранение объекта модели данных в хранилище
        /// </summary>
        protected ISaver<DataObject, ExecContext> dataObjectSaver = null;
        /// <summary>
        /// Валидация модели данных перед отправкой
        /// </summary>
        protected IValidator<DataObject> dataObjectValidator = null; // сделать валидаторы списком
        /// <summary>
        /// Валидация ответа внешней системы
        /// </summary>
        protected IValidator<ResponseObject> responseValidator = null; // сделать валидаторы списком
        /// <summary>
        /// Генерация запроса
        /// </summary>
        protected IBuilder<DataObject, RequestObject, ExecContext> requestBuilder = null;
        /// <summary>
        /// Обновление модели при отправке запроса (процессинг)
        /// </summary>
        // protected ITransformer<RequestObject, DataObject, ExecContext> requestTransformer = null;
        /// <summary>
        /// Обновление модели при получении ответа
        /// </summary>
        protected ITransformer<ResponseObject, DataObject, ExecContext> responseTransformer = null;
        /// <summary>
        /// Обновление модели при получении ответа (процессинг)
        /// </summary>
        // protected ITransformer<ResponseObject, DataObject, ExecContext> responseProcessingTransformer = null;
        /// <summary>
        /// Отправка запроса
        /// </summary>
        protected IReqRespSender<RequestObject, ResponseObject, ExecContext> requestSender = null;
        /// <summary>
        /// Сохранение запроса
        /// </summary>
        protected ISaver<RequestObject, ExecContext> requestSaver = null;
        /// <summary>
        /// Сохранение ответа
        /// </summary>
        protected ISaver<ResponseObject, ExecContext> responseSaver = null;
        /// <summary>
        /// Логирование
        /// </summary>
        //protected ILogger<DataObject, ExecContext, CtrlContext, ThreadContext> logger = null;
        /// <summary>
        /// 
        /// </summary>
        //protected IExceptionHandler<ExecContext> exceptionHandler = null;

        public override sealed void ExecuteInternal(DataObject obj, ExecContext execContext)
        {
            // валидация модели
            IList<string> validationMessages = new List<string>();
            if (dataObjectValidator != null && !dataObjectValidator.Validate(obj, ref validationMessages))
                throw new ValidationException(string.Format("Ошибка при валидации объекта <{0}>.", obj), validationMessages);

            // формирование запроса
            RequestObject request = requestBuilder.Transform(obj, execContext);

            // обновление модели (процессинг)
            /*if (requestTransformer != null)
                requestTransformer.Transform(request, obj, execContext);           
            */
            // сохранение запроса
            if (requestSaver != null)
                Task.Factory.StartNew(()=> { requestSaver.Save(request, execContext); });

            // отправка запроса и получение ответа
            ResponseObject response = requestSender.SendRequest(request, execContext);

            // сохранение ответа
            if (responseSaver != null)
                Task.Factory.StartNew(() => { responseSaver.Save(response, execContext); });           

            // валидация ответа
            IList<string> validationMessagesRsp = new List<string>();
            if (responseValidator != null && !responseValidator.Validate(response, ref validationMessagesRsp))
                throw new ValidationException(string.Format("Ошибка при валидации ответа <{0}>.", obj), validationMessagesRsp);

            // обновление данных модели            
            if (responseTransformer != null)
                responseTransformer.Transform(response, obj, execContext);
            /*
            // обновление модели (процессинг)            
            if (responseProcessingTransformer != null)
                responseProcessingTransformer.Transform(response, obj, execContext);
            */
            // сохранение изменений модели в хранилище            
            dataObjectSaver.Save(obj, execContext);
        }
    }
}