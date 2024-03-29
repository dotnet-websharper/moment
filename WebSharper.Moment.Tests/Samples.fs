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
namespace WebSharper.Moment.Tests

open WebSharper

[<JavaScript>]
module Main =
    open WebSharper.UI.Html
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
        div [] [
            yield h1 [] [text lang]
            yield p [] [
                text ("Local: " + now.Format("LLL"))
                br [] [] 
                text (diff.Humanize(true) + ": " + ago.Format("LLL"))
            ]
            for showName, zoneName in zones do
                yield p [] [
                    text (showName + ": " + now.Tz(zoneName).Format("LLL"))
                    br [] []
                    text (diff.Humanize(true) + ": " + ago.Tz(zoneName).Format("LLL"))
                ]
        ]

    let Samples() =
        div [] [
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

    open WebSharper.UI.Html

    let HomePage ctx =
        Content.Page(
            Title = "WebSharper MomentJs Tests",
            Body = [div [] [client <@ Main.Samples() @>]])

    let Main = Sitelet.Content "/" Index HomePage

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Action.Index]

[<assembly: Website(typeof<Website>)>]
do ()
