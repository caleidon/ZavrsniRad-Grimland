using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("DeflatedBase", "DeflatedFloor", "ThingSavers")]
	public class ES3UserType_NodeManagerSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_NodeManagerSaver() : base(typeof(NodeManagerSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (NodeManagerSaver)obj;
			
			writer.WriteProperty("DeflatedBase", instance.DeflatedBase, ES3Type_string.Instance);
			writer.WriteProperty("DeflatedFloor", instance.DeflatedFloor, ES3Type_string.Instance);
			writer.WriteProperty("ThingSavers", instance.ThingSavers, ES3Internal.ES3TypeMgr.GetES3Type(typeof(System.Collections.Generic.List<ISaver>)));
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (NodeManagerSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "DeflatedBase":
						instance.DeflatedBase = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "DeflatedFloor":
						instance.DeflatedFloor = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "ThingSavers":
						instance.ThingSavers = reader.Read<System.Collections.Generic.List<ISaver>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new NodeManagerSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_NodeManagerSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_NodeManagerSaverArray() : base(typeof(NodeManagerSaver[]), ES3UserType_NodeManagerSaver.Instance)
		{
			Instance = this;
		}
	}
}