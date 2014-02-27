using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Common.XML
{
    /// <summary>
    /// In memory XSL transformations with strings
    /// </summary>
    public class XslInMemoryTransform
    {
        private readonly XslCompiledTransform _transform = new XslCompiledTransform();

        public void Load(string stylesheetUri)
        {
            // load the transform with script execution enabled.
            XsltSettings settings = new XsltSettings {EnableScript = true};
            _transform.Load(stylesheetUri, settings, null);
        }

        public void Load(IXPathNavigable stylesheet)
        {
            // load the transform with script execution enabled.
            XsltSettings settings = new XsltSettings {EnableScript = true};
            _transform.Load(stylesheet, settings, null);
        }

        public string Transform(string sourceXml)
        {
            XmlDocument xmlDoc = new XmlDocument {PreserveWhitespace = true};
            xmlDoc.LoadXml(sourceXml);

            StringBuilder retval = new StringBuilder();
            XmlWriterSettings writerSettings = new XmlWriterSettings {OmitXmlDeclaration = true};
            // Create a writer for writing the transformed file.
            XmlWriter writer = XmlWriter.Create(retval, writerSettings);
            // Execute the transformation.
            _transform.Transform(xmlDoc, writer);

            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + retval;
        }
    }
}