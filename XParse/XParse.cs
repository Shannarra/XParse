using System;
using System.Xml;

namespace XParse
{
    class XParse
    {
        private string codeRoot;

        private static XmlDocument document;

        const string path = @"..\..\..\testing\test.xparse";

        public static void Main(string[]? args)
        {
            if (args.Length == 0 || args == null)
            {
                document = new XmlDocument();

                new XParse();
            }
            else { } // we'll parse cli args later
        }

        XParse()
        {
            try
            {
                document.Load(path);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("File not found.");
                Main(null);
            }
            catch (System.UnauthorizedAccessException) 
            {
                Console.WriteLine("XParse cannot access the given file");
            }
            catch (System.Xml.XmlException)
            {
                Console.WriteLine("No XML found in the current file");
            }

            if (document.GetElementsByTagName("meta") == null || document.GetElementsByTagName("meta")[0]["codeRoot"] == null)
                throw new XParseException("No \"META\" tag found.");
            else
            {
                codeRoot = document.GetElementsByTagName("meta")[0]["codeRoot"].Value;

                if (document.GetElementsByTagName(codeRoot) == null)
                    throw new XParseException("The meta code root specified is invalid.");
                else
                    ParseXml(document.GetElementsByTagName(codeRoot)[0]);
            }
        }

        public void ParseXml(XmlNode root)
        {

        }
    }


    [Serializable]
    public class XParseException : Exception
    {
        public XParseException() { }
        public XParseException(string message) : base(message) { }
        public XParseException(string message, Exception inner) : base(message, inner) { }
        protected XParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
