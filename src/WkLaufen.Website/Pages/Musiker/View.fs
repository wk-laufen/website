module Musiker.View

open Fable.Import.Slick
open Fable.React
open Fable.React.Props
open Fulma
open global.Data
open Global

let root =
  Layout.page
    "member-groups"
    Images.die_musikerinnen_musiker_der_wk_laufen_w1000h600
    [
      Heading.h1 [ Heading.Is3 ] [ str "Die Musikerinnen & Musiker der WK Laufen" ]
      ul [Class "menu"] (
        MemberQuery.getGroups()
        |> List.map (fst >> fun group ->
          let image =
            Images.memberGroups_w200h130
            |> Map.find group.Id
          let href = MusikerRegister group.Id |> toLink
          App.Html.menuItem image group.Name href
        )
      )
    ]

let detail groupId =
  MemberQuery.getGroups()
  |> List.tryFind (fst >> fun g -> g.Id = groupId)
  |> function
    | Some (group, members) ->
      Layout.page
        "members"
        Images.die_musikerinnen_musiker_der_wk_laufen_w1000h600
        [
          Heading.h1 [ Heading.Is3 ] [ str group.Name ]
          div [Class "rich-text"] [
            Fable.Import.Slick.slider
                [
                  Draggable false
                  Infinite false
                  // AdaptiveHeight true
                ]
                (
                  members
                  |> List.map (fun m ->
                      div [Class "member"] [
                          yield Heading.h2 [ Heading.Is4 ] [ sprintf "%s %s" m.FirstName m.LastName |> str ]

                          yield!
                            Images.members_w200h270
                            |> Map.tryFind (string m.BMVId)
                            |> function
                            | Some photo -> [ div [Class "image"] [ App.Html.image photo (Some 200, Some 270)] ]
                            | None -> []

                          yield ul [] [
                              yield!
                                m.Instruments
                                |> List.tryHead
                                |> Option.map (fun instrument ->
                                  li [] [
                                    sprintf "Instrument: %s" instrument |> str
                                  ]
                                )
                                |> Option.toList
                              yield!
                                match m.Roles |> List.map (DataModels.Role.toString m.Gender) with
                                | [] -> []
                                | [ head ] -> [ li [] [ sprintf "Funktion: %s" head |> str ] ]
                                | x -> [ li [] [ x |> String.concat ", " |> sprintf "Funktionen: %s" |> str ] ]
                              yield!
                                m.MemberSince
                                |> Option.map (fun x -> li [] [ sprintf "Aktiv seit: %d" x.Year |> str ])
                                |> Option.toList
                              yield li [] [ sprintf "Wohnort: %s" m.City |> str ]
                          ]
                          yield div [Class "clear"] []
                      ]
                  )
                )
          ]
        ]
    | None ->
      Browser.Dom.console.error ("Can't find member group with id " + groupId)
      root
