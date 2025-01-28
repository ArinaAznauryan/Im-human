using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_tutorialTrigger : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_tutorialTrigger() : base(typeof(tutorialTrigger)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (tutorialTrigger)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (tutorialTrigger)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_tutorialTriggerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_tutorialTriggerArray() : base(typeof(tutorialTrigger[]), ES3UserType_tutorialTrigger.Instance)
		{
			Instance = this;
		}
	}
}