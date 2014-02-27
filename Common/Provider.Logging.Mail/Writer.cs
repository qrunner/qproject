using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Common.Net.Mail;
using Provider.Logging.BatchWrite;
using RazorEngine;

namespace Provider.Logging.Mail
{
    public class Writer : BatchWriteProvider
    {
        private const string TemplateId = "mailTemplate";
        private string _server;
        private string _subject;
        private string _fromAddress;
        private string _fromName;
        private string _mailToList;
        private string _copyToList;
        private string _bodyTemplate;

        protected override void WriteBatch(IEnumerable<IEntry> records)
        {
            if (!records.Any()) return;

            MailMessage msg = Mailer.CreateMessage(_fromAddress, _fromName, _mailToList, _subject, CreateMailBody(records));
            Mailer.Send(_server, msg);
        }

        protected override void Initialize(IDictionary<string, string> settings)
        {
            base.Initialize(settings);

            CheckConfigParametersExists(settings, "bodyTemplatePath", "mailToList", "fromName", "fromAddress", "subject", "server");

            _server = settings["server"];
            _subject = settings["subject"];
            _fromAddress = settings["fromAddress"];
            _fromName = settings["fromName"];
            _mailToList = settings["mailToList"];
            _copyToList = settings["copyToList"];
            _bodyTemplate = System.IO.File.ReadAllText(settings["bodyTemplatePath"]);

            Razor.Compile(_bodyTemplate, typeof (IEnumerable<IEntry>), TemplateId);
        }

        private string CreateMailBody(IEnumerable<IEntry> records)
        {
            return Razor.Run(records, TemplateId);
        }

        public override bool CheckConnection(out Exception ex)
        {
            throw new NotImplementedException("Method CheckConnection not implemented in " + GetType().FullName);
        }
    }
}