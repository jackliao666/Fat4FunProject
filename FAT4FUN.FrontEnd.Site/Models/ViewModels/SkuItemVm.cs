using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAT4FUN.FrontEnd.Site.Models.ViewModels
{
	public class SkuItemVm
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public List<string> KeyList 
		{
			get
			{
				return Key.Split('/').ToList();
			}	
		}
		public string Value { get; set; }
		public List<string> ValueList 
		{
			get
			{
				return Value.Split('/').ToList();
			}	
		}
        public int? SkuPrice { get; set; }	
    }
}