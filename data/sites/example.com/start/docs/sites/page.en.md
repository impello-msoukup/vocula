---
Title: 'How to work with sites'
Date: '2020-12-14 08:00'
Metadata:
    Description: 'How to work with sites.'
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
# How to work with sites

Sites are the basis for the content itself, the site is the root element for the structure of pages.

The site description is provided by a YAML file in the root directory of the site. The name format of this file consists of the following elements separated by dots:

* `site`, fixed keyword
* `xx`, two characters represent language
* `yaml`, file extension

For example `site.en.yaml` represent site descriptor in English language.

The model of site elements inside of this descriptor looks like:

```
Title: 'Vocula Example Site'
Metadata:
  Description: 'Example site for Vocula app.'
  Author:
    Name: 'Michal Soukup'
    Email: michal.soukup@example.com
  Keywords:
    - Content Provider
    - CMS
```

These attributes are provided by following REST API dedicated for sites:

| Method | URI                              | 
|--------|:--------------------------------:|
| GET    | /Sites                           |
| GET    | /Sites/`{siteName}`              |

## Request & response

Make GET call on `/Sites` in case when list of available sites is needed.
Data will be provided as following **JArray** formated structure:

```
[
  {
    "name": "example.com",
    "lang": "en",
    "title": "Vocula Example Site",
    "metadata": {
      "description": "Example site for Vocula app.",
      "author": {
        "name": "Michal Soukup",
        "email": "michal.soukup@example.com"
      },
      "keywords": [
        "Content Management System",
        "CMS"
      ]
    },
    "alternatives": []
  }, ...
  ```

In case of needs to retrieve the details about a specific site, let's make GET call on `/Sites/{siteName}` where
attribute `{siteName}` represent directory name of site.

Data will be provided as one **JObject** with same structure:

```
{
  "name": "example.com",
  "lang": "en",
  "title": "Vocula Example Site",
  "metadata": {
    "description": "Example site for Vocula app.",
    "author": {
      "name": "Michal Soukup",
      "email": "michal.soukup@impello.cz"
    },
    "keywords": [
      "Content Provider",
      "CMS"
    ]
  },
  "alternatives": []
}
```

If different site file descriptor for another language is available, then the `alternatives` attribute will contains list of these alternatives.

Both GET methods can be called with another query attribute `lang` to get site details in a specific language.
For example: `/Sites/{siteName}?lang=cs`

## What's next

Discover [more details](/start/docs/pages/) about using pages.