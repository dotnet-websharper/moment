namespace IntelliFactory.WebSharper.GlMatrix.Tests

open IntelliFactory.WebSharper

[<JavaScript>]
module Main =
    open IntelliFactory.WebSharper.Html
    open IntelliFactory.WebSharper.Moment

    let Sample (lang:string) =
        Moment.Lang(lang)
        let m = new Moment()
        let diff = Duration(-4, "hours").Add(-47, "minutes")
        let ago = m.Clone().Add(diff)
        let zones =
            [
                "Cairo", "Africa/Cairo"
                "New York", "America/New_York"
                "Tokyo", "Asia/Tokyo"
            ]
        Div [
            yield H1 [Text lang]
            yield P [
                Text ("Local: " + m.Format("LLL"))
                Br [] :> _
                Text (diff.Humanize(true) + ": " + ago.Format("LLL"))
            ]
            for showName, zoneName in zones do
                yield P [
                    Text (showName + ": " + m.Tz(ZoneName(zoneName)).Format("LLL"))
                    Br [] :> _
                    Text (diff.Humanize(true) + ": " + ago.Tz(ZoneName(zoneName)).Format("LLL"))
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


open IntelliFactory.WebSharper.Sitelets

type Action = | Index

module Site =

    open IntelliFactory.Html

    let HomePage =
        Content.PageContent <| fun ctx ->
            { Page.Default with
                Title = Some "WebSharper MomentJs Tests"
                Body = [Div [new Samples()]] }

    let Main = Sitelet.Content "/" Index HomePage

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Action.Index]

[<assembly: Website(typeof<Website>)>]
do ()
