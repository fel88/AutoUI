using System;
using System.Collections.Generic;

namespace AutoUI.TestItems
{
    public class CodeSection
    {
        public List<AutoTestItem> Items = new List<AutoTestItem>();

        internal CodeSection Clone()
        {
            CodeSection ret = new CodeSection();
            foreach (var item in Items)
            {
                ret.Items.Add(item.Clone());
            }
            return ret;
        }
    }
}
