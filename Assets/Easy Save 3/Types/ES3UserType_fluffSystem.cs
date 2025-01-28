using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_fluffSystem : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_fluffSystem() : base(typeof(fluffSystem)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (fluffSystem)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (fluffSystem)obj;
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


	public class ES3UserType_fluffSystemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_fluffSystemArray() : base(typeof(fluffSystem[]), ES3UserType_fluffSystem.Instance)
		{
			Instance = this;
		}
	}
}