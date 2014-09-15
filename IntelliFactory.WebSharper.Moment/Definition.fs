namespace IntelliFactory.WebSharper.MomentExtension

open IntelliFactory.WebSharper.EcmaScript
open IntelliFactory.WebSharper.InterfaceGenerator

module Res =

    let Js =
        Resource "Js" "moment-with-langs.min.js"

    let TzJs =
        Resource "TimezoneJs" "moment-timezone-with-data.min.js"
        |> Requires [Js]

module Definition =

    // http://momentjs.com/docs/
    let MomentT = Type.New()
    let MomentWithTzT = Type.New()

    let ParsingFlags =
        Class "moment.parsingFlags"
        |+> [
            "overflow" =? T<int>
            "invalidMonth" =? T<string>
            "empty" => T<bool>
            "nullInput" => T<bool>
            "invalidFormat" => T<bool>
            "userInvalidated" => T<bool>
            "unusedTokens" => T<string[]>
            "unusedInput" => T<string[]>
        ]

    let Duration =
        let Duration = Type.New()
        Class "moment.duration"
        |=> Duration
        |+> [
            Constructor (T<int>)
            Constructor (T<int> * T<string>)
            Constructor (T<string>)
            Constructor (T<obj>)
        ]
        |+> Protocol [
            "humanize" => T<unit> ^-> T<string>
            "humanize" => T<bool> ^-> T<string>
            "milliseconds" => T<unit> ^-> T<int>
            "asMilliseconds" => T<unit> ^-> T<int>
            "seconds" => T<unit> ^-> T<int>
            "asSeconds" => T<unit> ^-> T<int>
            "minutes" => T<unit> ^-> T<int>
            "asMinutes" => T<unit> ^-> T<int>
            "hours" => T<unit> ^-> T<int>
            "asHours" => T<unit> ^-> T<int>
            "days" => T<unit> ^-> T<int>
            "asDays" => T<unit> ^-> T<int>
            "months" => T<unit> ^-> T<int>
            "asMonths" => T<unit> ^-> T<int>
            "years" => T<unit> ^-> T<int>
            "asYears" => T<unit> ^-> T<int>
            "add" => T<int> * !?T<string> ^-> Duration
            "add" => Duration ^-> Duration
            "subtract" => T<int> * !?T<string> ^-> Duration
            "subtract" => Duration ^-> Duration
            "as" => T<string> ^-> T<int>
            "get" => T<string> ^-> T<int>
        ]

    let LocaleData =
        Class "moment.localeData"
        |+> Protocol [
            "months" => MomentT ^-> T<string>
            "monthsShort" => MomentT ^-> T<string>
            "monthsParse" => T<string> ^-> T<int>
            "weekdays" => MomentT ^-> T<string>
            "weekdaysShort" => MomentT ^-> T<string>
            "weekdaysMin" => MomentT ^-> T<string>
            "weekdaysParse" => T<string> ^-> T<int>
            "longDateFormat" => T<string> ^-> T<string>
            "isPM" => T<string> ^-> T<bool>
            "meridiem" => T<int> * T<int> * T<bool> ^-> T<string>
            "calendar" => T<string> * MomentT ^-> T<string>
            "relativeTime" => T<int> * T<bool> * T<string> * T<bool> ^-> T<string>
            "pastFuture" => T<int> * T<string> ^-> T<string>
            "ordinal" => T<int> ^-> T<string>
            "preparse" => T<string> ^-> T<string>
            "postformat" => T<string> ^-> T<string>
            "week" => MomentT ^-> T<int>
            "invalidDate" => T<unit> ^-> T<string>
        ]

    let ZoneName =
        Class "moment.ZoneName"
        |+> [
            Constructor (T<string>?zone)
            |> WithInline "$zone"
        ]
        |+> Protocol [
            "name" =? T<string>
            |> WithGetterInline "$0"
        ]
        |> Requires [Res.TzJs]

    let Zone =
        Class "moment.tz.Zone"
        |> Requires [Res.TzJs]
        |+> Protocol [
            "name" =? ZoneName
            "abbrs" =? T<string[]>
            "untils" =? T<int[]>
            "offsets" =? T<int[]>
            "abbr" => T<int> ^-> T<string>
            "offset" => T<int> ^-> T<int>
            "parse" => T<int> ^-> T<int>
        ]

    let Tz =
        Class "moment.tz"
        |> Requires [Res.TzJs]
        |+> [
            "add" => ZoneName ^-> T<unit>
            "add" => T<string[]> ^-> T<unit>
            "link" => T<string> ^-> T<unit>
            "link" => T<string[]> ^-> T<unit>
            "load" => T<obj> ^-> T<unit>
            "zone" => ZoneName ^-> Zone
            "names" => T<unit> ^-> Type.ArrayOf ZoneName
        ]

    let Moment =
        let RelaxMoment = MomentT + T<string> + T<int> + T<Date> + T<int[]>
        Class "moment"
        |=> MomentT
        |=> Nested [Tz]
        |+> [
            Constructor (T<unit>)
            |> WithComment "To get the current date and time, just call moment() with no parameters."
            Constructor (T<string>?d)
            |> WithComment "When creating a moment from a string, we first check if the string matches known ISO 8601 formats, then fall back to new Date(string) if a known format is not found."
            Constructor (T<string>?d * T<string>?format * !?T<string>?language * !?T<bool>?strict)
            |> WithComment "If you know the format of an input string, you can use that to parse a moment."
            Constructor (T<string>?d * T<string[]>?formats * !?T<string>?language * !?T<bool>?strict)
            |> WithComment "If you don't know the exact format of an input string, but know it could be one of many, you can use an array of formats."
            "ISO_8601" =? T<string>
            Constructor T<obj>
            |> WithComment "You can create a moment by specifying some of the units in an object."
            Constructor T<int>?timestamp
            |> WithComment "Similar to new Date(Number), you can create a moment by passing an integer value representing the number of milliseconds since the Unix Epoch (Jan 1 1970 12AM UTC)."
            "unix" => T<int> ^-> MomentT
            |> WithComment "To create a moment from a Unix timestamp (seconds since the Unix Epoch), use moment.unix(Number)."
            Constructor (T<Date>?d)
            |> WithComment "You can create a Moment with a pre-existing native Javascript Date object."
            Constructor (T<int[]>?d)
            |> WithComment "You can create a moment with an array of numbers that mirror the parameters passed to new Date()"
            Constructor T<string>
            |> WithComment "ASP.NET returns dates in JSON as /Date(1198908717056)/ or /Date(1198908717056-0700)/"
            Constructor (MomentT?d)
            |> WithComment "All moments are mutable. If you want a clone of a moment, you can do so explicitly or implicitly."

            "utc" => T<unit>?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<int>?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<int[]>?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<string>?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<string>?d * T<string>?format ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<string>?d * T<string[]>?formats ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<string>?d * T<string>?format * T<string>?language ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => MomentT?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."
            "utc" => T<Date>?d ^-> MomentT
            |> WithComment "If you want to parse or display a moment in UTC, you can use moment.utc() instead of moment()."

            "parseZone" => T<string>?d ^-> MomentT
            |> WithComment "Moment normally interprets input times as local times (or UTC times if moment.utc() is used). However, often the input string itself contains time zone information. #parseZone parses the time and then sets the zone according to the input string."

            "max" => !+MomentT ^-> MomentT
            |> WithComment "Returns the maximum (most distant past) of the given moment instances."
            "min" => !+MomentT ^-> MomentT
            |> WithComment "Returns the minimum (most distant past) of the given moment instances."
            Generic - fun t -> "isMoment" => t ^-> T<bool>
            "locale" => T<unit> ^-> MomentT
            |> WithComment "By default, Moment.js comes with English locale strings. If you need other locales, you can load them into Moment.js for later use."
            "locale" => T<string> ^-> MomentT
            |> WithComment "By default, Moment.js comes with English locale strings. If you need other locales, you can load them into Moment.js for later use."
            "locale" => T<string[]> ^-> MomentT
            |> WithComment "By default, Moment.js comes with English locale strings. If you need other locales, you can load them into Moment.js for later use."
            "locale" => T<string> * T<obj> ^-> MomentT
            |> WithComment "By default, Moment.js comes with English locale strings. If you need other locales, you can load them into Moment.js for later use."

            "months" => (T<unit> + T<string>) ^-> T<string[]>
            "months" => (!? T<string>?format * T<int>) ^-> T<string>
            "monthsShort" => (T<unit> + T<string>) ^-> T<string[]>
            "monthsShort" => (!? T<string>?format * T<int>) ^-> T<string>
            "weekdays" => T<unit> ^-> T<string[]>
            "weekdays" => T<int> ^-> T<string>
            |> WithComment "Currently, weekdays always have Sunday as index 0, regardless of the local first day of the week."
            "weekdaysShort" => T<unit> ^-> T<string[]>
            "weekdaysShort" => T<int> ^-> T<string>
            "weekdaysMin" => T<unit> ^-> T<string[]>
            "weekdaysMin" => T<int> ^-> T<string>
            "relativeTimeThreshold" => T<string>?unit ^-> T<int>
            "relativeTimeThreshold" => T<string>?unit * T<int>?limit ^-> MomentT
            "normalizeUnits" => T<string> ^-> T<string>
            "invalid" => T<unit> ^-> MomentT
            "invalid" => T<obj> ^-> MomentT
            "tz" => T<string>?date * ZoneName ^-> MomentWithTzT
            "tz" => T<string>?date * T<string>?format * ZoneName ^-> MomentWithTzT
            "tz" => T<int>?timestamp * ZoneName ^-> MomentWithTzT
            "tz" => T<int[]>?date * ZoneName ^-> MomentWithTzT
            "tz" => MomentT * ZoneName ^-> MomentWithTzT
            "tz" => T<Date> * ZoneName ^-> MomentWithTzT
            Constructor (T<System.DateTime>?d)
            |> WithInline "moment($d)"
         ]
         |+> Protocol [
            "isValid" => T<unit> ^-> T<bool>
            |> WithComment "Moment applies stricter initialization rules than the Date constructor."
            "invalidAt" => T<unit> ^-> T<int>
            "millisecond" => (T<unit> + T<int>) ^-> T<int>
            "milliseconds" => (T<unit> + T<int>) ^-> T<int>
            "second" => (T<unit> + T<int>) ^-> T<int>
            "seconds" => (T<unit> + T<int>) ^-> T<int>
            "minute" => (T<unit> + T<int>) ^-> T<int>
            "minutes" => (T<unit> + T<int>) ^-> T<int>
            "hour" => (T<unit> + T<int>) ^-> T<int>
            "hours" => (T<unit> + T<int>) ^-> T<int>
            "date" => (T<unit> + T<int>) ^-> T<int>
            "dates" => (T<unit> + T<int>) ^-> T<int>
            "day" => (T<unit> + T<int> + T<string>) ^-> T<int>
            "days" => (T<unit> + T<int> + T<string>) ^-> T<int>
            "weekday" => (T<unit> + T<int>) ^-> T<int>
            "isoWeekday" => (T<unit> + T<int>) ^-> T<int>
            "dayOfYear" => (T<unit> + T<int>) ^-> T<int>
            "week" => (T<unit> + T<int>) ^-> T<int>
            "weeks" => (T<unit> + T<int>) ^-> T<int>
            "isoWeek" => (T<unit> + T<int>) ^-> T<int>
            "isoWeeks" => (T<unit> + T<int>) ^-> T<int>
            "month" => (T<unit> + T<int> + T<string>) ^-> T<int>
            "months" => (T<unit> + T<int> + T<string>) ^-> T<int>
            "quarter" => (T<unit> + T<int>) ^-> T<int>
            "year" => (T<unit> + T<int>) ^-> T<int>
            "years" => (T<unit> + T<int>) ^-> T<int>
            "weekYear" => (T<unit> + T<int>) ^-> T<int>
            "isoWeekYear" => (T<unit> + T<int>) ^-> T<int>
            "weeksInYear" => T<unit> ^-> T<int>
            "isoWeeksInYear" => T<unit> ^-> T<int>
            "get" => T<string> ^-> T<int>
            "set" => T<string> * T<int> ^-> MomentT
            |> WithComment "String setter."
            "add" => T<string> * T<int> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => T<int> * T<string> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => T<string> * T<string> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => Duration ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => T<obj> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "subtract" => T<string> * T<int> ^-> MomentT
            "subtract" => T<int> * T<string> ^-> MomentT
            "subtract" => T<string> * T<string> ^-> MomentT
            "subtract" => Duration ^-> MomentT
            "subtract" => T<obj> ^-> MomentT
            |> WithComment "Mutates the original moment by subtracting time."
            "startOf" => T<string>?unit ^-> MomentT
            "endOf" => T<string>?unit ^-> MomentT
            "local" => T<unit> ^-> MomentT
            "utc" => T<unit> ^-> MomentT
            "zone" => T<unit> ^-> T<int>
            "zone" => (T<int> + T<string>)?offset ^-> MomentT
            "format" => T<unit> ^-> T<string>
            "format" => T<string>?format ^-> T<string>
            "fromNow" => T<unit> ^-> T<string>
            "fromNow" => T<bool>?withoutSuffix ^-> T<string>
            "from" => RelaxMoment * !?T<bool>?withoutSuffix ^-> T<string>
            "calendar" => T<unit> ^-> T<string>
            "calendar" => RelaxMoment?referenceTime ^-> T<string>
            "diff" => RelaxMoment ^-> T<int>
            "diff" => RelaxMoment * T<string>?unit ^-> T<int>
            "diff" => RelaxMoment * T<string>?unit * T<bool>?dontRound ^-> T<float>
            "valueOf" => T<unit> ^-> T<int>
            "unix" => T<unit> ^-> T<int>
            "daysInMonth" => T<unit> ^-> T<int>
            "toDate" => T<unit> ^-> T<Date>
            "toArray" => T<unit> ^-> T<int[]>
            "toJSON" => T<unit> ^-> T<obj>
            "toISOString" => T<unit> ^-> T<string>
            "isBefore" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            "isSame" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            "isAfter" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            "isLeapYear" => T<unit> ^-> T<bool>
            "isDST" => T<unit> ^-> T<bool>
            "isDSTShifted" => T<unit> ^-> T<bool>
            "locale" => T<string> ^-> MomentT
            "locale" => T<string[]> ^-> MomentT
            "locale" => T<string> * T<obj> ^-> MomentT
        ]

    let Assembly =
        Assembly [
            Namespace "IntelliFactory.WebSharper.Moment.Resources" [
                Res.Js.AssemblyWide()
                Res.TzJs
            ]
            Namespace "IntelliFactory.WebSharper.Moment" [
                ParsingFlags
                Duration
                LocaleData
                Zone
                ZoneName
                Moment
            ]
        ]


[<Sealed>]
type MomentExtension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<MomentExtension>)>]
do ()
