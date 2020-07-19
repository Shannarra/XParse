namespace XParseCs
{
    /// <summary>
    /// Container for some class extensions and the "trigger" for parsing the given file. 
    /// </summary>
    static class ParseExtensions
    {
        /// <summary>
        /// The last visited <see cref="System.Xml.XmlNode"/>.
        /// </summary>
        private static string lastNodeVisited;

        private static uint namespacesCount;

        /// <summary>
        /// Counter for all the classes in the current namespace
        /// </summary>
        private static uint classesCount;

        public static readonly string TAB = "\t";

        /// <summary>
        /// The currently visited class.
        /// </summary>
        public static string CurrentClass;

#nullable enable
        /// <summary>
        /// Acts like a "trigger" by running the parser in linear time.
        /// </summary>
        /// <param name="root">The root to parse.</param>
        /// <returns>The text accumulated throughout the parsing stage.</returns>
        public static string? ParseTrigger(System.Xml.XmlNode root)
        {
            string accumulatedText = "";

            if (root.Name == "namespace")
                if (root.Attributes["name"] == null)
                    throw new XParseException("The given namespace has no name!");
                else
                {
                    string name = root.Attributes["name"].Value, braces = "\t}\n}";
                    accumulatedText = $"namespace {name}" + "\n{";
                    classesCount = 0;

                    if (namespacesCount > 0)
                        accumulatedText = $"{braces}\n\nnamespace {name}" + "\n{";

                    namespacesCount++;
                }
            if (lastNodeVisited == "namespace") // start a #region
                accumulatedText = $"{TAB}#region Dependencies";
            if (root.Name == "class" && classesCount == 0) // all "using" directives MUST be b4 class' start.
            {
                accumulatedText = $"{TAB}#endregion Dependencies\n";
                classesCount++;
            }
            else if (classesCount > 0) // close the class's brace
                accumulatedText = $"{TAB}{'}'}\n";

            if (root.Name == "ref")
                accumulatedText = new CodeGeneration.DepedenciesGenerator(
                    root.Attributes["name"],
                    root.Attributes["alias"],
                    root.Attributes["aliasName"]).Parse();

            if (root.Name == "class")
            {
                accumulatedText += new CodeGeneration.ClassGenerator(root.Attributes).Parse();
                CurrentClass = root.Attributes["name"].Value;
            }

            if (root.Name == "field")
                accumulatedText = new CodeGeneration.FieldGenerator(root.Attributes).Parse();

            if (root.Name == "property")
                accumulatedText = new CodeGeneration.PropertyGenerator(root.Attributes).Parse();

            if (root.Name == "ctor")
            {
                //adding a summary by default for the constructor
                accumulatedText = $"\n\t\t///<summary>\n\t\t/// Creates a new {CurrentClass} object\n\t\t///</summarry>\n";
                if (root.Attributes.Count > 0 && !string.IsNullOrEmpty(root.Attributes["params"].Value))
                    accumulatedText += $"{TAB}{TAB}{CurrentClass}" +
                        $"({(root.Attributes["params"].Value.Contains(',') ? root.Attributes["params"].Value.Split(',').Reduce(',') : root.Attributes["params"].Value)})\n\t\t" + "{\n\t\t\t//initializing object of type " + CurrentClass + " here\n\t\t}";
                else accumulatedText += $"{TAB}{TAB}{CurrentClass}()\n\t\t" + "{\n\t\t\t//initializing object of type " + CurrentClass + " here\n\t\t}"; // no parameters
            }

            if (root.Name == "method")
            {
                if (root.HasChildNodes)
                    accumulatedText = new CodeGeneration.MethodsGenerator(root.Attributes, root.FirstChild).Parse();
                else accumulatedText = new CodeGeneration.MethodsGenerator(root.Attributes).Parse();
            }

            lastNodeVisited = root.Name;

            return accumulatedText;
        }

        /// <summary>
        /// Acts like the JS' Array.reduce() method + a given join string.
        /// </summary>
        /// <param name="arr">The array to reduce.</param>
        /// <param name="join">The join character.</param>
        /// <returns>The reduced value.</returns>
        public static string Reduce(this string[] arr, char join = ' ')
        {
            string str = "";

            for (int i = 0; i < arr.Length; i++)
            {
                str += arr[i];
                if (i < arr.Length - 1)
                    str += join;
            }
            return str;
        }
    }
}
