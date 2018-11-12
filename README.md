# Changes from the original version
This module is a fork from [adamconn/sitecore-adaptive-rules](https://github.com/adamconn/sitecore-adaptive-rules). In the original version it is possible to use those rules to personalize content on the site but I have added the possibility to use it in the list manager to create the segmented lists of contacts.

# sitecore-contact-utilities
This module adds components that make it easier to work with contacts in Sitecore 8.X and 9.X

You can [watch a video](https://www.youtube.com/watch?v=ajSZar5aShc) that shows what this module does.

## Packages
In order to make the various components that make up this module managable, this module is broken up into a number of separate package.

### Contact Utilities
This package installs a data provider that exposes contact facets as Sitecore items. After the package is installed the contact facets will be exposed under ```/sitecore/system/Settings/Contacts```.

### Contact Utilities Sample Data
This package installs a custom facet named ```Employee Data```. The facet has members of different data types (string, int) as well as a member whose values are limited to a set of pre-defined values (the ```Location``` property).

### Contact Rules
This package installs a condition for the Sitecore Rules Engine that allows you to compare a value to the value of a contact facet member. This condition is exposed under ```/sitecore/system/Settings/Rules/Definitions/Elements/Contacts```.

## Download
The installation packages are available [here](https://github.com/vgbenjamin/sitecore-contact-utilities/tree/master/sitecore/).
