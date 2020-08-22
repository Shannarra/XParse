using System;

namespace XParseCs
{
    class XParse
    {
        /// <summary>
        /// The specified argument codeRoot="" in the file. The starting point of the program.
        /// </summary>
        private readonly string codeRoot;

        private readonly string fileExtension;

        private readonly string language;

        private readonly string configCodeRoot;

        private readonly string mainStructure;

        private readonly bool multipleRoots;

        private readonly bool multipleMains;
        
        /// <summary>
        /// The given .xparse file.
        /// </summary>
        private static System.Xml.XmlDocument document;

        /// <summary>
        /// The <see cref="document"/>'s contents.
        /// </summary>
        private readonly System.Collections.Specialized.StringCollection xmlContent;

        private const string path = @"..\..\..\testing\test.xparse";

        private const string configPath = @"..\..\..\XParse.config";

        private const string errorLogPath = @"..\..\..\errors.log";

        public static string TAB = "\t";

#nullable enable
        public static void Main(string[]? args)
        {
#if DEBUG
            document = new System.Xml.XmlDocument();

            try
            {
                new XParse();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("An error was thrown. Please see the errors.log for more information");

                if (!System.IO.File.Exists(errorLogPath))
                    System.IO.File.Create(errorLogPath);
                else
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(errorLogPath, true))
                        writer.WriteLine($"[{e.Source} at {DateTime.Now}] {e.Message}");
            }
            
#else
            if (args == null || args.Length == 0)
            {
                document = new System.Xml.XmlDocument();

                new XParse();
            }
            else { } // we'll parse cli args later
#endif
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

            System.Xml.XmlDocument config = new System.Xml.XmlDocument();

            if (System.IO.File.Exists(configPath))
                config.Load(configPath);

            System.Xml.XmlNode langNode = config.LastChild.FirstChild;

            this.language = langNode.Attributes["name"].Value;
            this.fileExtension = langNode.Attributes["fileExtension"].Value;
            this.configCodeRoot = langNode.Attributes["codeRoot"].Value;
            this.multipleRoots = langNode.Attributes["allowMultipleRoots"].Value.ToLower() == "true";
            this.mainStructure = langNode.Attributes["mainStruct"].Value;
            this.multipleMains = langNode.Attributes["allowMultipleMains"].Value.ToLower() == "true";
            
            if (document.GetElementsByTagName(this.mainStructure).Count > 1 && !this.multipleMains)
                throw new XParseConfigException("Main structures in this language MUST be in different files. Please read the .config file for more info.");
            if (document.GetElementsByTagName(this.configCodeRoot).Count > 1 && !this.multipleRoots)
                throw new XParseConfigException("Code roots in this language MUST be in different files. Please read the .config file for more info.");


            if (document.GetElementsByTagName("content") == null ||
                document.GetElementsByTagName("content")[0].ChildNodes[0].Name != "meta")
                throw new XParseException("No \"META\" tag found.");
            else
            {
                System.Xml.XmlNode meta = document.GetElementsByTagName("content")[0].ChildNodes[0];
                codeRoot = meta.Attributes["codeRoot"].Value;

                if (meta.Attributes["codeRoot"].Value == null)
                    throw new XParseException("The meta code root specified is invalid.");
                else if (codeRoot != configCodeRoot || meta.Attributes["language"].Value.ToLower() != this.language.ToLower())
                    throw new XParseException("The meta of the parsed file DIFFERS from the .config file.");
                else
                {
                    xmlContent = new System.Collections.Specialized.StringCollection();
                    ParseXml(document.GetElementsByTagName(codeRoot)[0]);

                    xmlContent.Add("\n\t}\n}");

                    string __filename = document.GetElementsByTagName("class")[0].Attributes["name"].Value;

                    if (!System.IO.File.Exists($@"..\..\..\testing\{__filename}{fileExtension}"))
                        System.IO.File.Create($@"..\..\..\testing\{__filename}{fileExtension}");

                    try
                    {
                        using var writer = new System.IO.StreamWriter($@"..\..\..\testing\{__filename}{fileExtension}");
                        foreach (var item in xmlContent)
                            writer.WriteLine(item);
                    }
                    catch (System.IO.IOException)
                    {
                        System.Console.WriteLine("The file to write in is not free at the moment. Please try again.");
                    }
                    new TestNamespace.MyClass(1); // let's call our auto-generated code :)
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
}
