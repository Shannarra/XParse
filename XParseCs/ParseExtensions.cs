namespace XParseCs
{
    static class ParseExtensions
    {
        private static string lastNodeVisited;

        public static readonly string TAB = "\t";

        public static string? ParseTrigger(System.Xml.XmlNode root)
        {
            string accumulatedText = "";

            if (root.Name == "namespace")
                if (root.Attributes["name"] == null)
                    throw new XParseException("The given namespace has no name!");
                else accumulatedText = $"namespace {root.Attributes["name"].Value}" + "\n{";

            if (lastNodeVisited == "namespace") // start a #region
                accumulatedText = $"{TAB}#region Dependencies";
            if (root.Name != lastNodeVisited && lastNodeVisited == "ref") // we went trough the last "using" directive
                accumulatedText = $"{TAB}#endregion Dependencies";

            if (root.Name == "ref")
                accumulatedText = new CodeGeneration.DepedenciesGenerator(
                    root.Attributes["name"],
                    root.Attributes["alias"],
                    root.Attributes["aliasName"]).Parse();

            if (root.Name == "class")
                accumulatedText = new CodeGeneration.ClassGenerator(root.Attributes).Parse();


            lastNodeVisited = root.Name;

            return accumulatedText; // not found or recognized?
        }
    }
}
