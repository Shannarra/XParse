namespace TestNamespace
{
	#region Dependencies
	using System;
	using System.Linq;
	using System.Collections;
	using Doc = System.Xml.XmlDocument;
	#endregion Dependencies

	///<summary>
	/// Just a test class summary
	///</summary>
	class MyClass : Doc, IEnumerable
	{
	
		private string myName  = "Shannarra";
	
		private uint myAge  = 69;
	
		///<summary>
		/// This is my static property
		///</summary>
		public static int MyProp { get; set; }
	
		///<summary>
		/// This is my accessor property
		///</summary>
		public uint MyAge { get => myAge; set => myAge = value; }
	
		///<summary>
		/// Creates a new MyClass object
		///</summarry>
		MyClass()
		{
			//initializing object of type MyClass here
		}
	
		///<summary>
		/// Prints 'HI' to the console.
		///</summary>
		public virtual int SayHi()
		{
			 Console.WriteLine("hi");
		     return 0;
		}
	
		///<summary>
		/// Prints a greeting to the console.
		///</summary>
		public static void GreetMe()
		{
			 Console.WriteLine("hello there");
		}

	}
}
