using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Strategy.Contacts.DataProviders;
using Sitecore.Rules;
using Sitecore.Data.Items;
using Sitecore.Strategy.Contacts.IdProvider;

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.GetItemDefinition
{
    public class DefaultProcessor
    {
        public virtual void Process(GetItemDefinitionArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.ItemId, "args.ItemId");
            Assert.ArgumentNotNull(args.Context, "args.Context");

            //TODO: finish
            if (args.ItemId == Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder)
            {
                args.ItemDefinition = new ItemDefinition(args.ItemId, "Contacts", Sitecore.TemplateIDs.Folder, ID.Null);
                return;
            }
            else if (args.ItemId == Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder)
            {
                args.ItemDefinition = new ItemDefinition(args.ItemId, "Facets", Sitecore.TemplateIDs.Folder, ID.Null);
                return;
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetItem(args.ItemId))
            {
                var key = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetName(args.ItemId);
                if (!string.IsNullOrEmpty(key))
                {
                    args.ItemDefinition = new ItemDefinition(args.ItemId, ItemUtil.ProposeValidItemName(key), Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetTemplate, ID.Null);
                    return;
                }
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberItem(args.ItemId))
            {
                var key = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberName(args.ItemId);
                if (!string.IsNullOrEmpty(key))
                {
                    args.ItemDefinition = new ItemDefinition(args.ItemId, ItemUtil.ProposeValidItemName(key), Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberTemplate, ID.Null);
                    return;
                }
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberValueItem(args.ItemId))
            {
                var key = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberValue(args.ItemId);
                if (!string.IsNullOrEmpty(key))
                {
                    args.ItemDefinition = new ItemDefinition(args.ItemId, ItemUtil.ProposeValidItemName(key), Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberValueTemplate, ID.Null);
                    return;
                }
            }
        }
    }
}