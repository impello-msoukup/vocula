---
Title: 'Documentation'
Date: '2020-12-14 09:00'
Metadata:
    Description: 'Basic documentation how to configure and work with Vocula.'
    Author:
        Name: 'Michal Soukup'
        Email: michal.soukup@impello.cz
    Keywords:
        - Vocula
        - Content provider
        - REST API
Taxonomy:
    Category:
        - Page
    Tag:
        - Site page
---
# REST API guide and basic setup

REST is acronym for **RE**presentational **S**tate **T**ransfer. It is architectural style for distributed hypermedia systems.

Here is a overall list of REST APIs available for content consumers.

| Method | URI                              | 
|--------|:--------------------------------:|
| GET    | /Sites                           |
| GET    | /Sites/`{siteName}`              |
| GET    | /Sites/`{siteName}`/Pages        |
| GET    | /Sites/`{siteName}`/Pages/Search |
| GET    | /Sites/`{siteName}`/Images       |

Vocula is a powerful content provider, all APIs are GET methods that provide responses as JSON data or image file.

In order to consume these APIs, it is necessary to set the so-called CORS security policy in the application settings.

CORS settings are made in the __appsettings.json__ file:

```
{
  "Vocula": {
    "Server": {
      "BaseUrl": "https://example.com",
      "Cors": {
        "AllowOrigins": []
      }
    },
    "Data": {
      "Sites": "/data/sites",
      "DefaultLanguage": "en"
    }  
  }, ...
```
If the __AllowOrigins__ field is blank, the header of each response will contain the key: **Access-Control-Allow-Origin:** *

This means that all APIs can be consumed by a remote browser without any restrictions.

If it is necessary to restrict the consumer to a specific address, then one or more URL addresses can be entered in this field:

```
{
    ...
      "Cors": {
        "AllowOrigins": ["https://example.com", ...]
      }
    ...
```
It is also necessary to set __BaseUrl__ field as the base address of the REST API host:

```
{
  "Vocula": {
    "Server": {
      "BaseUrl": "https://example.com",
    ...
```
This base address will be used for image source link rendering. More details about this mechanism is available [here](/start/docs/images/).

And last but not least, needs to be set __Sites__ field as the path to the repository where the application will find all folders of each site,
plus needs to be set __DefaultLanguage__ field as the default language.

```
    ...
    "Data": {
      "Sites": "/data/sites",
      "DefaultLanguage": "en"
    }  
  },
```
That's all. You're done.

## What's next

Discover [more details](/start/docs/sites/) about using sites.