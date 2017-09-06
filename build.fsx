#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.Moment")
        .VersionFrom("WebSharper", versionSpec = "(,4.0)")
        .WithFramework(fun fw -> fw.Net45)

let main =
    bt.WebSharper.Extension("WebSharper.Moment")
        .Embed(["moment-with-locales.min.js"; "moment-timezone-with-data.min.js"])
        .SourcesFromProject()

let test =
    bt.WebSharper.HtmlWebsite("WebSharper.Moment.Tests")
        .SourcesFromProject()
        .References(fun r -> 
            [
                r.NuGet("WebSharper.Html").Version("(,4.0)").Reference()
                r.Project main
            ])

bt.Solution [
    main
    test

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.Moment-2.12.0"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.moment"
                Description = "WebSharper Extensions for Moment.js 2.12.0"
                RequiresLicenseAcceptance = true })
        .Add(main)

]
|> bt.Dispatch
