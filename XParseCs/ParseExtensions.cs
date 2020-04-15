namespace XParseCs
{
    static class ParseExtensions
    {
        private static string lastNodeVisited;

        public static readonly string TAB = "\t";

        public static string CurrentClass;

        public static string? ParseTrigger(System.Xml.XmlNode root)
        {
            string accumulatedText = "";

            if (root.Name == "namespace")
                if (root.Attributes["name"] == null)
                    throw new XParseException("The given namespace has no name!");
                else accumulatedText = $"namespace {root.Attributes["name"].Value}" + "\n{";

            if (lastNodeVisited == "namespace") // start a #region
                accumulatedText = $"{TAB}#region Dependencies";
            if (root.Name == "class") // all "using" directives MUST be b4 class' start.
                accumulatedText = $"{TAB}#endregion Dependencies\n";

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

            if (root.Name == "ctor")
            {
                //adding a summary by default for the constructor
                accumulatedText = $"\n\t\t///<summary>\n\t\t/// Creates a new {CurrentClass} object\n\t\t///</summarry>\n";
                if (root.Attributes.Count > 0 && !string.IsNullOrEmpty(root.Attributes["params"].Value))
                    accumulatedText += $"{TAB}{TAB}{CurrentClass}" +
                        $"({(root.Attributes["params"].Value.Contains(',') ? root.Attributes["params"].Value.Split(',').Reduce(',') : root.Attributes["params"].Value)})\n\t\t" + "{\n\t\t\t//initializing object of type " + CurrentClass + " here\n\t\t}";
                else accumulatedText += $"{TAB}{TAB}{CurrentClass}()\n\t\t" + "{\n\t\t\t//initializing object of type " + CurrentClass + " here\n\t\t}"; // no parameters
            }

            lastNodeVisited = root.Name;

            return accumulatedText;
        }

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
