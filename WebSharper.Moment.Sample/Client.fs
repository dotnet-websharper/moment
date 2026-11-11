namespace WebSharper.Moment.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open WebSharper.UI.Html
open WebSharper.Moment

[<JavaScript>]
module Client =

    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    // ------------------------------------------------------------------
    // Small helpers
    // ------------------------------------------------------------------

    let private truth (b: bool) = if b then "true" else "false"

    let private mkMoment (s: string) = new Moment(s)

    // Parse `s`, falling back to the current time when it is not a valid moment.
    let private safeMoment (s: string) =
        let m = mkMoment s
        if m.IsValid() then m else new Moment()

    let private day (m: Moment) = m.Format("YYYY-MM-DD")
    let private dayTime (m: Moment) = m.Format("YYYY-MM-DD HH:mm:ss")

    // ------------------------------------------------------------------
    // Assertion suite
    // ------------------------------------------------------------------

    type Check =
        {
            Group: string
            Name: string
            Expected: string
            Actual: string
        }

    let private chk group name expected actual =
        { Group = group; Name = name; Expected = expected; Actual = actual }

    let private runChecks () : Check list =
        [
            // --- Construction & formatting ---------------------------------
            chk "Formatting" "format with an ISO string" "2014-02-11"
                (mkMoment("2014-02-11T00:00:00").Format("YYYY-MM-DD"))
            chk "Formatting" "full date format" "Friday, February 8th 2013, 9:30:26 am"
                (mkMoment("2013-02-08T09:30:26").Format("dddd, MMMM Do YYYY, h:mm:ss a"))
            chk "Formatting" "format tokens" "2013-02-08 09:30"
                (mkMoment("2013-02-08T09:30:26").Format("YYYY-MM-DD HH:mm"))
            chk "Formatting" "ISO 8601 output" "2013-02-08T09:30:26+00:00"
                (Moment.Utc("2013-02-08T09:30:26").Format("YYYY-MM-DDTHH:mm:ssZ"))
            chk "Formatting" "HTML5 constant format" "2013-02-08T09:30"
                (mkMoment("2013-02-08T09:30:26").Format(HTML5ConstantFormats.DATETIME_LOCAL))
            chk "Formatting" "toISOString" "2013-02-08T09:30:26.000Z"
                (Moment.Utc("2013-02-08T09:30:26").ToISOString())
            chk "Formatting" "unix epoch" "1360315826"
                (string (Moment.Utc("2013-02-08T09:30:26").Unix()))
            chk "Formatting" "valueOf round-trips the epoch" "1360315826000"
                (string ((Moment.Unix(1360315826)).ValueOf()))
            chk "Formatting" "out-of-range ISO date" "false"
                (truth (mkMoment("2013-02-30T00:00:00").IsValid()))
            chk "Formatting" "strict parse rejects overflow" "false"
                (truth ((new Moment("2013-02-30", "YYYY-MM-DD", true)).IsValid()))
            chk "Formatting" "strict parse accepts a valid date" "true"
                (truth ((new Moment("2013-02-08", "YYYY-MM-DD", true)).IsValid()))

            // --- Static helpers --------------------------------------------
            chk "Static" "isMoment" "true" (truth (Moment.IsMoment(mkMoment("2013-02-08"))))
            chk "Static" "isDate" "true" (truth (Moment.IsDate(new Date())))
            chk "Static" "isDuration" "true" (truth (Moment.IsDuration(Duration(1, "days"))))
            chk "Static" "max" "2013-02-09"
                (day (Moment.Max(mkMoment("2013-02-08"), mkMoment("2013-02-09"))))
            chk "Static" "min" "2013-02-08"
                (day (Moment.Min(mkMoment("2013-02-08"), mkMoment("2013-02-09"))))
            chk "Static" "unix" "1970-01-01"
                (Moment.Unix(0).ToUtc().Format("YYYY-MM-DD"))

            // --- Arithmetic ------------------------------------------------
            chk "Arithmetic" "add days" "2013-02-09"
                (day (mkMoment("2013-02-08").Add(1, "days")))
            chk "Arithmetic" "add a Duration" "2013-03-08"
                (day (mkMoment("2013-02-08").Add(Duration(1, "months"))))
            chk "Arithmetic" "subtract months" "2013-01-08"
                (day (mkMoment("2013-02-08").Subtract(1, "months")))
            chk "Arithmetic" "startOf month" "2013-02-01"
                (day (mkMoment("2013-02-08").StartOf("month")))
            chk "Arithmetic" "endOf month" "2013-02-28"
                (day (mkMoment("2013-02-08").EndOf("month")))
            chk "Arithmetic" "dayOfYear" "39"
                (string (mkMoment("2013-02-08").DayOfYear()))
            chk "Arithmetic" "daysInMonth" "28"
                (string (mkMoment("2013-02-08").DaysInMonth()))
            chk "Arithmetic" "quarter" "1"
                (string (mkMoment("2013-02-08").Quarter()))

            // --- Comparisons ----------------------------------------------
            chk "Comparisons" "isBefore" "true"
                (truth (mkMoment("2013-02-08").IsBefore(mkMoment("2013-02-09"))))
            chk "Comparisons" "isAfter" "false"
                (truth (mkMoment("2013-02-08").IsAfter(mkMoment("2013-02-09"))))
            chk "Comparisons" "isSame" "true"
                (truth (mkMoment("2013-02-08").IsSame(mkMoment("2013-02-08"))))
            chk "Comparisons" "isBetween" "true"
                (truth (mkMoment("2013-02-08").IsBetween(mkMoment("2013-01-01"), mkMoment("2013-12-31"))))
            chk "Comparisons" "isLeapYear" "true"
                (truth (mkMoment("2012-02-08").IsLeapYear()))
            chk "Comparisons" "diff in days" "1"
                (string (mkMoment("2013-02-08").Diff(mkMoment("2013-02-07"), "days")))
            chk "Comparisons" "diff in hours (rounded)" "24"
                (string (mkMoment("2013-02-08").Diff(mkMoment("2013-02-07"), "hours")))

            // --- Relative time --------------------------------------------
            chk "Relative" "from a year ago" "a year ago"
                (mkMoment("2012-02-08").From(mkMoment("2013-02-08")))
            chk "Relative" "from with suffix only" "a day"
                (mkMoment("2013-02-09").From(mkMoment("2013-02-08"), true))
            chk "Relative" "to (future)" "in 2 days"
                (mkMoment("2013-02-08").To(mkMoment("2013-02-10")))

            // --- Durations -------------------------------------------------
            chk "Durations" "asHours" "24"
                (string (Duration(1, "days").AsHours()))
            chk "Durations" "asMinutes" "120"
                (string (Duration(2, "hours").AsMinutes()))
            chk "Durations" "humanize" "2 hours"
                (Duration(2, "hours").Humanize())
            chk "Durations" "humanize (suffixed)" "in a minute"
                (Duration(1, "minutes").Humanize(true))
            chk "Durations" "toISOString" "P1D"
                (Duration(1, "days").ToISOString())
            chk "Durations" "add" "2013-02-09"
                (day (mkMoment("2013-02-08").Add(Duration(1, "days"))))

            // --- Time zones ------------------------------------------------
            chk "TimeZones" "names() is populated" "true"
                (truth (TimeZone.Names().Length > 0))
            chk "TimeZones" "names() contains a well-known zone" "true"
                (truth (TimeZone.Names() |> Array.exists (fun z -> z = "America/New_York")))
            chk "TimeZones" "guess() returns a zone string" "true"
                (truth (TimeZone.Guess().Length > 0))
            chk "TimeZones" "tz() converts a UTC instant" "2013-02-08 04:30:26 -05:00"
                (Moment.Utc("2013-02-08T09:30:26").Tz("America/New_York").Format("YYYY-MM-DD HH:mm:ss Z"))
            chk "TimeZones" "zoneAbbr()" "EST"
                (Moment.Utc("2013-02-08T09:30:26").Tz("America/New_York").ZoneAbbr())
            chk "TimeZones" "zone().UtcOffset()" "300"
                (string (TimeZone.Zone("America/New_York").UtcOffset((Moment.Utc("2013-01-01T00:00:00")).ValueOf())))
            chk "TimeZones" "zone().Abbr()" "EST"
                (TimeZone.Zone("America/New_York").Abbr((Moment.Utc("2013-01-01T00:00:00")).ValueOf()))
            chk "TimeZones" "instance tz() setter" "2013-02-08 18:30:26 +09:00"
                (Moment.Utc("2013-02-08T09:30:26").Tz("Asia/Tokyo").Format("YYYY-MM-DD HH:mm:ss Z"))

            // --- Locales ---------------------------------------------------
            chk "Locales" "localeData months" "January"
                (Moment.LocaleData("en").Months(mkMoment("2013-01-01")))
            chk "Locales" "instance locale formatting" "février"
                (mkMoment("2013-02-08").Locale("fr").Format("MMMM"))
            chk "Locales" "instance locale weekday" "Freitag"
                (mkMoment("2013-02-08").Locale("de").Format("dddd"))
        ]

    // ------------------------------------------------------------------
    // Rendering
    // ------------------------------------------------------------------

    let private checkRow (c: Check) =
        let ok = c.Expected = c.Actual
        tr [Attr.Create "class" (if ok then "row ok" else "row fail")] [
            td [Attr.Create "class" "name"] [text c.Name]
            td [] [code [] [text c.Expected]]
            td [] [code [] [text c.Actual]]
            td [] [span [Attr.Create "class" (if ok then "badge ok" else "badge fail")] [text (if ok then "pass" else "fail")]]
        ]

    let private testsPage () =
        let results = runChecks ()
        let total = results.Length
        let passed = results |> List.filter (fun c -> c.Expected = c.Actual) |> List.length
        let groups = results |> List.groupBy (fun c -> c.Group)
        div [] [
            header [Attr.Create "class" "page-head"] [
                h2 [] [text "Assertion suite"]
                p [Attr.Create "class" "lede"] [
                    text "Every call below runs in this browser against the moment.js and moment-timezone bindings, and the result is compared with a known value."
                ]
                span [Attr.Create "class" (if passed = total then "summary ok" else "summary fail")] [
                    text (sprintf "%d / %d assertions passed" passed total)
                ]
            ]
            yield! (groups |> List.map (fun (group, checks) ->
                section [Attr.Create "class" "group"] [
                    h3 [] [text group]
                    table [Attr.Create "class" "results"] [
                        thead [] [
                            tr [] [
                                th [] [text "Assertion"]
                                th [] [text "Expected"]
                                th [] [text "Actual"]
                                th [] [text "Result"]
                            ]
                        ]
                        tbody [] (checks |> List.map checkRow)
                    ]
                ]))
        ]

    // ------------------------------------------------------------------
    // Demo page
    // ------------------------------------------------------------------

    let private demoLocales = [ "en"; "en-gb"; "fr"; "de"; "es"; "ja"; "ru"; "ar"; "zh-cn" ]
    let private demoZones =
        [ "UTC"; "America/New_York"; "America/Los_Angeles"; "Europe/Berlin"; "Europe/Budapest"; "Asia/Tokyo"; "Asia/Kolkata"; "Australia/Sydney" ]

    let private demDate = Var.Create "2013-02-08T09:30"
    let private demFormat = Var.Create "dddd, MMMM Do YYYY, h:mm:ss a"
    let private demLocale = Var.Create "en"
    let private demZone = Var.Create "America/New_York"
    let private demDurationValue = Var.Create "90"
    let private demDurationUnit = Var.Create "minutes"
    let private demDateA = Var.Create "2013-02-08"
    let private demDateB = Var.Create "2013-02-10"

    let private formatOutput =
        View.Map3
            (fun (ds: string) (f: string) (loc: string) ->
                safeMoment(ds).Locale(loc).Format(f))
            demDate.View demFormat.View demLocale.View

    let private relativeOutput =
        demDate.View
        |> View.Map (fun (ds: string) -> safeMoment(ds).FromNow())

    let private zoneOutput =
        View.Map2
            (fun (ds: string) (zone: string) ->
                safeMoment(ds).Tz(zone).Format("YYYY-MM-DD HH:mm:ss (ZZ) z"))
            demDate.View demZone.View

    let private durationOutput =
        View.Map2
            (fun (v: string) (unit: string) ->
                let n = match System.Int32.TryParse v with | true, n -> n | _ -> 0
                let d = Duration(n, unit)
                sprintf "%s  ·  %d ms  ·  %s" (d.Humanize(true)) (d.AsMilliseconds()) (d.ToISOString()))
            demDurationValue.View demDurationUnit.View

    let private compareOutput =
        View.Map2
            (fun (a: string) (b: string) ->
                let ma = safeMoment(a)
                let mb = safeMoment(b)
                sprintf "before: %s · same: %s · after: %s · diff: %d days"
                    (truth (ma.IsBefore(mb))) (truth (ma.IsSame(mb))) (truth (ma.IsAfter(mb))) (ma.Diff(mb, "days")))
            demDateA.View demDateB.View

    let private textField (caption: string) (v: Var<string>) =
        div [Attr.Create "class" "field"] [
            label [] [text caption]
            Doc.InputType.Text [Attr.Create "class" "control"] v
        ]

    let private dateField (caption: string) (v: Var<string>) =
        div [Attr.Create "class" "field"] [
            label [] [text caption]
            Doc.InputType.Text [Attr.Create "class" "control"; Attr.Create "type" "datetime-local"] v
        ]

    let private selectField (caption: string) (items: string list) (v: Var<string>) =
        div [Attr.Create "class" "field"] [
            label [] [text caption]
            Doc.InputType.Select [Attr.Create "class" "control"] (fun (s: string) -> s) items v
        ]

    let private card (title: string) (hint: string) (body: Doc) =
        section [Attr.Create "class" "card"] [
            h3 [] [text title]
            p [Attr.Create "class" "hint"] [text hint]
            body
        ]

    let private outputFor (v: View<string>) =
        div [Attr.Create "class" "output"] [textView v]

    let private demoPage () =
        div [] [
            header [Attr.Create "class" "page-head"] [
                h2 [] [text "Interactive demo"]
                p [Attr.Create "class" "lede"] [
                    text "Change the fields below — every result is computed live by the moment.js binding running in this page."
                ]
            ]
            div [Attr.Create "class" "cards"] [
                card "Format" "Format a moment with Unicode tokens in any of the bundled locales." (
                    div [] [
                        div [Attr.Create "class" "field-row"] [
                            dateField "Date and time" demDate
                            textField "Format tokens" demFormat
                            selectField "Locale" demoLocales demLocale
                        ]
                        outputFor formatOutput
                    ])
                card "Relative time" "Describe the distance between a moment and now in words." (
                    div [] [
                        div [Attr.Create "class" "field-row"] [
                            dateField "Date and time" demDate
                        ]
                        outputFor relativeOutput
                    ])
                card "Time zones" "Convert a moment into another time zone with moment-timezone." (
                    div [] [
                        div [Attr.Create "class" "field-row"] [
                            dateField "Date and time" demDate
                            selectField "Zone" demoZones demZone
                        ]
                        outputFor zoneOutput
                    ])
                card "Durations" "Build a duration and read it back as humanized text, milliseconds and ISO 8601." (
                    div [] [
                        div [Attr.Create "class" "field-row"] [
                            textField "Amount" demDurationValue
                            selectField "Unit" [ "minutes"; "hours"; "days"; "weeks"; "months"; "years" ] demDurationUnit
                        ]
                        outputFor durationOutput
                    ])
                card "Comparisons" "Compare two moments and compute the difference in days." (
                    div [] [
                        div [Attr.Create "class" "field-row"] [
                            textField "First" demDateA
                            textField "Second" demDateB
                        ]
                        outputFor compareOutput
                    ])
            ]
        ]

    // ------------------------------------------------------------------
    // Shell
    // ------------------------------------------------------------------

    let private themeStorageKey = "websharper-moment-theme"
    let private theme = Var.Create "light"
    let private themeLabel = Var.Create "🌙 Dark mode"
    let private page = Var.Create "tests"

    let private applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let private toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    let private navDoc (p: string) =
        div [Attr.Create "class" "nav"] [
            Doc.Button "Tests" [Attr.Create "class" (if p = "tests" then "tab active" else "tab")] (fun () -> page := "tests")
            Doc.Button "Demo" [Attr.Create "class" (if p = "demo" then "tab active" else "tab")] (fun () -> page := "demo")
        ]

    let private pageView =
        page.View
        |> View.Map (fun p -> if p = "demo" then demoPage () else testsPage ())

    [<SPAEntryPoint>]
    let Main () =
        // Restore a previously chosen theme (light is the default).
        let savedTheme =
            try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
        if savedTheme = "dark" || savedTheme = "light" then
            theme.Value <- savedTheme
        applyTheme ()
        // moment defaults to the developer's locale; pin the page to English.
        Moment.Locale("en")
        IndexTemplate()
            .Nav(Doc.EmbedView (page.View |> View.Map navDoc))
            .Page(Doc.EmbedView pageView)
            .themeLabel(themeLabel.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .Bind()
