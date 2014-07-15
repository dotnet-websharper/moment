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

    let LangData =
        Class "moment.langData"
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
            Constructor (T<string>?d)
            Constructor (T<string>?d * T<string>?format * !?T<string>?language * !?T<bool>?strict)
            Constructor (T<string>?d * T<string[]>?formats * !?T<string>?language * !?T<bool>?strict)
            "ISO_8601" =? T<string>
            Constructor T<int>?timestamp
            "unix" => T<int> ^-> MomentT
            Constructor (T<Date>?d)
            Constructor (T<int[]>?d)
            Constructor (MomentT?d)
            "utc" => T<unit>?d ^-> MomentT
            "utc" => T<int>?d ^-> MomentT
            "utc" => T<int[]>?d ^-> MomentT
            "utc" => T<string>?d ^-> MomentT
            "utc" => T<string>?d * T<string>?format ^-> MomentT
            "utc" => T<string>?d * T<string[]>?formats ^-> MomentT
            "utc" => T<string>?d * T<string>?format * T<string>?language ^-> MomentT
            "utc" => MomentT?d ^-> MomentT
            "utc" => T<Date>?d ^-> MomentT
            "parseZone" => T<string>?d ^-> MomentT
            "max" => !+MomentT ^-> MomentT
            "min" => !+MomentT ^-> MomentT
            Generic - fun t -> "isMoment" => t ^-> T<bool>
            "lang" => T<string> ^-> T<unit>
            "lang" => T<string> * T<obj>?formats ^-> T<unit>
            "lang" => T<string[]> ^-> T<unit>
            "langData" => T<unit> ^-> LangData
            "normalizeUnits" => T<string> ^-> T<string>
            "invalid" => T<unit> ^-> MomentT
            "invalid" => T<obj> ^-> MomentT
            "tz" => T<string>?date * ZoneName ^-> MomentWithTzT
            "tz" => T<int>?timestamp * ZoneName ^-> MomentWithTzT
            "tz" => T<int[]>?date * ZoneName ^-> MomentWithTzT
            "tz" => MomentT * ZoneName ^-> MomentWithTzT
            "tz" => T<Date> * ZoneName ^-> MomentWithTzT
         ]
         |+> Protocol [
            "clone" => T<unit> ^-> MomentT
            "isValid" => T<unit> ^-> T<bool>
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
            "weeksInYear" => T<unit> ^-> T<int>
            "isoWeeksInYear" => T<unit> ^-> T<int>
            "get" => T<string> ^-> T<int>
            "get" => T<string> * T<int> ^-> T<unit>
            "add" => T<string> * T<int> ^-> MomentT
            "add" => T<int> * T<string> ^-> MomentT
            "add" => T<string> * T<string> ^-> MomentT
            "add" => Duration ^-> MomentT
            "subtract" => T<string> * T<int> ^-> MomentT
            "subtract" => T<int> * T<string> ^-> MomentT
            "subtract" => T<string> * T<string> ^-> MomentT
            "subtract" => Duration ^-> MomentT
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
            "lang" => T<string> ^-> MomentT
            "tz" => ZoneName ^-> MomentWithTzT
        ]

    let MomentWithTz =
        Class "MomentWithTimezone"
        |=> Inherits Moment
        |=> MomentWithTzT
        |> Requires [Res.TzJs]
        |+> Protocol [
            "zoneAbbr" => T<unit> ^-> T<string>
            "zoneName" => T<unit> ^-> T<string>
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
                LangData
                Zone
                ZoneName
                Moment
                MomentWithTz
            ]
        ]


[<Sealed>]
type MomentExtension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<MomentExtension>)>]
do ()
