﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web
{
    public class AddressSearchViewModel
    {
        public string PostCode { get; set; }
        public List<AddressInfo> AddressInfoList { get; set; }
        public string SelectedAddress { get; set; }
        public string PostcodeApiAddress { get; set; }
        public string PostcodeApiSubscriptionKey { get; set; }

        public IEnumerable<SelectListItem> SelectListItems
        {
            get
            {
                return AddressInfoList.Select(x => new SelectListItem
                {
                    Value = x.PostCode,
                    Text = string.Format("{0} {1} {2} {3} {4}", x.HouseNumber, x.AddressLine1, x.AddressLine2, x.City, x.PostCode),
                    Selected = false
                });
            }
        }

        public AddressSearchViewModel()
        {
            AddressInfoList = new List<AddressInfo>();
        }
    }
}