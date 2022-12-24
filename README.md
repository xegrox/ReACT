Pages
-----

 - /
 - /user
 - /admin
 - /company

Icons 
-----
#### [Phosphor Icons](https://phosphoricons.com/)

**Usage:** Add class `ph-{icon_name}` to your HTML element  
> Example:
```
<i class="ph-plus-circle"></i>
```

Styling
-------

#### [Tailwind](https://tailwindcss.com/docs)

#### [daisyUI](https://daisyui.com/components/)

You can customize the color scheme in `tailwind.config.js`  
Refer [here](https://daisyui.com/theme-generator/) to generate a color scheme  
> Example:

```
...

daisyui : {
    themes: [
        {
            default: {
                "primary": "#0a7b79",
                "primary-content": "#fafafa",
                "secondary": "#7b0a0c",
                "accent": "#4ade80",
                "neutral": "#3D4451",
                "base-100": "#FFFFFF"
            },
        },
    ],   
},

...
```

#### Global styles:
`Styles/input.css`

Running
-------

#### Hot reload:
`dotnet watch`

#### Building:
`dotnet run`

Adding client libraries
-----------------------

You may use [LibMan](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs?view=aspnetcore-7.0) to install frontend libraries locally

Afterwards, include it in `Views/Shared/_LibrariesPartial.cshtml`

