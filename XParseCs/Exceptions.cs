namespace XParseCs
{
    /// <summary>
    /// A generic <see cref="System.Exception"/> that throws when XParse encounter unexpected use of the XML formated file.
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
    /// Special <see cref="System.Exception"/> that throws when a required XML argument could not be found (is null) or is empty.
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

    /// <summary>
    /// An <see cref="System.Exception"/> that throws whenever a violation of the rules in the .config file has taken place.
    /// </summary>
    [System.Serializable]
    public class XParseConfigException : System.Exception
    {
        public XParseConfigException() { }
        public XParseConfigException(string message) : base(message) { }
        public XParseConfigException(string message, System.Exception inner) : base(message, inner) { }
        protected XParseConfigException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}