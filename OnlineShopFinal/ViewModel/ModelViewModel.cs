using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace OnlineShopFinal.Models.ViewModels
{
    public class ModelViewModel
    {
        public SubCategory SubCategory { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<SelectListItem> CSelectListItem<T>(IEnumerable<T> Items)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem
            {
                Text = "---Select---",
                Value = "0"
            };
            List.Add(sli);
            foreach (var item in Items)
            {
                sli = new SelectListItem
                {
                    Text = item.GetType().GetProperty("Name").GetValue(item, null).ToString(),
                    Value = item.GetType().GetProperty("Id").GetValue(item, null).ToString()
                };
                List.Add(sli);
            }
            return List;
        }
    }
}
