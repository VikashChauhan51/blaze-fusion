---
outline: deep
---

# Anti-forgery token

BlazeFusion supports mechanism built-in to ASP.NET Core to prevent prevent Cross-Site Request Forgery (XSRF/CSRF) attacks.

In the configuration of services use:
```c#
services.AddBlaze(options =>
{
    options.AntiforgeryTokenEnabled = true;
});
```

Make sure you've also added `meta` tag to the layout's `head`:
```html
<meta name="blaze-config" />
```
