using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Configuration;

namespace Sitecore.Strategy.Contacts.IdProvider
{
    public static class ContactFacetIdFactory
    {
        private static ContactFacetIDProvider _provider;

        private static object _lockObject = new object();

        public static ContactFacetIDProvider GetContactFacetIDProvider()
        {
            if (_provider == null)
            {
                lock (_lockObject)
                {
                    if (_provider == null)
                        _provider = Factory.CreateObject("ContactFacetIDProvider", true) as ContactFacetIDProvider;
                }
            }
            return _provider;
        }
    }
}