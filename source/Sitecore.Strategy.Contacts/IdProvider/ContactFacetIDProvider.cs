using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Sitecore.Data;
using Sitecore.Diagnostics;

namespace Sitecore.Strategy.Contacts.IdProvider
{
    public abstract class ContactFacetIDProvider
    {
        protected ID GenerateIdForValue(string prefix, string value, ID parentId, ID templateId)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(value))
            {
                return null;
            }
            var input = $"{prefix}-{value}-{parentId}-{templateId}";
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                var guid = new Guid(hash);
                return ID.Parse(guid);
            }
        }

        public abstract ID GenerateIdForFacet(string facetName, ID parentId, ID templateId);

        public abstract ID GenerateIdForFacetMember(MemberInfo memberInfo, ID parentId, ID templateId);

        public abstract ID GenerateIdForFacetMemberValue(string memberValue, string memberDescription, ID parentId, ID templateId);

        public abstract bool IsFacetItem(ID itemId);

        public abstract string GetFacetName(ID itemId);

        public abstract ID GetFacetParentId(ID itemId);

        public abstract bool IsFacetMemberItem(ID itemId);

        public abstract string GetFacetMemberName(ID itemId);

        public abstract ID GetFacetMemberParentId(ID itemId);

        public abstract bool IsFacetMemberValueItem(ID itemId);

        public abstract string GetFacetMemberValue(ID itemId);

        public abstract string GetFacetMemberValueDescription(ID itemId);

        public abstract ID GetFacetMemberValueParentId(ID itemId);

        public abstract string GetFacetMemberFacetName(ID itemId);
    }
}