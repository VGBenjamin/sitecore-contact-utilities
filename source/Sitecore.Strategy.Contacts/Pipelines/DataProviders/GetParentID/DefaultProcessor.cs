using Sitecore.Diagnostics;
using Sitecore.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Strategy.Contacts.DataProviders;
using Sitecore.Strategy.Contacts.IdProvider;

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.GetParentID
{
    public class DefaultProcessor : GetParentIDProcessorBase
    {
        public DefaultProcessor()
        {
            this.ParentIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder, Sitecore.Strategy.Contacts.DataProviders.ItemIDs.SettingsRoot);
            this.ParentIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder, Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder);
        }
        public virtual void Process(GetParentIDArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.ItemDefinition, "args.ItemDefinition");
            Assert.ArgumentNotNull(args.Context, "args.Context");

            var itemId = args.ItemDefinition.ID;

            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetItem(itemId))
            {
                args.ParentId = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetParentId(itemId);
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberItem(itemId))
            {
                args.ParentId = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberParentId(itemId);
            }
            else if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberValueItem(itemId))
            {
                args.ParentId = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberValueParentId(itemId);
            }
            else
            {
                base.Process(args);
            }
        }
    }
}