# WebSharper.Moment

This WebSharper extension provides classes and functions that mirror the
[moment.js](https://momentjs.com) and
[moment-timezone](https://momentjs.com/timezone/) APIs. When used in WebSharper
projects, these stub classes delegate the work to the actual library.

All types live in the `WebSharper.Moment` namespace:

| Type | JavaScript |
| --- | --- |
| `Moment` | `moment` |
| `Duration` | `moment.duration` |
| `LocaleData` | `moment.localeData` |
| `TimeZone` | `moment.tz` |
| `Zone` | `moment.tz.Zone` |
| `ParsingFlags` | `moment.parsingFlags` |
| `UnpackedBundle` | `moment.tz` bundle object |
| `HTML5ConstantFormats` | `moment.HTML5_FMT` |

## Resources

The binding is **npm-based**: it imports the default export of the
`moment-timezone` package (`import moment from "moment-timezone"`), which also
provides moment.js. There is no CDN `<script>` and no `Web.config` resource
override. Add `moment` and `moment-timezone` to your front-end dependencies; the
binding records them as `NpmDependencies` (`moment >= 2.31.0 < 3.0.0`,
`moment-timezone >= 0.6.6 < 1.0.0`).

moment's locale data is shipped as separate modules. Register the locales you
need with side-effect imports before using them:

```js
import "moment/locale/fr"
import "moment/locale/de"
```

## Differences with the JavaScript API

- **`utc()` is `ToUtc()`.** The instance method that converts a moment to UTC is
  exposed as `ToUtc()` so it does not clash with the static `Moment.Utc(...)`.
- **`moment.tz` is `TimeZone`.** The time-zone namespace is exposed as the
  `TimeZone` class (`TimeZone.Names()`, `TimeZone.Guess()`,
  `TimeZone.Zone(name)`, …), and `moment.tz.Zone` is the `Zone` type.
- **`Moment.ISO_8601` is the parsing sentinel**, not a format string. Use a
  format such as `"YYYY-MM-DDTHH:mm:ssZ"` with `Format`.
- **moment-timezone 0.6 removed the data-packing helpers** (`pack`,
  `packBase60`, `createLinks`, `filterYears`, `filterLinkPack`); the binding no
  longer exposes them.

## System.DateTime conversion

A `System.DateTime` can be passed to the `Moment` constructor:

```fsharp
let m = new Moment(System.DateTime.UtcNow)
```

Use `ToDate()` to get a JavaScript `Date` back, or `ToISOString()` for a string.
