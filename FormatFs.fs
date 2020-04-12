[<AutoOpen>]
module FsAst.FormatFs

open System.IO
open Fantomas
open FSharp.Compiler.SourceCodeServices

let formatAst ast =
    let cfg = { FormatConfig.FormatConfig.Default with StrictMode = true; PageWidth = 120 } // do not format comments
    CodeFormatter.FormatASTAsync(ast, "temp.fsx", [], None, cfg)

let formatFs fs checker = async {
    let s = File.ReadAllText fs
    let parsOpt = {FSharpParsingOptions.Default with SourceFiles = [|fs|]}
    let! res = CodeFormatter.ParseAsync(fs, SourceOrigin.SourceString s, parsOpt, checker)
    for (ast, _) in res do
        let! txt = formatAst ast
        printfn "%s" txt
}