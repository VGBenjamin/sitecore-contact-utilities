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
            var list = new VersionUriList();
            foreach (var language in args.Context.DataManager.Database.Languages)
            {
                list.Add(language, Sitecore.Data.Version.First);
            }
            args.VersionUriList = list;
        }
    }
}