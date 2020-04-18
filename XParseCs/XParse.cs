namespace XParseCs
{
    class XParse
    {
        /// <summary>
        /// The specified argument codeRoot="" in the file. The starting point of the program.
        /// </summary>
        private readonly string codeRoot;
        
        /// <summary>
        /// The given .xparse file.
        /// </summary>
        private static System.Xml.XmlDocument document;

        /// <summary>
        /// The <see cref="document"/>'s contents.
        /// </summary>
        private readonly System.Collections.Specialized.StringCollection xmlContent;

        private const string path = @"..\..\..\testing\test.xparse";

        public static string TAB = "\t";

#nullable enable
        public static void Main(string[]? args)
        {
            if (args == null || args.Length == 0)
            {
                document = new System.Xml.XmlDocument();

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
                System.Console.WriteLine("File not found.");
                Main(null);
            }
            catch (System.UnauthorizedAccessException) 
            {
                System.Console.WriteLine("XParse cannot access the given file");
            }
            catch (System.Xml.XmlException)
            {
                System.Console.WriteLine("No XML found in the current file");
            }

            if (document.GetElementsByTagName("content") == null ||
                document.GetElementsByTagName("content")[0].ChildNodes[0].Name != "meta")
                throw new XParseException("No \"META\" tag found.");
            else
            {
                System.Xml.XmlNode meta = document.GetElementsByTagName("content")[0].ChildNodes[0];
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
                        System.Console.WriteLine("The file to write in is not free at the moment. Please try again.");
                    }
                    TestNamespace.MyClass.GreetMe(); // let's call our auto-generated code :)
                }
            }
        }

        /// <summary>
        /// Recursively parses the given <see cref="document"/> by a given starting <see cref="System.Xml.XmlNode"/> named <paramref name="root"/>
        /// </summary>
        /// <param name="root">The code root to start traversing from.</param>
        public void ParseXml(System.Xml.XmlNode root)
        {
            if (root is System.Xml.XmlElement)
            {
                var result = ParseExtensions.ParseTrigger(root);

                if (result != null && result != string.Empty)
                    if (!result.StartsWith(TAB) && !result.StartsWith(codeRoot))
                        xmlContent.Add(TAB + result);
                    else xmlContent.Add(result);
            }

            if (root.HasChildNodes)
                ParseXml(root.FirstChild);
            if (root.NextSibling != null)
                ParseXml(root.NextSibling);
        }
    }

    /// <summary>
    /// A generic <see cref="Exception"/> that throws when XParse encounter unexpected use of the XML formated file.
    /// </summary>
    [System.Serializable]
    public class XParseException : System.Exception
    {
        public XParseException() { }
        public XParseException(string message) : base(message) { }
        public XParseException(string message, System.Exception inner) : base(message, inner) { }
        protected XParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Special <see cref="Exception"/> that throws when a required XML argument could not be found (is null) or is empty.
    /// </summary>
    [System.Serializable]
    public class XParseAttributeException : System.Exception
    {
        public XParseAttributeException() { }
        public XParseAttributeException(string attributeName) : base($"The value of the attribute \"{attributeName}\" is null or invalid.") { }
        public XParseAttributeException(string message, System.Exception inner) : base(message, inner) { }
        protected XParseAttributeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
