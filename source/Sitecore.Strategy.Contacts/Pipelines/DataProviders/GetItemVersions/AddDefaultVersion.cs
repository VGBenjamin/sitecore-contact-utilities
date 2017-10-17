using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Collections;

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.GetItemVersions
{
    public class AddDefaultVersion
    {
        public virtual void Process(GetItemVersionsArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.Context, "args.Context");
            Log.Info($"[SD] GetItemVersions - AddDefaultVersion - Process - Begin - args.ItemId: {args.ItemDefinition?.ID}", this);
            var list = new VersionUriList();
            foreach (var language in args.Context.DataManager.Database.Languages)
            {
                list.Add(language, Sitecore.Data.Version.First);
            Log.Info($"[SD] GetItemVersions - AddDefaultVersion - Process - Version added - args.ItemId: {args.ItemDefinition?.ID} - {language.Name}", this);
            }
            args.VersionUriList = list;
            Log.Info($"[SD] GetItemVersions - AddDefaultVersion - Process - End - args.ItemId: {args.ItemDefinition?.ID}", this);
        }
    }
}