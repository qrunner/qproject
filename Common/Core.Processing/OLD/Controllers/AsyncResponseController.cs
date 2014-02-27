using OpenBank.SOZ.DataModel.test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Processing.Controllers
{
    [Obsolete("Next step is use DI to assembly instance")]
    public abstract class AsyncResponseController<DataObject, ResponseObject, CtrlContext, ExecContext, ThreadContext> : 
        ControllerBase<ResponseObject, CtrlContext, ExecContext, ThreadContext>
        where ExecContext : IExecutionContext<CtrlContext, ThreadContext>
        where CtrlContext : class
    {
        public AsyncResponseController(CtrlContext context, IExceptionHandler<ExecContext> _exceptionHandler, ILogger<ExecContext> _logger)
            : base(context, _exceptionHandler, _logger) { }

        public AsyncResponseController(CtrlContext context,
            ILoader<DataObject, ExecContext> _dataObjectLoader,
            ISaver<DataObject, ExecContext> _dataObjectSaver,
            IValidator<ResponseObject> _responseValidator,
            ITransformer<ResponseObject, DataObject, ExecContext> _responseTransformer,
            ISaver<ResponseObject, ExecContext> _responseSaver,
            IExceptionHandler<ExecContext> _exceptionHandler,
            ILogger<ExecContext> _logger)
            : base(context, _exceptionHandler, _logger)
        {
            dataObjectLoader = _dataObjectLoader;
            dataObjectSaver = _dataObjectSaver;
            responseValidator = _responseValidator;
            responseTransformer = _responseTransformer;
            responseSaver = _responseSaver;
            exceptionHandler = _exceptionHandler;
        }
        /// <summary>
        /// Формирование объекта модели данных
        /// </summary>
        protected ILoader<DataObject, ExecContext> dataObjectLoader = null;
        /// <summary>
        /// Сохранение объекта модели данных в хранилище
        /// </summary>
        protected ISaver<DataObject, ExecContext> dataObjectSaver = null;
        protected IValidator<ResponseObject> responseValidator = null; // сделать валидаторы списком
        protected ITransformer<ResponseObject, DataObject, ExecContext> responseTransformer = null;
        //protected ITransformer<ResponseObject, DataObject, ExecContext> processingTransformer = null;
        protected ISaver<ResponseObject, ExecContext> responseSaver = null;

        public override sealed void ExecuteInternal(ResponseObject response, ExecContext execContext)
        {
            // сохранение ответа            
            if (responseSaver != null)
                Task.Factory.StartNew(() => { responseSaver.Save(response, execContext); });

            // валидация ответа            
            IList<string> validationMessages = new List<string>();
            if (responseValidator != null && !responseValidator.Validate(response, ref validationMessages))
                throw new ValidationException(string.Format("Ошибка при валидации ответа <{0}>.", response), validationMessages);

            // получение объекта модели данных            
            DataObject obj = dataObjectLoader.Load(response, execContext);

            // обновление данных модели            
            responseTransformer.Transform(response, obj, execContext);

            /*// обновление модели (процессинг)            
            processingTransformer.Transform(response, obj, execContext);*/

            // сохранение изменений модели в хранилище            
            dataObjectSaver.Save(obj, execContext);
        }
    }
}