using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Collections;
using Sitecore.Strategy.Contacts.DataProviders;
using Sitecore.Rules;
using Sitecore.Strategy.Contacts.Pipelines.GetFacetMemberValues;
using Sitecore.Pipelines;
using Sitecore.Data;
using Sitecore.Data.DataProviders;
using System.Reflection;
using System.Text;
using Sitecore.Strategy.Contacts.IdProvider;

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.GetChildIDs
{
    public class DefaultProcessor
    {
        public virtual void Process(GetChildIDsArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.ItemDefinition, "args.ItemDefinition");
            Assert.ArgumentNotNull(args.Context, "args.Context");
            //TODO: finish
            var ids = args.IDList;
           
            var itemId = args.ItemDefinition.ID;
            Log.Info($"[SD] GetChildIDs - DefaultProcessor - Process - Begin - itemId: {itemId}", this);
            if (itemId == Sitecore.Strategy.Contacts.DataProviders.ItemIDs.SettingsRoot)
            {
                ids.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder);
            }
            else if (itemId == Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder)
            {
                ids.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder);
            }
            else if (itemId == Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder)
            {
                AddChildIDsForContactFacetsRootItem(ids, args.ItemDefinition, args.Context);
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetItem(itemId))
            {
                AddChildIDsForContactFacetItem(ids, args.ItemDefinition, args.Context);
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberItem(itemId))
            {
                AddChildIDsForContactFacetMemberItem(ids, args.ItemDefinition, args.Context);
            }

            StringBuilder sbLog = new StringBuilder();
            foreach (ID id in ids)
            {
                sbLog.Append(id);
                sbLog.Append("|");
            }
            Log.Info($"[SD] GetChildIDs - DefaultProcessor - Process - End - itemId: {itemId} - ids: {sbLog}", this);
        }
        protected virtual void AddChildIDsForContactFacetsRootItem(IDList ids, ItemDefinition itemDefinition, CallContext context)
        {
            var facetNames = ContactFacetHelper.GetFacetNames();
            foreach (var name in facetNames)
            {
                var id = ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacet(name, itemDefinition.ID, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetTemplate);
                ids.Add(id);
            }
        }
        protected virtual void AddChildIDsForContactFacetItem(IDList ids, ItemDefinition itemDefinition, CallContext context)
        {
            var facetName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetName(itemDefinition.ID);
            var contractType = ContactFacetHelper.GetContractTypeForFacet(facetName);
            if (contractType == null)
            {
                return;
            }
            var members = contractType.GetMembers();
            foreach (var member in members)
            {
                if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
                {
                    var id = ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacetMember(member, itemDefinition.ID, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberTemplate);
                    ids.Add(id);
                }
            }
        }
        protected virtual void AddChildIDsForContactFacetMemberItem(IDList ids, ItemDefinition itemDefinition, CallContext context)
        {
            var itemId = itemDefinition.ID;
            var facetName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberFacetName(itemId);
            var memberName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberName(itemId);
            var args = new GetFacetMemberValuesArgs(facetName, memberName);
            CorePipeline.Run("getFacetMemberValues", args);
            if (!args.Values.Any())
            {
                return;
            }
            foreach (var pair in args.Values)
            {
                var id = ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacetMemberValue(pair.Key, pair.Value, itemId, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberValueTemplate);
                ids.Add(id);
            }
        }


    }
}