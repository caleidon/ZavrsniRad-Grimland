using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Size")]
	public class ES3UserType_Map : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Map() : base(typeof(Map)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Map)obj;
			
			writer.WriteProperty("Size", instance.Size, ES3Type_Vector2Int.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Map)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Size":
						instance.Size = reader.Read<UnityEngine.Vector2Int>(ES3Type_Vector2Int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Map();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_MapArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MapArray() : base(typeof(Map[]), ES3UserType_Map.Instance)
		{
			Instance = this;
		}
	}
}