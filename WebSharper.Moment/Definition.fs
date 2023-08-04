// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharper.Moment

open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    // http://momentjs.com/docs/
    let MomentT = Class "moment"

    let ParsingFlags =
        Class "moment.parsingFlags"
        |> WithSourceName "ParsingFlags"
        |+> Instance [
            "overflow" =? T<int>
            "invalidMonth" =? T<string>
            "empty" =? T<bool>
            "nullInput" =? T<bool>
            "invalidFormat" =? T<bool>
            "userInvalidated" =? T<bool>
            "meridiem" =? T<string>
            "parsedDateParts" =? T<obj[]>
            "unusedTokens" =? T<string[]>
            "unusedInput" =? T<string[]>
        ]

    let Duration =
        Class "moment.duration"
        |> WithSourceName "Duration"
        |+> Static [
            Constructor (T<int>)
            Constructor (T<int> * T<string>)
            Constructor (T<string>)
            Constructor (T<obj>)
            Constructor (T<string> * T<string>)
        ]
        |+> Instance [
            "humanize" => T<unit> ^-> T<string>
            "humanize" => T<bool> * !?T<obj> ^-> T<string>
            "humanize" => T<obj> ^-> T<string>
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
            "weeks" => T<unit> ^-> T<int>
            "asWeeks" => T<unit> ^-> T<int>
            "months" => T<unit> ^-> T<int>
            "asMonths" => T<unit> ^-> T<int>
            "years" => T<unit> ^-> T<int>
            "asYears" => T<unit> ^-> T<int>
            "add" => T<int> * !?T<string> ^-> TSelf
            "add" => TSelf ^-> TSelf
            "add" => T<obj> ^-> TSelf
            "subtract" => T<int> * !?T<string> ^-> TSelf
            "subtract" => TSelf ^-> TSelf
            "subtract" => T<obj> ^-> TSelf
            "as" => T<string> ^-> T<int>
            "get" => T<string> ^-> T<int>
            "toJSON" => T<unit> ^-> T<string>
            "toISOString" => T<unit> ^-> T<string>
            "isDuration" => T<obj> ^-> T<bool>
            "clone" => T<unit> ^-> TSelf
            "locale" => T<string> ^-> TSelf
        ]

    let LocaleData =
        Class "moment.localeData"
        |> WithSourceName "LocaleData"
        |+> Instance [
            "months" => !?MomentT ^-> T<string>
            "monthsShort" => !?MomentT ^-> T<string>
            "monthsParse" => T<string> ^-> T<int>
            "weekdays" => !?MomentT ^-> T<string>
            "weekdays" => T<bool> ^-> T<string>
            "weekdaysShort" => !?MomentT ^-> T<string>
            "weekdaysShort" => T<bool> ^-> T<string>
            "weekdaysMin" => !?MomentT ^-> T<string>
            "weekdaysMin" => T<bool> ^-> T<string>
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
            "firstDayOfWeek" => T<unit> ^-> T<int>
            "firstDayOfYear" => T<unit> ^-> T<int>
        ]

