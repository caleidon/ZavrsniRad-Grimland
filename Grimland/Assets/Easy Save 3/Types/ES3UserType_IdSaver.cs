using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("NextThingID", "NextCreatureID", "NextJobID", "NextZoneID")]
	public class ES3UserType_IdSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_IdSaver() : base(typeof(IdManagerSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (IdManagerSaver)obj;
			
			writer.WriteProperty("NextThingID", instance.NextThingID, ES3Type_uint.Instance);
			writer.WriteProperty("NextCreatureID", instance.NextCreatureID, ES3Type_uint.Instance);
			writer.WriteProperty("NextJobID", instance.NextJobID, ES3Type_uint.Instance);
			writer.WriteProperty("NextZoneID", instance.NextZoneID, ES3Type_uint.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (IdManagerSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "NextThingID":
						instance.NextThingID = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "NextCreatureID":
						instance.NextCreatureID = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "NextJobID":
						instance.NextJobID = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					case "NextZoneID":
						instance.NextZoneID = reader.Read<System.UInt32>(ES3Type_uint.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new IdManagerSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_IdSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_IdSaverArray() : base(typeof(IdManagerSaver[]), ES3UserType_IdSaver.Instance)
		{
			Instance = this;
		}
	}
}