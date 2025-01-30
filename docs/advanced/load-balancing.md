---
outline: deep
---

# Load balancing the application

Very often applications need to encrypt the data exposed to the client, for example:
- authentication cookies
- antiforgery token
- BlazeFusion is encrypting the state of the components

For that purpose ASP.NET Core apps use tools that are a part of [Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction), which provides a cryptographic API to protect data.
It's using a **cryptographic key** for the encryption.

### Problem

On hosting with only one node such a key is stored locally in `DataProtection-Keys` directory and everything works without additional configuration.
When using load balancing, so when having multiple nodes with the same application, we have to **provide the same cryptographic key for all the nodes**,
so the encrypted data looks the same no matter which node generated it.

### Solution

Use a shared storage to keep the key in one place and available for all the nodes. There are several [kinds of storages](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers) you can use:
- Database
- File system pointing to a network share
- Azure Storage
- Redis

Example of configuration using Entity Framework Core:

```c#
services.AddDataProtection()
    .PersistKeysToDbContext<MyDbContext>();
```
