using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveManager
{
   private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "inventory.json");

   public static void SaveData(InventoryData data)
   {
      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(SavePath, json);
   }

   public static InventoryData LoadData()
   {
      if (File.Exists(SavePath))
      {
         string json = File.ReadAllText(SavePath);
         return JsonConvert.DeserializeObject<InventoryData>(json);
      }

      return new InventoryData
      {
         coins = 0,
         PurchasedItems = new Dictionary<string, bool>()
      };
   }
}