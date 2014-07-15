# Overview

This WebSharper Extension provides a set of classes and functions almost
identical to the ones documented in [Moment JS](http://momentjs.com). When used
in WebSharper projects, these stub classes delegate the work to the actual
classes implemented by the library.

After adding the reference to the project all the classes can be found
under the `IntelliFactory.WebSharper.Moment` namespace.

# Resources

Both files of the Moment JS library (`moment.js` and `moment-timezone.js`)
exist in several versions, depending on the amount of data it contains:

  * `moment.js` has a basic version with only English and an international
  version with many translations.

  * `moment-timezone.js` has different versions depending on the number of
  historical time zone change events it contains.

To ensure maximum functionality by default, WebSharper.Moment includes the
largest version of both these files. If you wish to use a lighter version, you
can download the desired file from [the Moment JS website](http://momentjs.com),
add it to your project and add the following to your `Web.config`:

```xml
<configuration>
  <appSettings>
    <!-- to change the moment.js link -->
    <add key="IntelliFactory.WebSharper.Moment.Resources.Js" value="/path/to/my-moment.js" /> 
    <!-- to change the moment-timezone.js link -->
    <add key="IntelliFactory.WebSharper.Moment.Resources.TimezoneJs" value="/path/to/my-moment-timezone.js" /> 
  </appSettings>
</configuration>
```

# Usage

The Moment WebSharper extension is as far as possible a one-to-one
mapping of Moment JS. Therefore, this documentation will only discuss
the differences between Moment JS and the WebSharper extension. For a
complete reference, see [the Moment JS homepage](http://momentjs.com).

## Differences with the JavaScript API

  * In order to correctly pull the dependency on the Moment Timezone library
  when the `moment.tz()` method is called, the name of the timezone is wrapped
  in a `ZoneName` class. Therefore the following JavaScript code:

```javascript
moment().tz("America/New_York")
```

  needs to be translated as such in F#:

```fsharp
Moment().Tz(ZoneName("America/New_York"))
```
