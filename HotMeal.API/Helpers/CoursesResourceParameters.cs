﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Helpers
{
    public class CoursesResourceParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string OrderBy { get; set; } = "Name";

        public string SearchQuery { get; set; }
    }
}
