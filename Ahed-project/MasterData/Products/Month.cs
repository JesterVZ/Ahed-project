﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahed_project.MasterData.Products
{
    public class Month
    {
        public int month_number { get; set; }
        public string month_label { get; set; }
        public IEnumerable<ProductGet> products { get; set; }
    }
}
