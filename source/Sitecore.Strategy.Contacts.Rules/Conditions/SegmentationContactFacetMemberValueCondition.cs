using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Strategy.Adaptive.Rules.Conditions;
using Sitecore.Strategy.Contacts.DataProviders;
using Sitecore.Strategy.Contacts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Sitecore.Strategy.Contacts.Rules.Conditions
{
    // where the contact facet [contactFacetId,Tree,root={C630AE5C-9EA7-4F22-9EBA-2B385425F78B},facet name] 
    // has a member [contactFacetMemberId,AdaptiveTree,dependency=ContactFacetId,member name] 
    // with a value [operator,AdaptiveOperator,dependency=ContactFacetMemberId,operator] 
    // [value,AdaptiveValue,dependency=ContactFacetMemberId,value]
    public class SegmentationContactFacetMemberValueCondition<T> : SegmentationAdaptiveConditionBase<T> where T : RuleContext
    {
        public ID ContactFacetId { get; set; }
        public ID ContactFacetMemberId { get; set; }

        public override object GetLeftValue(T ruleContext)
        {
            var facetFullName = ContactFacetItemHelper.GetFacetMemberFullName(ruleContext.Item.Database, this.ContactFacetId, this.ContactFacetMemberId);
            if (string.IsNullOrEmpty(facetFullName))
            {
                return null;
            }
            return facetFullName;
        }

        public override object GetRightValue(T ruleContext)
        {
            if (!ID.IsID(this.Value))
            {
                return this.Value;
            }
            var id = new ID(this.Value);
            var item = ruleContext.Item.Database.GetItem(id);
            Assert.IsNotNull(item, string.Format("item {0} is used in a condition but cannot be located in the database", this.Value));
            var value = item[Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberValueValue];
            return value;
        }

        public override Type GetDataType(T ruleContext)
        {
            var type = ContactFacetItemHelper.GetFacetMemberValueType(ruleContext.Item.Database, this.ContactFacetId, this.ContactFacetMemberId);
            return type;
        }
    }
}