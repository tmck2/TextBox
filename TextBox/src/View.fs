namespace TextBox

module View =

    open Fable.Import
    open Fable.Core.JsInterop
    open Types

    type Context2D = Browser.CanvasRenderingContext2D

    let render (ctx: Context2D) (model: TbState) =
        ctx.save ()

        ctx.font <- model.FontString ()
        ctx.translate (-1. * float (fst model.Scroll), -1. * float (snd model.Scroll))

        let fontSize = float (model.FontSizeInPixels ())

        let renderText (ctx: Context2D) =
            model.Text
            |> List.iteri (fun row str ->
                ctx.fillText (str, 0., fontSize + fontSize * (float row), 10000000.)
            )

        let renderCursor (ctx: Context2D) model =
            let col, row = model.CursorPosition
            let tm = ctx.measureText (model.Text.[row].[0..col-1])
            ctx.fillRect (tm.width, 2. + (float row) * fontSize, 2., 2. + fontSize)

        ctx.clearRect (0., 0., 1000., 1000000.)
        renderCursor ctx model |> ignore
        renderText ctx
        ctx.restore ()