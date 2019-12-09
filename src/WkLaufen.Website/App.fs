module App.View

open Elmish
open Elmish.Browser.Navigation
open Elmish.Browser.UrlParser
open Elmish.HMR
open Fable.Core.JsInterop
open Types
open App.State
open Global

importAll "../../sass/main.sass"

open Fable.Helpers.React
open Fable.Helpers.React.Props

let view model dispatch =
  let backArrow = function
    | Home -> []
    | _ ->
      [
        a [ ClassName "back"; OnClick (fun _ev -> dispatch GoBack) ] [
          span [ ClassName "far fa-arrow-alt-circle-left fa-5x"; Style [ Color "rgba(169,132,20,0.7)" ] ] []
        ]
      ]

  let pageHtml = function
    | Home -> Home.View.root
    | Kontakte -> Kontakte.View.root
    | News -> div [] []
    | Termine -> Termine.View.root
    | Musiker -> Musiker.View.root
    | MusikerRegister groupId -> Musiker.View.detail groupId
    | Unterstuetzen -> Unterstuetzen.View.root model.UnterstuetzenModel (UnterstuetzenMsg >> dispatch)
    | WirUeberUns -> WirUeberUns.View.root model.WirUeberUnsModel (WirUeberUnsMsg >> dispatch)
    | MitgliedWerden -> MitgliedWerden.View.root
    | Wertungen -> Wertungen.View.root
    | Jugend -> Jugend.View.root
    | Floetenkids -> Floetenkids.View.root
    | Impressum -> Impressum.View.root
    | Jahreskonzert -> Jahreskonzert.View.root model.JahreskonzertModel (JahreskonzertMsg >> dispatch)

  div [] [
    yield pageHtml model.CurrentPage
    yield a [ ClassName "impressum"; toLink Impressum |> Href ] [ str "Impressum" ]
    yield! backArrow model.CurrentPage
  ]

open Elmish.React
open Elmish.Debug
open Elmish.HMR

// App
Program.mkProgram init update view
|> Program.toNavigable (parseHash pageParser) urlUpdate
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
#endif
|> Program.run
