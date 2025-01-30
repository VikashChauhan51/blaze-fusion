---
outline: deep
---

# User interface utilities

## Styles

### `.blaze-request`
CSS class added to elements that triggered a BlazeFusion operation and is currently being processed

### `.blaze-loading`
CSS class added to `body` element when a page is loading using blaze-link functionality

### `disabled`
Attribute added to elements that triggered a BlazeFusion operation and is currently being processed

## Styling examples:

### Page loading indicator

```html
<style>
#page-loading {
  display: none;
  position: absolute;
  left: 0;
  top: 0;
  width: 0;
  height: 2px;
  background: red;
  z-index: 2;
  animation: loadPage 5s forwards ease-out;
  animation-delay: 0.1s;
}

.blaze-loading #page-loading {
  display: block;
}

@keyframes loadPage {
  0% { width: 0; }
  100% { width: 100%; }
}
</style>

<body>
  <div id="page-loading"></div>
</body>
```

### BlazeFusion action loading indicator

```css
/* MyComponent.cshtml.css */

.loader {
  display: none;
}

.blaze-request .loader {
  display: inline-block;
}
```

```razor
<!-- MyComponent.cshtml -->

@model MyComponent

<button on:click="@(() => Model.Submit())">Submit <div class="loader">...</div></button>
```
