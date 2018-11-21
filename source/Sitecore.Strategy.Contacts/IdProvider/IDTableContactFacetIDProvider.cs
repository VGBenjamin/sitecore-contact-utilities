using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.IDTables;

namespace Sitecore.Strategy.Contacts.IdProvider
{
    public class IDTableContactFacetIDProvider : ContactFacetIDProvider
    {
        public IDTableContactFacetIDProvider()
        {
        }

        #region Private methods
        

        private static IDTableEntry UpdateOrCreateIDTableEntry(string prefix, string key, ID id, ID parentId, string customData)
        {
            var keys = IDTable.GetKeys(prefix, id);
            if (keys != null && keys.Length > 0)
            {
                var entry = keys[0];
                if (entry.ParentID == parentId && string.Equals(entry.CustomData, customData))
                {
                    return entry;
                }
                IDTable.RemoveID(prefix, id);
            }
            return IDTable.Add(prefix, key, id, parentId, customData);
        }

        private static bool IsItem(string prefix, ID itemId)
        {
            var keys = IDTable.GetKeys(prefix, itemId);
            return (keys != null && keys.Length > 0);
        }
        private static IDTableEntry GetEntry(string prefix, ID itemId)
        {
            var keys = IDTable.GetKeys(prefix, itemId);
            if (keys == null || keys.Length == 0)
            {
                return null;
            }
            return keys[0];
        }
        private static string GetCustomDataValue(string dataKey, string prefix, ID itemId)
        {
            var entry = GetEntry(prefix, itemId);
            if (entry == null || string.IsNullOrEmpty(entry.CustomData))
            {
                return null;
            }
            var dictionary = entry.CustomData.Split('|').Select(x => x.Split('=')).ToDictionary(y => y[0], y => y[1]);
            if (!dictionary.ContainsKey(dataKey))
            {
                return null;
            }
            return dictionary[dataKey];
        }
        private static string GetKey(string prefix, ID itemId)
        {
            var entry = GetEntry(prefix, itemId);
            if (entry == null)
            {
                return null;
            }
            return entry.Key;
        }
        private static ID GetParentId(string prefix, ID itemId)
        {
            var entry = GetEntry(prefix, itemId);
            if (entry == null)
            {
                return null;
            }
            return entry.ParentID;
        }
        #endregion

        public override ID GenerateIdForFacet(string facetName, ID parentId, ID templateId)
        {
            var prefix = "facet";
            var id = GenerateIdForValue(prefix, facetName, parentId, templateId);
            var customData = string.Format("templateId={0}", templateId.ToString());
            UpdateOrCreateIDTableEntry(prefix, facetName, id, parentId, customData);
            return id;
        }

        public override ID GenerateIdForFacetMember(MemberInfo memberInfo, ID parentId, ID templateId)
        {
            var prefix = "facet-member";
            var id = GenerateIdForValue(prefix, memberInfo.Name, parentId, templateId);
            var customData = string.Format("templateId={0}", templateId.ToString());
            UpdateOrCreateIDTableEntry(prefix, memberInfo.Name, id, parentId, customData);
            return id;
        }

        public override ID GenerateIdForFacetMemberValue(string memberValue, string memberDescription, ID parentId, ID templateId)
        {
            var prefix = "facet-member-value";
            var id = GenerateIdForValue(prefix, memberValue, parentId, templateId);
            var customData = string.Format("templateId={0}|description={1}", templateId.ToString(), memberDescription);
            UpdateOrCreateIDTableEntry(prefix, memberValue, id, parentId, customData);
            return id;
        }

        public override bool IsFacetItem(ID itemId)
        {
            return IsItem("facet", itemId);
        }
        public override string GetFacetName(ID itemId)
        {
            return GetKey("facet", itemId);
        }
        public override ID GetFacetParentId(ID itemId)
        {
            return GetParentId("facet", itemId);
        }

        public override bool IsFacetMemberItem(ID itemId)
        {
            return IsItem("facet-member", itemId);
        }
        public override string GetFacetMemberName(ID itemId)
        {
            return GetKey("facet-member", itemId);
        }
        public override ID GetFacetMemberParentId(ID itemId)
        {
            return GetParentId("facet-member", itemId);
        }

        public override bool IsFacetMemberValueItem(ID itemId)
        {
            return IsItem("facet-member-value", itemId);
        }
        public override string GetFacetMemberValue(ID itemId)
        {
            return GetKey("facet-member-value", itemId);
        }
        public override string GetFacetMemberValueDescription(ID itemId)
        {
            var entry = GetEntry("facet-member-value", itemId);
            if (entry == null)
            {
                return null;
            }
            return GetCustomDataValue("description", "facet-member-value", itemId);
        }

        public override ID GetFacetMemberValueParentId(ID itemId)
        {
            return GetParentId("facet-member-value", itemId);
        }

        public override string GetFacetMemberFacetName(ID itemId)
        {
            var parentId = GetFacetMemberParentId(itemId);
            var facetName = GetFacetName(parentId);
            return facetName;
        }
    }
}