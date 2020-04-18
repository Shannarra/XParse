using System;
using System.Xml;

namespace XParseCs
{
    class XParse
    {
        private string codeRoot;

        private static XmlDocument document;

        private System.Collections.Specialized.StringCollection xmlContent;

        private const string path = @"..\..\..\testing\test.xparse";

        public static string TAB = "\t";

#nullable enable
        public static void Main(string[]? args)
        {
            if (args == null || args.Length == 0)
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

            if (document.GetElementsByTagName("content") == null ||
                document.GetElementsByTagName("content")[0].ChildNodes[0].Name != "meta")
                throw new XParseException("No \"META\" tag found.");
            else
            {
                XmlNode meta = document.GetElementsByTagName("content")[0].ChildNodes[0];
                codeRoot = meta.Attributes["codeRoot"].Value;

                if (document.GetElementsByTagName(codeRoot)[0] == null)
                    throw new XParseException("The meta code root specified is invalid.");
                else
                {
                    xmlContent = new System.Collections.Specialized.StringCollection();
                    ParseXml(document.GetElementsByTagName(codeRoot)[0]);

                    xmlContent.Add("\n\t}\n}");

                    if (!System.IO.File.Exists(@"..\..\..\testing\MyClass.cs"))
                        System.IO.File.Create(@"..\..\..\testing\MyClass.cs");

                    try
                    {
                        using var writer = new System.IO.StreamWriter(@"..\..\..\testing\MyClass.cs");
                        foreach (var item in xmlContent)
                            writer.WriteLine(item);
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine("The file to write in is not free at the moment. Please try again.");
                    }

                }
            }
        }

        public void ParseXml(XmlNode root)
        {
            if (root is XmlElement)
            {
                var result = ParseExtensions.ParseTrigger(root);

                if (result != null && result != string.Empty)
                    if (!result.StartsWith(TAB) && !result.StartsWith("namespace"))
                        xmlContent.Add(TAB + result);
                    else xmlContent.Add(result);
            }

            if (root.HasChildNodes)
                ParseXml(root.FirstChild);
            if (root.NextSibling != null)
                ParseXml(root.NextSibling);
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


    [Serializable]
    public class XParseAttributeException : Exception
    {
        public XParseAttributeException() { }
        public XParseAttributeException(string attributeName) : base($"The value of the attribute \"{attributeName}\" is null or invalid.") { }
        public XParseAttributeException(string message, Exception inner) : base(message, inner) { }
        protected XParseAttributeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
