using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Tick")]
	public class ES3UserType_TickManagerSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TickManagerSaver() : base(typeof(TickManagerSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (TickManagerSaver)obj;
			
			writer.WriteProperty("Tick", instance.Tick, ES3Type_ulong.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (TickManagerSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Tick":
						instance.Tick = reader.Read<System.UInt64>(ES3Type_ulong.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new TickManagerSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_TickManagerSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TickManagerSaverArray() : base(typeof(TickManagerSaver[]), ES3UserType_TickManagerSaver.Instance)
		{
			Instance = this;
		}
	}
}