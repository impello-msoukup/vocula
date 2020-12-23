---
Title: 'How to load images'
Date: '2020-12-14 10:00'
Metadata:
    Description: 'How to load images.'
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
# How to load images

The Vocula allows use images to enrich the page content. These images can be saved in the same directories as pages and
they can be loaded using the following REST API:

| Method | URI                              | 
|--------|:--------------------------------:|
| GET    | /Sites/`{siteName}`/Images       |

## Request & response

Make GET method call on `/Sites/{siteName}/Images?path={filePath}` address in case of needs to retrieve specific image from site repository.

Attribute `{siteName}` represent directory name of site and query attribute `path` contains the path to specific image file, such as `images/picture.jpg`.

Link onto this image can be placed inside of any Markdown page file by using following format: `![alt_text]({ !api }images/picture.jpg)`

The special keyword `{ !api }` on the file path beginning is a directional shortcut which will be replaced by the backend mechanism by value of **[BaseUrl](/start/docs/)** attribute in application config file.

The response will contain requested image.