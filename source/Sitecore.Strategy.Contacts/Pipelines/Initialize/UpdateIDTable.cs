using System;
using System.Linq;
using System.Web;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Pipelines;
using Sitecore.Strategy.Contacts.DataProviders;
using System.Reflection;
using Sitecore.Diagnostics;
using Sitecore.Strategy.Contacts.IdProvider;
using Sitecore.Strategy.Contacts.Pipelines.GetFacetMemberValues;

namespace Sitecore.Strategy.Contacts.Pipelines.Initialize
{
    public class UpdateIDTable
    {
        public UpdateIDTable()
        {
        }

        public virtual void Process(PipelineArgs args)
        {
            AddFacets(Sitecore.Strategy.Contacts.DataProviders.ItemIDs.ContactFacetsFolder);
        }
        protected virtual void AddFacets(ID parentId)
        {
            var facetNames = ContactFacetHelper.GetFacetNames();
            foreach (var name in facetNames)
            {
                var facetId = ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacet(name, parentId, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetTemplate);
                AddContactFacetMember(name, facetId);
            }
        }
        protected virtual void AddContactFacetMember(string facetName, ID parentId)
        {
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
                    var memberId = ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacetMember(member, parentId, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberTemplate);
                    AddContactFacetMemberValues(facetName, member.Name, memberId);
                }
            }
        }
        protected virtual void AddContactFacetMemberValues(string facetName, string memberName, ID parentId)
        {
            var args = new GetFacetMemberValuesArgs(facetName, memberName);
            CorePipeline.Run("getFacetMemberValues", args);
            if (!args.Values.Any())
            {
                return;
            }
            foreach (var pair in args.Values)
            {
                ContactFacetIdFactory.GetContactFacetIDProvider().GenerateIdForFacetMemberValue(pair.Key, pair.Value, parentId, Sitecore.Strategy.Contacts.DataProviders.TemplateIDs.ContactFacetMemberValueTemplate);
            }
        }

    }
}