#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("Zafir.Moment")
        .VersionFrom("Zafir")
        .WithFramework(fun fw -> fw.Net45)

let main =
    bt.Zafir.Extension("WebSharper.Moment")
        .Embed(["moment-with-locales.min.js"; "moment-timezone-with-data.min.js"])
        .SourcesFromProject()

let test =
    bt.Zafir.HtmlWebsite("WebSharper.Moment.Tests")
        .SourcesFromProject()
        .References(fun r -> 
            [
                r.NuGet("Zafir.Html").Latest(true).ForceFoundVersion().Reference()
                r.Project main
            ])

bt.Solution [
    main
    test

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "Zafir.Moment-2.10.3"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.moment"
                Description = "Zafir Extensions for Moment.js 2.10.3"
                RequiresLicenseAcceptance = true })
        .Add(main)

]
|> bt.Dispatch