//    let ZoneName =
//        Class "moment.ZoneName"
//        |+> Static [
//            Constructor (T<string>?zone)
//            |> WithInline "$zone"
//        ]
//        |+> Instance [
//            "name" =? T<string>
//            |> WithGetterInline "$0"
//        ]
//        |> Requires [Res.TzJs]

    let Zone =
        Class "moment.tz.Zone"
        |> ImportDefault "moment-timezone"
        |> WithSourceName "Zone"
        |+> Instance [
            "name" =? T<string>
            "abbrs" =? T<string[]>
            "untils" =? T<int[]>
            "offsets" =? T<int[]>
            "abbr" => T<int> ^-> T<string>
            "utcOffset" => T<int> ^-> T<int>
            "parse" => T<int> ^-> T<int>
        ]

    let UnpackedBundle =
        Pattern.Config "moment.tz.unpackedBundle"
            {
                Required = []
                Optional =
                    [
                        "zones", Type.ArrayOf Zone
                        "links", T<string[]>
                        "version", T<string>
                    ]
            }
        |> WithSourceName "UnpackedBundle"

    let Tz =
        Class "moment.tz"
        |> WithSourceName "TimeZone"
        |> ImportDefault "moment-timzone"
        |+> Static [
            "add" => T<string[]> ^-> T<unit>
            "add" => T<string> ^-> T<unit>
            "link" => T<string> ^-> T<unit>
            "link" => T<string[]> ^-> T<unit>
            "load" => UnpackedBundle ^-> T<unit>
            "zone" => T<string> ^-> Zone
            "names" => T<unit> ^-> T<string[]>
            "setDefault" => T<string> ^-> T<unit>
            "guess" => T<unit> ^-> T<string>
            "pack" => Zone?unpacked ^-> T<string>
            "unpack" => T<string>?packed ^-> Zone
            "packBase60" => T<int> ^-> T<string>
            "unpackBase60" => T<string> ^-> T<int>
            "createLinks" => UnpackedBundle ^-> UnpackedBundle
            "filterYears" => UnpackedBundle * T<int>?fromYear * T<int>?toYear ^-> UnpackedBundle
            "filterLinkPack" => UnpackedBundle * T<int>?fromYear * T<int>?toYear ^-> T<string>
        ]

    let RelaxMoment = MomentT + T<string> + T<int> + T<Date> + T<int[]>

    let HTML5ConstantFormats =
        Class "moment.HTML5_FMT"
        |> WithSourceName "HTML5ConstantFormats"
        |+> Static [
            "DATETIME_LOCAL" =? T<string>
            "DATETIME_LOCAL_SECONDS" =? T<string>
            "DATETIME_LOCAL_MS" =? T<string>
            "DATE" =? T<string>
            "TIME" =? T<string>
            "TIME_SECONDS" =? T<string>
            "TIME_MS" =? T<string>
            "WEEK" =? T<string>
            "MONTH" =? T<string>
        ]

    let Moment =
        MomentT
        |+> Static [
            "ISO_8601" =? T<string>
            |> WithComment "ISO 8601 date format."

            Constructor (T<unit>)
            |> WithComment "Initialize with the current time."
            Constructor (T<string>?d)
            |> WithComment "Check if the string matches known ISO 8601 formats, then fall back to new Date(string) if a known format is not found, or checking if the string matches with the JSON date."
            Constructor (T<string>?d * T<string>?format * !?T<bool>?strict)
            |> WithComment "Parse with exact format (with an optional strict parameter)."
            Constructor (T<string>?d * T<string>?format * T<string>?language * !?T<bool>?strict)
            |> WithComment "Parse with exact format (with optional locale and strict parameters)."
            Constructor (T<string>?d * T<string>?format * T<string[]>?languages)
            |> WithComment "Parse with exact format (with optional locales)."
            Constructor (T<string>?d * T<string[]>?formats * !?T<bool>?strict)
            |> WithComment "Parse with multiple format choices (with an optional strict parameter)."
            Constructor (T<string>?d * T<string[]>?formats * T<string>?language * !?T<bool>?strict)
            |> WithComment "Parse with multiple format choices (with optional locale and strict parameter)."
            Constructor T<obj>
            |> WithComment "Create a moment by specifying some of the units in an object."
            Constructor T<int>?timestamp
            |> WithComment "Create a moment by passing an the number of milliseconds since the Unix Epoch (Jan 1 1970 12AM UTC)."
            Constructor (T<Date>?d)
            |> WithComment "Create a Moment with a pre-existing native Javascript Date object."
            Constructor (T<int[]>?d)
            |> WithComment "Create a moment with an array of numbers that mirror the parameters passed to new Date()."
            Constructor (MomentT?d)
            |> WithComment "Copy constructor."
            Constructor (T<int>?d * T<string>?unit)
            |> WithComment "Create a moment by specifying the unit."
            Constructor (T<System.DateTime>?d)
            |> WithInline "moment($d)"

            "unix" => T<int> ^-> MomentT
            |> WithComment "Create a moment from a Unix timestamp (seconds since the Unix Epoch)."

            "utc" => T<unit>?d ^-> MomentT
            |> WithComment "Current UTC time."
            "utc" => T<int>?d ^-> MomentT
            |> WithComment "Create a moment by passing an the number of milliseconds since the Unix Epoch (Jan 1 1970 12AM UTC) in UTC."
            "utc" => T<int[]>?d ^-> MomentT
            |> WithComment "Create a moment with an array of numbers that mirror the parameters passed to new Date() in UTC."
            "utc" => T<string>?d ^-> MomentT
            |> WithComment "Check if the string matches known ISO 8601 formats, then fall back to new Date(string) if a known format is not found."
            "utc" => T<string>?d * T<string>?format * !?T<bool>?strict ^-> MomentT
            |> WithComment "Create an UTC moment."
            "utc" => T<string>?d * T<string[]>?formats ^-> MomentT
            |> WithComment "Create an UTC moment."
            "utc" => T<string>?d * T<string>?format * T<string>?language * !?T<bool>?strict ^-> MomentT
            |> WithComment "Create an UTC moment."
            "utc" => T<string>?d * T<string>?format * T<string[]>?languages ^-> MomentT
            |> WithComment "Create an UTC moment."
            "utc" => MomentT?d ^-> MomentT
            |> WithComment "Create an UTC moment."
            "utc" => T<Date>?d ^-> MomentT
            |> WithComment "Create an UTC moment."

            "parseZone" => !?T<string>?d ^-> MomentT
            |> WithComment "Parses the time and then sets the zone according to the input string."
            "parseZone" => T<string>?d * T<string>?format * !?T<bool>?strict ^-> MomentT
            |> WithComment "Parses the time and format (with an optional strict parameter) and then sets the zone according to the input string."
            "parseZone" => T<string>?d * T<string[]>?formats ^-> MomentT
            |> WithComment "Parses the time and formats and then sets the zone according to the input string."
            "parseZone" => T<string>?d * T<string>?format * T<string>?language * !?T<bool>?strict ^-> MomentT
            |> WithComment "Parses the time, format and locale (with an optional strict parameter) and then sets the zone according to the input string."

            "max" => !+MomentT ^-> MomentT
            |> WithComment "Returns the maximum (most distant future) of the given moment instances."
            "min" => !+MomentT ^-> MomentT
            |> WithComment "Returns the minimum (most distant past) of the given moment instances."

            Generic - fun t -> "isMoment" => t ^-> T<bool>
            |> WithComment "Check if the parameter is a moment object."
            Generic - fun t -> "isDate" => t ^-> T<bool>
            |> WithComment "Check if the parameter is a native js Date object."

            "locale" => T<unit> ^-> T<string>
            |> WithComment "Get the current global locale."
            "locale" => T<string> ^-> T<unit>
            |> WithComment "Sets the global locale."
            "locale" => T<string[]> ^-> T<unit>
            |> WithComment "Sets the global locale."
            "locale" => T<string> * T<obj> ^-> T<unit>
            |> WithComment "Sets the global locale."

            "localeData" => T<unit> ^-> LocaleData
            "localeData" => T<string> ^-> LocaleData

            "defineLocale" => T<string> * T<obj> ^-> T<unit>
            "updateLocale" => T<string> * T<obj> ^-> T<unit>
            "calendarFormat" =! T<Function>

            "months" => (T<unit> + T<string>) ^-> T<string[]>
            "months" => (!? T<string>?format * T<int>) ^-> T<string>
            "monthsShort" => (T<unit> + T<string>) ^-> T<string[]>
            "monthsShort" => (!? T<string>?format * T<int>) ^-> T<string>
            "weekdays" => T<unit> ^-> T<string[]>
            "weekdays" => T<int> ^-> T<string>
            |> WithComment "Weekdays always have Sunday as index 0, regardless of the local first day of the week."
            "weekdaysShort" => T<unit> ^-> T<string[]>
            "weekdaysShort" => T<int> ^-> T<string>
            "weekdaysMin" => T<unit> ^-> T<string[]>
            "weekdaysMin" => T<int> ^-> T<string>
            "relativeTimeThreshold" => T<string>?unit ^-> T<int>
            "relativeTimeThreshold" => T<string>?unit * T<int>?limit ^-> MomentT
            "relativeTimeRounding" => T<unit> ^-> T<Function>
            "relativeTimeRounding" => T<Function> ^-> T<unit>
            "now" =! ( T<unit> ^-> T<int>)
            "normalizeUnits" => T<string> ^-> T<string>
            "invalid" => T<unit> ^-> MomentT
            "invalid" => T<obj> ^-> MomentT

            "tz" => T<unit> * T<string>?timeZone ^-> MomentT
            "tz" => T<string>?d * T<string>?timeZone ^-> MomentT
            "tz" => T<string>?d * T<string>?format * !?T<string>?language * !?T<bool>?strict * T<string>?timeZone ^-> MomentT
            "tz" => T<string>?d * T<string[]>?formats * !?T<string>?language * !?T<bool>?strict * T<string>?timeZone ^-> MomentT
            "tz" => T<obj> * T<string>?timeZone ^-> MomentT
            "tz" => T<int>?timestamp * T<string>?timeZone ^-> MomentT
            "tz" => T<Date>?d * T<string>?timeZone ^-> MomentT
            "tz" => T<int[]>?d * T<string>?timeZone ^-> MomentT
            //"tz" => T<string> * T<string>?timeZone ^-> MomentT same as the second
            "tz" => MomentT?d * T<string>?timeZone ^-> MomentT
            "tz" => T<System.DateTime>?d * T<string>?timeZone ^-> MomentT
            |> WithInline "moment.tz($d, $timeZone)"
        ]
        |> ImportDefault "moment-timezone"
        |+> Instance [
            "isValid" => T<unit> ^-> T<bool>
            |> WithComment "Moment applies stricter initialization rules than the Date constructor."
            "invalidAt" => T<unit> ^-> T<int>
            "parsingFlags" => T<unit> ^-> ParsingFlags
            "creationData" => T<unit> ^-> T<obj>
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
            |> ObsoleteWithMessage "Use Moment().Date() instead."
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
            |> ObsoleteWithMessage "Use Moment().Month() instead."
            "quarter" => (T<unit> + T<int>) ^-> T<int>
            "quarters" => (T<unit> + T<int>) ^-> T<int>
            "year" => (T<unit> + T<int>) ^-> T<int>
            "years" => (T<unit> + T<int>) ^-> T<int>
            |> ObsoleteWithMessage "Use Moment().Year() instead."
            "weekYear" => (T<unit> + T<int>) ^-> T<int>
            "isoWeekYear" => (T<unit> + T<int>) ^-> T<int>
            "weeksInYear" => T<unit> ^-> T<int>
            "isoWeeksInYear" => T<unit> ^-> T<int>
            "get" => T<string> ^-> T<int>
            "set" => T<string> * T<int> ^-> MomentT
            |> WithComment "String setter."
            "set" => T<obj> ^-> MomentT
            |> WithComment "Object setter."
            "clone" => T<unit> ^-> MomentT
            |> WithComment "Clones the Moment instance."

            "add" => T<string> * T<int> ^-> MomentT
            |> ObsoleteWithMessage "Deprecated in 2.8.0. Please use Add(number, period) instead."
            |> WithComment "Mutates the original moment by adding time."
            "add" => T<int> * T<string> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => Duration ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "add" => T<obj> ^-> MomentT
            |> WithComment "Mutates the original moment by adding time."
            "subtract" => T<string> * T<int> ^-> MomentT
            |> ObsoleteWithMessage "Deprecated in 2.8.0. Please use Subtract(number, period) instead."
            |> WithComment "Mutates the original moment by subtracting time."
            "subtract" => T<int> * T<string> ^-> MomentT
            |> WithComment "Mutates the original moment by subtracting time."
            "subtract" => Duration ^-> MomentT
            |> WithComment "Mutates the original moment by subtracting time."
            "subtract" => T<obj> ^-> MomentT
            |> WithComment "Mutates the original moment by subtracting time."
            "startOf" => T<string>?unit ^-> MomentT
            |> WithComment "Mutates the original moment by setting it to the start of a unit of time."
            "endOf" => T<string>?unit ^-> MomentT
            |> WithComment "Mutates the original moment by setting it to the end of a unit of time."
            "local" => !?T<bool> ^-> MomentT
            "utc" => !?T<bool> ^-> MomentT
            |> WithSourceName "ToUtc"
            "utcOffset" => T<unit> ^-> T<int>
            |> WithComment "Get the utc offset in minutes."
            "utcOffset" => (T<int> + T<string>)?offset * !?T<bool>?keepTime ^-> T<int>
            |> WithComment "Set the utc offset."
            "zone" => T<unit> ^-> T<int>
            |> ObsoleteWithMessage "Deprecated in 2.9.0. Use utcOffset instead."
            "zone" => (T<int> + T<string>)?offset ^-> MomentT
            |> ObsoleteWithMessage "Deprecated in 2.9.0. Use utcOffset instead."
            "format" => T<unit> ^-> T<string>
            "format" => T<string>?format ^-> T<string>
            "fromNow" => T<unit> ^-> T<string>
            "fromNow" => T<bool>?withoutSuffix ^-> T<string>
            "from" => RelaxMoment * !?T<bool>?withoutSuffix ^-> T<string>
            "toNow" => T<unit> ^-> T<string>
            "toNow" => T<bool>?withoutPrefix ^-> T<string>
            "to" => RelaxMoment * !?T<bool>?withoutSuffix ^-> T<string>
            "calendar" => T<unit> ^-> T<string>
            "calendar" => RelaxMoment?referenceTime ^-> T<string>
            "calendar" => RelaxMoment?referenceTime * T<obj> ^-> T<string>
            "calendar" => T<obj> ^-> T<string>
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
            "toObject" => T<unit> ^-> T<obj>
            "toString" => T<unit> ^-> T<string>
            "inspect" => T<unit> ^-> T<string>
            "isBefore" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is before another moment."
            "isSame" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is the same as another moment."
            "isAfter" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is after another moment."
            "isSameOrBefore" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is before or the same as another moment."
            "isSameOrAfter" => RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is after or the same as another moment."
            "isBetween" => RelaxMoment * RelaxMoment * !?T<string>?unit ^-> T<bool>
            |> WithComment "Check if a moment is between two other moments, optionally looking at unit scale (minutes, hours, days, etc)."
            "isLeapYear" => T<unit> ^-> T<bool>
            "isDST" => T<unit> ^-> T<bool>
            |> WithComment "Checks if the current moment is in daylight saving time."
            "isDSTShifted" => T<unit> ^-> T<bool>
            |> ObsoleteWithMessage "It doesn't give the right answer after modifying the moment object. Please don't use this."
            |> WithComment "Checks if the date has been moved by a DST."

            "locale" => T<string> ^-> MomentT
            |> WithComment "Sets the local locale."
            "locale" => T<string[]> ^-> MomentT
            |> WithComment "Sets the local locale."
            "locale" => T<string> * T<obj> ^-> MomentT
            |> WithComment "Sets the local locale."

            "localeData" => T<unit> ^-> LocaleData
            |> WithSourceName "ToLocaleData"
            "localeData" => T<string> ^-> LocaleData
            |> WithSourceName "ToLocaleData"

            "tz" => T<string> ^-> MomentT
            "zoneAbbr" => T<unit> ^-> T<string>
            "zoneName" => T<unit> ^-> T<string>
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.Moment" [
                ParsingFlags
                Duration
                LocaleData
                Zone
                //ZoneName
                UnpackedBundle
                Tz
                HTML5ConstantFormats
                Moment
            ]
        ]


[<Sealed>]
type MomentExtension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<MomentExtension>)>]
do ()
