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

namespace Sitecore.Strategy.Contacts.Pipelines.DataProviders.GetItemFields
{
    public class DefaultProcessor
    {
        public virtual void Process(GetItemFieldsArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.ItemDefinition, "args.ItemDefinition");
            Assert.ArgumentNotNull(args.Context, "args.Context");

            var itemId = args.ItemDefinition.ID;           
            var fields = new FieldList();
            var owner = typeof(ContactFacetDataProvider).Name;
            fields.Add(Sitecore.FieldIDs.CreatedBy, owner); 
            fields.Add(Sitecore.FieldIDs.Owner, owner);
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetItem(itemId))
            {
                var facetName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetName(itemId);
                fields.Add(Sitecore.FieldIDs.DisplayName, facetName);
                fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetName, facetName);
                var contractType = ContactFacetHelper.GetContractTypeForFacet(facetName);
                if (contractType != null)
                {
                    fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetContract, contractType.AssemblyQualifiedName);
                    if (ContactFacetHelper.ContactFacetImplementations.ContainsKey(contractType))
                    {
                        var implType = ContactFacetHelper.ContactFacetImplementations[contractType];
                        if (implType != null)
                        {
                            fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetImplementation, implType.AssemblyQualifiedName);
                        }
                    }
                }
            }
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberItem(itemId))
            {
                var memberName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberName(itemId);
                fields.Add(Sitecore.FieldIDs.DisplayName, memberName);
                fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberName, memberName);
                var facetId = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberParentId(itemId);
                var facetName = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetName(facetId);
                var memberType = ContactFacetHelper.GetFacetMemberType(facetName, memberName);
                if (memberType != null)
                {
                    fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberType, memberType.AssemblyQualifiedName);
                }
            }
            if (ContactFacetIdFactory.GetContactFacetIDProvider().IsFacetMemberValueItem(itemId))
            {
                var value = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberValue(itemId);
                if (!string.IsNullOrEmpty(value))
                {
                    fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberValueValue, value);
                }
                var description = ContactFacetIdFactory.GetContactFacetIDProvider().GetFacetMemberValueDescription(itemId);
                if (!string.IsNullOrEmpty(description))
                {
                    fields.Add(Sitecore.FieldIDs.DisplayName, description);
                    fields.Add(Sitecore.Strategy.Contacts.DataProviders.FieldIDs.ContactFacetMemberValueDescription, description);
                }
            }
            args.FieldList = fields;
        }
    }
}