using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Strategy.Contacts.DataProviders;
using Sitecore.Rules;
using Sitecore.Strategy.Contacts.IdProvider;

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.IsHandled
{
    public class DefaultProcessor
    {
        protected static List<ID> AllHandledIds { get; private set; }
        protected static List<ID> FullyHandledIds { get; private set; }
        public DefaultProcessor()
        {
            if (FullyHandledIds == null)
            {
                FullyHandledIds = new List<ID>();
                FullyHandledIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsTemplatesFolder);

                FullyHandledIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder);
                FullyHandledIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder);
            }
            if (AllHandledIds == null)
            {
                AllHandledIds = new List<ID>();
                AllHandledIds.AddRange(FullyHandledIds);
                AllHandledIds.Add(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.SettingsRoot);
                AllHandledIds.Add(RuleIds.DefinitionsFolderId);
                AllHandledIds.Add(RuleIds.ElementsFolderID);
                AllHandledIds.Add(RuleIds.TagsFolderID);
                AllHandledIds.Add(RuleIds.MacrosesFolder);
            }
        }

        private static bool rootIsLoaded = false;
        private static object lockObject = new object();

        public virtual void Process(IsHandledArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.ItemId, "args.ItemId");

            //Log.Info($"[SD] Is handled? {args.ItemId}", this);
            Log.Info($"[SD] IsHandled - DefaultProcessor - Process - Begin - args.ItemId: {args.ItemId}", this);

            List<ID> ids = null;
            if (args.IncludeAllIds)
            {
                ids = AllHandledIds;
            }
            else
            {
                ids = FullyHandledIds;
            }
            if (ids.Contains(args.ItemId))
            {
                args.IsHandled = true;
                return;
            }
            //
            //ensure the root node has been loaded
            var database = args.Context.DataManager.Database;
            if (database != null)
            {
                if (!rootIsLoaded)
                {
                    lock (lockObject)
                    {
                        if (!rootIsLoaded)
                        {
                            database.GetItem(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactsFolder);
                            //item.GetChildren();
                            rootIsLoaded = true;
                        }
                    }
                }
            }
            
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetItem(args.ItemId))
            {
                args.IsHandled = true;
                return;
            }
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberItem(args.ItemId))
            {
                args.IsHandled = true;
                return;
            }
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberValueItem(args.ItemId))
            {
                args.IsHandled = true;
                return;
            }
            Log.Info($"[SD] IsHandled - DefaultProcessor - Process - End - args.ItemId: {args.ItemId} IsHandled: {args.IsHandled}", this);

            //TODO: check dynamically generated item ids
        }
    }
}