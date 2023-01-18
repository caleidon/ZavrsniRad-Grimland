using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Id", "MaterialDefName", "Health", "Amount", "InventoryId")]
	public class ES3UserType_MaterialSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MaterialSaver() : base(typeof(MaterialSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (MaterialSaver)obj;
			
			writer.WriteProperty("Id", instance.Id, ES3Type_string.Instance);
			writer.WriteProperty("MaterialDefName", instance.MaterialDefName, ES3Type_string.Instance);
			writer.WriteProperty("Health", instance.Health, ES3Type_int.Instance);
			writer.WriteProperty("Amount", instance.Amount, ES3Type_int.Instance);
			writer.WriteProperty("InventoryId", instance.InventoryId, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (MaterialSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Id":
						instance.Id = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "MaterialDefName":
						instance.MaterialDefName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Health":
						instance.Health = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Amount":
						instance.Amount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "InventoryId":
						instance.InventoryId = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new MaterialSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_MaterialSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MaterialSaverArray() : base(typeof(MaterialSaver[]), ES3UserType_MaterialSaver.Instance)
		{
			Instance = this;
		}
	}
}