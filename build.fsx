#load "tools/includes.fsx"
open IntelliFactory.Build

let bt =
    BuildTool().PackageId("WebSharper.Moment")
        .VersionFrom("WebSharper")
        .WithFramework(fun fw -> fw.Net40)

let main =
    bt.WebSharper.Extension("IntelliFactory.WebSharper.Moment")
        .Embed(["moment-with-langs.min.js"; "moment-timezone-with-data.min.js"])
        .SourcesFromProject()

let test =
    bt.WebSharper.HtmlWebsite("IntelliFactory.WebSharper.Moment.Tests")
        .SourcesFromProject()
        .References(fun r -> [r.Project main])

bt.Solution [
    main
    test

    bt.NuGet.CreatePackage()
        .Configure(fun c ->
            { c with
                Title = Some "WebSharper.Moment-2.5.1"
                LicenseUrl = Some "http://websharper.com/licensing"
                ProjectUrl = Some "https://github.com/intellifactory/websharper.moment"
                Description = "WebSharper Extensions for Moment.js 2.5.1"
                RequiresLicenseAcceptance = true })
        .Add(main)

]
|> bt.Dispatch
