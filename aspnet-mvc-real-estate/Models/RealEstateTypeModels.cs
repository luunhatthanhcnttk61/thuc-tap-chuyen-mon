using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspnet_mvc_real_estate.Models
{
    public class RE_TypeModels
    {
        public int id { get; set; }
        public string name { get; set; }
        public RE_TypeModels(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public static List<RE_TypeModels> GetTypeList()
        {
            List<RE_TypeModels> list = new List<RE_TypeModels>();
            list.Add(new RE_TypeModels(1, "Chung Cư"));
            list.Add(new RE_TypeModels(2, "Căn Hộ"));
            list.Add(new RE_TypeModels(3, "Đất Đai"));
            list.Add(new RE_TypeModels(4, "Biệt Thự"));
            list.Add(new RE_TypeModels(5, "Nhà Ở"));

            return list;
        }
        public static RE_TypeModels Find(int i)
        {
            var list = GetTypeList();
            RE_TypeModels type = list.Find(r => r.id == i);
            return type;
        }
    }
}