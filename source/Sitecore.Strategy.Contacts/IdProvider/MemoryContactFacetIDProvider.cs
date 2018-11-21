using System.Collections.Generic;
using System.Reflection;
using Sitecore.Data;

namespace Sitecore.Strategy.Contacts.IdProvider
{
    public class MemoryContactFacetIDProvider : ContactFacetIDProvider
    {
        private Dictionary<ID, FacetEntry> facetDictionary = new Dictionary<ID, FacetEntry>();
        private Dictionary<ID, FacetMemberEntry> facetMemberDictionary = new Dictionary<ID, FacetMemberEntry>();
        private Dictionary<ID, FacetValueEntry> facetMemberValueDictionary = new Dictionary<ID, FacetValueEntry>();

        protected class BaseEntry
        {
            public ID TemplateID { get; set; }
            public string Name { get; set; }

            public ID ParentID { get; set; }
        }

        protected class FacetEntry : BaseEntry
        {
        }

        protected class FacetMemberEntry : BaseEntry
        {
        }

        protected class FacetValueEntry : BaseEntry
        {
            public string Value { get; set; }
        }

        public MemoryContactFacetIDProvider()
        {
        }

        public MemoryContactFacetIDProvider(string test)
        {
        }

        public MemoryContactFacetIDProvider(string test, string test2)
        {
        }


        public override ID GenerateIdForFacet(string facetName, ID parentId, ID templateId)
        {
            var id = GenerateIdForValue("facet", facetName, parentId, templateId);
            var value = new FacetEntry
            {
                Name = facetName,
                ParentID = parentId,
                TemplateID = templateId
            };

            if (facetDictionary.ContainsKey(id))
                facetDictionary[id] = value;
            else
                facetDictionary.Add(id, value);

            return id;
        }

        public override ID GenerateIdForFacetMember(MemberInfo memberInfo, ID parentId, ID templateId)
        {
            var id = GenerateIdForValue("facet-member", memberInfo.Name, parentId, templateId);
            var value = new FacetMemberEntry() { Name = memberInfo.Name, ParentID = parentId, TemplateID = templateId };

            if (facetMemberDictionary.ContainsKey(id))
                facetMemberDictionary[id] = value;
            else
                facetMemberDictionary.Add(id, value);

            return id;
        }

        public override ID GenerateIdForFacetMemberValue(string memberValue, string memberDescription, ID parentId, ID templateId)
        {
            var id = GenerateIdForValue("facet-member-value", memberValue, parentId, templateId);
            var value = new FacetValueEntry
            { 
                Name = memberDescription,
                ParentID = parentId,
                TemplateID = templateId,
                Value = memberValue
            };

            if (facetMemberValueDictionary.ContainsKey(id))
                facetMemberValueDictionary[id] = value;
            else
                facetMemberValueDictionary.Add(id, value);

            return id;
        }

        public override bool IsFacetItem(ID itemId)
        {
            return facetDictionary.ContainsKey(itemId);
        }
        public override string GetFacetName(ID itemId)
        {
            return facetDictionary.ContainsKey(itemId) ? facetDictionary[itemId]?.Name : null;
        }
        public override ID GetFacetParentId(ID itemId)
        {
            return facetDictionary.ContainsKey(itemId) ? facetDictionary[itemId]?.ParentID : null;
        }

        public override bool IsFacetMemberItem(ID itemId)
        {
            return facetMemberDictionary.ContainsKey(itemId);
        }
        public override string GetFacetMemberName(ID itemId)
        {
            return facetMemberDictionary.ContainsKey(itemId) ? facetMemberDictionary[itemId]?.Name : null;
        }
        public override ID GetFacetMemberParentId(ID itemId)
        {
            return facetMemberDictionary.ContainsKey(itemId) ? facetMemberDictionary[itemId]?.ParentID : null;
        }

        public override bool IsFacetMemberValueItem(ID itemId)
        {
            return facetMemberValueDictionary.ContainsKey(itemId);
        }
        public override string GetFacetMemberValue(ID itemId)
        {
            return facetMemberValueDictionary.ContainsKey(itemId) ? facetMemberValueDictionary[itemId]?.Value : null;
        }
        public override string GetFacetMemberValueDescription(ID itemId)
        {
            return facetMemberValueDictionary.ContainsKey(itemId) ? facetMemberValueDictionary[itemId]?.Name : null;
        }

        public override ID GetFacetMemberValueParentId(ID itemId)
        {
            return facetMemberValueDictionary.ContainsKey(itemId) ? facetMemberValueDictionary[itemId]?.ParentID : null;
        }

        public override string GetFacetMemberFacetName(ID itemId)
        {
            return GetFacetName(GetFacetMemberParentId(itemId));            
        }
    }
}