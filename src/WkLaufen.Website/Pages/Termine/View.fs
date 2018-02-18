module Termine.View

open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Global
open System
open System.IO

type Activity = {
  Title: string
  BeginTime: DateTime
  EndTime: DateTime option
  Location: string
}

let data =
  [
    {
        Title = "Konzertwertung"
        BeginTime = DateTime(2017, 11, 04, 00, 00, 00)
        EndTime = None
        Location = "Kitzmantelfabrik Vorchdorf"
    }
    {
        Title = "Weihnachtsfeier"
        BeginTime = DateTime(2017, 12, 9, 19, 30, 00)
        EndTime = None
        Location = "Engelhof"
    }
    {
        Title = "Adventkonzert"
        BeginTime = DateTime(2017, 12, 17, 18, 00, 00)
        EndTime = None
        Location = "Kapuzinerkirche"
    }
    {
        Title = "Neujahrsblasen"
        BeginTime = DateTime(2017, 12, 29, 00, 00, 00)
        EndTime = DateTime(2017, 12, 30, 00, 00, 00) |> Some
        Location = "Gmunden"
    }
    {
        Title = "Silvester"
        BeginTime = DateTime(2017, 12, 31, 00, 00, 00)
        EndTime = None
        Location = "Rathausplatz"
    }
    // {
    //     Title = "BTV Spielerei"
    //     BeginTime = DateTime(2018, 1, 29, 00, 00, 00)
    //     EndTime = None
    //     Location = "SEP"
    // }
    {
        Title = "Jahreskonzert"
        BeginTime = DateTime(2018, 3, 3, 00, 00, 00)
        EndTime = None
        Location = "Toscana Gmunden"
    }
    {
        Title = "Liebstattsonntag"
        BeginTime = DateTime(2018, 3, 11, 00, 00, 00)
        EndTime = None
        Location = "Gmunden"
    }
    {
        Title = "Palmsonntag"
        BeginTime = DateTime(2018, 3, 25, 00, 00, 00)
        EndTime = None
        Location = "Gmunden"
    }
    {
        Title = "Maibaum aufstellen"
        BeginTime = DateTime(2018, 4, 30, 00, 00, 00)
        EndTime = None
        Location = "Rathausplatz"
    }
    {
        Title = "Weckruf"
        BeginTime = DateTime(2018, 5, 1, 00, 00, 00)
        EndTime = None
        Location = "Gmunden"
    }
    {
        Title = "Fronleichnam"
        BeginTime = DateTime(2018, 5, 31, 00, 00, 00)
        EndTime = None
        Location = "Stadtpfarrkirche"
    }
    {
        Title = "BMF MV Roitham am Traunfall"
        BeginTime = DateTime(2018, 6, 9, 00, 00, 00)
        EndTime = None
        Location = "Roitham"
    }
    {
        Title = "BMF MV Hofkirchen an der Trattnach"
        BeginTime = DateTime(2018, 6, 30, 00, 00, 00)
        EndTime = None
        Location = "Hofkirchen"
    }
  ]

let formatTime (beginTime: DateTime) endTime =
  let endTime = endTime |> function | Some x -> x | None -> beginTime
  let showTime = beginTime.TimeOfDay <> TimeSpan.Zero || endTime.TimeOfDay <> TimeSpan.Zero
  let sameTime = beginTime.ToString("dd.MM.yyyy HH:mm") = endTime.ToString("dd.MM.yyyy HH:mm")
  let sameDate = beginTime.ToString("dd.MM.yyyy") = endTime.ToString("dd.MM.yyyy")
  let sameMonth = beginTime.ToString("MM.yyyy") = endTime.ToString("MM.yyyy")
  let sameYear = beginTime.ToString("yyyy") = endTime.ToString("yyyy")
  let dateTimeFormat = sprintf "dd.MM.yyyy%s" (if showTime then " HH:mm" else "")

  if sameTime then beginTime.ToString dateTimeFormat
  elif sameDate then (beginTime.ToString dateTimeFormat) + (if showTime then " - " + endTime.ToString "HH:mm" else "")
  elif sameMonth && not showTime then beginTime.ToString("dd.") + " - " + endTime.ToString("dd.") + beginTime.ToString("MM.yyyy")
  else beginTime.ToString(dateTimeFormat) + " - " + endTime.ToString(dateTimeFormat)

let root =
  Layout.page
    "activities"
    "terminkalender.jpg"
    [
      div [ ClassName "activities rich-text" ] [
        h1 [] [ str "Terminkalender" ]
        div [ ClassName "list" ] [
          table [] [
            tbody [] (
              data
              |> List.filter (fun act -> act.BeginTime > DateTime.Today.AddDays(-7.))
              |> List.groupBy (fun act -> act.BeginTime.Year)
              |> List.collect (fun (year, entries) ->
                let entryNodes =
                  entries
                  |> List.map (fun entry ->
                    tr [] [
                      td [] [ str (formatTime entry.BeginTime entry.EndTime) ]
                      td [] [ str entry.Title ]
                      td [] [ str entry.Location ]
                    ]
                  )
                tr [] [ th [ ColSpan 3. ] [ str (string year) ] ] :: entryNodes
              )
            )
          ]
        ]
      ]
    ]