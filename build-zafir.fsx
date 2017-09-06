#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.Moment")
        .VersionFrom("WebSharper")
        .WithFramework(fun fw -> fw.Net45)

let main =
    bt.WebSharper4.Extension("WebSharper.Moment")
        .Embed(["moment-with-locales.min.js"; "moment-timezone-with-data.min.js"])
        .SourcesFromProject()

let test =
    bt.WebSharper4.HtmlWebsite("WebSharper.Moment.Tests")
        .SourcesFromProject()
        .References(fun r -> 
            [
                r.NuGet("WebSharper.Html").Latest(true).ForceFoundVersion().Reference()
                r.Project main
            ])

bt.Solution [
    main
    test

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.Moment-2.10.3"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.moment"
                Description = "WebSharper Extensions for Moment.js 2.10.3"
                RequiresLicenseAcceptance = true })
        .Add(main)

]
|> bt.Dispatch
