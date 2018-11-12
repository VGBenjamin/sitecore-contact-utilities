using Sitecore.Analytics.Model.Framework;
using Sitecore.Data;
using Sitecore.Strategy.Contacts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Sitecore.Diagnostics;

namespace Sitecore.Strategy.Contacts.DataProviders
{
    public static class ContactFacetItemHelper
    {
        private static string GetFacetName(Database database, ID contactFacetId)
        {
            if (contactFacetId == default(ID))
            {
                return null;
            }

            ContactFacetNameItem cfnItem = database.GetItem(contactFacetId);
            if (cfnItem == null)
            {
                return null;
            }
            return cfnItem.FacetName;
        }

        private static bool FacetExist(Database database, ID contactFacetId) => !string.IsNullOrEmpty(GetFacetName(database, contactFacetId));

        private static IFacet GetFacet(Database database, ID contactFacetId)
        {
            var facetName = GetFacetName(database, contactFacetId);
            var contact = Sitecore.Analytics.Tracker.Current.Contact;
            if (contact == null)
            {
                return null;
            }
            var facet = contact.Facets[facetName];
            return facet;
        }
        private static MemberInfo GetFacetMember(Database database, ID contactFacetId, ID contactFacetMemberId)
        {
            if (!FacetExist(database, contactFacetId))
            {
                return null;
            }
            if (contactFacetMemberId == default(ID))
            {
                Log.Error($"{nameof(ContactFacetItemHelper)} - {nameof(GetFacetMember)} - The contactFacetMemberId cannot be null. database: '{database?.Name}' contactFacetId: '{contactFacetId?.ToString()}'. Stack: {Environment.StackTrace}", typeof(ContactFacetItemHelper));
                return null;
            }
            var item = database.GetItem(contactFacetMemberId);
            if(item == null)
            {
                Log.Error($"{nameof(ContactFacetItemHelper)} - {nameof(GetFacetMember)} - Cannot retreive the item contactFacetMemberId: '{contactFacetMemberId}'. database: '{database?.Name}' contactFacetId: '{contactFacetId?.ToString()}'. Stack: {Environment.StackTrace}", typeof(ContactFacetItemHelper));
                return null;
            }

            ContactFacetMemberItem cfmItem = item;
            if(cfmItem == null)
            {
                Log.Error($"{nameof(ContactFacetItemHelper)} - {nameof(GetFacetMember)} - Cannot cast the item contactFacetMemberId: '{contactFacetMemberId}' to {nameof(ContactFacetMemberItem)}. database: '{database?.Name}' contactFacetId: '{contactFacetId?.ToString()}'. Stack: {Environment.StackTrace}", typeof(ContactFacetItemHelper));
                return null;
            }
            return cfmItem?.Member;
            
        }
        public static Type GetFacetMemberValueType(Database database, ID contactFacetId, ID contactFacetMemberId)
        {
            try
            {
                var member = GetFacetMember(database, contactFacetId, contactFacetMemberId);
                if (member == null)
                {
                    return null;
                }
                if (member is PropertyInfo)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        return property.PropertyType;
                    }
                }
                else if (member is FieldInfo)
                {
                    var field = member as FieldInfo;
                    if (field != null)
                    {
                        return field.FieldType;
                    }
                }
                else if (member is MethodInfo)
                {
                    var method = member as MethodInfo;
                    if (method != null)
                    { 
                        return method.ReturnType;
                    }
                }
                return null;
            }  
            catch (Exception e)
            {
                Log.Error($"Evaluation of condition failed. database: '{database}', contactFacetId: '{contactFacetId}', contactFacetMemberId: '{contactFacetMemberId}'", e, typeof(ContactFacetItemHelper));
                throw;
            }
        }
        public static object GetFacetMemberValue(Database database, ID contactFacetId, ID contactFacetMemberId)
        {
            var facet = GetFacet(database, contactFacetId);
            if (facet == null)
            {
                return null;
            }
            var member = GetFacetMember(database, contactFacetId, contactFacetMemberId);
            if (member == null)
            {
                return null;
            }
            if (member.MemberType == MemberTypes.Property)
            {
                var property = member as PropertyInfo;
                if (property == null)
                {
                    return null;
                }
                return property.GetValue(facet);
            }
            return null;
        }

        public static string GetFacetMemberFullName(Database database, ID contactFacetId, ID contactFacetMemberId)
        {
            string facetName = GetFacetName(database, contactFacetId);
            if (string.IsNullOrEmpty(facetName))
            {
                return null;
            }
            ContactFacetMemberItem cfmItem = database.GetItem(contactFacetMemberId);
            if (cfmItem == null)
            {
                return null;
            }
            return $"contact.{facetName}.{cfmItem.MemberName}".ToLower();
        }

    }
}