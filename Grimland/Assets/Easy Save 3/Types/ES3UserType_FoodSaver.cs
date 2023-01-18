using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Id", "FoodDefName", "Health", "Amount", "Extra", "InventoryId")]
	public class ES3UserType_FoodSaver : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_FoodSaver() : base(typeof(FoodSaver)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (FoodSaver)obj;
			
			writer.WriteProperty("Id", instance.Id, ES3Type_string.Instance);
			writer.WriteProperty("FoodDefName", instance.FoodDefName, ES3Type_string.Instance);
			writer.WriteProperty("Health", instance.Health, ES3Type_int.Instance);
			writer.WriteProperty("Amount", instance.Amount, ES3Type_int.Instance);
			writer.WriteProperty("Extra", instance.Extra, ES3Type_string.Instance);
			writer.WriteProperty("InventoryId", instance.InventoryId, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (FoodSaver)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Id":
						instance.Id = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "FoodDefName":
						instance.FoodDefName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "Health":
						instance.Health = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Amount":
						instance.Amount = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Extra":
						instance.Extra = reader.Read<System.String>(ES3Type_string.Instance);
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
			var instance = new FoodSaver();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_FoodSaverArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_FoodSaverArray() : base(typeof(FoodSaver[]), ES3UserType_FoodSaver.Instance)
		{
			Instance = this;
		}
	}
}