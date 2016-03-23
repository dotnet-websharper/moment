namespace WebSharper.Moment.Tests

open WebSharper

[<JavaScript>]
module Main =
    open WebSharper.Html.Client
    open WebSharper.Moment

    let Sample (lang:string) =
        Moment.Locale(lang)
        let now = new Moment()
        let diff = Duration(-4, "hours").Add(-47, "minutes")
        let ago = now.Clone().Add(diff)
        let zones =
            [
                "Cairo", "Africa/Cairo"
                "New York", "America/New_York"
                "Tokyo", "Asia/Tokyo"
            ]
        Div [
            yield H1 [Text lang]
            yield P [
                Text ("Local: " + now.Format("LLL"))
                Br [] :> _
                Text (diff.Humanize(true) + ": " + ago.Format("LLL"))
            ]
            for showName, zoneName in zones do
                yield P [
                    Text (showName + ": " + now.Tz(zoneName).Format("LLL"))
                    Br [] :> _
                    Text (diff.Humanize(true) + ": " + ago.Tz(zoneName).Format("LLL"))
                ]
        ]

    let Samples() =
        Div [
            Sample "en"
            Sample "fr"
        ]

type Samples() =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body = Main.Samples() :> _


open WebSharper.Sitelets

type Action = | Index

module Site =

    open WebSharper.Html.Server

    let HomePage ctx =
        Content.Page(
            Title = "WebSharper MomentJs Tests",
            Body = [Div [new Samples()]])

    let Main = Sitelet.Content "/" Index HomePage

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Action.Index]

[<assembly: Website(typeof<Website>)>]
do ()
