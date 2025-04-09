﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AutoUI.TestItems
{
    [XmlParse(XmlKey = "iterator")]
    public class Iterator : AutoTestItem
    {
        public string Label { get; set; }
        public override TestItemProcessResultEnum Process(AutoTestRunContext ctx)
        {
            var collection = ctx.Vars[CollectionVarName] as IList<PatternFindInfo>;
            if (collection.Count > 0)
            {
                var fr = collection.First();

                var fr1 = ctx.Test.Main.Items.OfType<LabelAutoTestItem>().First(z => z.Label == Label);
                ctx.CodePointer = ctx.Test.Main.Items.IndexOf(fr1);
                ctx.ForceCodePointer = true;

                collection.RemoveAt(0);
                ctx.Vars[ItemStoreVarName] = fr;
            }
            return TestItemProcessResultEnum.Success;
        }

        public override void ParseXml(AutoTest set, XElement item)
        {
            if (item.Attribute("itemStoreVarName") != null)
                ItemStoreVarName = item.Attribute("itemStoreVarName").Value;
            
            if (item.Attribute("collectionVarName") != null)
                CollectionVarName = item.Attribute("collectionVarName").Value;

            if (item.Attribute("label") != null)
                Label = item.Attribute("label").Value;

            base.ParseXml(set, item);
        }

        public string ItemStoreVarName { get; set; }
        public string CollectionVarName { get; set; }
        internal override string ToXml()
        {
            return $"<iterator label=\"{Label}\" itemStoreVarName=\"{ItemStoreVarName}\"  collectionVarName=\"{CollectionVarName}\"></iterator>";

        }
    }
}