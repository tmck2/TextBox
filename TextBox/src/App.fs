namespace TextBox

module App =
    open Fable.Import
    open Fable.Core.JsInterop
    open Types
    open Domain
    open View
    open Keys

    let fetch onLoaded filename =
        let xhr = Browser.XMLHttpRequest.Create()
        xhr?("open")("GET", filename) |> ignore
        xhr.send (obj)
        xhr.addEventListener_readystatechange (fun _ ->
            if xhr.readyState = 4. && xhr.status = 200. then
                Browser.console.log xhr.responseText
                onLoaded xhr.responseText)
        
    let initModel () = {
        Font = "Arial", Px 30
        CursorPosition = 0, 0
        Scroll = 0, 0
        Text = [""]
    }

    let init() =
        let mutable model = initModel ()

        let canvas = Browser.document.getElementsByTagName_canvas().[0]
        canvas.width <- 1000.
        canvas.height <- 750.
        let ctx = canvas.getContext_2d()

        ctx.translate (0., 0.)
        render ctx model

        fetch (fun str ->
            model <- { model with
                        Text = str.Split([|'\n'|]) |> Array.toList }
            render ctx model
        ) "README.md"

        Browser.document.addEventListener_keyup (fun ev ->
            match Seq.toList ev.key with
            | ch::[] -> 
                model <- update (KeyPress ch) model
            | _ ->
                match keyFromKeyCode ev.keyCode with
                | Some key ->
                    match key with
                    | Keys.LEFT      -> model <- update MoveLeft model
                    | Keys.UP        -> model <- update MoveUp model
                    | Keys.RIGHT     -> model <- update MoveRight model
                    | Keys.DOWN      -> model <- update MoveDown model
                    | Keys.BACKSPACE -> model <- update Backspace model
                    | Keys.RETURN    -> model <- update Return model
                    | _ -> ()
                | None -> ()
            render ctx model
            ev.preventDefault ()
            ev.stopPropagation ()
        )

        Browser.document.addEventListener_keydown (fun ev ->
            ev.preventDefault ()
            ev.stopPropagation ()
        )

    init()