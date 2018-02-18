module App.Html

open Fable.Helpers.React
open Fable.Helpers.React.Props
open System
open System.Text.RegularExpressions

let splitFileName (fileName: string) =
  match fileName.LastIndexOf '.' with
  | -1 -> fileName, ""
  | idx -> fileName.Substring(0, idx), fileName.Substring idx

let imageUrl container fileName (width: int option, height: int option) =
  let fileNameWithSize =
    let fileNameWithoutExtensions, extension = splitFileName fileName
    let widthPart = match width with | Some v -> sprintf "w%d" v | None -> ""
    let heightPart = match height with | Some v -> sprintf "h%d" v | None -> ""
    sprintf "%s_%s%s%s" fileNameWithoutExtensions widthPart heightPart extension
  sprintf "images/%s/%s" container fileNameWithSize

let image container fileName (width: int option, height: int option) =
  [
    Src (imageUrl container fileName (width, height)) |> Some
    //width |> Option.map (fun v -> Fable.Helpers.React.Props.Width v)
    //height |> Option.map (fun v -> Fable.Helpers.React.Props.Height v)
  ]
  |> List.choose id
  |> List.map (fun p -> p :> IHTMLProp)
  |> img

let private random = Random()
let private obfuscate (text: string) =
  text
  |> Seq.map (fun ch ->
    let chars = "abcdefghijklmnopqrstuvwxyz0123456789"
    let randomChar = chars.[random.Next(chars.Length)] |> string
    [
      span [ Style [ Display "none" ] ] [ str randomChar ]
      span [] [ str (string ch) ]
    ]
  )
  |> List.concat

let obfuscatePhone phoneNumber =
  phoneNumber |> string |> obfuscate

let obfuscateEmail emailAddress =
  emailAddress
  |> Option.map obfuscate
  |> Option.toList
  |> List.concat

let (|Uri|_|) str =
  if Regex.IsMatch(str, "^https?://")
  then Some str
  else None

let htmlify (text: string) =
  Regex.Matches(text, @"\S+|(\r\n|\r|\n)|\s+")
  |> Seq.cast<Match>
  |> Seq.map (fun m ->
    match m.Value with
    | Uri uri -> a [ Href (uri.ToString()) ] [ str (uri.ToString()) ]
    | "\r" | "\n" | "\r\n" -> br []
    | text -> str text
  )
  |> Seq.toList