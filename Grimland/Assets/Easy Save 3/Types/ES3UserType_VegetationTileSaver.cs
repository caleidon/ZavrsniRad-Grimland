using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Id", "Node", "VegetationDefName", "Health", "GrowthProgress")]
	public class ES3UserType_VegetationTileSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_VegetationTileSaver() : base(typeof(VegetationTileSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (VegetationTileSaver)obj;
			
			writer.WriteProperty("Id", instance.Id, ES3Type_string.Instance);
			writer.WriteProperty("Node", instance.Node, ES3Type_Vector3Int.Instance);
			writer.WriteProperty("VegetationDefName", instance.VegetationDefName, ES3Type_string.Instance);
			writer.WriteProperty("Health", instance.Health, ES3Type_int.Instance);
			writer.WriteProperty("GrowthProgress", instance.GrowthProgress, ES3Type_float.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (VegetationTileSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Id":
						instance.Id = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Node":
						instance.Node = reader.Read<UnityEngine.Vector3Int>(ES3Type_Vector3Int.Instance);
						break;
					case "VegetationDefName":
						instance.VegetationDefName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Health":
						instance.Health = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "GrowthProgress":
						instance.GrowthProgress = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new VegetationTileSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_VegetationTileSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_VegetationTileSaverArray() : base(typeof(VegetationTileSaver[]), ES3UserType_VegetationTileSaver.Instance)
		{
			Instance = this;
		}
	}
}