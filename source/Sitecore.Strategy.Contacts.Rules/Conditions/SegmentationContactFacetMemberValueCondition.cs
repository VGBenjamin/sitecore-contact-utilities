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
            if (ruleContext == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetLeftValue)} - ruleContext cannot be null. Stack: {Environment.StackTrace}", this);
                return null;
            }
            Database db;
            if (ruleContext.Item == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetLeftValue)} - ruleContext.Item is null the database will be set to the ContentDatabase by default: '{Sitecore.Context.ContentDatabase}'. Stack: {Environment.StackTrace}", this);
                db = Sitecore.Context.ContentDatabase;
            }
            else if (ruleContext.Item.Database == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetLeftValue)} - ruleContext.Item.Database is null the database will be set to the ContentDatabase by default: '{Sitecore.Context.ContentDatabase}'. Stack: {Environment.StackTrace}", this);
                db = Sitecore.Context.ContentDatabase;
            }
            else
            {
                db = ruleContext.Item.Database;
            }

            var facetFullName = ContactFacetItemHelper.GetFacetMemberFullName(db, this.ContactFacetId, this.ContactFacetMemberId);
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
            Assert.IsNotNull(item, $"item {this.Value} is used in a condition but cannot be located in the database. Stack: {Environment.StackTrace}");
            var value = item[Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberValueValue];
            return value;
        }

        public override Type GetDataType(T ruleContext)
        {
            if (ruleContext == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetDataType)} - ruleContext cannot be null. Stack: {Environment.StackTrace}", this);
                return null;
            }
            Database db;
            if (ruleContext.Item == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetDataType)} - ruleContext.Item is null the database will be set to the ContentDatabase by default: '{Sitecore.Context.ContentDatabase}'. Stack: {Environment.StackTrace}", this);
                db = Sitecore.Context.ContentDatabase;
            }
            else if (ruleContext.Item.Database == null)
            {
                Log.Error($"SegmentationContactFacetMemberValueCondition<T> - {nameof(GetDataType)} - ruleContext.Item.Database is null the database will be set to the ContentDatabase by default: '{Sitecore.Context.ContentDatabase}'. Stack: {Environment.StackTrace}", this);
                db = Sitecore.Context.ContentDatabase;
            }
            else
            {
                db = ruleContext.Item.Database;
            }
            var type = ContactFacetItemHelper.GetFacetMemberValueType(db, this.ContactFacetId, this.ContactFacetMemberId);
            return type;
        }
    }
}